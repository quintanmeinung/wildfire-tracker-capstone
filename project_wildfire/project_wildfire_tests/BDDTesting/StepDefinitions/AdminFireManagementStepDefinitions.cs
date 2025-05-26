using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using Reqnroll.Bindings;
using System;
using System.Threading;
using project_wildfire_tests.BDDTesting.Drivers;
using SeleniumExtras.WaitHelpers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class AdminFireManagementStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:5205";

        public AdminFireManagementStepDefinitions(ScenarioContext context)
        {
            _scenarioContext = context;
            WebDriverFactory.CreateDriver();
            _driver = WebDriverFactory.Driver;
        }

        [BeforeScenario]
        public static void BeforeScenario()
        {
            WebDriverFactory.CreateDriver();
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            WebDriverFactory.QuitDriver();
        }

        [Given("I am logged in as an admin")]
        public void GivenIAmLoggedInAsAnAdmin()
        {
            _driver.Navigate().GoToUrl(_baseUrl + "/Identity/Account/Login");
            _driver.FindElement(By.Id("login-email-field")).SendKeys("adminone@example.com");
            _driver.FindElement(By.Id("login-password-field")).SendKeys("Your09Password!");
            _driver.FindElement(By.Id("login-submit")).Click();
        }

        [Given("I click the \"(.*)\" button")]
        public void GivenIClickTheButton(string buttonText)
        {
            var button = _driver.FindElement(By.XPath($"//button[contains(text(), '{buttonText}')]"));
            button.Click();
        }

        [Given("I click on the map to place a fire")]
        public void GivenIClickOnTheMapToPlaceAFire()
        {
            try
            {
                IAlert alert = _driver.SwitchTo().Alert();
                Console.WriteLine($"Alert detected: {alert.Text}");
                alert.Accept();
            }
            catch (NoAlertPresentException) { }

            Thread.Sleep(2000);
            var mapElement = _driver.FindElement(By.Id("map"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(mapElement, 200, 200).Click().Perform();
        }

        [Then("a red fire marker should appear on the map")]
        public void ThenARedFireMarkerShouldAppearOnTheMap()
        {
            Thread.Sleep(2000);
            var redMarkers = _driver.FindElements(By.CssSelector(".admin-fire-marker"));
            Assert.That(redMarkers.Count > 0, "No red fire marker was found.");
        }

        [Given("I place a simulated fire on the map")]
        public void GivenIPlaceASimulatedFireOnTheMap()
        {
            GivenIClickTheButton("Create Simulated Fire");
            GivenIClickOnTheMapToPlaceAFire();
        }

        [Given("I log out")]
        public void GivenILogOut()
        {
            _driver.Navigate().GoToUrl(_baseUrl + "/Identity/Account/Logout");
            _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();
        }

        [Given("I log in as a regular user")]
        public void GivenILogInAsARegularUser()
        {
            _driver.Navigate().GoToUrl(_baseUrl + "/Identity/Account/Login");
            _driver.FindElement(By.Id("login-email-field")).SendKeys("userone@example.com");
            _driver.FindElement(By.Id("login-password-field")).SendKeys("User09Password!");
            _driver.FindElement(By.Id("login-submit")).Click();
        }

        [Then("I should see the admin-created fire marker")]
        public void ThenIShouldSeeTheAdmin_CreatedFireMarker()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(40));

            Thread.Sleep(2000); // Wait for map to settle

            var markers = wait.Until(driver =>
            {
                var candidates = driver.FindElements(By.CssSelector(".wildfire-marker"));
                return candidates.Count > 0 ? candidates : null;
            });

            foreach (var marker in markers)
            {
                try
                {
                    // Attempt to trigger the Leaflet popup
                    ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                        arguments[0].dispatchEvent(new MouseEvent('click', {
                            bubbles: true,
                            cancelable: true,
                            view: window
                        }));
                    ", marker);

                    Thread.Sleep(1000); // Let popup render

                    var popup = _driver.FindElement(By.ClassName("leaflet-popup-content"));
                    if (popup.Text.Contains("Subscribe to fire") || popup.Text.Contains("Delete Fire"))
                    {
                        Console.WriteLine("🔥 Popup found: " + popup.Text);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("⚠️ Marker click failed: " + ex.Message);
                }
            }

            Assert.Fail("No admin-created marker popup was successfully opened.");
        }

        [Then("I should see the \"(.*)\" button on the popup")]
        public void ThenIShouldSeeTheButtonOnThePopup(string buttonText)
        {
            Thread.Sleep(2000);
            var popup = _driver.FindElement(By.ClassName("leaflet-popup-content"));
            Assert.That(popup.Text.Contains(buttonText), $"Popup does not contain button with text: {buttonText}");
        }
    }
}



