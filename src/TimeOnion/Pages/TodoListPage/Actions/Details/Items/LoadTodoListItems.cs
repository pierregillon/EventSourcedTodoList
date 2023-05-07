using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record LoadTodoListItemsAction(TodoListId ListId) : IAction<TodoListDetailsState>;

internal class LoadTodoListItemsActionHandler :
    ActionHandlerBase<TodoListDetailsState, LoadTodoListItemsAction>
{
    public LoadTodoListItemsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        LoadTodoListItemsAction action
    )
    {
        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}