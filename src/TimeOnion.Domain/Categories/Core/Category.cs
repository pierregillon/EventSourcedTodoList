using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories.Core;

public class Category : EventSourcedAggregate<CategoryId>
{
    public Category(CategoryId id) : base(id)
    {
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
    }

    public static Category New(TodoListId listId, CategoryName name)
    {
        var category = new Category(CategoryId.New());
        category.StoreEvent(new CategoryCreated(category.Id, name, listId));
        return category;
    }

    public static Category Rehydrate(CategoryId id, IReadOnlyCollection<IDomainEvent> eventHistory)
    {
        var todoList = new Category(id);

        todoList.LoadFromHistory(eventHistory);

        return todoList;
    }
}