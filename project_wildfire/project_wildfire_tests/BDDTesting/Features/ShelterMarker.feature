Feature: Shelter Markers Display
  As a user in a wildfire-affected area
  I want to see nearby emergency shelters on the map
  So that I can plan evacuation safely

  Scenario: Shelter markers are displayed on the map
    Given there are shelters available
    When the map loads and emergency shelters are toggled on
    Then shelter markers should appear on the map

  Scenario: Shelter marker displays popup with shelter details
    Given a shelter marker is visible on the map
    When I click on the shelter marker
    Then a popup should appear with the shelter’s name, address, and status

  Scenario: [TEST] Fire hazard layer toggle full cycle
    Given the map is visible
    When I toggle the fire hazard layer button
    Then the fire hazard layer should be displayed on the map
    And the toggle button should reflect the active state

    When I toggle the fire hazard layer button
    Then the fire hazard layer should be hidden
    And the toggle button should reflect the inactive state




