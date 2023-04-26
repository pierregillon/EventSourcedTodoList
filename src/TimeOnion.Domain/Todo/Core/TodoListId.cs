namespace TimeOnion.Domain.Todo.Core;

public record TodoListId(Guid Value)
{
    public static TodoListId New() => new(Guid.NewGuid());
}