using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Infrastructure;

public class Repository<TAggregate, TAggregateId> : IRepository<TAggregate, TAggregateId>
    where TAggregate : IEventSourcedAggregate
    where TAggregateId : IAggregateId
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IUserContextProvider _userContextProvider;
    private readonly IEventStore _eventStore;
    private readonly IClock _clock;

    public Repository(
        IEventStore eventStore,
        IDomainEventPublisher domainEventPublisher,
        IUserContextProvider userContextProvider,
        IClock clock
    )
    {
        _eventStore = eventStore;
        _domainEventPublisher = domainEventPublisher;
        _userContextProvider = userContextProvider;
        _clock = clock;
    }

    public async Task<TAggregate> Get(TAggregateId id)
    {
        var eventHistory = await _eventStore.GetAll(id.Value);

        if (eventHistory.Count == 0)
        {
            throw new InvalidOperationException($"The {typeof(TAggregate).Name.ToLower()} could not be found.");
        }

        return IEventSourcedAggregate.Rehydrate<TAggregate, TAggregateId>(id, eventHistory);
    }

    public async Task Save(TAggregate aggregate)
    {
        var domainEvents = aggregate.UncommittedChanges.ToArray();

        if (!domainEvents.Any())
        {
            return;
        }

        foreach (var domainEvent in domainEvents)
        {
            domainEvent.CreatedAt = _clock.Now();

            if (domainEvent is IUserDomainEvent { UserId: null } userDomainEvent)
            {
                userDomainEvent.UserId = _userContextProvider.GetUserContext().UserId;
            }
        }

        await _eventStore.Save(domainEvents);
        await _domainEventPublisher.Publish(domainEvents);
        aggregate.MarkAsCommitted();
    }
}