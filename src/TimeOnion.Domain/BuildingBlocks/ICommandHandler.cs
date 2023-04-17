using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

internal interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
    Task IRequestHandler<TCommand>.Handle(TCommand request, CancellationToken _) => Handle(request);

    Task Handle(TCommand command);
}