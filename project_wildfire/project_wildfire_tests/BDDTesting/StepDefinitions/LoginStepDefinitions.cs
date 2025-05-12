using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public sealed class LoginStepDefinitions
{
    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;

        // _driver starts on the page
        _driver.Navigate().GoToUrl("http://localhost:5205");
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Scope(Tag = "login")]
    [Scope(Tag = "smoke")]
    [Given(@"I navigate to the login form")]
    public void GivenINavigateToTheLoginForm()
    {
        Console.WriteLine("Navigating to login form...");
        _driver.FindElement(By.Id("login")).Click();
    }

    [Scope(Tag = "login")]
    [Scope(Tag = "smoke")]
    [When(@"I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        Console.WriteLine("Entering valid credentials...");
        var emailField = _driver.FindElement(By.Id("login-email-field"));
        var passwordField = _driver.FindElement(By.Id("login-password-field"));
        var loginButton = _driver.FindElement(By.Id("login-submit"));

        // Valid email; it exists in the database
        emailField.SendKeys("batman@email.com");

        // Password also valid
        passwordField.SendKeys("Password1!");

        loginButton.Click();
    }

    [Scope(Tag = "login")]
    [Scope(Tag = "smoke")]
    [When(@"I enter invalid credentials")]
    public void WhenIEnterInvalidCredentials()
    {
        Console.WriteLine("Entering invalid credentials...");
        var emailField = _driver.FindElement(By.Id("login-email-field"));
        var passwordField = _driver.FindElement(By.Id("login-password-field"));
        var loginButton = _driver.FindElement(By.Id("login-submit"));

        // Invalid email; it doesn't exist in the database
        emailField.SendKeys("nobody@real.com");

        // Password is valid
        passwordField.SendKeys("Password1!");

        loginButton.Click();
    }

    [Scope(Tag = "login")]
    [Scope(Tag = "smoke")]
    [Then(@"I should be logged in")]
    public void ThenIShouldBeLoggedIn()
    {
        Console.WriteLine("Checking if logged in...");
        
        // Try to find the element for 10 seconds
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        var greeting = wait.Until(drv => drv.FindElement(By.Id("manage")));

        // Check if the element is displayed
        Assert.That(greeting.Displayed, Is.True, "User is not logged in.");
    }

    [Scope(Tag = "login")]
    [Scope(Tag = "smoke")]
    [Then(@"I should see an error message")]
    public void ThenIShouldSeeAnErrorMessage()
    {
        Console.WriteLine("Checking for error message...");

        // Try to find the error message for 3 seconds
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        var errorMessage = wait.Until(drv => drv.FindElement(By.ClassName("validation-summary-errors")));

        // Check if the error message is displayed
        Assert.That(errorMessage.Displayed, Is.True, "Error message is not displayed.");
    }

}