using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions;

internal abstract record TodoItemAction(TodoListId ListId) : IAction<TodoListDetailsState>, IActionOnScopedState
{
    object IActionOnScopedState.Scope => ListId;
}