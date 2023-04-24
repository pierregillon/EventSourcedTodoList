using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Categories;

public record CreateNewCategory(CategoryName Name) : ICommand;

internal class CreateNewCategoryCommandHandler : ICommandHandler<CreateNewCategory>
{
    private readonly ICategoryRepository _repository;

    public CreateNewCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;

    public async Task Handle(CreateNewCategory command)
    {
        var category = Category.New(command.Name);

        await _repository.Save(category);
    }
}