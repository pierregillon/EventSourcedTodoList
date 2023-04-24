namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemAdded(
    TodoListId Id,
    TodoItemId ItemId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizon
) : TodoListDomainEvent(Id);