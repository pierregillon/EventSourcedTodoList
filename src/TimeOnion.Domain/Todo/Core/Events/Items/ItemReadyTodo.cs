namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record ItemReadyTodo(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);