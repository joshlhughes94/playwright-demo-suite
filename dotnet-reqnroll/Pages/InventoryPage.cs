using Microsoft.Playwright;
using PlaywrightDemo.Tests.Services;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightDemo.Tests.Pages;

public class InventoryPage : BasePage
{
    public InventoryPage(PageDependencyService dependencies) : base(dependencies)
    {
    }

    public ILocator Title => Page.GetByTestId("title");

    public ILocator Items => Page.GetByTestId("inventory-item");

    public ILocator Names => Page.GetByTestId("inventory-item-name");

    public ILocator Prices => Page.GetByTestId("inventory-item-price");

    public ILocator SortMenu => Page.GetByTestId("product-sort-container");

    public ILocator BasketBadge => Page.GetByTestId("shopping-cart-badge");

    public ILocator BasketLink => Page.GetByTestId("shopping-cart-link");

    public async Task GotoAsync()
    {
        await Page.GotoAsync("/inventory.html");
        await Expect(Title).ToHaveTextAsync("Products");
    }

    public Task AddAsync(string productName) =>
        Page.GetByTestId($"add-to-cart-{ProductId(productName)}").ClickAsync();

    public Task RemoveAsync(string productName) =>
        Page.GetByTestId($"remove-{ProductId(productName)}").ClickAsync();

    /// <summary>Uses the label shown in the menu, e.g. "Price (low to high)".</summary>
    public Task SortByAsync(string label) =>
        SortMenu.SelectOptionAsync(new SelectOptionValue { Label = label });

    public async Task<IReadOnlyList<decimal>> PriceValuesAsync() =>
        (await Prices.AllTextContentsAsync()).Select(Money).ToList();

    public Task OpenBasketAsync() => BasketLink.ClickAsync();
}
