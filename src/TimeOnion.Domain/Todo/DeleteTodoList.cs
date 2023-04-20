using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record DeleteTodoListCommand(TodoListId Id) : ICommand;

internal class DeleteTodoListCommandHandler : ICommandHandler<DeleteTodoListCommand>
{
    private readonly ITodoListRepository _repository;

    public DeleteTodoListCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(DeleteTodoListCommand command)
    {
        var todoList = await _repository.Get(command.Id);

        todoList.Delete();

        await _repository.Save(todoList);
    }
}