Feature: List undone items from temporality
As a user
I want to list the undone items from temporality
In order to choose the one I want to move into lower temporality

Background:
    Given a personal todo list has been created

Scenario: Undone tasks from temporality are listed
    When I add the item "call daddy" to do this week in my personal list
    Then the undone tasks from this week are
      | Description |
      | call daddy  |

Scenario: Done tasks from temporality are not listed
    When I add the item "call daddy" to do this week in my personal list
    And I mark the item "call daddy" in my personal list as done
    Then the undone tasks from this week are
      | Description |

Scenario: Undone tasks from another temporality are not listed
    When I add the item "call daddy" to do this day in my personal list
    Then the undone tasks from this week are
      | Description |