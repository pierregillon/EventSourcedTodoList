using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListCreated(
    TodoListId ListId, 
    TodoListName Name
) : UserDomainEvent(ListId.Value);