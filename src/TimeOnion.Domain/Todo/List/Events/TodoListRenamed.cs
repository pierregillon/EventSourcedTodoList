namespace TimeOnion.Domain.Todo.List.Events;

public record TodoListRenamed(TodoListId Id, TodoListName PreviousName, TodoListName NewName) : TodoListDomainEvent(Id);