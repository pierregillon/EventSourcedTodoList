using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class CategorySteps
{
    private readonly TestApplication _application;

    public CategorySteps(TestApplication application) => _application = application;

    [When(@"I create the ""(.*)"" empty category")]
    public async Task WhenICreateTheEmptyCategory(string categoryName) =>
        await _application.Dispatch(() => new CreateNewCategory(new CategoryName(categoryName), TodoListId.New()));

    [When(@"I create the (.*) category in my (.*) list")]
    [Given(@"the (.*) category has been created in my (.*) list")]
    public async Task WhenICreateTheCategory(string categoryName, string listName)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();
        await _application.Dispatch(() => new CreateNewCategory(new CategoryName(categoryName), listId));
    }

    [Then(@"my (.*) list categories are")]
    public async Task ThenTheCategoriesAre(string listName, Table table)
    {
        var listId = await FindListId(listName) ?? TodoListId.New();

        var expectedCategoryNames = table.Rows.Select(x => x["Name"]);
        var categories = await _application.Dispatch(new ListCategoriesQuery(listId));

        categories!
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(expectedCategoryNames);
    }

    private async Task<TodoListId?> FindListId(string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery(TimeHorizons.ThisDay));

        return todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id;
    }
}