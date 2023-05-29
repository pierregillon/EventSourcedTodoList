namespace TimeOnion.Domain.UserManagement.Core;

public record UnverifiedPassword(string Value)
{
    public static implicit operator string(UnverifiedPassword password) => password.Value;
}