namespace EventSourcedTodoList.Domain.BuildingBlocks;

public abstract class EventSourcedAggregate<TId>
{
    private readonly List<IDomainEvent> _uncommittedDomainEvents = new();

    protected EventSourcedAggregate(TId id)
    {
        Id = id;
    }

    public IEnumerable<IDomainEvent> UncommittedChanges => _uncommittedDomainEvents;

    public TId Id { get; }

    protected void StoreEvent(IDomainEvent domainEvent)
    {
        _uncommittedDomainEvents.Add(domainEvent);

        Apply(domainEvent);
    }

    public void MarkAsCommitted()
    {
        _uncommittedDomainEvents.Clear();
    }

    protected abstract void Apply(IDomainEvent domainEvent);
}