using BlazorState;
using MediatR;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class ReloadTodoListItemsActionHandler: ActionHandler<TodoListState.ReloadTodoListItems>
{
    private readonly IMediator _mediator;

    public ReloadTodoListItemsActionHandler(IStore aStore, IMediator mediator) : base(aStore)
    {
        _mediator = mediator;
    }

    public override async Task Handle(TodoListState.ReloadTodoListItems aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        foreach (var todoList in state.TodoLists)
        {
            await _mediator.Send(new TodoListState.LoadTodoListItems(todoList.Id), aCancellationToken);
        }
    }
}