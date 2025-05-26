Feature: Admin Fire Creation and Deletion

  @AdminFireCreation
  Scenario: Admin creates a simulated fire and sees it on the map
    Given I am logged in as an admin
    And I click the "Create Simulated Fire" button
    And I click on the map to place a fire
    Then a red fire marker should appear on the map

  @AdminFireVisibility
  Scenario: Regular user sees admin-created fire and can subscribe to it
    Given I am logged in as an admin
    And I place a simulated fire on the map
    And I log out
    And I log in as a regular user
    Then I should see the admin-created fire marker
    And I should see the "Subscribe to fire" button on the popup