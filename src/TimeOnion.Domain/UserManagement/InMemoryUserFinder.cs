using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Domain.UserManagement.Projections;

namespace TimeOnion.Domain.UserManagement;

internal record InMemoryUserFinder(IReadModelDatabase Database) : IUserFinder
{
    public async Task<bool> IsAnyRegisteredUserWithEmailAddress(EmailAddress emailAddress) =>
        (await Database.GetAll<UserProjectItem>())
        .Any(x => x.EmailAddress == emailAddress);
}