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

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll(Guid aggregateId)
    {
        if (!_cache.IsInitialized)
        {
            _cache.Initialize(await _decorated.GetAll());
        }

        return _cache.GetAggregateEvents(aggregateId);
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