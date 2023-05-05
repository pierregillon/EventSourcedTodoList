using MediatR;

namespace TimeOnion.Shared.MVU;

public interface IActionHandler<in TAction> : IRequestHandler<TAction> where TAction : IAction
{
    Task IRequestHandler<TAction>.Handle(TAction request, CancellationToken _) => Handle(request);

    Task Handle(TAction action);
}