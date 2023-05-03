Feature: Rename todo list
As a user
I want to rename my todo list
In order to be more precise

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot rename an unknown todo list
    When I rename my personal2 todo list into "professional"
    Then an error occurred with the message "The todolist could not be found."

@ErrorHandling
Scenario: Cannot rename with an empty name
    When I rename my personal todo list into "    "
    Then an error occurred with the message "A todo list name cannot be null or empty"

Scenario: Renamed todo list are correctly listed
    When I rename my personal todo list into "professional"
    Then the todo list are
      | Name         |
      | professional |