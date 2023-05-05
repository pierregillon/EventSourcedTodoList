using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class DeleteTodoListActionHandler : ActionHandlerBase<TodoListState, TodoListState.DeleteTodoList>
{
    public DeleteTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, TodoListState.DeleteTodoList action)
    {
        await Dispatch(new DeleteTodoListCommand(action.ListId));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}