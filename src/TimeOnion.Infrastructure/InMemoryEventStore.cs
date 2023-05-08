using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Infrastructure;

public class InMemoryEventStore : IEventStore
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly IClock _clock;

    public InMemoryEventStore(IClock clock) => _clock = clock;

    public Task<IReadOnlyCollection<IDomainEvent>> GetAll(Guid aggregateId) =>
        Task.FromResult<IReadOnlyCollection<IDomainEvent>>(_domainEvents.Where(x => x.AggregateId == aggregateId)
            .ToArray());

    public Task<IReadOnlyCollection<IDomainEvent>> GetAll() =>
        Task.FromResult<IReadOnlyCollection<IDomainEvent>>(_domainEvents);

    public Task Save(IEnumerable<IDomainEvent> domainEvents)
    {
        var list = domainEvents.ToList();

        foreach (var domainEvent in list)
        {
            domainEvent.CreatedAt = _clock.Now();
        }

        _domainEvents.AddRange(list);

        return Task.CompletedTask;
    }
}