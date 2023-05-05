using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.Actions.Details;

namespace TimeOnion.Pages.TodoListPage;

public record TodoListDetails(List<TodoListDetailState> Details)
{
    public static TodoListDetails From(IEnumerable<TodoListReadModel> lists) =>
        new(lists.Select(x =>
            new TodoListDetailState(x.Id, new List<CategoryReadModel>(), new List<TodoListItemReadModel>())).ToList());


    public static TodoListDetails Empty => new(new List<TodoListDetailState>());

    public TodoListDetailState Get(TodoListId listId)
    {
        var list = Details.FirstOrDefault(x => x.TodoListId == listId);
        if (list is null)
        {
            list = new TodoListDetailState(listId, new List<CategoryReadModel>(), new List<TodoListItemReadModel>());
            Details.Add(list);
        }

        return list;
    }


    public TodoListItemReadModel GetItem(TodoListId listId, TodoItemId itemId) =>
        Get(listId).TodoListItems.SingleOrDefault(x => x.Id == itemId)
        ?? throw new InvalidOperationException("Unknown item id");

    public TodoListDetails RemoveItem(TodoListItemReadModel item)
    {
        var list = Get(item.ListId);

        return this with
        {
            Details = Details
                .Replace(list, list with
                {
                    TodoListItems = list.TodoListItems.Where(x => x.Id != item.Id).ToList()
                })
                .ToList()
        };
    }

    public TodoListItemReadModel? GetAboveItem(TodoListItemReadModel item)
    {
        var items = Get(item.ListId).TodoListItems
            .Where(x => x.CategoryId == item.CategoryId)
            .ToList();
        var index = items.IndexOf(item);
        return index == 0 ? null : items[index - 1];
    }

    public TodoListDetails InsertNewItemTodoAfter(TodoListItemReadModel item)
    {
        var list = Get(item.ListId);
        var items = list.TodoListItems.ToList();

        var clone = new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            item.ListId,
            string.Empty,
            false,
            item.TimeHorizons,
            item.CategoryId
        );

        var todoListItemReadModels = list.TodoListItems.InsertAt(clone, items.IndexOf(item) + 1).ToList();
        
        return this with
        {
            Details = Details
                .Replace(list, list with
                {
                    TodoListItems = todoListItemReadModels
                })
                .ToList()
        };
    }

    public TodoListDetails InsertAtTheEnd(TodoListId listId, TimeHorizons timeHorizon)
    {
        var list = Get(listId);

        var newItem = new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            listId,
            string.Empty,
            false,
            timeHorizon,
            CategoryId.None
        );

        return this with
        {
            Details = Details
                .Replace(list, list with
                {
                    TodoListItems = list.TodoListItems.Append(newItem).ToList()
                })
                .ToList()
        };
    }

    public TodoListDetails InsertNewItemOnTopOfCategory(
        TodoListId listId,
        TimeHorizons timeHorizon,
        CategoryId categoryId
    )
    {
        var list = Get(listId);

        var newItem = new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            listId,
            string.Empty,
            false,
            timeHorizon,
            categoryId
        );

        return this with
        {
            Details = Details
                .Replace(list, list with
                {
                    TodoListItems = list.TodoListItems.Prepend(newItem).ToList()
                })
                .ToList()
        };
    }

    public TodoListDetails UpdateCategories(TodoListId listId, IReadOnlyCollection<CategoryReadModel> categories)
    {
        var list = Get(listId);

        return this with
        {
            Details = Details.Replace(list, list with { Categories = categories }).ToList()
        };
    }

    public TodoListDetails UpdateItems(TodoListId listId, IReadOnlyCollection<TodoListItemReadModel> items)
    {
        var list = Get(listId);

        return this with
        {
            Details = Details.Replace(list, list with { TodoListItems = items }).ToList()
        };
    }
}