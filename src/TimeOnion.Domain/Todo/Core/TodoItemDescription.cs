namespace TimeOnion.Domain.Todo.Core;

public record TodoItemDescription(string? Value)
{
    public string Value { get; } = !string.IsNullOrWhiteSpace(Value)
        ? Value.Trim()
        : throw new ArgumentException("An item to do must have a description");
}