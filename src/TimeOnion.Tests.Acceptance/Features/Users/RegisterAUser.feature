Feature: Register a user
As a user
I want to register
In order to use the app

@ErrorHandling
Scenario: Cannot register with invalid email
    When I register with the email "test" and password "P@ssw0rd"
    Then an error occurred with the message "The provided email has an invalid format"

Scenario: Registering a new account
    When I register with the email "test@test.com" and password "P@ssw0rd"
    Then I am correctly registered
    And the user with email "test@test.com" and password "P@ssw0rd" exists

@ErrorHandling
Scenario: Cannot register if another account is already registered
    Given a user has registered with the email "test@test.com" and password "P@ssw0rd"
    When I register with the email "test@test.com" and password "P@ssw0rd2"
    Then an error occurred with the message "The email is already taken by another user."