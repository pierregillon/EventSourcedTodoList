using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class InMemoryEventStore : IEventStore
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Task<IEnumerable<IDomainEvent>> GetAll() => Task.FromResult(_domainEvents.AsEnumerable());

    public Task AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        _domainEvents.AddRange(domainEvents);

        return Task.CompletedTask;
    }
}