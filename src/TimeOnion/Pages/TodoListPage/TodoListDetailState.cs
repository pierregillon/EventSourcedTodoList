using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage;

public class TodoListDetailState
{
    public IReadOnlyCollection<CategoryReadModel> Categories { get; set; } = new List<CategoryReadModel>();

    public IReadOnlyCollection<TodoListItemReadModel> TodoListItems { get; set; } =
        new LinkedList<TodoListItemReadModel>();
}