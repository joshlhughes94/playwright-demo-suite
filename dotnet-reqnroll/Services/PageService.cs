using PlaywrightDemo.Tests.Pages;

namespace PlaywrightDemo.Tests.Services;

/// <summary>
/// Gives step definitions one place to get page objects. Each page object is created
/// the first time it's used in a scenario and reused after that.
/// </summary>
public class PageService
{
    private readonly PageDependencyService _dependencies;
    private LoginPage? _login;
    private InventoryPage? _inventory;
    private CartPage? _cart;
    private CheckoutPage? _checkout;

    public PageService(PageDependencyService dependencies)
    {
        _dependencies = dependencies;
    }

    public LoginPage Login => _login ??= new LoginPage(_dependencies);

    public InventoryPage Inventory => _inventory ??= new InventoryPage(_dependencies);

    public CartPage Cart => _cart ??= new CartPage(_dependencies);

    public CheckoutPage Checkout => _checkout ??= new CheckoutPage(_dependencies);
}
