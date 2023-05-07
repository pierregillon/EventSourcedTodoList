namespace TimeOnion.Shared.MVU.ActionHandling;

public interface IActionApplier<in TAction, TState> : IActionHandler<TAction, TState>
    where TState : IState
    where TAction : IAction<TState>
{
    public IStore Store { get; }

    public Task<TState> Apply(TAction action, TState state);

    async Task IActionHandler<TAction, TState>.Handle(TAction action)
    {
        if (action is IActionOnScopedState actionOnScopedState)
        {
            var state = Store.GetState<TState>(actionOnScopedState.Scope);

            var newState = await Apply(action, state);

            Store.SetState(newState, actionOnScopedState.Scope);
        }
        else
        {
            var state = Store.GetState<TState>();

            var newState = await Apply(action, state);

            Store.SetState(newState);
        }
    }
}