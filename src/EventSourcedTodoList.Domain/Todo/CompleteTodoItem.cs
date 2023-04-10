using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record MarkItemAsDoneCommand(TodoItemId ItemId) : ICommand;

internal class MarkItemAsDoneCommandHandler : ICommandHandler<MarkItemAsDoneCommand>
{
    private readonly ITodoListRepository _repository;

    public MarkItemAsDoneCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(MarkItemAsDoneCommand command)
    {
        var todoList = await _repository.Get();

        todoList.MarkItemAsDone(command.ItemId);

        await _repository.Save(todoList);
    }
}