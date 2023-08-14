using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Domain.UserManagement.Projections;

namespace TimeOnion.Domain.UserManagement;

public record GetUserLoginDetailsQuery(
    EmailAddress Email,
    UnverifiedPassword Password
) : IQuery<LoginDetailsDto?>, IAnonymousQuery;

internal record GetUserLoginDetailsQueryHandler(IReadModelDatabase Database, IPasswordHasher PasswordHasher)
    : IQueryHandler<GetUserLoginDetailsQuery, LoginDetailsDto?>
{
    public async Task<LoginDetailsDto?> Handle(GetUserLoginDetailsQuery query)
    {
        var entity = (await Database.GetAll<UserProjectItem>())
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

        return new LoginDetailsDto(
            entity.UserId,
            entity.EmailAddress,
            PasswordHasher.VerifyHash(query.Password, entity.PasswordHash)
        );
    }
}

public record LoginDetailsDto(UserId UserId, EmailAddress Email, bool IsPasswordValid);