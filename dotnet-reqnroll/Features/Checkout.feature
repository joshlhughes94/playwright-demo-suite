Feature: Checkout
  Shoppers can review their basket, enter delivery details and place an order.

  Background:
    Given I am on the products page
    And I have added these products to my basket:
      | Product             |
      | Sauce Labs Backpack |
      | Sauce Labs Onesie   |
    And I open my basket

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

  Scenario: Order totals add up
    When I check out with my usual details
    Then the item total matches the item prices
    And the total equals the item total plus tax

  Scenario: Delivery details are required
    When I continue to checkout without entering any details
    Then I see the checkout error "First Name is required"

  Scenario: Removing an item updates the basket
    When I remove "Sauce Labs Onesie" from my basket
    Then my basket contains:
      | Product             |
      | Sauce Labs Backpack |
