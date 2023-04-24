namespace TimeOnion.Domain.Todo.Core;

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());
}