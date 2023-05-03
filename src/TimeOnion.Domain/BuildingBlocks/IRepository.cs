using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core;

public interface IRepository<TAggregate, in TAggregateId>
    where TAggregate : IEventSourcedAggregate
    where TAggregateId : IAggregateId
{
    Task Save(TAggregate category);
    Task<TAggregate> Get(TAggregateId id);
}