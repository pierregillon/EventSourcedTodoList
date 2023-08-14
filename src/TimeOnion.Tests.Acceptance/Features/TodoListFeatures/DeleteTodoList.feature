Feature: Delete todo list
As a user
I want to delete a todo list
In order to remove all todo items and todo list

Background: 
    Given I am registered and logged in
    
@ErrorHandling
Scenario: Cannot delete a todo list when not authenticated
    Given I am disconnected
    When I try to delete my personal todo list
    Then an error occurred with the message "You are not authorized to execute the request 'DeleteTodoListCommand'."

@ErrorHandling
Scenario: Cannot delete an unknown todo list
    When I delete my personal todo list
    Then an error occurred with the message "The todolist could not be found."

Scenario: Deleted todo list is not listed anymore
    Given a personal todo list has been created
    When I delete my personal todo list
    Then the todo list are
      | Name |

Scenario: Todo items belonging to the deleted todo list are also deleted
    Given a personal todo list has been created
    When I add the item "call daddy" to do this day in my personal list
    And I delete my personal todo list
    Then the todo list are
      | Name |