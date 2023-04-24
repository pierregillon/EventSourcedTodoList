using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.List;

public record CreateNewCategoryCommand(CategoryName Name) : ICommand;

internal class CreateNewCategoryCommandHandler : ICommandHandler<CreateNewCategoryCommand>
{
    private readonly ICategoryRepository _repository;

    public CreateNewCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;

    public async Task Handle(CreateNewCategoryCommand command)
    {
        var category = Category.New(command.Name);

        await _repository.Save(category);
    }
}

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

public record CategoryCreated(CategoryId Id, CategoryName Name) : CategoryDomainEvent(Id);

public record CategoryDomainEvent(CategoryId Id) : IDomainEvent
{
    public Guid AggregateId => Id.Value;
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
}

public record CategoryName(string Value)
{
    public string Value { get; } = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("A category name must not be null or whitespace.")
        : Value;
}

public interface ICategoryRepository
{
    Task Save(Category category);
    Task<Category> Get(CategoryId commandCategoryId);
}