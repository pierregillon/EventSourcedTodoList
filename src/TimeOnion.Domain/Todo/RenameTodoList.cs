using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Domain.Todo.List.Events;

namespace TimeOnion.Domain.Todo;

public record RenameTodoListCommand(TodoListId Id, TodoListName NewName) : ICommand;

internal class RenameTodoListCommandHandler : ICommandHandler<RenameTodoListCommand>
{
    private readonly ITodoListRepository _repository;

    public RenameTodoListCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(RenameTodoListCommand command)
    {
        var todoList = await _repository.Get(command.Id);

        todoList.Rename(command.NewName);

        await _repository.Save(todoList);
    }
}