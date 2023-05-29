using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement;

internal interface IUserFinder
{
    Task<bool> IsAnyRegisteredUserWithEmailAddress(EmailAddress emailAddress);
}