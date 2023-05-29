using MediatR;

namespace TimeOnion.Domain.BuildingBlocks;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}