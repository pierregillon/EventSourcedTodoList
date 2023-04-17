using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IDomainEventListener<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task INotificationHandler<TDomainEvent>.Handle(TDomainEvent notification, CancellationToken _) => On(notification);

    Task On(TDomainEvent domainEvent);
}