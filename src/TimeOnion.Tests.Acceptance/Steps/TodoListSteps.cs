using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;
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

    [Then(@"the todo list are")]
    public async Task ThenTheTodoListAre(Table table)
    {
        var expectedNames = table.Rows.Select(x => x["Name"]);
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(Temporality.ThisDay));

        todoLists!
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(expectedNames);
    }
}