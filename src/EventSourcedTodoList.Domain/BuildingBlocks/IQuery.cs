using MediatR;

namespace EventSourcedTodoList.Domain.BuildingBlocks;

public interface IQuery<out TResult> : IRequest<TResult>
{
}