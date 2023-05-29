using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Actions;

public record InitializeLoginState(string RedirectUrl) : IAction<LoginState>;

internal record InitializeLoginStateHandler(IStore Store) : ISyncActionApplier<InitializeLoginState, LoginState>
{
    public LoginState Apply(InitializeLoginState action, LoginState state) => LoginState.Initialize() with
    {
        RedirectUrl = action.RedirectUrl
    };
}