import { type Locator, type Page } from '@playwright/test';

export class CartPage {
  readonly items: Locator;
  readonly names: Locator;
  readonly checkout: Locator;
  readonly continueShopping: Locator;

  constructor(private readonly page: Page) {
    this.items = page.getByTestId('inventory-item');
    this.names = page.getByTestId('inventory-item-name');
    this.checkout = page.getByTestId('checkout');
    this.continueShopping = page.getByTestId('continue-shopping');
  }

  async goto() {
    await this.page.goto('/cart.html');
  }

  async remove(productId: string) {
    await this.page.getByTestId(`remove-${productId}`).click();
  }
}
