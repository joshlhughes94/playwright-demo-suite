import { defineConfig, devices } from '@playwright/test';

import { AUTH_FILE } from './data/paths';

export default defineConfig({
  testDir: './tests',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 2 : undefined,
  reporter: process.env.CI
    ? [['html', { open: 'never' }], ['junit', { outputFile: 'test-results/junit.xml' }], ['github']]
    : [['html', { open: 'on-failure' }], ['list']],

  use: {
    baseURL: process.env.BASE_URL ?? 'https://www.saucedemo.com',
    // The demo shop tags elements with data-test, so getByTestId() reads that attribute.
    testIdAttribute: 'data-test',
    trace: 'on-first-retry',
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
  },

  projects: [
    // Logs in once and saves the session, so most tests start already signed in.
    { name: 'setup', testMatch: /.*\.setup\.ts/ },

    // Login tests need a fresh, signed-out browser.
    {
      name: 'auth',
      testMatch: /login\.spec\.ts/,
      use: { ...devices['Desktop Chrome'] },
    },

    {
      name: 'chromium',
      testIgnore: /login\.spec\.ts/,
      dependencies: ['setup'],
      use: { ...devices['Desktop Chrome'], storageState: AUTH_FILE },
    },
    {
      name: 'mobile-safari',
      testIgnore: /login\.spec\.ts/,
      dependencies: ['setup'],
      use: { ...devices['iPhone 14'], storageState: AUTH_FILE },
    },
  ],
});

