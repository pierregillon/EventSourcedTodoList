using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core;

public record CategoryDomainEvent(CategoryId Id) : IDomainEvent
{
    public Guid AggregateId => Id.Value;
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
}