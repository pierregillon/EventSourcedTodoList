Feature: Rename a category
As a user
I want to rename a category
In order to adjust the name if I misspelled it

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot rename an unknown category
    When I rename the health category in my personal list to health care
    Then an error occurred with the message "The category could not be found."

@ErrorHandling
Scenario: Cannot rename a category with an empty name
    Given the health category has been created in my personal list
    When I rename the health category in my personal list to "   "
    Then an error occurred with the message "A category name must not be null or whitespace."

Scenario: Renamed category are correctly listed
    Given the health category has been created in my personal list
    When I rename the health category in my personal list to health care
    Then my personal list categories are
      | Name        |
      | health care |