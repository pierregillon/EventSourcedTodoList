using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record DeleteTodoListAction(TodoListId ListId) : IAction<TodoListState>;

internal class DeleteTodoListActionHandler : ActionApplier<DeleteTodoListAction, TodoListState>
{
    public DeleteTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(DeleteTodoListAction action, TodoListState state)
    {
        await Dispatch(new DeleteTodoListCommand(action.ListId));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}