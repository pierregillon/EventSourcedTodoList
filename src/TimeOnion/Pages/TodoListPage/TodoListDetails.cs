using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.Actions.Details;
using TimeOnion.Pages.TodoListPage.Actions.Details.Items;

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


    public TodoListItemReadModel GetItem(TodoListId listId, TodoItemId itemId) =>
        Get(listId).TodoListItems.SingleOrDefault(x => x.Id == itemId)
        ?? throw new InvalidOperationException("Unknown item id");

    public void RemoveItem(TodoListItemReadModel item)
    {
        var items = Get(item.ListId).TodoListItems.ToList();
        items.Remove(item);
        Get(item.ListId).TodoListItems = items;
    }

    public TodoListItemReadModel? GetAboveItem(TodoListItemReadModel item)
    {
        var items = Get(item.ListId).TodoListItems
            .Where(x => x.CategoryId == item.CategoryId)
            .ToList();
        var index = items.IndexOf(item);
        return index == 0 ? null : items[index - 1];
    }

    public void InsertNewItemTodoAfter(TodoListItemReadModel item)
    {
        var items = Get(item.ListId).TodoListItems.ToList();

        var clone = TodoListItemReadModelBeingCreated.From(item);

        items.Insert(items.IndexOf(item) + 1, clone);

        Get(item.ListId).TodoListItems = items;
    }

    public void InsertAtTheEnd(TodoListId listId, TimeHorizons timeHorizon)
    {
        var items = Get(listId).TodoListItems.ToList();

        items.Add(new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            listId,
            string.Empty,
            false,
            timeHorizon,
            CategoryId.None
        ));

        Get(listId).TodoListItems = items;
    }

    public void InsertNewItemOnTopOfCategory(TodoListId listId, TimeHorizons timeHorizon, CategoryId categoryId)
    {
        var items = Get(listId).TodoListItems.ToList();

        items.Insert(0, new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            listId,
            string.Empty,
            false,
            timeHorizon,
            categoryId
        ));

        Get(listId).TodoListItems = items;
    }
}