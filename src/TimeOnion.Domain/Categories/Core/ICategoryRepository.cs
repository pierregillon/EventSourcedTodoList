namespace TimeOnion.Domain.Categories.Core;

public interface ICategoryRepository
{
    Task Save(Category category);
    Task<Category> Get(CategoryId commandCategoryId);
}