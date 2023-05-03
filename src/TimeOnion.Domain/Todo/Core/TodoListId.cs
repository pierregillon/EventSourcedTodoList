using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core;

public record TodoListId(Guid Value) : IAggregateId
{
    public static TodoListId New() => new(Guid.NewGuid());
}