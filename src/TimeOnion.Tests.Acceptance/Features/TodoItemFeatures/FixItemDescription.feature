Feature: Fix item description
As a user
I want to fix a spelling mistake or complete an item description
In order to have the correct description on my todo list

Background:
    Given I am registered and logged in
    And a personal todo list has been created
    
@ErrorHandling
Scenario: Cannot fix item description when not authenticated
    Given I am disconnected
    When I try to fix description of the item "call dad" to "call daddy" in my personal list
    Then an error occurred with the message "You are not authorized to execute the request 'FixItemDescriptionCommand'."

@ErrorHandling
Scenario: Cannot fix description of an unknown item
    When I fix description of the item "call dad" to "call daddy" in my personal list
    Then an error occurred with the message "Cannot fix item description: unknown item"

Scenario: Item marked as to do are listed as not done
    Given the item "call dad" has been added to do this day in my personal list
    When I fix description of the item "call dad" to "call daddy" in my personal list
    Then my personal todo list of this day is
      | Description |
      | call daddy  |