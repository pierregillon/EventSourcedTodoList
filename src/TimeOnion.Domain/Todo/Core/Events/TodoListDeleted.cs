namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListDeleted(TodoListId Id) : TodoListDomainEvent(Id);