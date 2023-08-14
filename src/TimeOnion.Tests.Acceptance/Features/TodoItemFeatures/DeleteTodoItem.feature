Feature: Delete todo item
As a user
I want to delete a todo item
In order to stop seeing it because it is no longer a item to do

Background:
    Given I am registered and logged in
    And a personal todo list has been created
    
@ErrorHandling
Scenario: Cannot delete an item when not authenticated
    Given I am disconnected
    When I try to delete the item "call dad" on my personal list
    Then an error occurred with the message "You are not authorized to execute the request 'DeleteTodoItemCommand'."

@ErrorHandling
Scenario: Cannot delete an unknown item
    When I delete the item "call dad" on my personal list
    Then an error occurred with the message "Cannot delete the item: unknown item"

Scenario: Deleted item are not listed anymore
    Given the item "call dad" has been added to do this day in my personal list
    When I delete the item "call dad" on my personal list
    Then my personal todo list of this day is
      | Description |

@ErrorHandling
Scenario: Cannot delete an already deleted item
    Given the item "call dad" has been added to do this day in my personal list
    And the item "call dad" has been deleted on my personal list
    When I delete the item "call dad" on my personal list
    Then an error occurred with the message "Cannot delete the item: unknown item"