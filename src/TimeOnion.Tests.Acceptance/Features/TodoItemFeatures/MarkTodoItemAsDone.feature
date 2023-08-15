Feature: Mark todo item as done
As a user
I want to mark a todo item as done
In order to track items a completed

Background:
    Given the current date is 2023-08-03
    And I am registered and logged in
    And a personal todo list has been created

@ErrorHandling
Scenario: Cannot mark an item as done when not authenticated
    Given I am disconnected
    When I try to mark the item "call dad" in my personal list as done
    Then an error occurred with the message "You are not authorized to execute the request 'MarkItemAsDoneCommand'."

@ErrorHandling
Scenario: Cannot mark an unknown item as done
    When I mark the item "call dad" in my personal list as done
    Then an error occurred with the message "Cannot complete the item: unknown item"

Scenario: Done items are listed
    Given the item "call dad" has been added to do this day in my personal list
    When I mark the item "call dad" in my personal list as done
    Then my personal todo list of this day is
      | Description | Is done? |
      | call dad    | true     |

Scenario: Marking an already done item as done do nothing
    Given the item "call dad" has been added to do this day in my personal list
    When I mark the item "call dad" in my personal list as done
    And I mark the item "call dad" in my personal list as done
    Then no error occurred

Scenario: Done items not listed anymore after day-scoped time horizon period passed
    Given the current date is 2023-05-03 12:00
    And the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When the current date is now the <new date>
    Then my personal todo list of <time horizon> is
      | Description | Is done? |
Examples:
  | time horizon | new date   |
  | this day     | 2023-05-04 |
  | this week    | 2023-05-08 |

Scenario: Done items are still listed when day-scoped time horizon still running
    Given the current date is 2023-05-03 12:00
    And the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When the current date is now the <new date>
    Then my personal todo list of <time horizon> is
      | Description | Is done? |
      | call dad    | true     |
Examples:
  | time horizon | new date            |
  | this day     | 2023-05-03 13:00    |
  | this day     | 2023-05-03 23:59:59 |
  | this week    | 2023-05-05 13:00    |
  | this week    | 2023-05-07 23:59:59 |

Scenario: Done items are not listed anymore after month-scoped time horizon period passed
    Given the current date is 2023-05-03
    And the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When the current date is now the <new date>
    Then my personal todo list of <time horizon> is
      | Description | Is done? |
Examples:
  | time horizon | new date   |
  | this month   | 2023-06-01 |
  | this quarter | 2023-07-01 |
  | this year    | 2024-01-01 |

Scenario: Done items are still listed when month-scoped time horizon period still running
    Given the current date is 2023-05-03
    And the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When the current date is now the <new date>
    Then my personal todo list of <time horizon> is
      | Description | Is done? |
      | call dad    | true     |
Examples:
  | time horizon | new date            |
  | this month   | 2023-05-15          |
  | this month   | 2023-05-31 23:59:59 |
  | this quarter | 2023-06-15          |
  | this quarter | 2023-06-30 23:59:59 |
  | this year    | 2023-12-15          |
  | this year    | 2023-12-31 23:59:59 |