namespace TimeOnion.Domain.Todo.List;

public record TodoItemDescription(string? Value)
{
    public string Value { get; } = !string.IsNullOrWhiteSpace(Value)
        ? Value
        : throw new ArgumentException("An item to do must have a description");
}