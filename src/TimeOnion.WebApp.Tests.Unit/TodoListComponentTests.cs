using Bunit;
using FluentAssertions;
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
    public void Pressing_enter_at_the_end_of_an_item_pre_create_new_empty_item_and_focus_it()
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

        JSInterop
            .Setup<int>("getSelectedStart", _ => true)
            .SetResult(todoItemDescription.Length);

        var textAreas = component.FindAll("textarea[data-test='todo-item-description']");

        textAreas
            .Last()
            .KeyDown("Enter");

        textAreas.Refresh();

        textAreas
            .Should()
            .HaveCount(2);

        // JSInterop
        //     .VerifyFocusAsyncInvoke()
        //     .Arguments[0]
        //     .ShouldBeElementReferenceTo(textAreas.Last());
    }

    [Fact]
    public async Task Pressing_enter_in_the_middle_of_a_text_creates_a_new_todo_item_with_after_cursor_text()
    {
        var todoListId = TodoListId.New();
        const string todoItemDescription = "todo item 1";

        var todoListItemReadModel = new TodoListItemReadModel(
            TodoItemId.New(),
            todoListId,
            todoItemDescription,
            null,
            TimeHorizons.ThisDay,
            null
        );

        GetService<IQueryDispatcher>()
            .Dispatch(new ListTodoItemsQuery(todoListId, TimeHorizons.ThisDay))
            .Returns(new[]
            {
                todoListItemReadModel
            });

        var component = RenderComponent<TodoListComponent>(
            builder => builder.Add(c => c.TodoList, new TodoListReadModel(todoListId, "test"))
        );

        JSInterop
            .Setup<int>("getSelectedStart", _ => true)
            .SetResult(4);

        var textAreas = component.FindAll("textarea[data-test='todo-item-description']");

        textAreas
            .Last()
            .KeyDown("Enter");

        await CommandDispatcher
            .Received(1)
            .Dispatch(
                Arg.Is<AddItemToDoCommand>(x =>
                    x.ListId == todoListItemReadModel.ListId
                    && x.Description == new TodoItemDescription("item 1")
                    && x.TimeHorizons == todoListItemReadModel.TimeHorizons
                    && x.CategoryId == todoListItemReadModel.CategoryId
                    && x.AboveItemId == todoListItemReadModel.Id
                )
            );
    }

    [Fact]
    public async Task
        Pressing_enter_in_the_middle_of_a_text_edits_description_of_existing_item_with_before_cursor_text()
    {
        var todoListId = TodoListId.New();
        const string todoItemDescription = "todo item 1";

        var todoListItemReadModel = new TodoListItemReadModel(
            TodoItemId.New(),
            todoListId,
            todoItemDescription,
            null,
            TimeHorizons.ThisDay,
            null
        );

        GetService<IQueryDispatcher>()
            .Dispatch(new ListTodoItemsQuery(todoListId, TimeHorizons.ThisDay))
            .Returns(new[]
            {
                todoListItemReadModel
            });

        var component = RenderComponent<TodoListComponent>(
            builder => builder.Add(c => c.TodoList, new TodoListReadModel(todoListId, "test"))
        );

        JSInterop
            .Setup<int>("getSelectedStart", _ => true)
            .SetResult(4);

        var textAreas = component.FindAll("textarea[data-test='todo-item-description']");

        textAreas
            .Last()
            .KeyDown("Enter");

        await CommandDispatcher
            .Received(1)
            .Dispatch(
                Arg.Is<FixItemDescriptionCommand>(x =>
                    x.TodoListId == todoListItemReadModel.ListId
                    && x.TodoItemId == todoListItemReadModel.Id
                    && x.NewItemDescription == new TodoItemDescription("todo")
                )
            );
    }

    [Fact]
    public async Task Pressing_backspace_on_empty_todo_item_deletes_it()
    {
        var todoListId = TodoListId.New();
        const string todoItemDescription = "todo item 1";

        var todoListItemReadModel = new TodoListItemReadModel(
            TodoItemId.New(),
            todoListId,
            todoItemDescription,
            null,
            TimeHorizons.ThisDay,
            null
        );

        GetService<IQueryDispatcher>()
            .Dispatch(new ListTodoItemsQuery(todoListId, TimeHorizons.ThisDay))
            .Returns(new[]
            {
                todoListItemReadModel
            });

        var component = RenderComponent<TodoListComponent>(
            builder => builder.Add(c => c.TodoList, new TodoListReadModel(todoListId, "test"))
        );

        var textAreas = component.FindAll("textarea[data-test='todo-item-description']");

        textAreas.Last().Input(string.Empty);
        textAreas.Last().KeyDown("Backspace");

        await CommandDispatcher
            .Received(1)
            .Dispatch(
                Arg.Is<DeleteTodoItemCommand>(x =>
                    x.TodoListId == todoListItemReadModel.ListId
                    && x.ItemId == todoListItemReadModel.Id
                )
            );
    }
}