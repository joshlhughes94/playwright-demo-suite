import { type Locator, type Page } from '@playwright/test';

export class CheckoutPage {
  readonly firstName: Locator;
  readonly lastName: Locator;
  readonly postcode: Locator;
  readonly continue: Locator;
  readonly finish: Locator;
  readonly error: Locator;
  readonly itemTotal: Locator;
  readonly tax: Locator;
  readonly total: Locator;
  readonly itemPrices: Locator;
  readonly completeHeader: Locator;

  constructor(private readonly page: Page) {
    this.firstName = page.getByTestId('firstName');
    this.lastName = page.getByTestId('lastName');
    this.postcode = page.getByTestId('postalCode');
    this.continue = page.getByTestId('continue');
    this.finish = page.getByTestId('finish');
    this.error = page.getByTestId('error');
    this.itemTotal = page.getByTestId('subtotal-label');
    this.tax = page.getByTestId('tax-label');
    this.total = page.getByTestId('total-label');
    this.itemPrices = page.getByTestId('inventory-item-price');
    this.completeHeader = page.getByTestId('complete-header');
  }

  async fillDetails(details: { firstName: string; lastName: string; postcode: string }) {
    await this.firstName.fill(details.firstName);
    await this.lastName.fill(details.lastName);
    await this.postcode.fill(details.postcode);
    await this.continue.click();
  }

  /** Reads "Item total: $39.98" style labels as numbers. */
  static money(text: string | null): number {
    return Number((text ?? '').replace(/[^0-9.]/g, ''));
  }
}
