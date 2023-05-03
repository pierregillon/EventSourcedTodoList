using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record CreateNewTodoListCommand(TodoListName Name) : ICommand;

internal class CreateNewTodoListCommandHandler : ICommandHandler<CreateNewTodoListCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public CreateNewTodoListCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(CreateNewTodoListCommand command)
    {
        var todoList = TodoList.New(command.Name);

        await _repository.Save(todoList);
    }
}