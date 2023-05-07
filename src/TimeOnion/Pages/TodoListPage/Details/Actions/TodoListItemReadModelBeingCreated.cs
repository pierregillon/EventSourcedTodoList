using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage.Details.Actions;

public record TodoListItemReadModelBeingCreated(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
) : TodoListItemReadModel(Id, ListId, Description, IsDone, TimeHorizons, CategoryId);