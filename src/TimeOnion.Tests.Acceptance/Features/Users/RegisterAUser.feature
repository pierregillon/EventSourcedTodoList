Feature: Register an account
As a user
I want to register an account
In order to use the app

@ErrorHandling
Scenario: Cannot register an account with an invalid email
    When I register with the email "test" and password "P@ssw0rd"
    Then an error occurred with the message "The provided email has an invalid format"

Scenario: Registering a new account
    When I register with the email "test@test.com" and password "P@ssw0rd"
    Then I am correctly registered
    And I can log in with email "test@test.com" and password "P@ssw0rd"

@ErrorHandling
Scenario: Cannot register if another account is already registered with the same email address
    Given a user has registered with the email "test@test.com" and password "P@ssw0rd"
    When I register with the email "test@test.com" and password "P@ssw0rd2"
    Then an error occurred with the message "The email is already taken by another user."