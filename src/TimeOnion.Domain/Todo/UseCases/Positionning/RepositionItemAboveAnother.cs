using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Positionning;

public record RepositionItemAboveAnotherCommand(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : ICommand;

internal class RepositionItemCommandHandler : ICommandHandler<RepositionItemAboveAnotherCommand>
{
    private readonly ITodoListRepository _repository;

    public RepositionItemCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(RepositionItemAboveAnotherCommand command)
    {
        var todoList = await _repository.Get(command.ListId);

        todoList.RepositionItemAboveAnother(command.ItemId, command.ReferenceItemId);

        await _repository.Save(todoList);
    }
}