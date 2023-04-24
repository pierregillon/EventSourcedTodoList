namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemDeleted(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);