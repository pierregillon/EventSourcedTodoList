using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record AddNewItemTodoAfterItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    string NewDescription
) : IAction<TodoListDetailsState>;

internal class AddNewItemTodoAfterItemActionHandler :
    ActionHandlerBase<TodoListDetailsState, AddNewItemTodoAfterItemAction>
{
    public AddNewItemTodoAfterItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        AddNewItemTodoAfterItemAction action
    )
    {
        var item = state.GetItem(action.ListId, action.ItemId);

        if (string.IsNullOrWhiteSpace(action.NewDescription))
        {
            return state.InsertNewItemTodoAfter(item);
        }

        var command =
            new AddItemToDoCommand(
                action.ListId,
                TodoItemId.New(),
                new TodoItemDescription(action.NewDescription),
                item.TimeHorizons,
                item.CategoryId,
                item.Id
            );

        await Dispatch(command);

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}