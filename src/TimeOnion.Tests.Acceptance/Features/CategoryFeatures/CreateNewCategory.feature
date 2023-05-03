Feature: Create new category
As a user
I want to create new categories
In order to categorize my todo items

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot create a category with empty name
    When I create the "   " empty category
    Then an error occurred with the message "A category name must not be null or whitespace."

@ErrorHandling
Scenario: Cannot a category on an unknown todo list
    When I create the food category in my professional list
    Then an error occurred with the message "The todolist could not be found."

Scenario: Created categories can be listed
    When I create the food category in my personal list
    And I create the health category in my personal list
    Then my personal list categories are
      | Name   |
      | food   |
      | health |

Scenario: Categories are scoped to a todo list
    Given a professional todo list has been created
    When I create the food category in my professional list
    Then my personal list categories are
      | Name |