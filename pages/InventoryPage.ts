import { type Locator, type Page, expect } from '@playwright/test';

export type SortOption = 'az' | 'za' | 'lohi' | 'hilo';

export class InventoryPage {
  readonly title: Locator;
  readonly items: Locator;
  readonly prices: Locator;
  readonly names: Locator;
  readonly sort: Locator;
  readonly cartBadge: Locator;
  readonly cartLink: Locator;

  constructor(private readonly page: Page) {
    this.title = page.getByTestId('title');
    this.items = page.getByTestId('inventory-item');
    this.prices = page.getByTestId('inventory-item-price');
    this.names = page.getByTestId('inventory-item-name');
    this.sort = page.getByTestId('product-sort-container');
    this.cartBadge = page.getByTestId('shopping-cart-badge');
    this.cartLink = page.getByTestId('shopping-cart-link');
  }

  async goto() {
    await this.page.goto('/inventory.html');
    await expect(this.title).toHaveText('Products');
  }

  /** Product ids match the shop's data-test names, e.g. "sauce-labs-backpack". */
  async add(productId: string) {
    await this.page.getByTestId(`add-to-cart-${productId}`).click();
  }

  async remove(productId: string) {
    await this.page.getByTestId(`remove-${productId}`).click();
  }

  async sortBy(option: SortOption) {
    await this.sort.selectOption(option);
  }

  async priceValues(): Promise<number[]> {
    const texts = await this.prices.allTextContents();
    return texts.map((t) => Number(t.replace(/[^0-9.]/g, '')));
  }

  async openCart() {
    await this.cartLink.click();
  }
}
