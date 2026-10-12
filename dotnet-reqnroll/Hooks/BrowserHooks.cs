using System.Text.RegularExpressions;
using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightDemo.Tests.Pages;
using PlaywrightDemo.Tests.Services;
using PlaywrightDemo.Tests.Settings;
using Reqnroll;
using Reqnroll.BoDi;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightDemo.Tests.Hooks;

[Binding]
public class BrowserHooks
{
    private const string SignedOutTag = "signedOut";

    private static IPlaywright? _playwright;
    private static IBrowser? _browser;
    private static string? _signedInState;

    private readonly IObjectContainer _container;
    private readonly ScenarioContext _scenario;
    private IBrowserContext? _context;

    public BrowserHooks(IObjectContainer container, ScenarioContext scenario)
    {
        _container = container;
        _scenario = scenario;
    }

    [BeforeTestRun]
    public static async Task StartBrowserAsync()
    {
        var settings = AppSettings.Current;

        _playwright = await Playwright.CreateAsync();
        // The demo shop tags elements with data-test, so GetByTestId() reads that attribute.
        _playwright.Selectors.SetTestIdAttribute("data-test");

        var browserType = settings.Browser.ToLowerInvariant() switch
        {
            "firefox" => _playwright.Firefox,
            "webkit" => _playwright.Webkit,
            _ => _playwright.Chromium,
        };
        _browser = await browserType.LaunchAsync(new BrowserTypeLaunchOptions { Headless = settings.Headless });

        _signedInState = await SignInOnceAsync(settings);
    }

    /// <summary>Signs in once and saves the session, so most scenarios start already signed in.</summary>
    private static async Task<string> SignInOnceAsync(AppSettings settings)
    {
        var context = await _browser!.NewContextAsync(new BrowserNewContextOptions { BaseURL = settings.BaseUrl });
        var page = await context.NewPageAsync();

        var login = new LoginPage(new PageDependencyService(page, settings));
        var user = settings.GetUser("standard");
        await login.GotoAsync();
        await login.SignInAsync(user.Username, user.Password);
        await Expect(page).ToHaveURLAsync(new Regex("inventory"));

        var state = await context.StorageStateAsync();
        await context.CloseAsync();
        return state;
    }

    [BeforeScenario(Order = 0)]
    public async Task StartScenarioAsync()
    {
        var settings = AppSettings.Current;
        var signedOut = _scenario.ScenarioInfo.CombinedTags.Contains(SignedOutTag);

        _context = await _browser!.NewContextAsync(new BrowserNewContextOptions
        {
            BaseURL = settings.BaseUrl,
            StorageState = signedOut ? null : _signedInState,
        });
        _context.SetDefaultTimeout(settings.DefaultTimeoutMs);

        // Record a trace for every scenario; it's only kept if the scenario fails.
        await _context.Tracing.StartAsync(new TracingStartOptions { Screenshots = true, Snapshots = true, Sources = true });

        var page = await _context.NewPageAsync();
        _container.RegisterInstanceAs(new PageDependencyService(page, settings));
    }

    [AfterScenario]
    public async Task EndScenarioAsync()
    {
        if (_context is null)
        {
            return;
        }

        if (_scenario.TestError is not null)
        {
            var folder = Path.Combine(TestContext.CurrentContext.WorkDirectory, "playwright-traces");
            Directory.CreateDirectory(folder);
            var safeName = string.Join("_", _scenario.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
            var tracePath = Path.Combine(folder, $"{safeName}.zip");

            await _context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
            TestContext.AddTestAttachment(tracePath, "Playwright trace: open at trace.playwright.dev");
        }
        else
        {
            await _context.Tracing.StopAsync();
        }

        await _context.CloseAsync();
    }

    [AfterTestRun]
    public static async Task StopBrowserAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
        }

        _playwright?.Dispose();
    }
}
