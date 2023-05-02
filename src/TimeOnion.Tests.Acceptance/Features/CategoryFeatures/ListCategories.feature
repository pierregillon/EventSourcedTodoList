Feature: List categories
As a user
I want to list the categories of a todo list
In order to know where I can categorize list items

Background:
    Given a personal todo list has been created

Scenario: Categories are ordered alphabetically
    When I create the health category in my personal list
    And I create the hobbies category in my personal list
    And I create the food category in my personal list
    Then my personal list categories are
      | Name    |
      | food    |
      | health  |
      | hobbies |