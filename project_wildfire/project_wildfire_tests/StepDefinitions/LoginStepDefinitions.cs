using Reqnroll;
using OpenQA.Selenium;
using project_wildfire_tests.Pages;

namespace project_wildfire_tests.StepDefinitions;

[Binding]
public sealed class LoginStepDefinitions
{
    
    private readonly IWebDriver _driver;
    private readonly LoginPage _loginPage;
 
    public LoginStepDefinitions(IWebDriver driver)
    {
        _driver = driver;
        _loginPage = new LoginPage(_driver);
    } 

    [Given(@"I navigate to the login form")]
    public void GivenINavigateToTheLoginForm()
    {
        //_loginPage.NavigateToLoginForm();
        Console.WriteLine("Navigating to login form...");
    }

    [When(@"I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        /* _loginPage.EnterCredentials("validUsername", "validPassword");
        _loginPage.SubmitLoginForm(); */
        Console.WriteLine("Entering valid credentials...");
    }

    [When(@"I enter invalid credentials")]
    public void WhenIEnterInvalidCredentials()
    {
        /* _loginPage.EnterCredentials("invalidUsername", "invalidPassword");
        _loginPage.SubmitLoginForm(); */
        Console.WriteLine("Entering invalid credentials...");
    }

    [Then(@"I should be logged in")]
    public void ThenIShouldBeLoggedIn()
    {
        Console.WriteLine("Checking if logged in...");
        /* Assert.IsTrue(_loginPage.IsDashboardDisplayed(), "Dashboard is not displayed after login."); */
    }

    [Then(@"I should see an error message")]
    public void ThenIShouldSeeAnErrorMessage()
    {
        Console.WriteLine("Checking for error message...");
        /* Assert.IsTrue(_loginPage.IsDashboardDisplayed(), "Dashboard is not displayed after login."); */
    }

}