using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record DecategorizeItemAction(
    TodoListId ListId,
    TodoItemId ItemId
) : TodoItemAction(ListId);

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

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}