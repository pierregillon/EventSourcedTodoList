namespace TimeOnion.Domain.Todo.List.Events;

public record TodoItemDecategorized(
    TodoListId Id,
    TodoItemId ItemId
) : TodoListDomainEvent(Id);