using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record RenameTodoListAction(TodoListId ListId, string NewName) : IAction<TodoListState>;

internal class RenameTodoListActionHandler : ActionHandlerBase<TodoListState, RenameTodoListAction>
{
    public RenameTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, RenameTodoListAction action)
    {
        await Dispatch(new RenameTodoListCommand(
            action.ListId,
            new TodoListName(action.NewName)
        ));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}