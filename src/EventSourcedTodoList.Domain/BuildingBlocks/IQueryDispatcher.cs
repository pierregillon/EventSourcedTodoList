namespace EventSourcedTodoList.Domain.BuildingBlocks;

public interface IQueryDispatcher
{
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
}