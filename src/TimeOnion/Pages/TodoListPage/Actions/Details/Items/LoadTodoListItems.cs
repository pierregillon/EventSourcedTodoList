using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record LoadTodoListItemsAction(TodoListId ListId) : IAction<TodoListState>;

internal class LoadTodoListItemsActionHandler :
    ActionHandlerBase<TodoListState, LoadTodoListItemsAction>
{
    public LoadTodoListItemsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        LoadTodoListItemsAction action
    )
    {
        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}