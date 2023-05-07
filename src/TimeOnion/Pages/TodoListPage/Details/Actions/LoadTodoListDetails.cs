using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions;

internal record LoadTodoListDetailsAction(TodoListId ListId, TimeHorizons TimeHorizon) : TodoItemAction(ListId);

internal class LoadTodoListDetailsActionHandler :
    ActionApplier<LoadTodoListDetailsAction, TodoListDetailsState>
{
    public LoadTodoListDetailsActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        LoadTodoListDetailsAction action,
        TodoListDetailsState state
    ) => state with
    {
        TodoListId = action.ListId,
        CurrentTimeHorizon = action.TimeHorizon,
        TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, action.TimeHorizon)),
        Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
    };
}