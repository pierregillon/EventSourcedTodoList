namespace TimeOnion.Domain.Todo.List;

public record TodoListName(string Value)
{
    public string Value { get; } = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("A todo list name cannot be null or empty")
        : Value;
}