using Microsoft.Playwright;
using PlaywrightDemo.Tests.Settings;

namespace PlaywrightDemo.Tests.Services;

/// <summary>
/// Everything a page object needs for the current scenario: the browser page and the settings.
/// Created by the hooks for each scenario and registered in Reqnroll's container.
/// </summary>
public class PageDependencyService
{
    public PageDependencyService(IPage page, AppSettings settings)
    {
        Page = page;
        Settings = settings;
    }

    public IPage Page { get; }

    public AppSettings Settings { get; }
}
