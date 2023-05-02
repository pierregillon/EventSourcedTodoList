namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAtTheEnd(TodoListId ListId, TodoItemId ItemId) : TodoListDomainEvent(ListId);