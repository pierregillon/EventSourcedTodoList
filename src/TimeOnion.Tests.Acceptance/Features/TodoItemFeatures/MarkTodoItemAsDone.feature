Feature: Mark todo item as done
As a user
I want to mark a todo item as done
In order to track items a completed

Background:
    Given a personal todo list has been created

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

Scenario: Marking an already done item as done to nothing
    Given the item "call dad" has been added to do this day in my personal list
    When I mark the item "call dad" in my personal list as done
    And I mark the item "call dad" in my personal list as done
    Then no error occurred

Scenario: Done items not listed anymore after day-scoped time horizon period passed
    Given the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When <day count> day passed
    Then my personal todo list of <time horizon> is
      | Description | Is done? |

Examples:
  | time horizon | day count |
  | this day     | 1         |
  | this week    | 7         |

Scenario: Done items not listed anymore after month-scoped time horizon period passed
    Given the current date is 2023-05-01
    And the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When <month count> months passed
    Then my personal todo list of <time horizon> is
      | Description | Is done? |

Examples:
  | time horizon | month count |
  | this month   | 1           |
  | this quarter | 3           |
  | this year    | 12          |

Scenario: Done items are still listed when month-scoped time horizon period not passed yet
    Given the item "call dad" has been added to do <time horizon> in my personal list
    And the item "call dad" in my personal list has been marked as done
    When <month count> months passed
    Then my personal todo list of <time horizon> is
      | Description | Is done? |
      | call dad    | true     |

Examples:
  | time horizon | month count |
  | this quarter | 2           |
  | this year    | 11          |