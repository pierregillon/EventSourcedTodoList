using MediatR;

namespace TimeOnion.Shared.MVU;

public interface IAction : IRequest
{
}

public interface IAction<TState> : IAction where TState : IState
{
}