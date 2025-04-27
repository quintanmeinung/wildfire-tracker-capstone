Feature: Wildfire Map Marker Display

  Scenario: Fire markers appear for today's date
    Given I am on the wildfire map page
    When the page loads
    Then I should see wildfire markers on the map

  Scenario: Fire markers update when I select a different date
    Given I am on the wildfire map page
    When I select a different valid date
    And I click the filter date button
    Then I should see wildfire markers updated on the map

  Scenario: User is warned when selecting a date outside the valid range
    Given I am on the wildfire map page
    When I select a date with no wildfires
    And I click the filter date button
    Then I should see a "Please select a date within the valid range." alert

  Scenario: Verify wildfire marker popup displays detailed info
    Given I am on the wildfire map page
    When the page loads
    And I click on a wildfire marker
    Then I should see radiative power, latitude, and longitude in the popup


