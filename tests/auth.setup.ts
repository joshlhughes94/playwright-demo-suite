import { test as setup, expect } from '@playwright/test';
import { LoginPage } from '../pages/LoginPage';
import { users } from '../data/users';
import { AUTH_FILE } from '../data/paths';

setup('sign in once and save the session', async ({ page }) => {
  const login = new LoginPage(page);
  await login.goto();
  await login.login(users.standard.username, users.standard.password);
  await expect(page).toHaveURL(/inventory/);
  await page.context().storageState({ path: AUTH_FILE });
});
