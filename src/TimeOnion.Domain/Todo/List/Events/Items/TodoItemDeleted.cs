namespace TimeOnion.Domain.Todo.List.Events.Items;

public record TodoItemDeleted(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);