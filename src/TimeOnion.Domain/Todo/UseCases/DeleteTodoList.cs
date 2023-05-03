using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record DeleteTodoListCommand(TodoListId Id) : ICommand;

internal class DeleteTodoListCommandHandler : ICommandHandler<DeleteTodoListCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public DeleteTodoListCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(DeleteTodoListCommand command)
    {
        var todoList = await _repository.Get(command.Id);

        todoList.Delete();

        await _repository.Save(todoList);
    }
}