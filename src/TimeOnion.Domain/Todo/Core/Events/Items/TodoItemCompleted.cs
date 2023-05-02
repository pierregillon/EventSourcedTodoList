namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemCompleted(TodoListId ListId, TodoItemId ItemId) : TodoListDomainEvent(ListId);