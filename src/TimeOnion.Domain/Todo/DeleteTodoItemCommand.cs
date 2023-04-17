using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record DeleteTodoItemCommand(TodoItemId ItemId) : ICommand;

public class DeleteTodoItemCommandHandler : ICommandHandler<DeleteTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public DeleteTodoItemCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(DeleteTodoItemCommand command)
    {
        var todoList = await _repository.Get();

        todoList.Delete(command.ItemId);

        await _repository.Save(todoList);
    }
}