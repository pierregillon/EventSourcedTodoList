Feature: Mark item to do as completed
As a user
I want to mark a todo item as completed
In order to track I completed the item

@ErrorHandling
Scenario: Cannot complete an unknown item
    When I mark the item "call dad" as completed
    Then an error occurred with the message "Cannot complete the item: unknown item"

Scenario: Item to do marked as completed are listed
    Given the item "call dad" has been added to to
    When I mark the item "call dad" as completed
    Then the todo list is
      | Description | Is completed? |
      | call dad    | true          |

Scenario: Marking an already completed item as completed to nothing
    Given the item "call dad" has been added to to
    When I mark the item "call dad" as completed
    And I mark the item "call dad" as completed
    Then no error occurred