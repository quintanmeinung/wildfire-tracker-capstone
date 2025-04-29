using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System;
using System.Linq;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using System.Globalization;



namespace project_wildfire_tests.StepDefinitions
{
    [Binding]
    public class WildfireMapSteps
    {
        private readonly IWebDriver _driver;

        public WildfireMapSteps(ScenarioContext scenarioContext)
        {
            _driver = scenarioContext.Get<IWebDriver>();
        }

        //Test to make sure we are on the webpage on current date
        [Given(@"I am on the wildfire map page")]
        public void GivenIAmOnTheWildfireMapPage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5205/"); // adjust to your dev URL
            Thread.Sleep(2000); // Give Leaflet and map time to initialize
        }

        [When(@"the page loads")]
        public void WhenThePageLoads()
        {
            // Wait for fire markers to appear
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElements(By.ClassName("leaflet-interactive")).Any());
        }

        [Then(@"I should see wildfire markers on the map")]
        public void ThenIShouldSeeWildfireMarkersOnTheMap()
        {
            var markers = _driver.FindElements(By.ClassName("leaflet-interactive"));
            if (markers.Count == 0)
                throw new Exception("No fire markers found on the map.");
        }

        //Change the test case to pick a different date
        //Make sure fire markers are not the same as before
        [When(@"I select a different valid date")]
        public void WhenISelectADifferentValidDate()
        {
            var dateInput = _driver.FindElement(By.Id("fire-date"));

            var minDateStr = dateInput.GetAttribute("min");
            var maxDateStr = dateInput.GetAttribute("max");

            Console.WriteLine($"🛠️ [DEBUG] Page min date = {minDateStr}, max date = {maxDateStr}");

            var minDate = DateTime.Parse(minDateStr);
            var safeDate = minDate.AddDays(1);
            var safeDateStr = safeDate.ToString("yyyy-MM-dd");

            Console.WriteLine($"✅ [Test] Picking valid safe date: {safeDateStr}");

            // ✅ Set value through JavaScript instead of SendKeys
            var jsExecutor = (IJavaScriptExecutor)_driver;
            jsExecutor.ExecuteScript($"arguments[0].value = '{safeDateStr}';", dateInput);
        }

        [When(@"I click the filter date button")]
        public void WhenIClickTheFilterDateButton()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the button to be present in the DOM and visible
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("filter-date-btn")));

            // Wait until it’s clickable (extra layer)
            var filterBtn = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("filter-date-btn")));

            // Scroll into view if needed
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", filterBtn);

            // Slight delay to ensure no overlay (optional but helps)
            Thread.Sleep(500);

            // Try click
            filterBtn.Click();
        }

        [Then(@"I should see wildfire markers updated on the map")]
        public void ThenIShouldSeeWildfireMarkersUpdatedOnTheMap()
        {
            var markers = _driver.FindElements(By.ClassName("leaflet-interactive"));
            if (markers.Count == 0)
                throw new Exception("No wildfire markers appeared after date filter.");
        }

        //Test to make sure no wildfire markers show up on old dates
        [When(@"I select a date with no wildfires")]
        public void WhenISelectADateWithNoWildfires()
        {
            var dateInput = _driver.FindElement(By.Id("fire-date"));

            // Use a far past date (before fires in the dataset, adjust if needed)
            var noFireDate = new DateTime(2025, 03, 11).ToString("yyyy-MM-dd");

            dateInput.Clear();
            dateInput.SendKeys(noFireDate);
        }

        [Then(@"I should see a ""(.*)"" alert")]
        public void ThenIShouldSeeAnAlert(string expectedMessage)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                var alert = _driver.SwitchTo().Alert();
                var actualText = alert.Text;

                if (!actualText.Contains(expectedMessage))
                    throw new Exception($"Expected alert to say \"{expectedMessage}\", but got \"{actualText}\".");

                alert.Accept(); // Close the alert
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("No alert appeared.");
            }
        }

        [When(@"I click on a wildfire marker")]
        public void WhenIClickOnAWildfireMarker()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            wait.Until(d =>
            {
                var count = d.FindElements(By.ClassName("wildfire-marker")).Count;
                return count > 0;
            });

            var markers = _driver.FindElements(By.ClassName("wildfire-marker")).ToList(); // ❗ No Displayed check

            if (!markers.Any())
                throw new Exception("❌ No wildfire markers found to click.");

            foreach (var marker in markers)
            {
                try
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center' });", marker);
                    Thread.Sleep(300);

                    // Force a JS click instead of Selenium Click
                    ((IJavaScriptExecutor)_driver).ExecuteScript(
                        "arguments[0].dispatchEvent(new MouseEvent('click', { bubbles: true }));", marker
                    );

                    Console.WriteLine("✅ Successfully clicked wildfire marker via JavaScript!");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Failed to click marker: {ex.Message}");
                }
            }

            throw new Exception("❌ Could not click any wildfire marker after retries.");
        }


        [Then(@"I should see radiative power, latitude, and longitude in the popup")]
        public void ThenIShouldSeeRadiativePowerLatitudeLongitudeInPopup()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait until a popup appears
            wait.Until(d => d.FindElement(By.ClassName("leaflet-popup-content")));

            var popup = _driver.FindElement(By.ClassName("leaflet-popup-content"));
            var popupText = popup.Text;

            Console.WriteLine($"[DEBUG] Popup text: {popupText}");

            bool hasRadiativePower = popupText.Contains("Radiative Power", StringComparison.OrdinalIgnoreCase);
            bool hasLatitude = popupText.Contains("Latitude", StringComparison.OrdinalIgnoreCase);
            bool hasLongitude = popupText.Contains("Longitude", StringComparison.OrdinalIgnoreCase);

            if (!hasRadiativePower || !hasLatitude || !hasLongitude)
            {
                throw new Exception($"Popup does not contain expected wildfire info. Full popup text:\n{popupText}");
            }
        }
    }
}
