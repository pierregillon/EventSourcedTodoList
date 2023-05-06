using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record EditItemDescriptionAction
    (TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction<TodoListState>;

internal class EditItemDescriptionActionHandler :
    ActionHandlerBase<TodoListState, EditItemDescriptionAction>
{
    public EditItemDescriptionActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, EditItemDescriptionAction action)
    {
        var item = state.TodoListDetails.GetItem(action.ListId, action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            var aboveItem = state.TodoListDetails.GetAboveItem(item);

            var command =
                new AddItemToDoCommand(
                    action.ListId,
                    item.Id,
                    new TodoItemDescription(action.NewDescription),
                    item.TimeHorizons,
                    item.CategoryId,
                    aboveItem?.Id
                );

            await Dispatch(command);
        }
        else
        {
            var command =
                new FixItemDescriptionCommand(
                    action.ListId,
                    action.ItemId,
                    new TodoItemDescription(action.NewDescription)
                );

            await Dispatch(command);
        }

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}