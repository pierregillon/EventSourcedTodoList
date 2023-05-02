using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class MarkAsToDoActionHandler : ActionHandler<TodoListState.MarkItemAsToDo>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public MarkAsToDoActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task<Unit> Handle(TodoListState.MarkItemAsToDo action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new MarkItemAsToDoCommand(action.ListId, action.ItemId));

        state.TodoListDetails.Get(action.ListId).TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return Unit.Value;
    }
}