using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Projections;

namespace TimeOnion.Domain.UserManagement;

public record GetUserInformationQuery : IQuery<UserInformation>;

internal record GetUserInformationQueryHandler(IReadModelDatabase Database, IUserContextProvider UserContextProvider) : IQueryHandler<GetUserInformationQuery, UserInformation>
{
    public async Task<UserInformation> Handle(GetUserInformationQuery query)
    {
        var userInformation =
            await Database.Find<UserProjectItem>(item => item.UserId == UserContextProvider.GetUserContext().UserId);

        if (userInformation is null)
        {
            throw new InvalidOperationException("Unknown current user");
        }

        return new UserInformation(userInformation.EmailAddress, userInformation.EmailAddress.Value.ToAcronym());
    }
}

public record UserInformation(string EmailAddress, string UserAcronym);