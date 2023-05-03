using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record AddItemToDoCommand(
    TodoListId TodoListId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizons
) : ICommand;

internal class AddItemToDoCommandHandler : ICommandHandler<AddItemToDoCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public AddItemToDoCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(AddItemToDoCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.AddItem(command.Description, command.TimeHorizons);

        await _repository.Save(todoList);
    }
}