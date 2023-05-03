using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record DeleteTodoItemCommand(TodoListId TodoListId, TodoItemId ItemId) : ICommand;

public class DeleteTodoItemCommandHandler : ICommandHandler<DeleteTodoItemCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public DeleteTodoItemCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(DeleteTodoItemCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.Delete(command.ItemId);

        await _repository.Save(todoList);
    }
}