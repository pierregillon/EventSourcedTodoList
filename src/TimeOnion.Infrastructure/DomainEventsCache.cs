using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class DomainEventsCache
{
    private readonly List<IDomainEvent> _allEvents = new();
    private readonly List<IDomainEvent> _uncommittedEvents = new();
    public bool IsInitialized { get; private set; }

    public IReadOnlyCollection<IDomainEvent> AllEvents => _allEvents;

    public void AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        var events = domainEvents as IDomainEvent[] ?? domainEvents.ToArray();

        _allEvents.AddRange(events);
        _uncommittedEvents.AddRange(events);
    }

    public void Initialize(IEnumerable<IDomainEvent> domainEvents)
    {
        _allEvents.AddRange(domainEvents);
        IsInitialized = true;
    }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => _uncommittedEvents;

    public void MarkAsCommitted() => _uncommittedEvents.Clear();

    public IReadOnlyCollection<IDomainEvent> GetAggregateEvents(Guid aggregateId) =>
        AllEvents.Where(x => x.AggregateId == aggregateId).ToArray();
}