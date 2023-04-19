using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public interface IEventStore
{
    Task<IReadOnlyCollection<IDomainEvent>> GetAll();
    Task AddRange(IEnumerable<IDomainEvent> domainEvents);
}