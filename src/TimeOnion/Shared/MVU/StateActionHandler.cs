namespace TimeOnion.Shared.MVU;

public abstract class StateActionHandler<TState, TAction> : IActionHandler<TAction, TState>
    where TState : IState
    where TAction : IAction<TState>
{
    private readonly IStore _store;

    protected StateActionHandler(IStore store) => _store = store;

    public async Task Handle(TAction action)
    {
        var state = _store.GetState<TState>();

        var newState = await Apply(state, action);

        _store.SetState(newState);
    }

    protected abstract Task<TState> Apply(TState state, TAction action);
}