using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record RepositionItemAboveAnotherAction(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemId ReferenceItemId
) : IAction<TodoListState>;

internal class
    RepositionItemAboveAnotherActionHandler : ActionHandlerBase<TodoListState, RepositionItemAboveAnotherAction>
{
    public RepositionItemAboveAnotherActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        RepositionItemAboveAnotherAction action
    )
    {
        await Dispatch(new RepositionItemAboveAnotherCommand(
            action.ListId,
            action.ItemId,
            action.ReferenceItemId
        ));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}