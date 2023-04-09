using MediatR;

namespace EventSourcedTodoList.Domain.BuildingBlocks;

public class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher, IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public MediatorDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand
    {
        return _mediator.Send(command);
    }

    public Task Publish(IDomainEvent domainEvent)
    {
        return _mediator.Publish(domainEvent);
    }

    public Task<TResult> Dispatch<TResult>(IQuery<TResult> query)
    {
        return _mediator.Send(query);
    }
}