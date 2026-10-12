using NUnit.Framework;
using PlaywrightDemo.Tests.Services;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightDemo.Tests.StepDefinitions;

[Binding]
public class ProductSteps
{
    private readonly PageService _pages;

    public ProductSteps(PageService pages)
    {
        _pages = pages;
    }

    [Given("I am on the products page")]
    public Task GivenIAmOnTheProductsPage() => _pages.Inventory.GotoAsync();

    [Given("I have added these products to my basket:")]
    public async Task GivenIHaveAddedTheseProducts(Table products)
    {
        foreach (var row in products.Rows)
        {
            await _pages.Inventory.AddAsync(row["Product"]);
        }
    }

    [When("I add {string} to my basket")]
    public Task WhenIAddToMyBasket(string product) => _pages.Inventory.AddAsync(product);

    [When("I remove {string} on the products page")]
    public Task WhenIRemoveOnTheProductsPage(string product) => _pages.Inventory.RemoveAsync(product);

    [When("I sort products by {string}")]
    public Task WhenISortProductsBy(string option) => _pages.Inventory.SortByAsync(option);

    [Then("I see {int} products")]
    public Task ThenISeeProducts(int count) => Expect(_pages.Inventory.Items).ToHaveCountAsync(count);

    [Then("the product prices go from lowest to highest")]
    public async Task ThenPricesAscend() =>
        Assert.That(await _pages.Inventory.PriceValuesAsync(), Is.Ordered.Ascending);

    [Then("the product prices go from highest to lowest")]
    public async Task ThenPricesDescend() =>
        Assert.That(await _pages.Inventory.PriceValuesAsync(), Is.Ordered.Descending);

    [Then("the product names are in reverse alphabetical order")]
    public async Task ThenNamesAreReverseAlphabetical()
    {
        var names = await _pages.Inventory.Names.AllTextContentsAsync();
        Assert.That(names, Is.Ordered.Descending.Using((IComparer<string>)StringComparer.Ordinal));
    }

    [Then("the basket shows {int} item(s)")]
    public Task ThenTheBasketShows(int count) =>
        Expect(_pages.Inventory.BasketBadge).ToHaveTextAsync(count.ToString());

    [Then("the basket is empty")]
    public Task ThenTheBasketIsEmpty() => Expect(_pages.Inventory.BasketBadge).ToBeHiddenAsync();
}
