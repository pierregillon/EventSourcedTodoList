namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListCreated(TodoListId Id, TodoListName Name) : TodoListDomainEvent(Id);