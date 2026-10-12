@signedOut
Feature: Login
  Shoppers must sign in before they can browse or buy.

  Background:
    Given I am on the login page

  Scenario: A registered user can sign in
    When I sign in as "standard"
    Then I see the products page

  Scenario Outline: Sign-in problems show a clear message
    When I sign in with username "<username>" and password "<password>"
    Then I see the login error "<message>"

    Examples:
      | case            | username        | password         | message                            |
      | wrong password  | standard_user   | not-the-password | Username and password do not match |
      | locked account  | locked_out_user | secret_sauce     | locked out                         |
      | empty form      |                 |                  | Username is required               |

  Scenario: Signed-out users cannot open the shop directly
    When I open the products page directly
    Then I see the login error "You can only access"
