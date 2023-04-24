using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Categorization;

public record DecategorizeTodoItemCommand(TodoListId ListId, TodoItemId ItemId) : ICommand;

internal class DecategorizeTodoItemCommandHandler : ICommandHandler<DecategorizeTodoItemCommand>
{
    private readonly ITodoListRepository _todoListRepository;

    public DecategorizeTodoItemCommandHandler(ITodoListRepository todoListRepository) =>
        _todoListRepository = todoListRepository;

    public async Task Handle(DecategorizeTodoItemCommand command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);

        todoList.DecategorizeItem(command.ItemId);

        await _todoListRepository.Save(todoList);
    }
}