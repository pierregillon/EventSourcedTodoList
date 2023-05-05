using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class
    AddNewItemTodoAfterItemActionHandler : ActionHandlerBase<TodoListState, TodoListState.AddNewItemTodoAfterItem>
{
    public AddNewItemTodoAfterItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        TodoListState.AddNewItemTodoAfterItem action
    )
    {
        var item = state.TodoListDetails.GetItem(action.ListId, action.ItemId);

        if (string.IsNullOrWhiteSpace(action.NewDescription))
        {
            return state with
            {
                TodoListDetails = state.TodoListDetails.InsertNewItemTodoAfter(item)
            };
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

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}