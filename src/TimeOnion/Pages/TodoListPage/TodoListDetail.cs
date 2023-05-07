using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Pages.TodoListPage;

public record TodoListDetail(
    TodoListId TodoListId,
    IReadOnlyCollection<CategoryReadModel> Categories,
    IReadOnlyCollection<TodoListItemReadModel> TodoListItems
);