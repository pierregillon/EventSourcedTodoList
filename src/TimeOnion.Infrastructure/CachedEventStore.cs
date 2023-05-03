using Microsoft.Extensions.Logging;
using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class CachedEventStore : IEventStore
{
    private readonly IEventStore _decorated;
    private readonly DomainEventsCache _cache;
    private readonly ILogger<CachedEventStore> _logger;
    private readonly ManualResetEvent _lock = new(true);

    public CachedEventStore(IEventStore decorated, DomainEventsCache cache, ILogger<CachedEventStore> logger)
    {
        _decorated = decorated;
        _cache = cache;
        _logger = logger;
    }

    public async Task Save(IEnumerable<IDomainEvent> domainEvents) => await Lock(() =>
    {
        _cache.AddRange(domainEvents);

        return Task.CompletedTask;
    });

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll(Guid aggregateId)
    {
        await Lock(async () =>
        {
            if (!_cache.IsInitialized)
            {
                _cache.Initialize(await _decorated.GetAll());
            }
        });

        return _cache.GetAggregateEvents(aggregateId);
    }

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll()
    {
        await Lock(async () =>
        {
            if (!_cache.IsInitialized)
            {
                _cache.Initialize(await _decorated.GetAll());
            }
        });

        return _cache.AllEvents;
    }

    public async Task SaveUncommittedEvents() => await Lock(async () =>
    {
        var uncommittedEvents = _cache.GetUncommittedEvents();

        _logger.LogInformation($"{uncommittedEvents.Count} domain events to save.");

        if (!uncommittedEvents.Any())
        {
            return;
        }

        await _decorated.Save(uncommittedEvents);

        _cache.MarkAsCommitted();
    });

    private async Task Lock(Func<Task> action)
    {
        _lock.WaitOne();
        await action();
        _lock.Set();
    }
}