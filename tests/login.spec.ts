import { test, expect } from '../fixtures/test';
import { users } from '../data/users';

test.describe('Login', () => {
  test.beforeEach(async ({ loginPage }) => {
    await loginPage.goto();
  });

  test('a registered user can sign in', async ({ loginPage, inventoryPage, page }) => {
    await loginPage.login(users.standard.username, users.standard.password);
    await expect(page).toHaveURL(/inventory/);
    await expect(inventoryPage.title).toHaveText('Products');
  });

  test('a wrong password is rejected with a clear message', async ({ loginPage }) => {
    await loginPage.login(users.standard.username, 'not-the-password');
    await loginPage.expectError('Username and password do not match');
  });

  test('a locked-out user cannot sign in', async ({ loginPage }) => {
    await loginPage.login(users.lockedOut.username, users.lockedOut.password);
    await loginPage.expectError('locked out');
  });

  test('both fields are required', async ({ loginPage }) => {
    await loginPage.submit.click();
    await loginPage.expectError('Username is required');
  });

  test('signed-out users cannot open the shop directly', async ({ page, loginPage }) => {
    await page.goto('/inventory.html');
    await loginPage.expectError('You can only access');
  });
});
