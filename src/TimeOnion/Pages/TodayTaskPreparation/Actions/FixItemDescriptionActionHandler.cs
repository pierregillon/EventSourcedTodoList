using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class FixItemDescriptionActionHandler : ActionHandler<TodoListState.FixItemDescription>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public FixItemDescriptionActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.FixItemDescription action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new FixItemDescriptionCommand(
                action.ListId,
                action.ItemId,
                new TodoItemDescription(action.NewDescription)
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoListItems[action.ListId] =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));
    }
}