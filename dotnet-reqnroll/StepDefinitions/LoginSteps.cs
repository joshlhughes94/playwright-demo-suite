using System.Text.RegularExpressions;
using PlaywrightDemo.Tests.Services;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightDemo.Tests.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly PageService _pages;
    private readonly PageDependencyService _dependencies;

    public LoginSteps(PageService pages, PageDependencyService dependencies)
    {
        _pages = pages;
        _dependencies = dependencies;
    }

    [Given("I am on the login page")]
    public Task GivenIAmOnTheLoginPage() => _pages.Login.GotoAsync();

    [When("I sign in as {string}")]
    public Task WhenISignInAs(string userKey)
    {
        var user = _dependencies.Settings.GetUser(userKey);
        return _pages.Login.SignInAsync(user.Username, user.Password);
    }

    [When("I sign in with username {string} and password {string}")]
    public Task WhenISignInWith(string username, string password) =>
        _pages.Login.SignInAsync(username, password);

    [When("I open the products page directly")]
    public Task WhenIOpenTheProductsPageDirectly() => _dependencies.Page.GotoAsync("/inventory.html");

    [Then("I see the products page")]
    public async Task ThenISeeTheProductsPage()
    {
        await Expect(_dependencies.Page).ToHaveURLAsync(new Regex("inventory"));
        await Expect(_pages.Inventory.Title).ToHaveTextAsync("Products");
    }

    [Then("I see the login error {string}")]
    public Task ThenISeeTheLoginError(string message) =>
        Expect(_pages.Login.Error).ToContainTextAsync(message);
}
