namespace TimeOnion.Domain.Todo.List;

public record TodoListId(Guid Value)
{
    public static readonly TodoListId Unique = new(Guid.Empty);

    public static TodoListId New() => new(Guid.NewGuid());
}