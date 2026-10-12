using Microsoft.Extensions.Configuration;
using PlaywrightDemo.Tests.Models;

namespace PlaywrightDemo.Tests.Settings;

/// <summary>
/// Test settings from appsettings.test.json. Any value can be overridden with an
/// environment variable prefixed DEMO_, e.g. DEMO_BaseUrl or DEMO_Users__standard__Password.
/// </summary>
public class AppSettings
{
    private static readonly Lazy<AppSettings> Loaded = new(Load);

    public static AppSettings Current => Loaded.Value;

    public string BaseUrl { get; set; } = "https://www.saucedemo.com";

    /// <summary>chromium, firefox or webkit.</summary>
    public string Browser { get; set; } = "chromium";

    public bool Headless { get; set; } = true;

    public int DefaultTimeoutMs { get; set; } = 10000;

    public Dictionary<string, User> Users { get; set; } = new();

    public Shopper Shopper { get; set; } = new();

    public User GetUser(string key) =>
        Users.TryGetValue(key, out var user)
            ? user
            : throw new ArgumentException($"No test user called '{key}' in appsettings.test.json.");

    private static AppSettings Load()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("Settings/appsettings.test.json", optional: false)
            .AddEnvironmentVariables(prefix: "DEMO_")
            .Build();

        var settings = new AppSettings();
        config.Bind(settings);
        return settings;
    }
}
