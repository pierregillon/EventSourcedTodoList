namespace TimeOnion.Domain.UserManagement.Core;

internal interface IUserFinder
{
    Task<bool> IsAnyRegisteredUserWithEmailAddress(EmailAddress emailAddress);
}