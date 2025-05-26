Feature: Subscribing to Wildfires
  As a logged-in user
  I want to subscribe and unsubscribe to wildfires
  So that I can track them on the map and in my profile

  Scenario: Subscribe to a wildfire from the map popup
    When I navigate to the map page
    And I click on the fire marker with data-fire-id "9"
    And I click the "Subscribe to Fire" button
    Then I should see a toast message "Subscribed successfully."
    And the fire with id "9" appears in the subscribed fires list

  Scenario: Unsubscribe from a wildfire via profile
    Given I have subscribed to fire with id "9"
    When I open the profile modal
    And I click the "Unsubscribe" button for fire "9"
    Then the fire "9" should no longer appear in the subscribed fires list
