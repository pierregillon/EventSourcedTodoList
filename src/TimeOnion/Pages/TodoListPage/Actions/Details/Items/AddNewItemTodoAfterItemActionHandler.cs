using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class AddNewItemTodoAfterItemActionHandler : ActionHandler<TodoListState.AddNewItemTodoAfterItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AddNewItemTodoAfterItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.AddNewItemTodoAfterItem aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        var item = state.TodoListDetails.GetItem(aAction.ListId, aAction.ItemId);

        if (string.IsNullOrWhiteSpace(aAction.NewDescription))
        {
            state.TodoListDetails.InsertNewItemTodoAfter(item);
        }
        else
        {
            var command =
                new AddItemToDoCommand(
                    aAction.ListId,
                    TodoItemId.New(),
                    new TodoItemDescription(aAction.NewDescription),
                    item.TimeHorizons,
                    item.CategoryId,
                    item.Id
                );

            await _commandDispatcher.Dispatch(command);

            state.TodoListDetails.Get(aAction.ListId).TodoListItems =
                await _queryDispatcher.Dispatch(new ListTodoItemsQuery(aAction.ListId, state.CurrentTimeHorizon));
        }
    }
}