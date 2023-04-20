namespace TimeOnion.Domain.Todo.List.Events.Items;

public record ItemReadyTodo(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);