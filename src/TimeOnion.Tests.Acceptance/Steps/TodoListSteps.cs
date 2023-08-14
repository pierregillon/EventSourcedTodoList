using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
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
    [When(@"I try to rename my (.*) todo list into ""(.*)""")]
    public async Task WhenITryToRenameMyPersonalTodoListIntoProfessional(string listName, string newListName)
    {
        await _application.Dispatch(() => new RenameTodoListCommand(TodoListId.New(), new TodoListName(newListName)));
    }

    [When(@"I try to rename any list")]
    public async Task WhenITryToRenameAnyList()
    {
        await _application.Dispatch(() => new RenameTodoListCommand(TodoListId.New(), new TodoListName("test")));
    }

    [When(@"I delete my (.*) todo list")]
    public async Task WhenIDeleteMyPersonalTodoList(string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();

        await _application.Dispatch(new DeleteTodoListCommand(listId));
    }

    [When(@"I try to delete my (.*) todo list")]
    public async Task WhenTryIDeleteMyPersonalTodoList(string listName)
    {
        await _application.Dispatch(new DeleteTodoListCommand(TodoListId.New()));
    }

    [When(@"I try to delete any list")]
    public async Task WhenITryToDeleteAnyList()
    {
        await _application.Dispatch(new DeleteTodoListCommand(TodoListId.New()));
    }

    [Then(@"the todo list are")]
    public async Task ThenTheTodoListAre(Table table)
    {
        var expectedNames = table.Rows.Select(x => x["Name"]);
        var todoLists = await _application.Dispatch(new ListTodoListsQuery());

        todoLists!
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(expectedNames);
    }

    private async Task<TodoListId?> FindListId(string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery());

        return todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id;
    }
}