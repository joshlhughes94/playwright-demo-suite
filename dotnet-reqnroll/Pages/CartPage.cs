using Microsoft.Playwright;
using PlaywrightDemo.Tests.Services;

namespace PlaywrightDemo.Tests.Pages;

public class CartPage : BasePage
{
    public CartPage(PageDependencyService dependencies) : base(dependencies)
    {
    }

    public ILocator Items => Page.GetByTestId("inventory-item");

    public ILocator Names => Page.GetByTestId("inventory-item-name");

    public ILocator CheckoutButton => Page.GetByTestId("checkout");

    public Task RemoveAsync(string productName) =>
        Page.GetByTestId($"remove-{ProductId(productName)}").ClickAsync();
}
