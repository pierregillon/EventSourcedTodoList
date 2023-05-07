using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.List;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record ReloadTodoListItemsAction : IAction<TodoListState>;

internal class ReloadTodoListItemsActionHandler : IActionHandler<ReloadTodoListItemsAction, TodoListState>
{
    private readonly IStore _store;
    private readonly IQueryDispatcher _queryDispatcher;

    public ReloadTodoListItemsActionHandler(
        IStore store,
        IQueryDispatcher queryDispatcher
    )
    {
        _store = store;
        _queryDispatcher = queryDispatcher;
    }

    public async Task Handle(ReloadTodoListItemsAction action)
    {
        var detailStates = _store.GetAllStates<TodoListDetailsState>();

        foreach (var details in detailStates)
        {
            var query = new ListTodoItemsQuery(details.TodoListId, details.CurrentTimeHorizon);

            var items = await _queryDispatcher.Dispatch(query);

            _store.UpdateState(details, details with { TodoListItems = items });
        }
    }
}