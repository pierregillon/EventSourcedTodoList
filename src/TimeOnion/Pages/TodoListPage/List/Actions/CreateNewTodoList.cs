using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record CreateNewTodoListAction : IAction<TodoListState>;

internal class CreateNewTodoListActionHandler :
    IActionApplier<CreateNewTodoListAction, TodoListState>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    public IStore Store { get; }

    public CreateNewTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    )
    {
        Store = store;
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public async Task<TodoListState> Apply(CreateNewTodoListAction action, TodoListState state)
    {
        await _commandDispatcher.Dispatch(new CreateNewTodoListCommand(new TodoListName("Nouvelle todo liste")));

        return state with
        {
            TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery())
        };
    }
}