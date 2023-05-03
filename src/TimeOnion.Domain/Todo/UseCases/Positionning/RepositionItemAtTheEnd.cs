using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Positionning;

public record RepositionItemAtTheEndCommand(TodoListId ListId, TodoItemId ItemId) : ICommand;

internal class RepositionItemAtTheEndCommandHandler : ICommandHandler<RepositionItemAtTheEndCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public RepositionItemAtTheEndCommandHandler(IRepository<TodoList, TodoListId> repository) =>
        _repository = repository;

    public async Task Handle(RepositionItemAtTheEndCommand command)
    {
        var todoList = await _repository.Get(command.ListId);

        todoList.RepositionItemAtTheEnd(command.ItemId);

        await _repository.Save(todoList);
    }
}