using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.Authentication.Register.Actions;

public record InitializeRegisterAction : IAction<RegisterState>;

internal record InitializeRegisterActionHandler
    (IStore Store) : ISyncActionApplier<InitializeRegisterAction, RegisterState>
{
    public RegisterState Apply(InitializeRegisterAction action, RegisterState state) => RegisterState.Initialize();
}