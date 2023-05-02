using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class RescheduleTodoItemActionHandler : ActionHandler<TodoListState.RescheduleTodoItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RescheduleTodoItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.RescheduleTodoItem action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RescheduleTodoItemCommand(
                action.ListId,
                action.ItemId,
                action.TimeHorizons
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoListDetails[action.ListId].TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));
    }
}