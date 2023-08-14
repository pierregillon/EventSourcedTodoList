using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAboveAnother(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : UserDomainEvent(ListId.Value);