Feature: Mark todo item as done
As a user
I want to mark a todo item as done
In order to track items a completed

@ErrorHandling
Scenario: Cannot mark an unknown item as done
    When I mark the item "call dad" as done
    Then an error occurred with the message "Cannot complete the item: unknown item"

Scenario: Item to do marked as completed are listed
    Given the item "call dad" has been added to do this day
    When I mark the item "call dad" as done
    Then the todo list is
      | Description | Is done? |
      | call dad    | true     |

Scenario: Marking an already completed item as completed to nothing
    Given the item "call dad" has been added to do this day
    When I mark the item "call dad" as done
    And I mark the item "call dad" as done
    Then no error occurred