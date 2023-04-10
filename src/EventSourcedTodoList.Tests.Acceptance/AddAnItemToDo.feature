Feature: Add an item to do
As a user
I want to add a new item to do
In order to remember it

@ErrorHandling
Scenario: Cannot add an item to do with no description specified
    When I add the item "    " to do
    Then an error occurred with the message "An item to do must have a description"

Scenario: By default, there is nothing to do
    Then the todo list is
      | Description |

Scenario: Items to do are listed
    When I add the item "call daddy" to do
    And I add the item "prepare job interview" to do
    Then the todo list is
      | Description           |
      | call daddy            |
      | prepare job interview |