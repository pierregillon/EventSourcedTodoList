Feature: Reschedule todo item
As a user
I want to reschedule a todo item
In order to better organize my items across time ranges

Background:
    Given I am registered and logged in
    And a personal todo list has been created
    
@ErrorHandling
Scenario: Cannot reschedule an item in list when not authenticated
    Given I am disconnected
    When I try to reschedule the item "call dad" in my personal list to this day
    Then an error occurred with the message "You are not authorized to execute the request 'RescheduleTodoItemCommand'."

@ErrorHandling
Scenario: Cannot reschedule an unknown todo item
    When I reschedule the item "call dad" in my personal list to this day
    Then an error occurred with the message "Cannot reschedule item to do: unknown item"

Scenario: Rescheduling an item to the same time range do nothing
    Given the item "call dad" has been added to do this day in my personal list
    When I reschedule the item "call dad" in my personal list to this day
    Then no error occurred

Scenario: Rescheduling an item correctly list it on new time range
    Given the item "call dad" has been added to do this day in my personal list
    When I reschedule the item "call dad" in my personal list to this week
    Then my personal todo list of this day is
      | Description |
    Then my personal todo list of this week is
      | Description |
      | call dad    |