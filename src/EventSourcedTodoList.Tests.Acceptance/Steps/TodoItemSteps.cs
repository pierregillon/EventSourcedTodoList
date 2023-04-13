using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;
using TechTalk.SpecFlow;

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

    [When(@"I reschedule the item ""(.*)"" to (.*)")]
    public async Task WhenIRescheduleTheItemToThisDay(string itemDescription, Temporality temporality)
    {
        var itemId = await FindItemId(itemDescription) ?? TodoItemId.New();

        await _application.Dispatch(new RescheduleTodoItemCommand(itemId, temporality));
    }

    [Then(@"the todo list of (.*) is")]
    public async Task ThenTheTodoListIs(Temporality temporality, Table table)
    {
        var items = await _application.Dispatch(new ListTodoListItemsQuery(temporality));

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var item = items.ElementAt(index);
            var tableRow = table.Rows[index];
            foreach (var cell in tableRow)
                if (cell.Key == "Description")
                {
                    Assert.Equal(cell.Value, item.Description);
                }
                else if (cell.Key == "Is done?")
                {
                    Assert.Equal(bool.Parse(cell.Value), item.IsDone);
                }
                else if (cell.Key == "Temporality")
                {
                    var normalized = cell.Value
                        .Replace(" ", string.Empty)
                        .Trim();
                    Assert.Equal(Enum.Parse<Temporality>(normalized, true), item.Temporality);
                }
                else
                {
                    throw new NotImplementedException("Unknown cell");
                }
        }
    }

    private async Task<TodoItemId?> FindItemId(string itemDescription)
    {
        var items = (await Task.WhenAll(
                Enum.GetValues<Temporality>()
                    .Select(x => _application.Dispatch(new ListTodoListItemsQuery(x)))
            ))
            .SelectMany(x => x)
            .ToArray();

        var id = items!.FirstOrDefault(x => x.Description == itemDescription)?.Id;

        if (id is null) return null;

        return new TodoItemId(id.Value);
    }
}