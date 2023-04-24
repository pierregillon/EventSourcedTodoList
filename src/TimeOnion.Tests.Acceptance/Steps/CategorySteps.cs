using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class CategorySteps
{
    private readonly TestApplication _application;

    public CategorySteps(TestApplication application) => _application = application;

    [When(@"I create the ""(.*)"" empty category")]
    [When(@"I create the (.*) category")]
    [Given(@"the (.*) category has been created")]
    public async Task WhenICreateTheCategory(string categoryName) =>
        await _application.Dispatch(() => new CreateNewCategory(new CategoryName(categoryName)));

    [Then(@"the categories are")]
    public async Task ThenTheCategoriesAre(Table table)
    {
        var expectedCategoryNames = table.Rows.Select(x => x["Name"]);
        var categories = await _application.Dispatch(new ListCategoriesQuery());

        categories!
            .Select(x => x.Name)
            .Should()
            .BeEquivalentTo(expectedCategoryNames);
    }
}