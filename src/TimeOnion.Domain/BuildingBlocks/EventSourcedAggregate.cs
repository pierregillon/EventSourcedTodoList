namespace TimeOnion.Domain.BuildingBlocks;

public abstract class EventSourcedAggregate<TId> : IEventSourcedAggregate where TId : IAggregateId
{
    private readonly List<IDomainEvent> _uncommittedDomainEvents = new();
    private int _version;

    protected EventSourcedAggregate(TId id) => Id = id;

    public IEnumerable<IDomainEvent> UncommittedChanges => _uncommittedDomainEvents;

    protected internal TId Id { get; }

    protected void StoreEvent(IDomainEvent domainEvent)
    {
        domainEvent.Version = ++_version;

        _uncommittedDomainEvents.Add(domainEvent);

        Apply(domainEvent);
    }

    void IEventSourcedAggregate.LoadFromHistory(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        if (domainEvents.Count == 0)
        {
            throw new InvalidOperationException("The aggregate has no history");
        }

        foreach (var domainEvent in domainEvents)
        {
            Apply(domainEvent);

            if (_version > domainEvent.Version)
            {
                throw new InvalidOperationException("Domain events have invalid order");
            }

            _version = domainEvent.Version;
        }
    }

    public void MarkAsCommitted() => _uncommittedDomainEvents.Clear();

    protected virtual void Apply(IDomainEvent domainEvent)
    {
    }
}