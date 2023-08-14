Feature: Create new todo list
As a user
I want to create a new todo list
In order to separate todo items from thematic like pro vs personal

Background: 
    Given I am registered and logged in
    
@ErrorHandling
Scenario: Cannot create new todo list when not authenticated
    Given I am disconnected
    When I create a personal todo list
    Then an error occurred with the message "You are not authorized to execute the request 'CreateNewTodoListCommand'."
    
Scenario: New todo list is listed
    When I create a personal todo list
    Then the todo list are
      | Name     |
      | personal |
      
Scenario: Todo lists are visible to only owners
    Given a personal todo list has been created
    When another user registered and logged in
    Then the todo list are
      | Name     |