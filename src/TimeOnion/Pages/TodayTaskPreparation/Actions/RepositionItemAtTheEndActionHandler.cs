using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class RepositionItemAtTheEndActionHandler : ActionHandler<TodoListState.RepositionItemAtTheEnd>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RepositionItemAtTheEndActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(
        TodoListState.RepositionItemAtTheEnd aAction,
        CancellationToken aCancellationToken
    )
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RepositionItemAtTheEndCommand(
                aAction.ListId,
                aAction.ItemId
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTimeHorizons));
    }
}