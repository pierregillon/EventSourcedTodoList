using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class LoadDataActionHandler : ActionHandler<TodoListState.LoadData>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadDataActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher
    ) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadData action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTimeHorizons));
        state.Categories = await _queryDispatcher.Dispatch(new ListCategoriesQuery());

        return Unit.Value;
    }
}