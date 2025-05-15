using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using NUnit.Framework;
using project_wildfire_tests.BDDTesting.Drivers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
[Binding]
public class MissingInputsStepDefinitions
{
    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;
        _driver.Navigate().GoToUrl("http://localhost:5205");
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Given(@"a user is submitting a new location")]
    public void GivenUserIsTryingToSubmitANewLocation()
    {
        _driver.FindElement(By.Id("login")).Click();
        _driver.FindElement(By.Id("login-email-field")).SendKeys("testuser@mail.com");
        _driver.FindElement(By.Id("login-password-field")).SendKeys("Password123$");
        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given(@"the user navigates to the saved locations page")]
    public void GivenUserIsOnSavedLocationsPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/SavedLocations");
    }

[When(@"the user submits the form without filling in the required fields")]
public void WhenUserSubmitsFormWithMissingFields()
{
    var submitButton = _driver.FindElement(By.Id("add-location-btn"));

    // Scroll into view using JavaScript
    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", submitButton);

    // Wait a short moment to ensure it’s in view
    Thread.Sleep(300);

    // Now click the button
    submitButton.Click();

    Thread.Sleep(500); // Wait for any validation messages or effects
}


    [Then(@"the form should not be submitted due to missing required fields")]
    public void ThenFormShouldNotBeSubmittedDueToMissingFields()
    {
        // If the page URL doesn't change, the form wasn't submitted
        var currentUrl = _driver.Url;
        Thread.Sleep(1000); // Wait in case of redirect
        var newUrl = _driver.Url;

        if (currentUrl != newUrl)
            throw new Exception("Form was submitted despite missing required fields.");
    }

    [Then(@"the location should not be added to the saved locations list")]
    public void ThenLocationShouldNotBeAdded()
    {
        var savedLocations = _driver.FindElements(By.CssSelector(".saved-location-title"));
        var invalidEntries = savedLocations.Any(loc => string.IsNullOrWhiteSpace(loc.Text));

        if (invalidEntries)
            throw new Exception("Invalid location entry was added.");
    }
}
}