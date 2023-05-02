namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAboveAnother(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : TodoListDomainEvent(ListId);