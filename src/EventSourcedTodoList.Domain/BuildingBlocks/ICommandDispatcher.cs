﻿namespace EventSourcedTodoList.Domain.BuildingBlocks;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
}