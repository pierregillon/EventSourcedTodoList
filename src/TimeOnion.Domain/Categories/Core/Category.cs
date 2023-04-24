using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core;

public class Category : EventSourcedAggregate<CategoryId>
{
    public Category(CategoryId id) : base(id)
    {
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
    }

    public static Category New(CategoryName name)
    {
        var category = new Category(CategoryId.New());
        category.StoreEvent(new CategoryCreated(category.Id, name));
        return category;
    }

    public static Category Rehydrate(CategoryId id, IReadOnlyCollection<IDomainEvent> eventHistory)
    {
        var todoList = new Category(id);

        todoList.LoadFromHistory(eventHistory);

        return todoList;
    }
}