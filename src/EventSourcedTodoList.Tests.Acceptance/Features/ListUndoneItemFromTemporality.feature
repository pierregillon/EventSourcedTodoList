Feature: List undone items from temporality
As a user
I want to list the undone items from temporality
In order to choose the one I want to move into lower temporality

Scenario: Undone tasks from temporality are listed
    When I add the item "call daddy" to do this week
    Then the undone tasks from this week are
      | Description |
      | call daddy  |

Scenario: Done tasks from temporality are not listed
    When I add the item "call daddy" to do this week
    And I mark the item "call daddy" as done
    Then the undone tasks from this week are
      | Description |

Scenario: Undone tasks from another temporality are not listed
    When I add the item "call daddy" to do this day
    Then the undone tasks from this week are
      | Description |