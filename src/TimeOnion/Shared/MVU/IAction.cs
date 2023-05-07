using MediatR;

namespace TimeOnion.Shared.MVU;

public interface IAction : IRequest
{
}

public interface IAction<TState> : IAction where TState : IState
{
}

public interface IActionOnScopedState : IAction
{
    object Scope { get; }
}