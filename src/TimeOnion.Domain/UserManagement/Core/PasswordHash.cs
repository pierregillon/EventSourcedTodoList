namespace TimeOnion.Domain.UserManagement.Core;

public record PasswordHash(string Value)
{
    public string Value { get; init; } = !string.IsNullOrWhiteSpace(Value)
        ? Value
        : throw new ArgumentException("Value cannot be null or whitespace.");

    public static PasswordHash Create(string value) => new(value);

    public static implicit operator string(PasswordHash password) => password.Value;
}