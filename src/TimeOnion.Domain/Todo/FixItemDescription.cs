using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record FixItemDescriptionCommand(
    TodoListId TodoListId,
    TodoItemId TodoItemId,
    TodoItemDescription NewItemDescription
) : ICommand;

public class FixItemDescriptionCommandHandler : ICommandHandler<FixItemDescriptionCommand>
{
    private readonly ITodoListRepository _repository;

    public FixItemDescriptionCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(FixItemDescriptionCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.FixItemDescription(command.TodoItemId, command.NewItemDescription);

        await _repository.Save(todoList);
    }
}