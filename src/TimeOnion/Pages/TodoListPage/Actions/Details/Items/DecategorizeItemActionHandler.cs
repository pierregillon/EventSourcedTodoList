using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class DecategorizeItemActionHandler : ActionHandler<TodoListState.DecategorizeItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public DecategorizeItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.DecategorizeItem aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new DecategorizeTodoItemCommand(
                aAction.ListId,
                aAction.ItemId
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoListDetails[aAction.ListId].TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(aAction.ListId, state.CurrentTimeHorizon));
    }
}