using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.UserInformation;

public record UserInformationState(string EmailAddress, string UserAcronym) : IState
{
    public static UserInformationState Initialize() => new(string.Empty, string.Empty);

    public record Load : IAction<UserInformationState>
    {
        public record Handler(IStore Store, IQueryDispatcher QueryDispatcher) : IActionApplier<Load, UserInformationState>
        {
            public async Task<UserInformationState> Apply(Load action, UserInformationState state)
            {
                var userInformation = await QueryDispatcher.Dispatch(new GetUserInformationQuery());

                return state with
                {
                    EmailAddress = userInformation.EmailAddress,
                    UserAcronym = userInformation.UserAcronym
                };
            }
        }
    }

    internal record Logout : IAction
    {
        internal record Handler(
            ILocalStorageService LocalStorage,
            NavigationManager NavigationManager
        ) : IActionHandler<Logout>
        {
            public async Task Handle(Logout action)
            {
                await LocalStorage.RemoveToken();
                NavigationManager.NavigateTo("/login", true);
            }
        }
    }
}