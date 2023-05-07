using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record DecategorizeItemAction(
    TodoListId ListId,
    TodoItemId ItemId
) : IAction<TodoListDetailsState>;

internal class DecategorizeItemActionHandler :
    ActionHandlerBase<TodoListDetailsState, DecategorizeItemAction>
{
    public DecategorizeItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, DecategorizeItemAction action)
    {
        await Dispatch(new DecategorizeTodoItemCommand(action.ListId, action.ItemId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}