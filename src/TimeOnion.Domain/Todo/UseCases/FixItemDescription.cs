using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record FixItemDescriptionCommand(
    TodoListId TodoListId,
    TodoItemId TodoItemId,
    TodoItemDescription NewItemDescription
) : ICommand;

public class FixItemDescriptionCommandHandler : ICommandHandler<FixItemDescriptionCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public FixItemDescriptionCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(FixItemDescriptionCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.FixItemDescription(command.TodoItemId, command.NewItemDescription);

        await _repository.Save(todoList);
    }
}