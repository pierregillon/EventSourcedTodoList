using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

public class TooWeakPasswordException : DomainException
{
    public readonly Reasons Reason;

    public enum Reasons
    {
        TooShort,
        MissingCapitalLetter,
        MissingLowerLetter,
        MissingDigit
    }

    public TooWeakPasswordException(string message, Reasons reason) : base(message) => Reason = reason;
}