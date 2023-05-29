using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IDomainEvent : INotification
{
    Guid AggregateId { get; }
    int Version { get; set; }
    DateTime CreatedAt { get; set; }
}

public record DomainEvent(Guid AggregateId) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => AggregateId;
    int IDomainEvent.Version { get; set; }
    DateTime IDomainEvent.CreatedAt { get; set; }
}