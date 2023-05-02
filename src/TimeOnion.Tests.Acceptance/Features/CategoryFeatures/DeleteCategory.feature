Feature: Delete category
As a user
I want to delete a category
In order to remove the ones I don't use anymore

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot delete an unknown category
    When I delete the health category in my personal list
    Then an error occurred with the message "The category could not be found."

Scenario: Deleted categories are not listed anymore
    Given the health category has been created in my personal list
    When I delete the health category in my personal list
    Then my personal list categories are
      | Name |

Scenario: Items belonging to deleted categories does not belong to the category anymore
    Given the health category has been created in my personal list
    And the item "go to the doctor" has been added to do this day in my personal list
    And "go to the doctor" has been categorized to health category on my personal list
    When I delete the health category in my personal list
    Then my personal todo list of this day is
      | Description      | Category |
      | go to the doctor |          |