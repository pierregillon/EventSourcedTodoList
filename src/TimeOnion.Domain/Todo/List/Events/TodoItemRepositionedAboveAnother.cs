namespace TimeOnion.Domain.Todo.List.Events;

public record TodoItemRepositionedAboveAnother
    (TodoListId Id, TodoItemId ItemId, TodoItemId ReferenceItemId) : TodoListDomainEvent(Id);