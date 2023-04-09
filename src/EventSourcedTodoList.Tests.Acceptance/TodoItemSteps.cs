using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace EventSourcedTodoList.Tests.Acceptance;

[Binding]
public class TodoItemSteps
{
    private readonly TestApplication _application;

    public TodoItemSteps(TestApplication application)
    {
        _application = application;
    }

    [When(@"I add the item ""(.*)"" to do")]
    public async Task WhenIAddTheItemToDo(string description)
    {
        await _application.Dispatch(new AddItemToDoCommand(description));
    }

    [Then(@"the todo list is")]
    public async Task ThenTheTodoListIs(Table table)
    {
        var expectedItems = table.CreateSet<TodoListItem>();
        var items = await _application.Dispatch(new ListTodoListItemsQuery());

        Assert.Equivalent(expectedItems, items);
    }
}