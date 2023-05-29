using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement;

public record GetUserLoginDetailsQuery(string Email, string Password) : IQuery<LoginDetailsDto?>;

public record LoginDetailsDto(UserId UserId, string Email);

internal class GetUserLoginDetailsQueryHandler : IQueryHandler<GetUserLoginDetailsQuery, LoginDetailsDto?>
{
    public async Task<LoginDetailsDto?> Handle(GetUserLoginDetailsQuery query) => new(UserId.New(), query.Email);
}

public record UserId(Guid Value)
{
    public static UserId New() => new(Guid.NewGuid());
}