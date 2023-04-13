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

    [Given(@"the item ""(.*)"" has been added to do (.*)")]
    [When(@"I add the item ""(.*)"" to do (.*)")]
    public async Task WhenIAddTheItemToDo(string description, Temporality temporality) =>
        await _application.Dispatch(new AddItemToDoCommand(description, temporality));

    [When(@"I mark the item ""(.*)"" as done")]
    public async Task WhenIMarkTheItemAsCompleted(string itemDescription)
    {
        var itemId = await FindItemId(itemDescription) ?? TodoItemId.New();

        await _application.Dispatch(new MarkItemAsDoneCommand(itemId));
    }

    [When(@"I mark the item ""(.*)"" as to do")]
    public async Task WhenIMarkTheItemAsToDo(string itemDescription)
    {
        var itemId = await FindItemId(itemDescription) ?? TodoItemId.New();

        await _application.Dispatch(new MarkItemAsToDoCommand(itemId));
    }

    [When(@"I fix description of the item ""(.*)"" to ""(.*)""")]
    public async Task WhenIFixDescriptionOfTheItemTo(string itemDescription, string newItemDescription)
    {
        var itemId = await FindItemId(itemDescription) ?? TodoItemId.New();

        await _application.Dispatch(new FixItemDescriptionCommand(itemId, new ItemDescription(newItemDescription)));
    }

    [Then(@"the todo list is")]
    public async Task ThenTheTodoListIs(Table table)
    {
        var expectedItems = table.CreateSet<TodoListItem>();
        var items = await _application.Dispatch(new ListTodoListItemsQuery());

        Assert.Equivalent(
            expectedItems.Select(x => new { x.Description, x.IsDone, x.Temporality }),
            items!.Select(x => new { x.Description, x.IsDone, x.Temporality })
        );
    }

    private async Task<TodoItemId?> FindItemId(string itemDescription)
    {
        var items = await _application.Dispatch(new ListTodoListItemsQuery());
        var id = items!.FirstOrDefault(x => x.Description == itemDescription)?.Id;

        if (id is null) return null;

        return new TodoItemId(id.Value);
    }
}