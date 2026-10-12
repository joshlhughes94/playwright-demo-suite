using Microsoft.Playwright;
using PlaywrightDemo.Tests.Models;
using PlaywrightDemo.Tests.Services;

namespace PlaywrightDemo.Tests.Pages;

public class CheckoutPage : BasePage
{
    public CheckoutPage(PageDependencyService dependencies) : base(dependencies)
    {
    }

    public ILocator FirstName => Page.GetByTestId("firstName");

    public ILocator LastName => Page.GetByTestId("lastName");

    public ILocator Postcode => Page.GetByTestId("postalCode");

    public ILocator ContinueButton => Page.GetByTestId("continue");

    public ILocator FinishButton => Page.GetByTestId("finish");

    public ILocator Error => Page.GetByTestId("error");

    public ILocator ItemPrices => Page.GetByTestId("inventory-item-price");

    public ILocator ItemTotal => Page.GetByTestId("subtotal-label");

    public ILocator Tax => Page.GetByTestId("tax-label");

    public ILocator Total => Page.GetByTestId("total-label");

    public ILocator ConfirmationHeader => Page.GetByTestId("complete-header");

    public async Task EnterDetailsAsync(Shopper shopper)
    {
        await FirstName.FillAsync(shopper.FirstName);
        await LastName.FillAsync(shopper.LastName);
        await Postcode.FillAsync(shopper.Postcode);
        await ContinueButton.ClickAsync();
    }
}
