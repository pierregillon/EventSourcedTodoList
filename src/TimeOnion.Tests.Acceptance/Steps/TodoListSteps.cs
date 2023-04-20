using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Domain.Todo.List.Events;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class TodoListSteps
{
    private readonly TestApplication _application;

    public TodoListSteps(TestApplication application) => _application = application;

    [Given(@"a (.*) todo list has been created")]
    [When(@"I create a (.*) todo list")]
    public async Task WhenICreateANewTodoList(string todoListName) =>
        await _application.Dispatch(new CreateNewTodoListCommand(new TodoListName(todoListName)));

    [When(@"I rename my (.*) todo list into ""(.*)""")]
    public async Task WhenIRenameMyPersonalTodoListIntoProfessional(string listName, string newListName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();

        await _application.Dispatch(() => new RenameTodoListCommand(listId, new TodoListName(newListName)));
    }

    [When(@"I delete my (.*) todo list")]
    public async Task WhenIDeleteMyPersonalTodoList(string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();

        await _application.Dispatch(new DeleteTodoListCommand(listId));
    }

    [Then(@"the todo list are")]
    public async Task ThenTheTodoListAre(Table table)
    {
        var expectedNames = table.Rows.Select(x => x["Name"]);
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(TimeHorizons.ThisDay));

        todoLists!
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(expectedNames);
    }

    private async Task<TodoListId?> FindListId(string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(TimeHorizons.ThisDay));

        return todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id;
    }
}