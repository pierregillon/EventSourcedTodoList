using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class LoadCategoriesActionHandler : ActionHandler<TodoListState.LoadCategories>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadCategoriesActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher
    ) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task Handle(TodoListState.LoadCategories action, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoListDetails[action.ListId].Categories =
            await _queryDispatcher.Dispatch(new ListCategoriesQuery(action.ListId));
    }
}