using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryDomainEvent(CategoryId CategoryId) : IDomainEvent
{
    public Guid AggregateId => CategoryId.Value;
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
}