using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class LoadListsActionHandler : ActionHandler<TodoListState.LoadLists>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadListsActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher
    ) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadLists action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery());
        state.TodoListDetails = new TodoListDetails(state.TodoLists);

        return Unit.Value;
    }
}