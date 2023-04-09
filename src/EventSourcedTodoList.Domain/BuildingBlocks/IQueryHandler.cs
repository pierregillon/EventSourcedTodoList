using MediatR;

namespace EventSourcedTodoList.Domain.BuildingBlocks;

internal interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> IRequestHandler<TQuery, TResult>.Handle(TQuery request, CancellationToken _)
    {
        return Handle(request);
    }

    Task<TResult> Handle(TQuery query);
}