using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record AddItemToDoCommand(string? Description, Temporality Temporality) : ICommand;

internal class AddItemToDoCommandHandler : ICommandHandler<AddItemToDoCommand>
{
    private readonly ITodoListRepository _repository;

    public AddItemToDoCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(AddItemToDoCommand command)
    {
        var todoList = await _repository.Get();

        todoList.AddItem(new ItemDescription(command.Description), command.Temporality);

        await _repository.Save(todoList);
    }
}