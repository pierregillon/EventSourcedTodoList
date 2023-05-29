using MediatR;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Configuration.Blazor;

public interface IActionDispatcher
{
    // Task Dispatch<TAction>(TAction action) where TAction : IAction;
    Task Dispatch<TState>(IAction<TState> action) where TState : IState;
}

internal class MediatorActionDispatcher : IActionDispatcher
{
    private readonly IMediator _mediator;

    public MediatorActionDispatcher(IMediator mediator) => _mediator = mediator;

    public Task Dispatch<TAction>(TAction action) where TAction : IAction => _mediator.Send(action);
    public Task Dispatch<TState>(IAction<TState> action) where TState : IState => _mediator.Send(action);
}