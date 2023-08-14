using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAtTheEnd(
    TodoListId ListId,
    TodoItemId ItemId
) : UserDomainEvent(ListId.Value);