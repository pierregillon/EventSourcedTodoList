Feature: Create new todo list
As a user
I want to create a new todo list
In order to separate todo items from thematic like pro vs personal

Scenario: New todo list is listed
    When I create a personal todo list
    Then the todo list are
      | Name     |
      | personal |