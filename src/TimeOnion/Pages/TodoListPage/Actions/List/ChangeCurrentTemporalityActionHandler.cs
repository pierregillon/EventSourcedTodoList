using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class ChangeCurrentTemporalityActionHandler :
    ActionHandlerBase<TodoListState, TodoListState.ChangeCurrentTemporality>
{
    public ChangeCurrentTemporalityActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        TodoListState.ChangeCurrentTemporality action
    )
    {
        state = state with
        {
            CurrentTimeHorizon = action.TimeHorizons,
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };

        foreach (var todoList in state.TodoLists)
        {
            var items = await Dispatch(new ListTodoItemsQuery(todoList.Id, state.CurrentTimeHorizon));

            state = state with
            {
                TodoListDetails = state.TodoListDetails.UpdateItems(todoList.Id, items)
            };
        }

        return state;
    }
}