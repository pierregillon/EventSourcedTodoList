using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class CachedEventStore : IEventStore
{
    private readonly IEventStore _decorated;
    private readonly DomainEventsCache _cache;

    public CachedEventStore(IEventStore decorated, DomainEventsCache cache)
    {
        _decorated = decorated;
        _cache = cache;
    }

    public Task Save(IEnumerable<IDomainEvent> domainEvents)
    {
        _cache.AddRange(domainEvents);

        return Task.CompletedTask;
    }

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll()
    {
        if (!_cache.IsInitialized)
        {
            _cache.Initialize(await _decorated.GetAll());
        }

        return _cache.AllEvents;
    }

    public async Task SaveUncommittedEvents()
    {
        var uncommittedEvents = _cache.GetUncommittedEvents();

        if (!uncommittedEvents.Any())
        {
            return;
        }

        await _decorated.Save(uncommittedEvents);

        _cache.MarkAsCommitted();
    }
}

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
}