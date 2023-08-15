Feature: List items
As a user
I want to list items
In order to see them and interact with

Background:
    Given I am registered and logged in
    And a personal todo list has been created

@ErrorHandling
Scenario: Cannot list items when not authenticated
    Given I am disconnected
    When I try to list items of my personal todo list
    Then an error occurred with the message "You are not authorized to execute the request 'ListTodoItemsQuery'."

Scenario: Done items are listed following original creation order
    Given the item "call dad" has been added to do this day in my personal list
    Given the item "call mum" has been added to do this day in my personal list
    When I mark the item "call dad" in my personal list as done
    Then my personal todo list of this day is
      | Description | Is done? |
      | call dad    | true     |
      | call mum    | false    |