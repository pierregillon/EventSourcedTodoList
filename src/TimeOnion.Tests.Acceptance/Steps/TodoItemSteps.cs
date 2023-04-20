using TechTalk.SpecFlow;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class TodoItemSteps
{
    private readonly TestApplication _application;

    public TodoItemSteps(TestApplication application) => _application = application;

    [Given(@"the item ""(.*)"" has been added to do (.*) in my (.*) list")]
    [When(@"I add the item ""(.*)"" to do (.*) in my (.*) list")]
    public async Task WhenIAddTheItemToDo(string description, Temporality temporality, string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(temporality));

        var todoListId = todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id ?? TodoListId.New();

        await _application.Dispatch(() =>
            new AddItemToDoCommand(todoListId, new ItemDescription(description), temporality));
    }

    [When(@"I mark the item ""(.*)"" in my (.*) list as done")]
    public async Task WhenIMarkTheItemAsCompleted(string itemDescription, string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        var itemId = await FindItemId(listId, itemDescription) ?? TodoItemId.New();

        var command = new MarkItemAsDoneCommand(listId, itemId);

        await _application.Dispatch(command);
    }

    [When(@"I mark the item ""(.*)"" in my (.*) list as to do")]
    public async Task WhenIMarkTheItemAsToDo(string itemDescription, string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        var itemId = await FindItemId(listId, itemDescription) ?? TodoItemId.New();

        var command = new MarkItemAsToDoCommand(listId, itemId);

        await _application.Dispatch(command);
    }

    [When(@"I fix description of the item ""(.*)"" to ""(.*)"" in my (.*) list")]
    public async Task WhenIFixDescriptionOfTheItemTo(string itemDescription, string newItemDescription, string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        var itemId = await FindItemId(listId, itemDescription) ?? TodoItemId.New();

        await _application.Dispatch(() =>
            new FixItemDescriptionCommand(listId, itemId, new ItemDescription(newItemDescription)));
    }

    [When(@"I reschedule the item ""(.*)"" in my (.*) list to (.*)")]
    public async Task WhenIRescheduleTheItemToThisDay(string itemDescription, string listName, Temporality temporality)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        var itemId = await FindItemId(listId, itemDescription) ?? TodoItemId.New();

        var command = new RescheduleTodoItemCommand(
            listId,
            itemId,
            temporality
        );

        await _application.Dispatch(command);
    }

    [Given(@"the item ""(.*)"" has been deleted on my (.*) list")]
    [When(@"I delete the item ""(.*)"" on my (.*) list")]
    public async Task WhenIDeleteTheItem(string itemDescription, string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        var itemId = await FindItemId(listId, itemDescription) ?? TodoItemId.New();

        var command = new DeleteTodoItemCommand(
            listId,
            itemId
        );

        await _application.Dispatch(command);
    }

    [Then(@"my (.*) todo list of (.*) is")]
    public async Task ThenTheTodoListIs(string todoListName, Temporality temporality, Table table)
    {
        var items = (await _application.Dispatch(new ListTodoListsQuery(temporality)))?
            .Where(x => x.Name == todoListName)
            .SelectMany(x => x.Items)
            .ToList()
            ?? throw new InvalidOperationException("Specflow: unable to load items");

        Assert.Equal(table.RowCount, items.Count);

        for (var index = 0; index < table.Rows.Count; index++)
        {
            var item = items.ElementAt(index);
            var tableRow = table.Rows[index];
            foreach (var cell in tableRow)
            {
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
    }

    [Then(@"the yesterday undone tasks are")]
    public async Task ThenTheYesterdayUndoneTasksAre(Table table)
    {
        var tasks = await _application.Dispatch(new ListYesterdayUndoneTasksQuery())
            ?? throw new InvalidOperationException("Specflow: unable to load items");

        var expectedTasks = table.Rows.Select(x => x["Description"]).ToArray();

        Assert.Equal(
            expectedTasks,
            tasks.Select(x => x.Description)
        );
    }

    [Then(@"the undone tasks from (.*) are")]
    public async Task ThenTheUndoneTasksFromThisWeekAre(Temporality temporality, Table table)
    {
        var tasks = await _application.Dispatch(new ListUndoneTasksFromTemporalityQuery(temporality))
            ?? throw new InvalidOperationException("Specflow: unable to load items");

        var expectedTasks = table.Rows.Select(x => x["Description"]).ToArray();

        Assert.Equal(
            expectedTasks,
            tasks.Select(x => x.Description)
        );
    }

    private async Task<TodoListId?> FindListId(string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(Temporality.ThisDay));

        return todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id;
    }

    private async Task<TodoItemId?> FindItemId(TodoListId todoListId, string itemDescription)
    {
        var data = await Task.WhenAll(
            Enum
                .GetValues<Temporality>()
                .Select(x => _application.Dispatch(new ListTodoListsQuery(x)))
        );

        var todoLists = data
            .SelectMany(x => x)
            .GroupBy(x => x.Id)
            .ToDictionary(
                x => x.Key,
                x => x
                    .SelectMany(g => g.Items)
                    .ToArray()
            );

        if (!todoLists.TryGetValue(todoListId, out var items))
        {
            throw new InvalidOperationException($"Unknown todo list {todoListId.Value}");
        }

        return items.FirstOrDefault(x => x.Description == itemDescription)?.Id;
    }
}