using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class LoadListsActionHandler : ActionHandlerBase<TodoListState, TodoListState.LoadLists>
{
    public LoadListsActionHandler(IStore store, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, TodoListState.LoadLists action)
    {
        var todoLists = await Dispatch(new ListTodoListsQuery());

        return state with
        {
            TodoLists = todoLists,
            TodoListDetails = TodoListDetails.From(todoLists)
        };
    }
}