using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record LoadTodoListDetailsAction(TodoListId ListId) : TodoItemAction(ListId);

internal class LoadTodoListDetailsActionHandler :
    ActionHandlerBase<TodoListDetailsState, LoadTodoListDetailsAction>
{
    public LoadTodoListDetailsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        LoadTodoListDetailsAction action
    ) => state with
    {
        TodoListId = action.ListId,
        TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon)),
        Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
    };
}