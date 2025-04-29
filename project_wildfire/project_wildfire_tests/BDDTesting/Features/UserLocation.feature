Feature: User Location

User's with confirmed accounts can save custom locations.

@user_location
Scenario: successful save of custom location
    Given the user is logged in to their account
    When they click on the interactive map
    And they enter a valid title
    Then they should see a success message

@user_location
Scenario: unsuccessful save of custom location
    Given the user is logged in to their account
    When they click on the interactive map
    And they enter an invalid title
    Then they should see an error message

@user_location
Scenario: attempt to save without being logged in
    Given the user is not logged in
    When they click on the interactive map
    Then nothing should happen