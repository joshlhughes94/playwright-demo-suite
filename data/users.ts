// Public demo accounts published on the Sauce Demo login page.
// For a real project, read these from environment variables or your CI secret store.
const password = process.env.DEMO_PASSWORD ?? 'secret_sauce';

export const users = {
  standard: { username: process.env.DEMO_USER ?? 'standard_user', password },
  lockedOut: { username: 'locked_out_user', password },
} as const;

export const shopper = {
  firstName: 'Test',
  lastName: 'Shopper',
  postcode: 'CH5 2AA',
} as const;
