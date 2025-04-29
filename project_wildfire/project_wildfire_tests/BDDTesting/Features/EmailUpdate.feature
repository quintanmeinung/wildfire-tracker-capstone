Feature: Email Update
  As a user
  I want to update my email address

  Scenario: Successfully update email
    Given a user is logged in
    And the user is on the email update page
    When the user enters a valid new email "testuser@mail.com"
    Then the email should be updated successfully


  Scenario: Fail to update email due to invalid format
    Given a user is logged in
    And the user is on the email update page
    When the user enters an invalid email "invalid-email@"
    Then an error message should be displayed



 