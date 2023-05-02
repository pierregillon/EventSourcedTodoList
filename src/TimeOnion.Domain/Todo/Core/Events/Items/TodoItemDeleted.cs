namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemDeleted(TodoListId ListId, TodoItemId ItemId) : TodoListDomainEvent(ListId);