namespace TimeOnion.Domain.Todo.List.Events.Items;

public record TodoItemDescriptionFixed(
    TodoListId Id,
    TodoItemId ItemId,
    TodoItemDescription PreviousItemDescription,
    TodoItemDescription NewItemDescription
) : TodoListDomainEvent(Id);