namespace TimeOnion.Domain.Todo.Core;

public record TodoListId(Guid Value)
{
    public static readonly TodoListId Unique = new(Guid.Empty);

    public static TodoListId New() => new(Guid.NewGuid());
}