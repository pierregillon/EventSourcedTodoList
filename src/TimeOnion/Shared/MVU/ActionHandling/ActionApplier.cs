using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Shared.MVU.ActionHandling;

public abstract class ActionApplier<TAction, TState> : IActionApplier<TAction, TState>
    where TState : IState
    where TAction : IAction<TState>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public IStore Store { get; }

    protected ActionApplier(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    )
    {
        Store = store;
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    protected Task Dispatch(ICommand command) => _commandDispatcher.Dispatch(command);
    protected Task<TResult> Dispatch<TResult>(IQuery<TResult> command) => _queryDispatcher.Dispatch(command);

    protected abstract Task<TState> Apply(TAction action, TState state);

    Task<TState> IActionApplier<TAction, TState>.Apply(TAction action, TState state) => Apply(action, state);
}