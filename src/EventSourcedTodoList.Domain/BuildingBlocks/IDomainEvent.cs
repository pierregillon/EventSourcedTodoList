using MediatR;

namespace EventSourcedTodoList.Domain.BuildingBlocks;

public interface IDomainEvent : INotification
{
}