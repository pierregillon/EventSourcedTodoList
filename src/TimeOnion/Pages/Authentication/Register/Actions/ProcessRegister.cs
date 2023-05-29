using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Register.Actions;

public record RegisterAction(string Email, string Password) : IAction<RegisterState>;

public record StartRegisterLoading : IAction<RegisterState>;

public record StopRegisterLoading : IAction<RegisterState>;

internal record RegisterActionHandler(
    IStore Store,
    IMediator Mediator,
    Authenticator Authenticator,
    ILocalStorageService LocalStorage,
    NavigationManager NavigationManager
) : IActionHandler<RegisterAction, RegisterState>,
    ISyncActionApplier<StartRegisterLoading, RegisterState>,
    ISyncActionApplier<StopRegisterLoading, RegisterState>
{
    public async Task Handle(RegisterAction action, RegisterState state)
    {
        await Mediator.Send(new StartRegisterLoading());
        try
        {
            var token = await Authenticator.Register(action.Email, action.Password);

            await LocalStorage.SetToken(token);

            NavigationManager.NavigateTo("/", true);
        }
        finally
        {
            await Mediator.Send(new StopRegisterLoading());
        }
    }

    public RegisterState Apply(StartRegisterLoading action, RegisterState state) => state with { IsLoading = true };

    public RegisterState Apply(StopRegisterLoading action, RegisterState state) => state with { IsLoading = false };
}