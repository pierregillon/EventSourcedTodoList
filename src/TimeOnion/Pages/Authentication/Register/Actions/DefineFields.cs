using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Register.Actions;

public record DefineRegisterEmailAction(string Email) : IAction<RegisterState>;

public record DefineRegisterPasswordAction(string Password) : IAction<RegisterState>;

public record DefineRegisterPasswordRepeatAction(string PasswordRepeat) : IAction<RegisterState>;

internal record RegisterFieldsActionHandler(IStore Store) :
    ISyncActionApplier<DefineRegisterEmailAction, RegisterState>,
    ISyncActionApplier<DefineRegisterPasswordAction, RegisterState>,
    ISyncActionApplier<DefineRegisterPasswordRepeatAction, RegisterState>
{
    public RegisterState Apply(DefineRegisterEmailAction action, RegisterState state) =>
        state with { Email = action.Email };

    public RegisterState Apply(DefineRegisterPasswordAction action, RegisterState state) =>
        state with { Password = action.Password };

    public RegisterState Apply(DefineRegisterPasswordRepeatAction action, RegisterState state) =>
        state with { PasswordRepeat = action.PasswordRepeat };
}