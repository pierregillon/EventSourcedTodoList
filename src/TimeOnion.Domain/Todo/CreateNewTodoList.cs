using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Domain.Todo.List.Events;

namespace TimeOnion.Domain.Todo;

public record CreateNewTodoListCommand(TodoListName Name) : ICommand;

internal class CreateNewTodoListCommandHandler : ICommandHandler<CreateNewTodoListCommand>
{
    private readonly ITodoListRepository _repository;

    public CreateNewTodoListCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(CreateNewTodoListCommand command)
    {
        var todoList = TodoList.New(command.Name);

        await _repository.Save(todoList);
    }
}