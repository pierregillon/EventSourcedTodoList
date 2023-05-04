using BlazorState;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage;

public class TodoListState : State<TodoListState>
{
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public TimeHorizons CurrentTimeHorizon { get; set; } = TimeHorizons.ThisDay;
    public IEnumerable<TodoListReadModel> TodoLists { get; set; } = Array.Empty<TodoListReadModel>();

    public TodoListDetails TodoListDetails { get; set; } = new(new List<TodoListReadModel>());

    public override void Initialize()
    {
    }

    public record LoadLists : IAction;

    public record CreateNewTodoList : IAction;

    public record MarkItemAsDone(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record MarkItemAsToDo(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record EditItemDescription(TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(TodoListId ListId, TodoItemId ItemId, TimeHorizons TimeHorizons) : IAction;

    public record ChangeCurrentTemporality(TimeHorizons TimeHorizons) : IAction;

    public record RepositionItemAboveAnother(
        TodoListId ListId,
        TodoItemId ItemId,
        TodoItemId ReferenceItemId
    ) : IAction;

    public record RepositionItemAtTheEnd(
        TodoListId ListId,
        TodoItemId ItemId
    ) : IAction;

    public record LoadCategories(
        TodoListId ListId
    ) : IAction;

    public record LoadTodoListItems(TodoListId ListId) : IAction;

    public record ReloadTodoListItems : IAction;

    public record CreateNewCategory(
        TodoListId ListId,
        string Name
    ) : IAction;

    public record RenameCategory(
        CategoryId Id,
        string Name,
        TodoListId ListId
    ) : IAction;

    public record DeleteCategory(
        CategoryId Id,
        TodoListId ListId
    ) : IAction;

    public record CategorizeItem(
        TodoListId ListId,
        TodoItemId ItemId,
        CategoryId CategoryId
    ) : IAction;

    public record DecategorizeItem(
        TodoListId ListId,
        TodoItemId ItemId
    ) : IAction;

    public record DeleteItem(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record RenameTodoList(TodoListId ListId, string NewName) : IAction;

    public record DeleteTodoList(TodoListId ListId) : IAction;

    public record InsertNewItemToDo(TodoListId ListId, TodoItemId? ItemId) : IAction;
}