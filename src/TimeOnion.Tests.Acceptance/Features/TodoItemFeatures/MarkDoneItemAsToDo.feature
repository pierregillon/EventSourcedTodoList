Feature: Mark done item as to do
As a user
I want to mark a done item to do
In order to re do it

Background:
    Given I am registered and logged in
    And a personal todo list has been created
    
@ErrorHandling
Scenario: Cannot mark an item as to do when not authenticated
    Given I am disconnected
    When I try to mark the item "call dad" in my personal list as to do
    Then an error occurred with the message "You are not authorized to execute the request 'MarkItemAsToDoCommand'."

@ErrorHandling
Scenario: Cannot mark an unknown item as to do
    When I mark the item "call dad" in my personal list as to do
    Then an error occurred with the message "Cannot mark the item as to do: unknown item"

Scenario: Item marked as to do are listed as not done
    Given the item "call dad" has been added to do this day in my personal list
    When I mark the item "call dad" in my personal list as done
    And I mark the item "call dad" in my personal list as to do
    Then my personal todo list of this day is
      | Description | Is done? |
      | call dad    | false    |