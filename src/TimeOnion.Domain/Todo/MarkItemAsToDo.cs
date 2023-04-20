using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record MarkItemAsToDoCommand(TodoListId TodoListId, TodoItemId ItemId) : ICommand;

public class MarkItemAsToDoCommandHandler : ICommandHandler<MarkItemAsToDoCommand>
{
    private readonly ITodoListRepository _repository;

    public MarkItemAsToDoCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(MarkItemAsToDoCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.MarkItemAsToDo(command.ItemId);

        await _repository.Save(todoList);
    }
}