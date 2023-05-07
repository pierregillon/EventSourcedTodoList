using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record LoadListsAction : IAction<TodoListState>;

internal class LoadListsActionHandler : ActionApplier<LoadListsAction, TodoListState>
{
    public LoadListsActionHandler(IStore store, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(LoadListsAction action, TodoListState state)
    {
        var todoLists = await Dispatch(new ListTodoListsQuery());

        return state with
        {
            TodoLists = todoLists
        };
    }
}