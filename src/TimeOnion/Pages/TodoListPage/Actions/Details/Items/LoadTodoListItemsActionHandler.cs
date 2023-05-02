using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class LoadTodoListItemsActionHandler : ActionHandler<TodoListState.LoadTodoListItems>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadTodoListItemsActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher
    ) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadTodoListItems action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoListDetails.Get(action.ListId).TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return Unit.Value;
    }
}