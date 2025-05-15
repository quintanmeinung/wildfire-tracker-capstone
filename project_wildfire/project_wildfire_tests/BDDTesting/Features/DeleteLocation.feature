Feature: Deleting Saved Locations

  Scenario: User deletes a saved location
    Given a user is logged in and on the saved locations page
    And the user has a saved location
    When the user deletes the saved location
    Then the location should no longer appear in the saved locations list
