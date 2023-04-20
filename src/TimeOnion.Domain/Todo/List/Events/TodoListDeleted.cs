namespace TimeOnion.Domain.Todo.List.Events;

public record TodoListDeleted(TodoListId Id) : TodoListDomainEvent(Id);