using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases.Categorization;

public record CategorizeTodoItemCommand(TodoListId ListId, TodoItemId ItemId, CategoryId CategoryId) : ICommand;

internal class CategorizeTodoItemCommandHandler : ICommandHandler<CategorizeTodoItemCommand>
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CategorizeTodoItemCommandHandler(
        ITodoListRepository todoListRepository,
        ICategoryRepository categoryRepository
    )
    {
        _todoListRepository = todoListRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(CategorizeTodoItemCommand command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);
        var category = await _categoryRepository.Get(command.CategoryId);

        todoList.CategorizeItem(command.ItemId, category);

        await _todoListRepository.Save(todoList);
    }
}