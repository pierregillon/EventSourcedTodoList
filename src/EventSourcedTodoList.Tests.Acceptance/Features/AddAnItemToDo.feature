Feature: Add an item to do
As a user
I want to add a new item to do in a certain temporality
In order to remember it

@ErrorHandling
Scenario: Cannot add an item to do with no description specified
    When I add the item "    " to do this day
    Then an error occurred with the message "An item to do must have a description"

Scenario: By default, there is nothing to do
    Then the todo list of this day is
      | Description |

Scenario: Listed items are filtered by their temporality
    When I add the item "call daddy" to do this day
    And I add the item "prepare job interview" to do this week
    Then the todo list of this day is
      | Description | Temporality |
      | call daddy  | this day    |
    And the todo list of this week is
      | Description           | Temporality |
      | prepare job interview | this week   |

Scenario: By default, items are to do
    When I add the item "call daddy" to do this day
    And I add the item "prepare job interview" to do this day
    Then the todo list of this day is
      | Description           | Is done? |
      | call daddy            | false    |
      | prepare job interview | false    |