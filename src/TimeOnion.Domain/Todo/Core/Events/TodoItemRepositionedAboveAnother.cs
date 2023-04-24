namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemRepositionedAboveAnother
    (TodoListId Id, TodoItemId ItemId, TodoItemId ReferenceItemId) : TodoListDomainEvent(Id);