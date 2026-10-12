Feature: Product listing
  Signed-in shoppers can browse, sort and add products to their basket.

  Background:
    Given I am on the products page

  Scenario: The full product range is shown
    Then I see 6 products

  Scenario: Sort by price, low to high
    When I sort products by "Price (low to high)"
    Then the product prices go from lowest to highest

  Scenario: Sort by price, high to low
    When I sort products by "Price (high to low)"
    Then the product prices go from highest to lowest

  Scenario: Sort by name, Z to A
    When I sort products by "Name (Z to A)"
    Then the product names are in reverse alphabetical order

  Scenario: The basket count follows adds and removes
    When I add "Sauce Labs Backpack" to my basket
    And I add "Sauce Labs Bike Light" to my basket
    Then the basket shows 2 items
    When I remove "Sauce Labs Backpack" on the products page
    Then the basket shows 1 item
    When I remove "Sauce Labs Bike Light" on the products page
    Then the basket is empty
