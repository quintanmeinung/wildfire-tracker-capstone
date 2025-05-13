Feature: Saved Locations
    As a logged-in user
    I want to save and manage locations
    So I can easily access and organize important places

Background:
    Given I am logged in to my account

Scenario: Save a new location
  When I click on the map
  Then I see a popup with a save location button appear on the map
  
  When I click the save location button
  Then I see the save location form
  
  When I enter a valid title "Dark Obelisk"
  And I enter a valid address "123 Main St"
  When I click the submit button
  Then I should see a success alert
  And I should see a marker for "Dark Obelisk" on the map