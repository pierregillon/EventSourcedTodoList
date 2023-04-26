using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories;

public record ListCategoriesQuery(TodoListId ListId) : IQuery<IReadOnlyCollection<CategoryReadModel>>;

public record CategoryReadModel(CategoryId Id, string Name, TodoListId ListId);

internal class ListCategoriesQueryHandler : IQueryHandler<ListCategoriesQuery, IReadOnlyCollection<CategoryReadModel>>,
    IDomainEventListener<CategoryCreated>
{
    private readonly IReadModelDatabase _database;

    public ListCategoriesQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<CategoryReadModel>> Handle(ListCategoriesQuery query) =>
        (await _database.GetAll<CategoryReadModel>()).Where(x => x.ListId == query.ListId).ToList();

    public async Task On(CategoryCreated domainEvent) =>
        await _database.Add(new CategoryReadModel(domainEvent.Id, domainEvent.Name.Value, domainEvent.ListId));
}

public record CategoryId(Guid Value)
{
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId? None => null;

    public static CategoryId From(string value) => new(Guid.Parse(value));
}