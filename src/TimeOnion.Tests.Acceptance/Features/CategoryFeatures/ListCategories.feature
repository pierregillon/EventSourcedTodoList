Feature: List categories
As a user
I want to list the categories of a todo list
In order to know where I can categorize list items

Background:
    Given I am registered and logged in
    And a personal todo list has been created
    
@ErrorHandling
Scenario: Cannot list categories when not authenticated
    Given I am disconnected
    When I try to list categories of any list
    Then an error occurred with the message "You are not authorized to execute the request 'ListCategoriesQuery'."

Scenario: Categories are ordered alphabetically
    When I create the health category in my personal list
    And I create the hobbies category in my personal list
    And I create the food category in my personal list
    Then my personal list categories are
      | Name    |
      | food    |
      | health  |
      | hobbies |