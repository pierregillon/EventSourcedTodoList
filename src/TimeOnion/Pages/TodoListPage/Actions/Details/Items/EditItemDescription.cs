using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record EditItemDescriptionAction
    (TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction<TodoListDetailsState>;

internal class EditItemDescriptionActionHandler :
    ActionHandlerBase<TodoListDetailsState, EditItemDescriptionAction>
{
    public EditItemDescriptionActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, EditItemDescriptionAction action)
    {
        var item = state.GetItem(action.ListId, action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            var aboveItem = state.GetAboveItem(item);

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

        return state.UpdateItems(action.ListId, items);
    }
}