using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.UseCases;

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

        return Unit.Value;
    }
}

public class LoadCategoriesActionHandler : ActionHandler<TodoListState.LoadCategories>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadCategoriesActionHandler(
        IStore aStore,
        IQueryDispatcher queryDispatcher
    ) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadCategories action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.Categories[action.ListId] = await _queryDispatcher.Dispatch(new ListCategoriesQuery(action.ListId));

        return Unit.Value;
    }
}