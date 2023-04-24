namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemDecategorized(
    TodoListId Id,
    TodoItemId ItemId
) : TodoListDomainEvent(Id);