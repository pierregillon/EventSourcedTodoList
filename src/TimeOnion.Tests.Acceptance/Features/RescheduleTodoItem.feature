Feature: Reschedule todo item
As a user
I want to reschedule a todo item
In order to better organize my items across time ranges

@ErrorHandling
Scenario: Cannot reschedule an unknown todo item
    When I reschedule the item "call dad" to this day
    Then an error occurred with the message "Cannot reschedule item to do: unknown item"

Scenario: Rescheduling an item to the same time range do nothing
    Given the item "call dad" has been added to do this day
    When I reschedule the item "call dad" to this day
    Then no error occurred

Scenario: Rescheduling an item correctly list it on new time range
    Given the item "call dad" has been added to do this day
    When I reschedule the item "call dad" to this week
    Then the todo list of this day is
      | Description |
    Then the todo list of this week is
      | Description |
      | call dad    |