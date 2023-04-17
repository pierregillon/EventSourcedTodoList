using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public interface IEventStore
{
    Task<IEnumerable<IDomainEvent>> GetAll();
    Task AddRange(IEnumerable<IDomainEvent> domainEvents);
}