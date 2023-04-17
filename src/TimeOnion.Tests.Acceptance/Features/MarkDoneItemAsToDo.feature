Feature: Mark done item as to do
As a user
I want to mark a done item to do
In order to re do it

@ErrorHandling
Scenario: Cannot mark an unknown item as to do
    When I mark the item "call dad" as to do
    Then an error occurred with the message "Cannot mark the item as to do: unknown item"

Scenario: Item marked as to do are listed as not done
    Given the item "call dad" has been added to do this day
    When I mark the item "call dad" as done
    And I mark the item "call dad" as to do
    Then the todo list of this day is
      | Description | Is done? |
      | call dad    | false    |