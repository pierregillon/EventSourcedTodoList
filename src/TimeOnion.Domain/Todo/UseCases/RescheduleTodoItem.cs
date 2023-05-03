using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record RescheduleTodoItemCommand(
    TodoListId TodoListId,
    TodoItemId TodoItemId,
    TimeHorizons AnotherTimeHorizons
) : ICommand;

internal class RescheduleTodoItemCommandHandler : ICommandHandler<RescheduleTodoItemCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public RescheduleTodoItemCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(RescheduleTodoItemCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.Reschedule(command.TodoItemId, command.AnotherTimeHorizons);

        await _repository.Save(todoList);
    }
}