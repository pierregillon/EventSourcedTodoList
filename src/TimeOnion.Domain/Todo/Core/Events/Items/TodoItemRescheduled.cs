namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemRescheduled(
    TodoListId Id,
    TodoItemId ItemId,
    TimeHorizons PreviousTimeHorizon,
    TimeHorizons NewTimeHorizon
) : TodoListDomainEvent(Id);