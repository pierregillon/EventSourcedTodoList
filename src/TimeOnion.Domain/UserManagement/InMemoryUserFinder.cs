using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement;

internal record InMemoryUserFinder(IReadModelDatabase Database) : IUserFinder
{
    public async Task<bool> IsAnyRegisteredUserWithEmailAddress(EmailAddress emailAddress) =>
        (await Database.GetAll<UserListItem>())
        .Any(x => x.EmailAddress == emailAddress);
}