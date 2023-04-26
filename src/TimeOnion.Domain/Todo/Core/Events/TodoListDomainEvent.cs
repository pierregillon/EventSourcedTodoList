using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListDomainEvent(TodoListId Id) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => Id.Value;
    int IDomainEvent.Version { get; set; }
    DateTime IDomainEvent.CreatedAt { get; set; }
}