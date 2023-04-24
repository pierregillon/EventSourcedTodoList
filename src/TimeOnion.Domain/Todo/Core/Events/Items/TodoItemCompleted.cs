namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemCompleted(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);