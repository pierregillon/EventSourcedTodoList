using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.List.Events;

public record TodoListDomainEvent(TodoListId Id) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => Id.Value;
}