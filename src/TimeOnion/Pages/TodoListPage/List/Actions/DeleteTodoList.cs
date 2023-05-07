using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record DeleteTodoListAction(TodoListId ListId) : IAction<TodoListState>;

internal class DeleteTodoListActionHandler : ActionHandlerBase<TodoListState, DeleteTodoListAction>
{
    public DeleteTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, DeleteTodoListAction action)
    {
        await Dispatch(new DeleteTodoListCommand(action.ListId));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}