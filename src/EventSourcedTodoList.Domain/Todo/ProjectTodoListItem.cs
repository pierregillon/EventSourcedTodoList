using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

internal class ProjectTodoListItem
{
    private readonly IReadModelDatabase _database;

    public ProjectTodoListItem(IReadModelDatabase database)
    {
        _database = database;
    }

    public async Task On(TodoItemAdded domainEvent)
    {
        await _database.Add(new TodoListItem(domainEvent.Description.Value));
    }
}