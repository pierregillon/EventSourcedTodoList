using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record EditItemDescriptionAction
    (TodoListId ListId, TodoItemId ItemId, string NewDescription) : TodoItemAction(ListId);

internal class EditItemDescriptionActionHandler :
    ActionApplier<EditItemDescriptionAction, TodoListDetailsState>
{
    public EditItemDescriptionActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        EditItemDescriptionAction action,
        TodoListDetailsState state
    )
    {
        var item = state.TodoListItems.Single(x => x.Id == action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            var command =
                new AddItemToDoCommand(
                    action.ListId,
                    item.Id,
                    new TodoItemDescription(action.NewDescription),
                    item.TimeHorizons,
                    item.CategoryId,
                    state.GetAboveItem(item)?.Id
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

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}