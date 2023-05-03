using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Positionning;

public record RepositionItemAboveAnotherCommand(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : ICommand;

internal class RepositionItemCommandHandler : ICommandHandler<RepositionItemAboveAnotherCommand>
{
    private readonly IRepository<TodoList, TodoListId> _repository;

    public RepositionItemCommandHandler(IRepository<TodoList, TodoListId> repository) => _repository = repository;

    public async Task Handle(RepositionItemAboveAnotherCommand command)
    {
        var todoList = await _repository.Get(command.ListId);

        todoList.RepositionItemAboveAnother(command.ItemId, command.ReferenceItemId);

        await _repository.Save(todoList);
    }
}