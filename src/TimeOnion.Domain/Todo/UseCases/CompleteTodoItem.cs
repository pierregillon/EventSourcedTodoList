using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record MarkItemAsDoneCommand(TodoListId TodoListId, TodoItemId ItemId) : ICommand;

internal class MarkItemAsDoneCommandHandler : ICommandHandler<MarkItemAsDoneCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public MarkItemAsDoneCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(MarkItemAsDoneCommand command)
    {
        var todoList = await _repository.Get(command.TodoListId);

        todoList.MarkItemAsDone(command.ItemId);

        await _repository.Save(todoList);
    }
}