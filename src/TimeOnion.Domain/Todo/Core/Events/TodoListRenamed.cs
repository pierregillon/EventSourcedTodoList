using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListRenamed(
    TodoListId ListId,
    TodoListName PreviousName,
    TodoListName NewName
) : UserDomainEvent(ListId.Value);