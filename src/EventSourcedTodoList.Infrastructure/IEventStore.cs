using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Infrastructure;

public interface IEventStore
{
    Task<IEnumerable<IDomainEvent>> GetAll();
    Task AddRange(IEnumerable<IDomainEvent> domainEvents);
}