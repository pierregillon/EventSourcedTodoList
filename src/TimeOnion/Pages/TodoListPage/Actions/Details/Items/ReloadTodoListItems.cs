using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record ReloadTodoListItemsAction : IAction<TodoListState>;

internal class ReloadTodoListItemsActionHandler : ActionHandlerBase<TodoListState, ReloadTodoListItemsAction>
{
    public ReloadTodoListItemsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, ReloadTodoListItemsAction action)
    {
        foreach (var todoList in state.TodoLists)
        {
            var items = await Dispatch(new ListTodoItemsQuery(todoList.Id, state.CurrentTimeHorizon));

            state = state with
            {
                TodoListDetails = state.TodoListDetails.UpdateItems(todoList.Id, items)
            };
        }

        return state;
    }
}