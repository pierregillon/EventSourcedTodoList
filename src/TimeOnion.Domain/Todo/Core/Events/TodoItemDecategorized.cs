namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemDecategorized(
    TodoListId ListId,
    TodoItemId ItemId
) : TodoListDomainEvent(ListId);