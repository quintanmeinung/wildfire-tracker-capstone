Feature: Wildfire Marker

  Scenario: Add marker to map
    Given a wildfire occurs at coordinates (45.0, -120.5)
    When the map loads the wildfire data
    Then a marker should be placed at (45.0, -120.5)

  Scenario: No wildfire data
    When the map loads the wildfire data
    Then no markers should be placed

  Scenario: Multiple wildfires are loaded
    Given a wildfire occurs at coordinates (45.0, -120.5)
    And a wildfire occurs at coordinates (46.0, -121.5)
    When the map loads the wildfire data
    Then a marker should be placed at (45.0, -120.5)
    And a marker should be placed at (46.0, -121.5)