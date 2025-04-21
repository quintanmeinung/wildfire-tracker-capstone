Feature: Login

User's who visit the site can use their credentials to log in to their account.

@login @smoke
Scenario: successful login with valid credentials
	Given I navigate to the login form
	When I enter valid credentials
	Then I should be logged in

@login @smoke
Scenario: unsuccessful login with invalid credentials
	Given I navigate to the login form
	When I enter invalid credentials
	Then I should see an error message
