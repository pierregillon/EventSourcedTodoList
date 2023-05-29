using System.Text.RegularExpressions;

namespace TimeOnion.Domain.UserManagement.Core;

public record Password
{
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }

        if (value.Length < 8)
        {
            throw new TooWeakPasswordException("Password must be at least of length 8",
                TooWeakPasswordException.Reasons.TooShort);
        }

        if (!Regex.IsMatch(value, @"[A-Z]"))
        {
            throw new TooWeakPasswordException("Password must contain at least one capital letter",
                TooWeakPasswordException.Reasons.MissingCapitalLetter);
        }

        if (!Regex.IsMatch(value, @"[a-z]"))
        {
            throw new TooWeakPasswordException("Password must contain at least one lowercase letter",
                TooWeakPasswordException.Reasons.MissingLowerLetter);
        }

        if (!Regex.IsMatch(value, @"[0-9]"))
        {
            throw new TooWeakPasswordException("Password must contain at least one digit",
                TooWeakPasswordException.Reasons.MissingDigit);
        }

        Value = value;
    }

    private string Value { get; }

    public static implicit operator string(Password password) => password.Value;
}