namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListRenamed(TodoListId Id, TodoListName PreviousName, TodoListName NewName) : TodoListDomainEvent(Id);