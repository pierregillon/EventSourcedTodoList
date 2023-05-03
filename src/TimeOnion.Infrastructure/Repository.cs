using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Infrastructure;

public class Repository<TAggregate, TAggregateId> : IRepository<TAggregate, TAggregateId>
    where TAggregate : IEventSourcedAggregate
    where TAggregateId : IAggregateId
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IEventStore _eventStore;

    public Repository(IEventStore eventStore, IDomainEventPublisher domainEventPublisher)
    {
        _eventStore = eventStore;
        _domainEventPublisher = domainEventPublisher;
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
        await _eventStore.Save(aggregate.UncommittedChanges);
        await _domainEventPublisher.Publish(aggregate.UncommittedChanges);
        aggregate.MarkAsCommitted();
    }
}