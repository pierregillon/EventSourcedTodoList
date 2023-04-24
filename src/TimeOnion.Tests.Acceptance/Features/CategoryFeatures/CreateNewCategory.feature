Feature: Create new category
As a user
I want to create new categories
In order to categorize my todo items

@ErrorHandling
Scenario: Cannot create a category with empty name
    When I create the "   " empty category
    Then an error occurred with the message "A category name must not be null or whitespace."

Scenario: Created categories can be listed
    When I create the food category
    And I create the health category
    Then the categories are
      | Name   |
      | food   |
      | health |