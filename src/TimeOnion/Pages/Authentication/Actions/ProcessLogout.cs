using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Actions;

internal record LogoutAction : IAction;

internal record LogoutActionHandler(
    IStore Store,
    ILocalStorageService LocalStorage,
    NavigationManager NavigationManager
) : IActionHandler<LogoutAction>
{
    public async Task Handle(LogoutAction action)
    {
        await LocalStorage.RemoveToken();

        NavigationManager.NavigateTo("/login", true);
    }
}