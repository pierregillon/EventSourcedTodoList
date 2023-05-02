using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class ChangeCurrentTemporalityActionHandler : ActionHandler<TodoListState.ChangeCurrentTemporality>
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly IMediator _mediator;

    public ChangeCurrentTemporalityActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher,
        IMediator mediator
    ) : base(aStore)
    {
        _queryDispatcher = queryDispatcher;
        _mediator = mediator;
    }

    public override async Task Handle(
        TodoListState.ChangeCurrentTemporality action,
        CancellationToken aCancellationToken
    )
    {
        var state = Store.GetState<TodoListState>();

        state.CurrentTimeHorizon = action.TimeHorizons;
        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery());

        await _mediator.Send(new TodoListState.ReloadTodoListItems(), aCancellationToken);
    }
}