using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core.Events;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories.Core;

public class Category : EventSourcedAggregate<CategoryId>
{
    private CategoryName? _currentName;

    private Category(CategoryId id) : base(id)
    {
    }

    public static Category New(TodoListId listId, CategoryName name)
    {
        var category = new Category(CategoryId.New());
        category.StoreEvent(new CategoryCreated(category.Id, name, listId));
        return category;
    }

    public void Rename(CategoryName newName)
    {
        if (newName != _currentName)
        {
            StoreEvent(new CategoryRenamed(Id, _currentName, newName));
        }
    }

    public void Delete() => StoreEvent(new CategoryDeleted(Id));

    protected override void Apply(IDomainEvent domainEvent) => _currentName = domainEvent switch
    {
        CategoryCreated created => created.Name,
        CategoryRenamed renamed => renamed.NewName,
        _ => _currentName
    };
}