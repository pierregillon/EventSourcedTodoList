using MediatR;

namespace TimeOnion.Shared.MVU;

public interface IActionHandler<in TAction, TState> : IRequestHandler<TAction>
    where TState : IState
    where TAction : IAction<TState>
{
    Task IRequestHandler<TAction>.Handle(TAction request, CancellationToken _) => Handle(request);

    Task Handle(TAction action);
}