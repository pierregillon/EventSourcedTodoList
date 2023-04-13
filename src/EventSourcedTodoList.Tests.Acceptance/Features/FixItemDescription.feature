Feature: Fix item description
As a user
I want to fix a spelling mistake or complete an item description
In order to have the correct description on my todo list

@ErrorHandling
Scenario: Cannot fix description of an unknown item
    When I fix description of the item "call dad" to "call daddy"
    Then an error occurred with the message "Cannot fix item description: unknown item"

Scenario: Item marked as to do are listed as not done
    Given the item "call dad" has been added to do
    When I fix description of the item "call dad" to "call daddy"
    Then the todo list is
      | Description | Is done? |
      | call daddy  | false    |