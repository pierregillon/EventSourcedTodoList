Feature: Category todo item
As a user
I want to categorize a todo item
In order to regroup item with the same thematic

Background:
    Given a personal todo list has been created

@ErrorHandling
Scenario: Cannot categorize an item on an unknown todo list
    When I categorize "call dad" to family category on my professional list
    Then an error occurred with the message "The todo list could not be found."

@ErrorHandling
Scenario: Cannot categorize to an unknown category
    Given the item "call dad" has been added to do this day in my personal list
    When I categorize "call dad" to family category on my personal list
    Then an error occurred with the message "The category could not be found."

@ErrorHandling
Scenario: Cannot categorize an unknown item
    Given the family category has been created
    When I categorize "call dad" to family category on my personal list
    Then an error occurred with the message "Cannot categorize: unknown item"

Scenario: By default, an item has no category
    Given the item "call dad" has been added to do this day in my personal list
    Then my personal todo list of this day is
      | Description | Category |
      | call dad    |          |

Scenario: Categorized todo item is listed
    Given the family category has been created
    And the item "call dad" has been added to do this day in my personal list
    When I categorize "call dad" to family category on my personal list
    Then my personal todo list of this day is
      | Description | Category |
      | call dad    | family   |

Scenario: De categorizing a todo item updates the list
    Given the family category has been created
    And the item "call dad" has been added to do this day in my personal list
    And "call dad" has been categorized to family category on my personal list
    When I decategorize "call dad" in my personal list
    Then my personal todo list of this day is
      | Description | Category |
      | call dad    |          |