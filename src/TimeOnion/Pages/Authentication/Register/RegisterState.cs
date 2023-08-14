using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Register;

public record RegisterState(
    string? Email,
    string? Password,
    string? PasswordRepeat
) : IState
{
    public static RegisterState Initialize() => new(null, null, null);

    public record InitializeRegisterAction : IAction<RegisterState>
    {
        internal record Handler(IStore Store) : ISyncActionApplier<InitializeRegisterAction, RegisterState>
        {
            public RegisterState Apply(InitializeRegisterAction action, RegisterState state) => Initialize();
        }
    }

    public record DefineRegisterEmailAction(string Email) : IAction<RegisterState>
    {
        internal record Handler(IStore Store) : ISyncActionApplier<DefineRegisterEmailAction, RegisterState>
        {
            public RegisterState Apply(DefineRegisterEmailAction action, RegisterState state) =>
                state with { Email = action.Email };
        }
    }

    public record DefineRegisterPasswordAction(string Password) : IAction<RegisterState>
    {
        internal record Handler(IStore Store) : ISyncActionApplier<DefineRegisterPasswordAction, RegisterState>
        {
            public RegisterState Apply(DefineRegisterPasswordAction action, RegisterState state) =>
                state with { Password = action.Password };
        }
    }

    public record DefineRegisterPasswordRepeatAction(string PasswordRepeat) : IAction<RegisterState>
    {
        internal record RegisterFieldsActionHandler(IStore Store)
            : ISyncActionApplier<DefineRegisterPasswordRepeatAction, RegisterState>
        {
            public RegisterState Apply(DefineRegisterPasswordRepeatAction action, RegisterState state) =>
                state with { PasswordRepeat = action.PasswordRepeat };
        }
    }

    public record RegisterAction(string Email, string Password) : IAction<RegisterState>
    {
        internal record Handler(
            IStore Store,
            Authenticator Authenticator,
            ILocalStorageService LocalStorage,
            NavigationManager NavigationManager
        ) : IActionHandler<RegisterAction, RegisterState>
        {
            public async Task Handle(RegisterAction action, RegisterState state)
            {
                var token = await Authenticator.Register(action.Email, action.Password);

                await LocalStorage.SetToken(token);

                NavigationManager.NavigateTo("/", true);
            }
        }
    }
}