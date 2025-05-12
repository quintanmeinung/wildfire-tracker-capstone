Feature: Update Phone Number
    As a registered user I want to update my phone number
    So that my account contact information is current

Scenario: Successfully updating phone number
    Given a user is signed in 
    And the user is on the phone number update page
    When the user enters a valid new phone number "1234567890"
    Then the phone number should be updated successfully

Scenario: Entering an invalid phone number
    Given a user is signed in
    And the user is on the phone number update page
    When the user enters an invalid phone number "abcdefg"
    Then a phone number error message should be displayed
