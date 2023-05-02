using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Categories.Core.Events;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories;

public record ListCategoriesQuery(TodoListId ListId) : IQuery<IReadOnlyCollection<CategoryReadModel>>;

public record CategoryReadModel(CategoryId Id, string Name, TodoListId ListId);

internal class ListCategoriesQueryHandler : IQueryHandler<ListCategoriesQuery, IReadOnlyCollection<CategoryReadModel>>,
    IDomainEventListener<CategoryCreated>,
    IDomainEventListener<CategoryRenamed>,
    IDomainEventListener<CategoryDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListCategoriesQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<CategoryReadModel>> Handle(ListCategoriesQuery query) =>
        (await _database.GetAll<CategoryReadModel>())
        .Where(x => x.ListId == query.ListId)
        .OrderBy(x => x.Name)
        .ToList();

    public async Task On(CategoryCreated domainEvent) =>
        await _database.Add(new CategoryReadModel(domainEvent.CategoryId, domainEvent.Name.Value, domainEvent.ListId));

    public async Task On(CategoryRenamed domainEvent) => await _database.Update<CategoryReadModel>(
        category => category.Id == domainEvent.CategoryId,
        category => category with { Name = domainEvent.NewName.Value }
    );

    public async Task On(CategoryDeleted domainEvent) => await _database.Delete<CategoryReadModel>(
        category => category.Id == domainEvent.CategoryId
    );
}