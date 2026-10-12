using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using PlaywrightDemo.Tests.Services;

namespace PlaywrightDemo.Tests.Pages;

public abstract class BasePage
{
    protected BasePage(PageDependencyService dependencies)
    {
        Dependencies = dependencies;
    }

    protected PageDependencyService Dependencies { get; }

    protected IPage Page => Dependencies.Page;

    /// <summary>"Sauce Labs Backpack" becomes "sauce-labs-backpack", matching the shop's data-test names.</summary>
    protected static string ProductId(string productName) =>
        productName.Trim().ToLowerInvariant().Replace(' ', '-');

    /// <summary>Reads "$29.99" or "Item total: $39.98" as 29.99 / 39.98.</summary>
    public static decimal Money(string? text) =>
        decimal.Parse(Regex.Replace(text ?? "0", "[^0-9.]", ""), CultureInfo.InvariantCulture);
}
