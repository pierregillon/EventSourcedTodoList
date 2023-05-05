using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

public class CreateNewTodoListActionHandler : ActionHandlerBase<TodoListState, TodoListState.CreateNewTodoList>
{
    public CreateNewTodoListActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, TodoListState.CreateNewTodoList action)
    {
        await Dispatch(new CreateNewTodoListCommand(new TodoListName("Nouvelle todo liste")));

        return state with
        {
            TodoLists = await Dispatch(new ListTodoListsQuery())
        };
    }
}