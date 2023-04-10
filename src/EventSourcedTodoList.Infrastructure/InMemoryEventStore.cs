using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Infrastructure;

public class InMemoryEventStore : IEventStore
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Task<IEnumerable<IDomainEvent>> GetAll()
    {
        return Task.FromResult(_domainEvents.AsEnumerable());
    }

    public Task AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        _domainEvents.AddRange(domainEvents);

        return Task.CompletedTask;
    }
}