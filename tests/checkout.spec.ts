import { test, expect } from '../fixtures/test';
import { CheckoutPage } from '../pages/CheckoutPage';
import { shopper } from '../data/users';

test.describe('Checkout', () => {
  test.beforeEach(async ({ inventoryPage }) => {
    await inventoryPage.goto();
    await inventoryPage.add('sauce-labs-backpack');
    await inventoryPage.add('sauce-labs-onesie');
    await inventoryPage.openCart();
  });

  test('a shopper can buy what is in their basket', async ({ cartPage, checkoutPage, page }) => {
    await expect(cartPage.names).toHaveText(['Sauce Labs Backpack', 'Sauce Labs Onesie']);
    await cartPage.checkout.click();

    await checkoutPage.fillDetails(shopper);
    await checkoutPage.finish.click();

    await expect(checkoutPage.completeHeader).toHaveText('Thank you for your order!');
    await expect(page).toHaveURL(/checkout-complete/);
  });

  test('order totals add up', async ({ cartPage, checkoutPage }) => {
    await cartPage.checkout.click();
    await checkoutPage.fillDetails(shopper);

    const prices = (await checkoutPage.itemPrices.allTextContents()).map(CheckoutPage.money);
    const itemTotal = CheckoutPage.money(await checkoutPage.itemTotal.textContent());
    const tax = CheckoutPage.money(await checkoutPage.tax.textContent());
    const total = CheckoutPage.money(await checkoutPage.total.textContent());

    const expectedItems = prices.reduce((sum, p) => sum + p, 0);
    expect(itemTotal).toBeCloseTo(expectedItems, 2);
    expect(total).toBeCloseTo(itemTotal + tax, 2);
  });

  test('delivery details are required', async ({ cartPage, checkoutPage }) => {
    await cartPage.checkout.click();
    await checkoutPage.continue.click();
    await expect(checkoutPage.error).toContainText('First Name is required');
  });

  test('removing an item in the basket updates it', async ({ cartPage }) => {
    await cartPage.remove('sauce-labs-onesie');
    await expect(cartPage.items).toHaveCount(1);
    await expect(cartPage.names).toHaveText(['Sauce Labs Backpack']);
  });
});
