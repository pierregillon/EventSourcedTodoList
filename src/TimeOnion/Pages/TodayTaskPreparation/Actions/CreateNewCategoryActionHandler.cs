using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class CreateNewCategoryActionHandler : ActionHandler<TodoListState.CreateNewCategory>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CreateNewCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.CreateNewCategory aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        var command = new CreateNewCategory(new CategoryName(aAction.Name));

        await _commandDispatcher.Dispatch(command);

        state.Categories = await _queryDispatcher.Dispatch(new ListCategoriesQuery());
    }
}