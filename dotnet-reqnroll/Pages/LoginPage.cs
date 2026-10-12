using Microsoft.Playwright;
using PlaywrightDemo.Tests.Services;

namespace PlaywrightDemo.Tests.Pages;

public class LoginPage : BasePage
{
    public LoginPage(PageDependencyService dependencies) : base(dependencies)
    {
    }

    public ILocator Username => Page.GetByTestId("username");

    public ILocator Password => Page.GetByTestId("password");

    public ILocator SignInButton => Page.GetByTestId("login-button");

    public ILocator Error => Page.GetByTestId("error");

    public Task GotoAsync() => Page.GotoAsync("/");

    public async Task SignInAsync(string username, string password)
    {
        await Username.FillAsync(username);
        await Password.FillAsync(password);
        await SignInButton.ClickAsync();
    }
}
