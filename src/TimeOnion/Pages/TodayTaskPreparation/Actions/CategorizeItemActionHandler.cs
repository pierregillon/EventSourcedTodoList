using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class CategorizeItemActionHandler : ActionHandler<TodoListState.CategorizeItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CategorizeItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.CategorizeItem aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new CategorizeTodoItemCommand(
                aAction.ListId,
                aAction.ItemId,
                aAction.CategoryId
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTimeHorizons));
    }
}