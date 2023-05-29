using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher, IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public MediatorDispatcher(IMediator mediator) => _mediator = mediator;

    public Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand => _mediator.Send(command);

    public Task<TResult> Dispatch<TResult>(ICommand<TResult> command) => _mediator.Send(command);

    public Task Publish(IDomainEvent domainEvent) => _mediator.Publish(domainEvent);

    public Task<TResult> Dispatch<TResult>(IQuery<TResult> query) => _mediator.Send(query);
}