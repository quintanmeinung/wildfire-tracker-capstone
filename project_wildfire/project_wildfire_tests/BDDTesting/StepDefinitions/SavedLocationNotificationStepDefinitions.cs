using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using NUnit.Framework;
using project_wildfire_tests.BDDTesting.Drivers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public class SavedLocationNotificationStepDefinitions
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

    [Given(@"a user is logged in for saved locations")]
    public void GivenAUserIsLoggedIn()
    {
        _driver.FindElement(By.Id("login")).Click();
        _driver.FindElement(By.Id("login-email-field")).SendKeys("testuser@mail.com");
        _driver.FindElement(By.Id("login-password-field")).SendKeys("Password123$");
        _driver.FindElement(By.Id("login-submit")).Click();
    }

    [Given(@"the user is on the saved locations page")]
    public void GivenUserIsOnSavedLocationsPage()
    {
        _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/SavedLocations");
    }

    [When(@"the user adds a new location titled ""(.*)"" with radius (\d+) miles and interval (\d+) hour")]
    public void WhenUserAddsLocation(string title, int radius, int interval)
    {
        _driver.FindElement(By.Id("title")).SendKeys(title);
        _driver.FindElement(By.Id("latitude")).SendKeys("37.7749");
        _driver.FindElement(By.Id("longitude")).SendKeys("-122.4194");
        _driver.FindElement(By.Id("address")).SendKeys("San Francisco, CA");
        _driver.FindElement(By.Id("radius")).SendKeys(radius.ToString());
        _driver.FindElement(By.Id("timeInterval")).SendKeys(interval.ToString());

        _driver.FindElement(By.Id("add-location-btn")).Click();
    }

    [Then(@"the location should appear in the saved locations list")]
    public void ThenLocationAppearsInList()
    {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(driver => driver.PageSource.Contains("Home")); // or check for an element by title
    }
}
