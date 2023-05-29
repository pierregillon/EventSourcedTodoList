using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement;

public record GetUserLoginDetailsQuery(EmailAddress Email, UnverifiedPassword Password) : IQuery<LoginDetailsDto?>;

public record LoginDetailsDto(UserId UserId, EmailAddress Email);

internal record GetUserLoginDetailsQueryHandler(IReadModelDatabase Database, IPasswordHasher PasswordHasher)
    : IQueryHandler<GetUserLoginDetailsQuery, LoginDetailsDto?>
{
    public async Task<LoginDetailsDto?> Handle(GetUserLoginDetailsQuery query)
    {
        var entity = (await Database.GetAll<UserListItem>())
            .Where(x => x.EmailAddress == query.Email)
            .Select(x => new
            {
                PasswordHash = PasswordHash.Create(x.PasswordHash),
                x.EmailAddress,
                x.UserId
            })
            .FirstOrDefault();

        if (entity is null)
        {
            return null;
        }

        var isPasswordValid = PasswordHasher.VerifyHash(query.Password, entity.PasswordHash);

        return new LoginDetailsDto(
            entity.UserId,
            entity.EmailAddress
        );
    }
}