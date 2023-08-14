using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemDeleted(
    TodoListId ListId, 
    TodoItemId ItemId
) : UserDomainEvent(ListId.Value);