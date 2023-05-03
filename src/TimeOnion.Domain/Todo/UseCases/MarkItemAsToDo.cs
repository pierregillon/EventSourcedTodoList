using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record MarkItemAsToDoCommand(TodoListId TodoListId, TodoItemId ItemId) : ICommand;

public class MarkItemAsToDoCommandHandler : ICommandHandler<MarkItemAsToDoCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public MarkItemAsToDoCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(MarkItemAsToDoCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.MarkItemAsToDo(command.ItemId);

        await _repository.Save(todoList);
    }
}