namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record ItemReadyTodo(TodoListId ListId, TodoItemId ItemId) : TodoListDomainEvent(ListId);