using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;
using OpenQA.Selenium.Interactions;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public sealed class UserLocationStepDefinitions
{

    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Scope(Tag = "user_location")]
    [Given(@"the user is logged in to their account")]
    public void GivenTheUserIsLoggedInToTheirAccount()
    {
        _driver.Navigate().GoToUrl("http://localhost:5205");
        _driver.FindElement(By.Id("login")).Click();

        // Wait for the login page to loag
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.Id("login-email-field")));

        // Enter valid credentials
        var emailField = _driver.FindElement(By.Id("login-email-field"));
        var passwordField = _driver.FindElement(By.Id("login-password-field"));
        var loginButton = _driver.FindElement(By.Id("login-submit"));

        emailField.SendKeys("batman@email.com");
        passwordField.SendKeys("Password1!");
        loginButton.Click();
        // We assume login is successful
        // The scope of this test does not include login functionality
    }

    [Scope(Tag = "user_location")]
    [When(@"they click on the interactive map")]
    public void WhenTheyClickOnTheInteractiveMap()
    {
        var mapElement = _driver.FindElement(By.ClassName("leaflet-container"));
    
        new Actions(_driver)
            .MoveToElement(mapElement, 50, 50) // Safe offset from corner
            .Click()
            .Perform();
    }
    
    
    [Scope(Tag = "user_location")]
    [When(@"they submit a valid title")]// 'And' steps use the 'When' attribute
    public void AndTheySubmitAValidTitle() 
    {
        // Wait for the title input field to be present
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElement(By.Id("titleInput")));

        // Enter a valid title
        var titleInput = _driver.FindElement(By.Id("titleInput"));
        titleInput.SendKeys("Valid Test Title");
    }

    [Scope(Tag = "user_location")]
    [Then("they should see a success message")]
    public void ThenTheyShouldSeeASuccessMessage()
    {
        // Wait for the success message to be present
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElement(By.ClassName("success-message")));

        // Check if the success message is displayed
        var successMessage = _driver.FindElement(By.ClassName("success-message"));
        Assert.That(successMessage.Displayed, Is.True, "Success message was not displayed.");
    }

    [Scope(Tag = "user_location")]
    [When(@"they submit an invalid title")]
    public void AndTheySubmitAnInvalidTitle()
    {
        // Does nothing to simulate empty title input
    }

    [Scope(Tag = "user_location")]
    [Then(@"they should see an error message")]
    public void ThenTheyShouldSeeAnErrorMessage()
    {
        // Wait for the error message to be present
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElement(By.ClassName("error-message")));

        // Check if the error message is displayed
        var errorMessage = _driver.FindElement(By.ClassName("error-message"));
        Assert.That(errorMessage.Displayed, Is.True, "Error message was not displayed.");
    }

}