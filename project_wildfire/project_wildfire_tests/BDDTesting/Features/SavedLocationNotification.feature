Feature: Fire Alert Notifications

  Scenario: User saves a location and it appears in the list
    Given a user is logged in for saved locations
    And the user is on the saved locations page
    When the user adds a new location titled "Test Fire" with radius 5 miles and interval 0 hour
    Then the location should appear in the saved locations list
