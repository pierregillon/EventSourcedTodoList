using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Categories;

public record RenameCategoryCommand(CategoryId CategoryId, CategoryName NewName) : ICommand;

internal class RenameCategoryCommandHandler : ICommandHandler<RenameCategoryCommand>
{
    private readonly ICategoryRepository _repository;

    public RenameCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;

    public async Task Handle(RenameCategoryCommand command)
    {
        var category = await _repository.Get(command.CategoryId);

        category.Rename(command.NewName);

        await _repository.Save(category);
    }
}