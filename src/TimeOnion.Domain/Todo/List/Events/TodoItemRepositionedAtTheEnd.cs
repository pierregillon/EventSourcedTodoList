namespace TimeOnion.Domain.Todo.List.Events;

public record TodoItemRepositionedAtTheEnd(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);