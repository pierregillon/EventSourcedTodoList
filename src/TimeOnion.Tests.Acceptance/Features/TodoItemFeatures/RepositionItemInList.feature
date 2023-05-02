Feature: Reposition item in todo list
As a user
I want to reposition an item in my todo list
In order to reorder items and have more important on top

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot reposition an item in an unknown todo list
    When I reposition "call dad" above "call mum" on my professional list
    Then an error occurred with the message "The todo list could not be found."

@ErrorHandling
Scenario: Cannot reposition an item at the end of unknown todo list
    When I reposition "call dad" at the end of my professional list
    Then an error occurred with the message "The todo list could not be found."

@ErrorHandling
Scenario: Cannot reposition an unknown item
    When I reposition "call dad" above "call mum" on my personal list
    Then an error occurred with the message "Cannot reposition the item: unknown item"

@ErrorHandling
Scenario: Cannot reposition an unknown item at the end
    When I reposition "call dad" at the end of my personal list
    Then an error occurred with the message "Cannot reposition the item at the end: unknown item"

@ErrorHandling
Scenario: Cannot reposition an item on unknown reference
    Given the item "call dad" has been added to do this day in my personal list
    When I reposition "call dad" above "call mum" on my personal list
    Then an error occurred with the message "Cannot reposition the item: unknown reference item"

Scenario: Repositioning an item at the same position does nothing
    Given the following items have been added to do this day in my personal list
      | Description |
      | call dad    |
      | call mum    |
    When I reposition "call dad" above "call mum" on my personal list
    Then my personal todo list of this day is
      | Description |
      | call dad    |
      | call mum    |

Scenario: Repositioned items are correctly ordered in the todo list (moving up)
    Given the following items have been added to do this day in my personal list
      | Description |
      | call dad    |
      | call mum    |
      | call bro    |
    When I reposition "call bro" above "call mum" on my personal list
    And I reposition "call dad" at the end of my personal list
    Then my personal todo list of this day is
      | Description |
      | call bro    |
      | call mum    |
      | call dad    |

Scenario: Repositioned items are correctly ordered in the todo list (moving down)
    Given the following items have been added to do this day in my personal list
      | Description |
      | call dad    |
      | call mum    |
      | call bro    |
    When I reposition "call dad" above "call bro" on my personal list
    Then my personal todo list of this day is
      | Description |
      | call mum    |
      | call dad    |
      | call bro    |