Feature: Email Update
  As a user
  I want to update my email address


  @REQ-1234
  Scenario: Successfully update email
    Given a user is logged in
    And the user is on the email update page
    When the user enters a valid new email "newemail@example.com"
    And submits the form
    Then the email should be updated successfully
    And a success message should be displayed

  @REQ-1235
  Scenario: Fail to update email due to invalid format
    Given a user is logged in
    And the user is on the email update page
    When the user enters an invalid email "invalid-email"
    And submits the form
    Then an error message should be displayed

  @REQ-1236
  Scenario: Fail to update email when user is not found
    Given a user is not logged in
    When the user tries to access the email update page
    Then an error message should be displayed