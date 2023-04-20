using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record RescheduleTodoItemCommand(
    TodoListId TodoListId,
    TodoItemId TodoItemId,
    TimeHorizons AnotherTimeHorizons
) : ICommand;

internal class RescheduleTodoItemCommandHandler : ICommandHandler<RescheduleTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public RescheduleTodoItemCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(RescheduleTodoItemCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.Reschedule(command.TodoItemId, command.AnotherTimeHorizons);

        await _repository.Save(todoList);
    }
}