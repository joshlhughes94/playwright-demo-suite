using NUnit.Framework;
using PlaywrightDemo.Tests.Models;
using PlaywrightDemo.Tests.Pages;
using PlaywrightDemo.Tests.Services;
using Reqnroll;
using Reqnroll.Assist;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightDemo.Tests.StepDefinitions;

[Binding]
public class CheckoutSteps
{
    private readonly PageService _pages;
    private readonly PageDependencyService _dependencies;

    public CheckoutSteps(PageService pages, PageDependencyService dependencies)
    {
        _pages = pages;
        _dependencies = dependencies;
    }

    [Given("I open my basket")]
    [When("I open my basket")]
    public Task OpenMyBasket() => _pages.Inventory.OpenBasketAsync();

    [Then("my basket contains:")]
    public Task ThenMyBasketContains(Table products) =>
        Expect(_pages.Cart.Names).ToHaveTextAsync(products.Rows.Select(row => row["Product"]));

    [When("I remove {string} from my basket")]
    public Task WhenIRemoveFromMyBasket(string product) => _pages.Cart.RemoveAsync(product);

    [When("I check out with these details:")]
    public async Task WhenICheckOutWithTheseDetails(Table details)
    {
        await _pages.Cart.CheckoutButton.ClickAsync();
        await _pages.Checkout.EnterDetailsAsync(details.CreateInstance<Shopper>());
    }

    [When("I check out with my usual details")]
    public async Task WhenICheckOutWithMyUsualDetails()
    {
        await _pages.Cart.CheckoutButton.ClickAsync();
        await _pages.Checkout.EnterDetailsAsync(_dependencies.Settings.Shopper);
    }

    [When("I continue to checkout without entering any details")]
    public async Task WhenIContinueWithoutDetails()
    {
        await _pages.Cart.CheckoutButton.ClickAsync();
        await _pages.Checkout.ContinueButton.ClickAsync();
    }

    [When("I finish the order")]
    public Task WhenIFinishTheOrder() => _pages.Checkout.FinishButton.ClickAsync();

    [Then("I see the order confirmation {string}")]
    public Task ThenISeeTheOrderConfirmation(string message) =>
        Expect(_pages.Checkout.ConfirmationHeader).ToHaveTextAsync(message);

    [Then("I see the checkout error {string}")]
    public Task ThenISeeTheCheckoutError(string message) =>
        Expect(_pages.Checkout.Error).ToContainTextAsync(message);

    [Then("the item total matches the item prices")]
    public async Task ThenTheItemTotalMatches()
    {
        var prices = (await _pages.Checkout.ItemPrices.AllTextContentsAsync()).Select(BasePage.Money);
        var itemTotal = BasePage.Money(await _pages.Checkout.ItemTotal.TextContentAsync());

        Assert.That(itemTotal, Is.EqualTo(prices.Sum()));
    }

    [Then("the total equals the item total plus tax")]
    public async Task ThenTheTotalAddsUp()
    {
        var itemTotal = BasePage.Money(await _pages.Checkout.ItemTotal.TextContentAsync());
        var tax = BasePage.Money(await _pages.Checkout.Tax.TextContentAsync());
        var total = BasePage.Money(await _pages.Checkout.Total.TextContentAsync());

        Assert.That(total, Is.EqualTo(itemTotal + tax));
    }
}
