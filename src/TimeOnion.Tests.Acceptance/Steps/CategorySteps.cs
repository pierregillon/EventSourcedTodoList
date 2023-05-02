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

    [When(@"I rename the (.*) category in my (.*) list to (.*)")]
    public async Task WhenIRenameTheHealthCategoryToHealthCare(
        string categoryName,
        string listName,
        string newCategoryName
    )
    {
        var listId = await FindListId(listName)
            ?? throw new InvalidOperationException($"Specflow: unable to find the list {listName}.");
        var categoryId = await FindCategoryId(listId, categoryName) ?? CategoryId.New();
        var newName = newCategoryName.Replace("\"", string.Empty);
        await _application.Dispatch(() => new RenameCategoryCommand(categoryId, new CategoryName(newName)));
    }

    [When(@"I delete the (.*) category in my (.*) list")]
    public async Task WhenIDeleteTheHealthCategoryInMyPersonalList(string categoryName, string listName)
    {
        var listId = await FindListId(listName)
            ?? throw new InvalidOperationException($"Specflow: unable to find the list {listName}.");
        var categoryId = await FindCategoryId(listId, categoryName) ?? CategoryId.New();
        await _application.Dispatch(() => new DeleteCategoryCommand(categoryId));
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
            .BeEquivalentTo(expectedCategoryNames, options => options.WithStrictOrdering());
    }

    private async Task<TodoListId?> FindListId(string todoListName)
    {
        var todoLists = await _application.Dispatch(new ListTodoListsQuery());

        return todoLists?.FirstOrDefault(x => x.Name == todoListName)?.Id;
    }

    private async Task<CategoryId?> FindCategoryId(TodoListId listId, string categoryName)
    {
        var categories = await _application.Dispatch(new ListCategoriesQuery(listId));

        return categories?.FirstOrDefault(x => x.Name == categoryName)?.Id;
    }
}