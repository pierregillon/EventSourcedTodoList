using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record ShowAllCategoriesAction : IAction<TodoListState>;

internal record ShowAllCategoriesActionHandler
    (IStore Store) : ISyncActionApplier<ShowAllCategoriesAction, TodoListState>
{
    public TodoListState Apply(ShowAllCategoriesAction action, TodoListState state) =>
        state with { CurrentCategoryVisibility = CategoryVisibility.ShowAll };
}

internal record HideAllCategoriesAction : IAction<TodoListState>;

internal record HideAllCategoriesActionHandler
    (IStore Store) : ISyncActionApplier<HideAllCategoriesAction, TodoListState>
{
    public TodoListState Apply(HideAllCategoriesAction action, TodoListState state) =>
        state with { CurrentCategoryVisibility = CategoryVisibility.HideAll };
}

internal record HideCategoriesWithoutItemsAction : IAction<TodoListState>;

internal record HideCategoriesWithoutItemsActionHandler
    (IStore Store) : ISyncActionApplier<HideCategoriesWithoutItemsAction, TodoListState>
{
    public TodoListState Apply(HideCategoriesWithoutItemsAction action, TodoListState state) =>
        state with { CurrentCategoryVisibility = CategoryVisibility.HideWithoutItems };
}