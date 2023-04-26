using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Positionning;

public record RepositionItemAtTheEndCommand(TodoListId ListId, TodoItemId ItemId) : ICommand;

internal class RepositionItemAtTheEndCommandHandler : ICommandHandler<RepositionItemAtTheEndCommand>
{
    private readonly ITodoListRepository _repository;

    public RepositionItemAtTheEndCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(RepositionItemAtTheEndCommand command)
    {
        var todoList = await _repository.Get(command.ListId);

        todoList.RepositionItemAtTheEnd(command.ItemId);

        await _repository.Save(todoList);
    }
}