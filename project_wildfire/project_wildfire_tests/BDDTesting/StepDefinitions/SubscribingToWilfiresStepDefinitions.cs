using project_wildfire_tests.BDDTesting.StepDefinitions;
using OpenQA.Selenium;
using Reqnroll;
using project_wildfire_tests.BDDTesting.Drivers;
using NUnit.Framework;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class SubscribingToWildfiresStepDefinitions
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

       

        [When("I navigate to the map page")]
        public void WhenTheUserNavigatesToTheMapPage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5205");
        }

        [When("I click on the fire marker with data-fire-id {string}")]
        public void WhenTheUserClicksOnTheFireMarkerWithDataFireId(string fireId)
        {
            var markerButton = _driver.FindElement(By.CssSelector($".subscribe-btn[data-fire-id='{fireId}']"));
            markerButton.Click();
        }

        [When("I click the \"Subscribe to Fire\" button")]
        public void WhenTheUserClicksTheSubscribeToFireButton()
        {
            var subscribeButton = _driver.FindElement(By.CssSelector(".subscribe-btn"));
            subscribeButton.Click();
        }

        [Then("I should see a toast message {string}")]
        public void ThenAToastMessageShouldBeDisplayed(string message)
        {
            var toast = _driver.FindElement(By.CssSelector(".toast-message"));
            Assert.That(toast.Text, Does.Contain(message));
        }

        [Then("the fire with id {string} appears in the subscribed fires list")]
        public void ThenTheFireWithIdAppearsInTheSubscribedFiresList(string fireId)
        {
            var unsubscribeButton = _driver.FindElement(By.CssSelector($".unsubscribe-btn[data-fire-id='{fireId}']"));
            //Assert.IsNotNull(unsubscribeButton);
            Assert.That(unsubscribeButton, Is.Not.Null);
        }

        [Given("the user has subscribed to the fire with id {string}")]
        public void GivenTheUserHasSubscribedToTheFireWithId(string fireId)
        {
            WhenTheUserClicksOnTheFireMarkerWithDataFireId(fireId);
            WhenTheUserClicksTheSubscribeToFireButton();
        }

        [When("the user opens the profile modal")]
        public void WhenTheUserOpensTheProfileModal()
        {
            var profileLink = _driver.FindElement(By.Id("profile"));
            profileLink.Click();
        }

        [When("the user clicks the \"Unsubscribe\" button for fire {string}")]
        public void WhenTheUserClicksTheUnsubscribeButtonForFire(string fireId)
        {
            var unsubscribeButton = _driver.FindElement(By.CssSelector($".unsubscribe-btn[data-fire-id='{fireId}']"));
            unsubscribeButton.Click();
        }

        [Then("the fire with id {string} should no longer appear in the subscribed fires list")]
        public void ThenTheFireWithIdShouldNoLongerAppearInTheSubscribedFiresList(string fireId)
        {
            var elements = _driver.FindElements(By.CssSelector($".unsubscribe-btn[data-fire-id='{fireId}']"));
            Assert.That(elements.Count, Is.EqualTo(0));
        }
    }
}


