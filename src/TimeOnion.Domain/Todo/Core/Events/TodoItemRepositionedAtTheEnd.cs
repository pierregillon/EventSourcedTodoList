namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAtTheEnd(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);