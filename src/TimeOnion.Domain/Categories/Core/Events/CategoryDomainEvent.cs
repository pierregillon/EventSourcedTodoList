using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryDomainEvent(CategoryId CategoryId) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => CategoryId.Value;
    int IDomainEvent.Version { get; set; }
    DateTime IDomainEvent.CreatedAt { get; set; }
}