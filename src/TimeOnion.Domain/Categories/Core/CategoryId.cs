namespace TimeOnion.Domain.Categories.Core;

public record CategoryId(Guid Value)
{
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId? None => null;

    public static CategoryId From(string value) => new(Guid.Parse(value));
}