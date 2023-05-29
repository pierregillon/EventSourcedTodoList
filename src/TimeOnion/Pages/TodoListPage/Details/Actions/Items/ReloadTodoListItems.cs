using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.List;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record ReloadTodoListItemsAction : IAction;

internal record ReloadTodoListItemsActionHandler(IStore Store, IQueryDispatcher QueryDispatcher) : IActionHandler<ReloadTodoListItemsAction>
{
    public async Task Handle(ReloadTodoListItemsAction action)
    {
        var detailStates = Store.GetAllStates<TodoListDetailsState>();

        foreach (var details in detailStates)
        {
            var query = new ListTodoItemsQuery(details.TodoListId, details.CurrentTimeHorizon);

            var items = await QueryDispatcher.Dispatch(query);

            Store.UpdateState(details, details with { TodoListItems = items });
        }
    }
}