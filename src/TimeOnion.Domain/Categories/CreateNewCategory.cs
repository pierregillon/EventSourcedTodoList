using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories;

public record CreateNewCategory(CategoryName Name, TodoListId ListId) : ICommand;

internal class CreateNewCategoryCommandHandler : ICommandHandler<CreateNewCategory>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ITodoListRepository _todoListRepository;

    public CreateNewCategoryCommandHandler(
        ICategoryRepository categoryRepository,
        ITodoListRepository todoListRepository
    )
    {
        _categoryRepository = categoryRepository;
        _todoListRepository = todoListRepository;
    }

    public async Task Handle(CreateNewCategory command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);

        var category = todoList.NewCategory(command.Name);

        await _categoryRepository.Save(category);
    }
}