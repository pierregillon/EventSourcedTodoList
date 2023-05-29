using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Login.Actions;

public record DefineEmailAction(string Email) : IAction<LoginState>;

internal record DefineEmailActionHandler(IStore Store) : ISyncActionApplier<DefineEmailAction, LoginState>
{
    public LoginState Apply(DefineEmailAction action, LoginState state) => state with { Email = action.Email };
}

public record DefinePasswordAction(string Password) : IAction<LoginState>;

internal record DefinePasswordActionHandler(IStore Store) : ISyncActionApplier<DefinePasswordAction, LoginState>
{
    public LoginState Apply(DefinePasswordAction action, LoginState state) => state with { Password = action.Password };
}