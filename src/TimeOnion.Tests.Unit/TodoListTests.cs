using FluentAssertions;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;

namespace TimeOnion.Tests.Unit;

public class TodoListTests
{
    [Fact]
    public void New_events_have_version()
    {
        var todoList = TodoList.New(new TodoListName("test"));

        todoList.Rename(new TodoListName("my todo list"));

        todoList.UncommittedChanges.Should().HaveCount(2);
        todoList.UncommittedChanges.First().Version.Should().Be(1);
        todoList.UncommittedChanges.Last().Version.Should().Be(2);
    }

    [Fact]
    public void New_events_starts_from_last_version()
    {
        var todoListId = TodoListId.New();

        IDomainEvent todoListCreated = new TodoListCreated(todoListId, new TodoListName("test"));

        todoListCreated.Version = 20;

        var todoList = TodoList.Rehydrate(todoListId, new[]
        {
            todoListCreated
        });

        todoList.Rename(new TodoListName("my todo list"));

        todoList.UncommittedChanges.Single().Version.Should().Be(21);
    }
}