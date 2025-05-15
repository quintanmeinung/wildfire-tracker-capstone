using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using NUnit.Framework;
using project_wildfire_tests.BDDTesting.Drivers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class SavedLocationDeletionStepDefinitions
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

        [Given(@"a user is logged in and on the saved locations page")]
        public void GivenUserIsLoggedInAndOnTheSavedLocationsPage()
        {
            _driver.FindElement(By.Id("login")).Click();
            _driver.FindElement(By.Id("login-email-field")).SendKeys("testuser@mail.com");
            _driver.FindElement(By.Id("login-password-field")).SendKeys("Password123$");
            _driver.FindElement(By.Id("login-submit")).Click();

            _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/SavedLocations");
        }

        [Given(@"the user has a saved location")]
        public void GivenUserHasASavedLocation()
        {
            // Check if there are saved locations, or add one if not
            var savedLocations = _driver.FindElements(By.CssSelector(".saved-location-title"));
            if (!savedLocations.Any())
            {
                // Add a location if none exists
                _driver.FindElement(By.Id("title")).SendKeys("Test Location");
                _driver.FindElement(By.Id("latitude")).SendKeys("37.7749");
                _driver.FindElement(By.Id("longitude")).SendKeys("-122.4194");
                _driver.FindElement(By.Id("address")).SendKeys("San Francisco, CA");
                _driver.FindElement(By.Id("radius")).SendKeys("5");
                _driver.FindElement(By.Id("timeInterval")).SendKeys("1");
                _driver.FindElement(By.Id("add-location-btn")).Click();
                Thread.Sleep(2000); // Wait for the location to be added
            }
        }

        [When(@"the user deletes the saved location")]
        public void WhenUserDeletesTheSavedLocation()
        {
            // Click the delete button for the first saved location
            var deleteButton = _driver.FindElement(By.Id("delete-location-btn"));
            deleteButton.Click();
            Thread.Sleep(1000); // Wait for the location to be deleted (this can be adjusted based on your app's behavior)
        }

        [Then(@"the location should no longer appear in the saved locations list")]
        public void ThenLocationShouldNoLongerAppearInTheSavedLocationsList()
        {
            var savedLocations = _driver.FindElements(By.CssSelector(".saved-location-title"));
            bool locationDeleted = savedLocations.All(loc => loc.Text != "Test Location");

            if (!locationDeleted)
                throw new Exception("The location was not deleted successfully.");
        }
    }
}
