using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage;

public class TodoListDetails
{
    private readonly Dictionary<TodoListId, TodoListDetailState> _details;

    public TodoListDetails(IEnumerable<TodoListReadModel> lists) =>
        _details = lists.ToDictionary(x => x.Id, _ => new TodoListDetailState());

    public TodoListDetailState Get(TodoListId listId)
    {
        if (!_details.TryGetValue(listId, out var state))
        {
            state = new TodoListDetailState();
            _details.Add(listId, state);
        }

        return state;
    }
}