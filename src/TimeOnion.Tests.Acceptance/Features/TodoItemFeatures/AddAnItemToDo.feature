Feature: Add an item to do
As a user
I want to add a new item to do in a certain temporality
In order to remember it

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot add an item on unknown todo list
    When I add the item "test" to do this day in my professional list
    Then an error occurred with the message "The todolist could not be found."

@ErrorHandling
Scenario: Cannot add an item to do with no description specified
    When I add the item "    " to do this day in my personal list
    Then an error occurred with the message "An item to do must have a description"

Scenario: By default, there is nothing to do
    Then my personal todo list of this day is
      | Description |

Scenario: Listed items are filtered by their temporality
    When I add the item "call daddy" to do this day in my personal list
    And I add the item "prepare job interview" to do this week in my personal list
    Then my personal todo list of this day is
      | Description | Temporality |
      | call daddy  | this day    |
    And my personal todo list of this week is
      | Description           | Temporality |
      | prepare job interview | this week   |

Scenario: By default, items are to do
    When I add the item "call daddy" to do this day in my personal list
    And I add the item "prepare job interview" to do this day in my personal list
    Then my personal todo list of this day is
      | Description           | Is done? |
      | call daddy            | false    |
      | prepare job interview | false    |

@ErrorHandling
Scenario: Cannot add an item below an unknown one
    When I add the item "call mummy" to do this day in my personal list just after "call daddy"
    Then an error occurred with the message "The reference item after the todo item must be created is unknown."

Scenario: Add an item to do below another one
    Given the following items have been added to do this day in my personal list
      | Description |
      | call daddy  |
      | do shopping |
    When I add the item "call mummy" to do this day in my personal list just after "call daddy"
    Then my personal todo list of this day is
      | Description |
      | call daddy  |
      | call mummy  |
      | do shopping |

Scenario: Add an item to do below the last one
    Given the following items have been added to do this day in my personal list
      | Description |
      | call daddy  |
      | do shopping |
    When I add the item "call mummy" to do this day in my personal list just after "do shopping"
    Then my personal todo list of this day is
      | Description |
      | call daddy  |
      | do shopping |
      | call mummy  |

Scenario: Add an item to do in a category
    Given the family category has been created in my personal list
    When I add the item "call mummy" to do this day in my personal list in the family category
    Then my personal todo list of this day is
      | Description | Category |
      | call mummy  | family   |