using BlazorState;
using MediatR;

namespace EventSourcedTodoList.Actions;

public class AddNewItemCommandHandler : ActionHandler<TodoListState.AddNewItemCommand>
{
    public AddNewItemCommandHandler(IStore aStore) : base(aStore)
    {
    }

    public override Task<Unit> Handle(TodoListState.AddNewItemCommand action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.Items.Add(new TodoListItem(action.Text));

        return Task.FromResult(Unit.Value);
    }
}