using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class DeleteCategoryActionHandler : ActionHandler<TodoListState.DeleteCategory>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public DeleteCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.DeleteCategory aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new DeleteCategoryCommand(aAction.Id));

        state.TodoListDetails.Get(aAction.ListId).Categories =
            await _queryDispatcher.Dispatch(new ListCategoriesQuery(aAction.ListId));

        state.TodoListDetails.Get(aAction.ListId).TodoListItems =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(aAction.ListId, state.CurrentTimeHorizon));
    }
}