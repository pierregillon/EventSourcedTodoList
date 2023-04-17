using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IQuery<out TResult> : IRequest<TResult>
{
}