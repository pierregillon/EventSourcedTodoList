using MediatR;

namespace EventSourcedTodoList.Domain.BuildingBlocks;

public interface IDomainEventListener<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task INotificationHandler<TDomainEvent>.Handle(TDomainEvent notification, CancellationToken _)
    {
        return On(notification);
    }

    Task On(TDomainEvent domainEvent);
}