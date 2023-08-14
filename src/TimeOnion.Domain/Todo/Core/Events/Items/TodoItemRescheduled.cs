using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemRescheduled(
    TodoListId ListId,
    TodoItemId ItemId,
    TimeHorizons PreviousTimeHorizon,
    TimeHorizons NewTimeHorizon
) : UserDomainEvent(ListId.Value);