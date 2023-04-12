using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record MarkItemAsToDoCommand(TodoItemId ItemId) : ICommand;

public class MarkItemAsToDoCommandHandler : ICommandHandler<MarkItemAsToDoCommand>
{
    private readonly ITodoListRepository _repository;

    public MarkItemAsToDoCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(MarkItemAsToDoCommand command)
    {
        var todoList = await _repository.Get();

        todoList.MarkItemAsToDo(command.ItemId);

        await _repository.Save(todoList);
    }
}