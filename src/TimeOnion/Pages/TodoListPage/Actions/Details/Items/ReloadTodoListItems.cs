using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record ReloadTodoListItemsAction : IAction<TodoListDetailsState>;

internal class ReloadTodoListItemsActionHandler : ActionHandlerBase<TodoListDetailsState, ReloadTodoListItemsAction>
{
    private readonly IStore _store;

    public ReloadTodoListItemsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher) => _store = store;

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, ReloadTodoListItemsAction action)
    {
        var todoListState = _store.GetState<TodoListState>();

        foreach (var todoList in todoListState.TodoLists)
        {
            var items = await Dispatch(new ListTodoItemsQuery(todoList.Id, state.CurrentTimeHorizon));

            state = state.UpdateItems(todoList.Id, items);
        }

        return state;
    }
}