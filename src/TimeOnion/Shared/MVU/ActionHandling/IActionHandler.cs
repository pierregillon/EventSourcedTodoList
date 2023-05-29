using MediatR;

namespace TimeOnion.Shared.MVU.ActionHandling;

public interface IActionHandler<in TAction, TState> : IRequestHandler<TAction>
    where TState : IState
    where TAction : IAction<TState>
{
    Task IRequestHandler<TAction>.Handle(TAction request, CancellationToken _)
    {
        var state = Store.GetState<TState>();
        return Handle(request, state);
    }

    public IStore Store { get; }

    Task Handle(TAction action, TState state);
}

public interface IActionHandler<in TAction> : IRequestHandler<TAction>
    where TAction : IAction
{
    Task IRequestHandler<TAction>.Handle(TAction request, CancellationToken _) => Handle(request);

    Task Handle(TAction action);
}