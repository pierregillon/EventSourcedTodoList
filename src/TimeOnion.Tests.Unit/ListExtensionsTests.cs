using FluentAssertions;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

namespace TimeOnion.Tests.Unit;

public abstract class ListExtensionsTests
{
    public class Insert_after
    {
        [Fact]
        public void Inserts_new_element_after_existing_element_when_found()
        {
            var list = new List<int> { 1, 2, 3 };

            list.InsertAfter(4, 1)
                .Should()
                .BeEquivalentTo(new[]
                {
                    1, 4, 2, 3
                }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Use_default_equality_comparison()
        {
            var existing = new TodoListItemReadModel(
                TodoItemId.New(),
                TodoListId.New(),
                "test",
                null,
                TimeHorizons.ThisDay,
                null
            );

            var list = new List<TodoListItemReadModel> { existing };

            var newElement = new TodoListItemReadModel(
                TodoItemId.New(),
                TodoListId.New(),
                "test 2",
                null,
                TimeHorizons.ThisWeek,
                null
            );

            list.InsertAfter(newElement, existing)
                .Should()
                .BeEquivalentTo(new[]
                {
                    existing, newElement
                }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void Throws_exception_when_previous_element_not_found()
        {
            var list = new List<int> { 1, 2, 3 };

            var action = () => list.InsertAfter(5, 4).ToArray();

            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("The element '4' was not found in the collection");
        }
    }
}