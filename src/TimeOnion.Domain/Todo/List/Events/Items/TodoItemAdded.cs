namespace TimeOnion.Domain.Todo.List.Events.Items;

public record TodoItemAdded(
    TodoListId Id,
    TodoItemId ItemId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizon
) : TodoListDomainEvent(Id);