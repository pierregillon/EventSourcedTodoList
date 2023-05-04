using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record AddItemToDoCommand(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId,
    TodoItemId? AboveItemId
) : ICommand;

internal class AddItemToDoCommandHandler : ICommandHandler<AddItemToDoCommand>
{
    private readonly IRepository<TodoList, TodoListId> _todoListRepository;
    private readonly IRepository<Category, CategoryId> _categoryRepository;

    public AddItemToDoCommandHandler(
        IRepository<TodoList, TodoListId> todoListRepository,
        IRepository<Category, CategoryId> categoryRepository
    )
    {
        _todoListRepository = todoListRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(AddItemToDoCommand command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);
        Category? category = null;

        if (command.CategoryId is not null)
        {
            category = await _categoryRepository.Get(command.CategoryId);
        }

        todoList.AddItem(
            command.ItemId,
            command.Description,
            command.TimeHorizons,
            category,
            command.AboveItemId
        );

        await _todoListRepository.Save(todoList);
    }
}