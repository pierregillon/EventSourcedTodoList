namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemAdded(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizon
) : TodoListDomainEvent(ListId);