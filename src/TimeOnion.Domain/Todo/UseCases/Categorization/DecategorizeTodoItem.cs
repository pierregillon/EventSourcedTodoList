using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Categorization;

public record DecategorizeTodoItemCommand(TodoListId ListId, TodoItemId ItemId) : ICommand;

internal class DecategorizeTodoItemCommandHandler : ICommandHandler<DecategorizeTodoItemCommand>
{
    private readonly IRepository<TodoList, TodoListId> _todoListRepository;

    public DecategorizeTodoItemCommandHandler(IRepository<TodoList, TodoListId> todoListRepository) =>
        _todoListRepository = todoListRepository;

    public async Task Handle(DecategorizeTodoItemCommand command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);

        todoList.DecategorizeItem(command.ItemId);

        await _todoListRepository.Save(todoList);
    }
}