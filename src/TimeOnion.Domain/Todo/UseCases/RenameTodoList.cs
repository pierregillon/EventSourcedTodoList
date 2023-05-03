using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record RenameTodoListCommand(TodoListId Id, TodoListName NewName) : ICommand;

internal class RenameTodoListCommandHandler : ICommandHandler<RenameTodoListCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public RenameTodoListCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(RenameTodoListCommand command)
    {
        var todoList = await _repository.Get(command.Id);

        todoList.Rename(command.NewName);

        await _repository.Save(todoList);
    }
}