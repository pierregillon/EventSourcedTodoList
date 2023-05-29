using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

internal class User : EventSourcedAggregate<UserId>
{
    private User(UserId id) : base(id)
    {
    }

    public static User Register(EmailAddress emailAddress, PasswordHash passwordHash)
    {
        var userId = UserId.New();
        var user = new User(userId);
        user.StoreEvent(new UserRegistered(userId, emailAddress, passwordHash));
        return user;
    }
}