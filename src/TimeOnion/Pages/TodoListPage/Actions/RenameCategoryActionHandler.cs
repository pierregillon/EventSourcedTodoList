using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Pages.TodayTaskPreparation;

namespace TimeOnion.Pages.TodoListPage.Actions;

public class RenameCategoryActionHandler : ActionHandler<TodoListState.RenameCategory>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RenameCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.RenameCategory aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new RenameCategoryCommand(aAction.Id, new CategoryName(aAction.Name)));

        state.Categories[aAction.ListId] = await _queryDispatcher.Dispatch(new ListCategoriesQuery(aAction.ListId));
    }
}