Feature: Missing Inputs

Scenario: User tries to add a location with missing required fields
  Given a user is submitting a new location
  And the user navigates to the saved locations page
  When the user submits the form without filling in the required fields
  Then the form should not be submitted due to missing required fields
  And the location should not be added to the saved locations list
