namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListDeleted(TodoListId ListId) : TodoListDomainEvent(ListId);