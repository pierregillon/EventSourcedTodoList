using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record AddItemToDoCommand(string? Description) : ICommand;

internal class AddItemToDoCommandHandler : ICommandHandler<AddItemToDoCommand>
{
    private readonly ITodoListRepository _repository;

    public AddItemToDoCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddItemToDoCommand command)
    {
        var todoList = await _repository.Get();

        todoList.AddItem(new ItemDescription(command.Description));

        await _repository.Save(todoList);
    }
}