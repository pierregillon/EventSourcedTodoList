using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Domain.UserManagement.Projections;

namespace TimeOnion.Domain.UserManagement;

public record UserExistsQuery(UserId UserId) : IQuery<bool>, IAnonymousQuery;

internal record UserExistsQueryHandler(
    IReadModelDatabase Database
) : IQueryHandler<UserExistsQuery, bool>, IAnonymousQuery
{
    public async Task<bool> Handle(UserExistsQuery query) =>
        await Database.Find<UserProjectItem>(x => x.UserId == query.UserId) is not null;
}