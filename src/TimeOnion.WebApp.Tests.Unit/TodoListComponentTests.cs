using Bunit;
using FluentAssertions;
using Microsoft.JSInterop;
using NSubstitute;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.Details;

namespace TimeOnion.WebApp.Tests.Unit;

public class TodoListComponentTests : ComponentTestBase
{
    [Fact]
    public void Renders_items()
    {
        var todoListId = TodoListId.New();
        const string todoListName = "test";

        var todoListItemReadModels = new[]
        {
            new TodoListItemReadModel(
                TodoItemId.New(),
                todoListId,
                "todo item 1",
                null,
                TimeHorizons.ThisDay,
                null
            ),
            new TodoListItemReadModel(
                TodoItemId.New(),
                todoListId,
                "todo item 2",
                null,
                TimeHorizons.ThisDay,
                null
            )
        };

        GetService<IQueryDispatcher>()
            .Dispatch(new ListTodoItemsQuery(todoListId, TimeHorizons.ThisDay))
            .Returns(todoListItemReadModels);

        var component = RenderComponent<TodoListComponent>(
            builder => builder.Add(c => c.TodoList, new TodoListReadModel(todoListId, todoListName))
        );

        component
            .Markup
            .Should()
            .Contain("todo item 1");

        component
            .Markup
            .Should()
            .Contain("todo item 2");

        component
            .FindAll("textarea[data-test='todo-item-description']")
            .Should()
            .HaveCount(2);
    }

    [Fact]
    public void Pressing_enter_at_the_end_of_an_item_pre_create_new_empty_item()
    {
        var todoListId = TodoListId.New();
        const string todoItemDescription = "todo item 1";

        var todoListItemReadModels = new[]
        {
            new TodoListItemReadModel(
                TodoItemId.New(),
                todoListId,
                todoItemDescription,
                null,
                TimeHorizons.ThisDay,
                null
            )
        };

        GetService<IQueryDispatcher>()
            .Dispatch(new ListTodoItemsQuery(todoListId, TimeHorizons.ThisDay))
            .Returns(todoListItemReadModels);

        var component = RenderComponent<TodoListComponent>(
            builder => builder.Add(c => c.TodoList, new TodoListReadModel(todoListId, "test"))
        );

        GetService<IJSRuntime>()
            .InvokeAsync<int>("getSelectedStart", Arg.Any<object?[]>())
            .Returns(todoItemDescription.Length);

        var textAreas = component.FindAll("textarea[data-test='todo-item-description']");

        textAreas
            .Last()
            .KeyDown("Enter");

        textAreas.Refresh();

        textAreas
            .Should()
            .HaveCount(2);
    }
}