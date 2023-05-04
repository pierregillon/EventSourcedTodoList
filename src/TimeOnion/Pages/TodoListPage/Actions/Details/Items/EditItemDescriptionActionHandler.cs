using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class EditItemDescriptionActionHandler : ActionHandler<TodoListState.EditItemDescription>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public EditItemDescriptionActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.EditItemDescription action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

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

            await _commandDispatcher.Dispatch(command);
        }
        else
        {
            var command =
                new FixItemDescriptionCommand(
                    action.ListId,
                    action.ItemId,
                    new TodoItemDescription(action.NewDescription)
                );

            await _commandDispatcher.Dispatch(command);
        }

        state.TodoListDetails.Get(action.ListId).TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));
    }
}