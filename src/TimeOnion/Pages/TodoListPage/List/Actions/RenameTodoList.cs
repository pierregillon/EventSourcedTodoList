using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record RenameTodoListAction(TodoListId ListId, string NewName) : IAction<TodoListState>;

internal class RenameTodoListActionHandler : ActionApplier<RenameTodoListAction, TodoListState>
{
    public RenameTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(RenameTodoListAction action, TodoListState state)
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