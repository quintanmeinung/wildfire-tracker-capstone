Feature: Email Update
  As a user
  I want to update my email address


  Scenario: Successfully update email
    Given a user is logged in
    And the user is on the email update page
    When the user enters a valid new email "test@example.com"
    And submits the form
    Then the email should be updated successfully
    And a success message should be displayed


  Scenario: Fail to update email due to invalid format
    Given a user is logged in
    And the user is on the email update page
    When the user enters an invalid email "invalid-email@"
    And submits the form
    Then an error message should be displayed



 