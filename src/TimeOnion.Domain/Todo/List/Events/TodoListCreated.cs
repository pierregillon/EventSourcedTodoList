namespace TimeOnion.Domain.Todo.List.Events;

public record TodoListCreated(TodoListId Id, TodoListName Name) : TodoListDomainEvent(Id);