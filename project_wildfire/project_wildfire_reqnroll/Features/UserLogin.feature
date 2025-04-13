Feature: User Login

User's who visit the site can use their credentials to log in to their account.

@smoke @login
Scenario: successful login with valid credentials
	Given the user can see the login form
	When the user enters valid credentials
	Then the user should see their name on the home page

@smoke @login	
Scenario: unsuccessful login with invalid credentials
	Given the user can see the login form
	When the user enters invalid credentials
	Then the user should see an error message
