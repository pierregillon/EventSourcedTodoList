using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IDomainEvent : INotification
{
    Guid AggregateId { get; }
    int Version { get; set; }
    DateTime CreatedAt { get; set; }
}