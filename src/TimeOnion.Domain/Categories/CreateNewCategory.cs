using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories;

public record CreateNewCategoryCommand(CategoryName Name, TodoListId ListId) : ICommand;

internal class CreateNewCategoryCommandHandler : ICommandHandler<CreateNewCategoryCommand>
{
    private readonly IRepository<Category, CategoryId> _categoryRepository;
    private readonly IRepository<TodoList, TodoListId> _todoListRepository;

    public CreateNewCategoryCommandHandler(
        IRepository<Category, CategoryId> categoryRepository,
        IRepository<TodoList, TodoListId> todoListRepository
    )
    {
        _categoryRepository = categoryRepository;
        _todoListRepository = todoListRepository;
    }

    public async Task Handle(CreateNewCategoryCommand command)
    {
        var todoList = await _todoListRepository.Get(command.ListId);

        var category = todoList.NewCategory(command.Name);

        await _categoryRepository.Save(category);
    }
}