using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record RepositionItemAboveAnotherAction(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : TodoItemAction(ListId);

internal class RepositionItemAboveAnotherActionHandler :
    ActionHandlerBase<TodoListDetailsState, RepositionItemAboveAnotherAction>
{
    public RepositionItemAboveAnotherActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        RepositionItemAboveAnotherAction action
    )
    {
        await Dispatch(new RepositionItemAboveAnotherCommand(
            action.ListId,
            action.ItemId,
            action.ReferenceItemId
        ));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}