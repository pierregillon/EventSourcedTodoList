using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

internal record CreateNewTodoListAction : IAction<TodoListState>;

internal class CreateNewTodoListActionHandler :
    ActionHandlerBase<TodoListState, CreateNewTodoListAction>
{
    public CreateNewTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, CreateNewTodoListAction action)
    {
        await Dispatch(new CreateNewTodoListCommand(new TodoListName("Nouvelle todo liste")));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}