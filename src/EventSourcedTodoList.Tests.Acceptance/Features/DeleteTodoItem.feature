Feature: Delete todo item
As a user
I want to delete a todo item
In order to stop seeing it because it is no longer a item to do

@ErrorHandling
Scenario: Cannot delete an unknown item
    When I delete the item "call dad"
    Then an error occurred with the message "Cannot delete the item: unknown item"

Scenario: Deleted item are not listed anymore
    Given the item "call dad" has been added to do this day
    When I delete the item "call dad"
    Then the todo list of this day is
      | Description |

@ErrorHandling
Scenario: Cannot delete an already deleted item
    Given the item "call dad" has been added to do this day
    And the item "call dad" has been deleted
    When I delete the item "call dad"
    Then an error occurred with the message "Cannot delete the item: unknown item"