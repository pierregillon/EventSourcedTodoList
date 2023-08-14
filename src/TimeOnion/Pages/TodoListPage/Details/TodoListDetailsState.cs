using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details;

public record TodoListDetailsState(
    TodoListId TodoListId,
    TimeHorizons CurrentTimeHorizon,
    IReadOnlyCollection<CategoryListItem> Categories,
    IReadOnlyCollection<TodoListItemReadModel> TodoListItems
) : IState
{
    // ReSharper disable once UnusedMember.Global
    public static TodoListDetailsState Initialize() =>
        new(
            null!,
            TimeHorizons.ThisDay,
            new List<CategoryListItem>(),
            new List<TodoListItemReadModel>()
        );

    public TodoListItemReadModel? GetAboveItem(TodoListItemReadModel item)
    {
        var items = TodoListItems
            .Where(x => x.CategoryId == item.CategoryId)
            .ToList();
        var index = items.IndexOf(item);
        return index == 0 ? null : items[index - 1];
    }
}