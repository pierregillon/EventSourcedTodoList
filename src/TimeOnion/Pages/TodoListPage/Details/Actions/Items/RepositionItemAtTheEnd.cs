using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record RepositionItemAtTheEndAction(
    TodoListId ListId,
    TodoItemId ItemId
) : TodoItemAction(ListId);

internal class RepositionItemAtTheEndActionHandler :
    ActionApplier<RepositionItemAtTheEndAction, TodoListDetailsState>
{
    public RepositionItemAtTheEndActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        RepositionItemAtTheEndAction action,
        TodoListDetailsState state
    )
    {
        await Dispatch(new RepositionItemAtTheEndCommand(
            action.ListId,
            action.ItemId
        ));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}