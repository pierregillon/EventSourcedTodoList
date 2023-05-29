namespace TimeOnion.Domain.BuildingBlocks;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command);
}