using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record RepositionItemAtTheEndAction(
    TodoListId ListId,
    TodoItemId ItemId
) : IAction<TodoListDetailsState>;

internal class RepositionItemAtTheEndActionHandler :
    ActionHandlerBase<TodoListDetailsState, RepositionItemAtTheEndAction>
{
    public RepositionItemAtTheEndActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, RepositionItemAtTheEndAction action)
    {
        await Dispatch(new RepositionItemAtTheEndCommand(
            action.ListId,
            action.ItemId
        ));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}