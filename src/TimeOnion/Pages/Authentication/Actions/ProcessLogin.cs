using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Actions;

public record LoginAction(string Login, string Password) : IAction<LoginState>;

internal record LoginActionHandler(
    IStore Store,
    Authenticator Authenticator,
    ILocalStorageService LocalStorage,
    NavigationManager NavigationManager
) : IActionHandler<LoginAction, LoginState>
{
    public async Task Handle(LoginAction action, LoginState state)
    {
        var token = await Authenticator.Authenticate(action.Login, action.Password);

        await LocalStorage.SetToken(token);

        NavigationManager.NavigateTo(state.RedirectUrl, true);
    }
}