# Reqnroll + Playwright for .NET

The same web shop journeys as the TypeScript suite, written as BDD scenarios in Gherkin with Reqnroll, Playwright for .NET and NUnit.

## What it covers

| Feature | Scenarios |
|---|---|
| `Login.feature` | Valid sign-in, wrong password, locked-out account, empty form, blocked direct access |
| `Products.feature` | Full range shown, sorting by price and name, basket count on add and remove |
| `Checkout.feature` | Full purchase, order totals add up, required delivery details, editing the basket |

Example scenario:

```gherkin
Scenario: A shopper can buy what is in their basket
  Then my basket contains:
    | Product             |
    | Sauce Labs Backpack |
    | Sauce Labs Onesie   |
  When I check out with these details:
    | First name | Last name | Postcode |
    | Test       | Shopper   | CH5 2AA  |
  And I finish the order
  Then I see the order confirmation "Thank you for your order!"
```

## How it's built

```
dotnet-reqnroll/
├── Features/          Gherkin feature files, readable by anyone on the team
├── StepDefinitions/   Steps that drive the page objects
├── Pages/             Page objects (login, products, basket, checkout)
├── Services/          PageDependencyService and PageService
├── Models/            User and Shopper
├── Settings/          AppSettings and appsettings.test.json
└── Hooks/             Browser start-up, sign-in and traces
```

- **Settings** live in `Settings/appsettings.test.json`. Override any value with an environment variable prefixed `DEMO_`, e.g. `DEMO_BaseUrl=https://staging.example.com` or `DEMO_Users__standard__Password=...`.
- **PageDependencyService** holds the browser page and settings for each scenario. **PageService** hands out page objects, so step classes only need one dependency.
- **Sign in once.** The hooks sign in before the run and reuse the saved session. Features tagged `@signedOut` start with a fresh browser.
- **Traces on failure.** A full Playwright trace is saved for any failed scenario and attached to the test result. Open it at [trace.playwright.dev](https://trace.playwright.dev).
- **Parallel.** Feature files run in parallel.

## Run it

```bash
cd dotnet-reqnroll
dotnet build
pwsh bin/Debug/net8.0/playwright.ps1 install
dotnet test
```

Run one feature: `dotnet test --filter "FullyQualifiedName~Checkout"`.
Watch it run: set `"Headless": false` in `Settings/appsettings.test.json`.

## CI

- **GitHub Actions:** `.github/workflows/reqnroll.yml` at the repo root.
- **Azure DevOps:** `dotnet-reqnroll/azure-pipelines.yml`.
