using System.Net.Mail;
using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

public record EmailAddress
{
    public string Value { get; }
    
    public EmailAddress(string value)
    {
        if (!MailAddress.TryCreate(value, out var mail))
        {
            throw new BadEmailFormatException();
        }

        Value = mail.Address;
    }

    public static implicit operator string(EmailAddress password) => password.Value;
}

public class BadEmailFormatException : DomainException
{
    public BadEmailFormatException() : base("The provided email has an invalid format")
    {
    }
}