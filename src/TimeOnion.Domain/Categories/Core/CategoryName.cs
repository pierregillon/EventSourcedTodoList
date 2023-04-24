namespace TimeOnion.Domain.Categories.Core;

public record CategoryName(string Value)
{
    public string Value { get; } = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("A category name must not be null or whitespace.")
        : Value;
}