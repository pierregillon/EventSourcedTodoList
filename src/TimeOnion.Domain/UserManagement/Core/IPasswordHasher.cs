namespace TimeOnion.Domain.UserManagement.Core;

public interface IPasswordHasher
{
    PasswordHash Hash(Password password);
    bool VerifyHash(UnverifiedPassword password, PasswordHash passwordHash);
}