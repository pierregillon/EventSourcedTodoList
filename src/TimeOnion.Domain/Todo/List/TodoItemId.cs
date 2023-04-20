namespace TimeOnion.Domain.Todo.List;

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());
}