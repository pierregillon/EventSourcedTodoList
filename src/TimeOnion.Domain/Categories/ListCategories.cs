using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Categories;

public record ListCategoriesQuery(TodoListId ListId) : IQuery<IReadOnlyCollection<CategoryListItem>>;

public record ListCategoriesQueryHandler(IUserScopedReadModelDatabase Database) :
    IQueryHandler<ListCategoriesQuery, IReadOnlyCollection<CategoryListItem>>
{
    public async Task<IReadOnlyCollection<CategoryListItem>> Handle(ListCategoriesQuery query) =>
        (await Database.GetAll<CategoryProjectionItem>())
        .Where(x => x.ListId == query.ListId)
        .OrderBy(x => x.Name)
        .Select(x => new CategoryListItem(x.Id, x.Name, x.ListId))
        .ToList();
}

public record CategoryListItem(
    CategoryId Id,
    string Name,
    TodoListId ListId
);
