using MediatR;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Configuration.Blazor;

public interface IActionDispatcher
{
    Task Dispatch(IAction action);
}

internal class MediatorActionDispatcher : IActionDispatcher
{
    private readonly IMediator _mediator;

    public MediatorActionDispatcher(IMediator mediator) => _mediator = mediator;

    public Task Dispatch(IAction action) => _mediator.Send(action);
}