namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListCreated(TodoListId ListId, TodoListName Name) : TodoListDomainEvent(ListId);