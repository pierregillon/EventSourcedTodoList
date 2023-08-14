using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement;

public record RegisterUserCommand(
    EmailAddress EmailAddress,
    Password Password
) : ICommand<UserId>, IAnonymousCommand;

internal record RegisterUserCommandHandler(
    IPasswordHasher PasswordHasher,
    IRepository<User, UserId> Repository,
    IUserFinder UserFinder
) : ICommandHandler<RegisterUserCommand, UserId>
{
    public async Task<UserId> Handle(RegisterUserCommand command)
    {
        await CheckEmailAddressNotAlreadyUsed(command.EmailAddress);

        var passwordHash = PasswordHasher.Hash(command.Password);

        var user = User.Register(command.EmailAddress, passwordHash);

        await Repository.Save(user);

        return user.Id;
    }

    private async Task CheckEmailAddressNotAlreadyUsed(EmailAddress emailAddress)
    {
        if (await UserFinder.IsAnyRegisteredUserWithEmailAddress(emailAddress))
        {
            throw new EmailAddressAlreadyUsedByAnotherUserException();
        }
    }
}

internal class EmailAddressAlreadyUsedByAnotherUserException : DomainException
{
    public EmailAddressAlreadyUsedByAnotherUserException() : base("The email is already taken by another user.")
    {
    }
}