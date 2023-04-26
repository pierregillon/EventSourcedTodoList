using BlazorState;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodayTaskPreparation;

public class TodoListState : State<TodoListState>
{
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public TimeHorizons CurrentTimeHorizon { get; set; } = TimeHorizons.ThisDay;
    public IEnumerable<TodoListReadModel> TodoLists { get; set; } = Array.Empty<TodoListReadModel>();
    public Dictionary<TodoListId, IReadOnlyCollection<CategoryReadModel>> Categories { get; set; } = new();
    public Dictionary<TodoListId, IReadOnlyCollection<TodoListItemReadModel>> TodoListItems { get; set; } = new();

    public override void Initialize()
    {
    }

    public record LoadData : IAction;

    public record CreateNewTodoList : IAction;

    public record AddNewItem(TodoListId ListId, string? Text) : IAction;

    public record MarkItemAsDone(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record MarkItemAsToDo(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record FixItemDescription(TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction;

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

    public record LoadTodoListItems(
        TodoListId ListId
    ) : IAction;

    public record CreateNewCategory(
        TodoListId ListId,
        string Name
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
}