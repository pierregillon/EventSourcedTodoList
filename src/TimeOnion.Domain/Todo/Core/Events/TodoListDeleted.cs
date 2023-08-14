using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListDeleted(
    TodoListId ListId
) : UserDomainEvent(ListId.Value);