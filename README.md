# Playwright demo suite

An example end-to-end test suite for a web shop, built the way I set them up for clients. It tests [Sauce Demo](https://www.saucedemo.com), a public practice shop, so anyone can clone it and run it.

[![Playwright tests](https://github.com/joshlhughes94/playwright-demo-suite/actions/workflows/playwright.yml/badge.svg)](https://github.com/joshlhughes94/playwright-demo-suite/actions/workflows/playwright.yml)

## What it covers

| Journey | What's checked |
|---|---|
| Login | Valid sign-in, wrong password, locked-out account, required fields, blocked direct access |
| Product listing | Full range shown, sorting by price and name, basket count on add and remove |
| Checkout | Full purchase, order totals add up, required delivery details, editing the basket |

Tests run in desktop Chrome and on an iPhone-sized Safari.

## How it's built

- **Page objects** in `pages/` keep selectors in one place. When the UI changes, you fix one file, not every test.
- **Fixtures** in `fixtures/test.ts` inject those page objects, so tests read like user journeys.
- **Sign in once.** `tests/auth.setup.ts` logs in and saves the session. Every other test starts signed in, which keeps the suite fast.
- **Stable selectors.** Tests use the app's `data-test` attributes through `getByTestId()`, not CSS classes that change with styling.
- **Debuggable failures.** Failed tests keep a screenshot, a video and a full trace you can step through.

```
├── pages/            Page objects (login, products, basket, checkout)
├── fixtures/         Custom test fixtures
├── tests/            Specs and the one-time sign-in
├── data/             Test users and shopper details
├── .github/workflows GitHub Actions pipeline
└── azure-pipelines.yml  Azure DevOps pipeline
```

## Run it

```bash
npm ci
npx playwright install
npm test            # run everything
npm run test:ui     # watch tests run in Playwright's UI mode
npm run report      # open the last HTML report
```

Point it at another environment with `BASE_URL=https://staging.example.com npm test`.

## CI

- **GitHub Actions** runs the suite on every push and pull request and uploads the HTML report.
- **Azure DevOps** runs the same suite and publishes results to the pipeline's Tests tab.

---

Want this set up for your own app? I build Playwright suites like this for small teams at a fixed price. See [github.com/joshlhughes94](https://github.com/joshlhughes94) to get in touch.
