using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Shared.MVU;

public abstract class ActionHandlerBase<TState, TAction> : StateActionHandler<TState, TAction>
    where TState : IState
    where TAction : IAction<TState>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    protected ActionHandlerBase(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    protected Task Dispatch(ICommand command) => _commandDispatcher.Dispatch(command);
    protected Task<TResult> Dispatch<TResult>(IQuery<TResult> command) => _queryDispatcher.Dispatch(command);
}