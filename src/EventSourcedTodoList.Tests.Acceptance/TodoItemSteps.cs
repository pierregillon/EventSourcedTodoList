using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace EventSourcedTodoList.Tests.Acceptance;

[Binding]
public class TodoItemSteps
{
    private readonly TestApplication _application;

    public TodoItemSteps(TestApplication application) => _application = application;

    [Given(@"the item ""(.*)"" has been added to do")]
    [When(@"I add the item ""(.*)"" to do")]
    public async Task WhenIAddTheItemToDo(string description)
    {
        await _application.Dispatch(new AddItemToDoCommand(description));
    }

    [When(@"I mark the item ""(.*)"" as done")]
    public async Task WhenIMarkTheItemAsCompleted(string itemDescription)
    {
        var items = await _application.Dispatch(new ListTodoListItemsQuery());
        var itemId = items!.FirstOrDefault(x => x.Description == itemDescription)?.Id;

        await _application.Dispatch(new MarkItemAsDoneCommand(new TodoItemId(itemId ?? Guid.NewGuid())));
    }

    [When(@"I mark the item ""(.*)"" as to do")]
    public async Task WhenIMarkTheItemAsToDo(string itemDescription)
    {
        var items = await _application.Dispatch(new ListTodoListItemsQuery());
        var itemId = items!.FirstOrDefault(x => x.Description == itemDescription)?.Id;

        await _application.Dispatch(new MarkItemAsToDoCommand(new TodoItemId(itemId ?? Guid.NewGuid())));
    }

    [Then(@"the todo list is")]
    public async Task ThenTheTodoListIs(Table table)
    {
        var expectedItems = table.CreateSet<TodoListItem>();
        var items = await _application.Dispatch(new ListTodoListItemsQuery());

        Assert.Equivalent(
            expectedItems.Select(x => new { x.Description, x.IsDone }),
            items!.Select(x => new { x.Description, x.IsDone })
        );
    }
}