namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemDescriptionFixed(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemDescription PreviousItemDescription,
    TodoItemDescription NewItemDescription
) : TodoListDomainEvent(ListId);