using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.List;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions;

internal record LoadTodoListDetailsAction(
    TodoListId ListId,
    TimeHorizons TimeHorizon,
    CategoryVisibility CategoryVisibility
) : TodoItemAction(ListId);

internal class LoadTodoListDetailsActionHandler :
    ActionApplier<LoadTodoListDetailsAction, TodoListDetailsState>
{
    public LoadTodoListDetailsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        LoadTodoListDetailsAction action,
        TodoListDetailsState state
    )
    {
        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, action.TimeHorizon));
        var categories = await FilterCategories(action, items);

        return state with
        {
            TodoListId = action.ListId,
            CurrentTimeHorizon = action.TimeHorizon,
            TodoListItems = items,
            Categories = categories
        };
    }

    private async Task<IReadOnlyCollection<CategoryReadModel>> FilterCategories(
        LoadTodoListDetailsAction action,
        IReadOnlyCollection<TodoListItemReadModel> items
    ) => action.CategoryVisibility switch
    {
        CategoryVisibility.ShowAll => await Dispatch(new ListCategoriesQuery(action.ListId)),
        CategoryVisibility.HideWithoutItems => await GetCategoriesWithAtLeastOneItem(action.ListId, items),
        CategoryVisibility.HideAll => Array.Empty<CategoryReadModel>(),
        _ => throw new ArgumentOutOfRangeException()
    };

    private async Task<IReadOnlyCollection<CategoryReadModel>> GetCategoriesWithAtLeastOneItem(
        TodoListId listId,
        IReadOnlyCollection<TodoListItemReadModel> items
    )
    {
        int GetCategoryItemCount(CategoryId categoryId)
        {
            return items.Count(x => x.CategoryId == categoryId);
        }

        return (await Dispatch(new ListCategoriesQuery(listId)))
            .Where(c => GetCategoryItemCount(c.Id) > 0)
            .ToList();
    }
}