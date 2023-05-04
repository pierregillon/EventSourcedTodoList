using BlazorState;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class InsertNewItemToDoActionHandler : ActionHandler<TodoListState.InsertNewItemToDo>
{
    public InsertNewItemToDoActionHandler(IStore aStore) : base(aStore)
    {
    }

    public override Task Handle(TodoListState.InsertNewItemToDo aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        if (aAction.ItemId is null)
        {
            state.TodoListDetails.InsertAtTheEnd(aAction.ListId, state.CurrentTimeHorizon);
        }
        else
        {
            var item = state.TodoListDetails.GetItem(aAction.ListId, aAction.ItemId);

            state.TodoListDetails.InsertNewItemTodoAfter(item);
        }

        return Task.CompletedTask;
    }
}

public record TodoListItemReadModelBeingCreated(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
) : TodoListItemReadModel(Id, ListId, Description, IsDone, TimeHorizons, CategoryId)
{
    public static TodoListItemReadModel From(TodoListItemReadModel item) => new TodoListItemReadModelBeingCreated(
        TodoItemId.New(),
        item.ListId,
        string.Empty,
        false,
        item.TimeHorizons,
        item.CategoryId
    );
}