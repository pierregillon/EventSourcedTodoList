namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemDescriptionFixed(
    TodoListId Id,
    TodoItemId ItemId,
    TodoItemDescription PreviousItemDescription,
    TodoItemDescription NewItemDescription
) : TodoListDomainEvent(Id);