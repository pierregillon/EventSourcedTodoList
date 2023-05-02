using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

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

        state.TodoListDetails.Get(aAction.ListId).TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(aAction.ListId, state.CurrentTimeHorizon));
    }
}