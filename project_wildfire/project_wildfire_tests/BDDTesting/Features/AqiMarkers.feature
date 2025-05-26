Feature: Display AQI station markers on the map
  As a user
  I want to see AQI station markers when the map loads
  So that I can be aware of nearby air quality readings

  Scenario: AQI markers are shown on the map
    Given I am on the map page
    When the AQI layer is loaded
    Then I should see AQI station markers on the map
