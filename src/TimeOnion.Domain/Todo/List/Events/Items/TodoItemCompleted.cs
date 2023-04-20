namespace TimeOnion.Domain.Todo.List.Events.Items;

public record TodoItemCompleted(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);