Feature: List yesterday undone tasks
As a user
I want to list yesterday undone tasks
In order to choose which to keep for today

Background:
    Given a personal todo list has been created

Scenario: Just added tasks to today are listed
    When I add the item "call daddy" to do this day in my personal list
    Then the yesterday undone tasks are
      | Description |
      | call daddy  |

Scenario: Done tasks are not listed
    When I add the item "call daddy" to do this day in my personal list
    And I mark the item "call daddy" in my personal list as done
    Then the yesterday undone tasks are
      | Description |

Scenario: Just added tasks to the week are not listed
    When I add the item "call daddy" to do this week in my personal list
    Then the yesterday undone tasks are
      | Description |