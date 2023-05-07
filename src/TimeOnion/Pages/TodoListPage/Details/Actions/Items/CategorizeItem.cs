using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record CategorizeItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    CategoryId CategoryId
) : TodoItemAction(ListId);

internal class CategorizeItemActionHandler : ActionHandlerBase<TodoListDetailsState, CategorizeItemAction>
{
    public CategorizeItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, CategorizeItemAction action)
    {
        await Dispatch(new CategorizeTodoItemCommand(
            action.ListId,
            action.ItemId,
            action.CategoryId
        ));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}