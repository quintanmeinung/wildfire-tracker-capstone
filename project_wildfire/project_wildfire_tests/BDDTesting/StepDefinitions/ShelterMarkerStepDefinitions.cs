using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reqnroll;
using System.Collections.ObjectModel;
using project_wildfire_tests.BDDTesting.Drivers;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class ShelterMarkerStepDefinitions
    {
        private readonly IWebDriver _driver;
        private List<(double lat, double lon)> shelterCoords = new();

        public ShelterMarkerStepDefinitions()
        {
            WebDriverFactory.CreateDriver(headless: true);
            _driver = WebDriverFactory.Driver;
        }

        [Given(@"there are no shelters")]
        public void GivenThereAreNoShelters() => shelterCoords.Clear();

        [Given(@"there are shelters available")]
        public void GivenThereAreSheltersAvailable()
        {
            shelterCoords = new List<(double lat, double lon)>
            {
                (45.512794, -122.679565),
                (45.523064, -122.676483),
            };
        }

        [Given(@"the map is visible")]
        public void GivenTheMapIsVisible()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var mapContainer = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("map")));
            Assert.That(mapContainer.Displayed, "Map is not visible.");
        }


        [BeforeScenario]
        public void BeforeScenario()
        {
            // Force reload of the map page to reset state
            _driver.Navigate().GoToUrl("http://localhost:5205/"); // 🔁 Replace with your local or deployed URL
            _driver.Navigate().Refresh();
        }

        [Given(@"a shelter marker is visible on the map")]
        public void GivenAShelterMarkerIsVisibleOnTheMap()
        {
            // Ensure map loads and marker layer is toggled on
            WhenTheMapLoadsAndEmergencySheltersAreToggledOn();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var marker = wait.Until(driver =>
            {
                var markers = driver.FindElements(By.CssSelector(".shelter-marker"));
                return markers.FirstOrDefault();
            });

            Assert.That(marker != null, "Expected at least one shelter marker to be visible on the map.");
        }

        [When(@"the map loads and emergency shelters are toggled on")]
        public void WhenTheMapLoadsAndEmergencySheltersAreToggledOn()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            // Step 1: Click the layers toggle button
            var toggle = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.ClassName("leaflet-control-layers-toggle")));
            toggle.Click();

            // Step 2: Wait until the checkbox's label span appears
            var labelSpan = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//span[normalize-space(text())='Emergency Shelters']")));

            // Step 3: Get the checkbox input before the label
            var checkbox = _driver.FindElement(By.XPath(
                "//span[normalize-space(text())='Emergency Shelters']/preceding-sibling::input"));

            if (!checkbox.Selected)
            {
                checkbox.Click();
            }
        }

        [When(@"I click on the shelter marker")]
        public void WhenIClickOnTheShelterMarker()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // ✅ Step 1: Wait until at least one shelter marker is present
            var marker = wait.Until(driver =>
            {
                var markers = driver.FindElements(By.CssSelector(".shelter-marker"));
                return markers.FirstOrDefault();
            });

            Assert.That(marker != null, "No shelter marker found to click.");

            // ✅ Step 2: Scroll the marker into view
            ((IJavaScriptExecutor)_driver).ExecuteScript(
                "arguments[0].scrollIntoView({block: 'center'});", marker);

            // ✅ Step 3: Optionally re-apply map zoom (fixes some Leaflet visibility issues)
            ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                const map = window._leaflet_map || document.querySelector('.leaflet-container')._leaflet_map;
                if (map) map.setZoom(map.getZoom());
            ");

            // ✅ Step 4: Let any animations or DOM shifts complete
            Thread.Sleep(500);

            // ✅ Step 5: Try to click; fallback to JS click if needed
            try
            {
                marker.Click();
            }
            catch (ElementNotInteractableException)
            {
                // Safe JS fallback that checks if click function exists
                ((IJavaScriptExecutor)_driver).ExecuteScript(@"
                    if (typeof arguments[0].click === 'function') {
                        arguments[0].click();
                    } else {
                        var event = document.createEvent('SVGEvents');
                        event.initEvent('click', true, true);
                        arguments[0].dispatchEvent(event);
                    }
                ", marker);
            }
        }

        [When(@"I toggle the fire hazard layer button")]
        public void WhenIToggleTheFireHazardLayerButton()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            var toggle = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.ClassName("leaflet-control-layers-toggle")));
            toggle.Click();

            var labelSpan = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//span[normalize-space(text())='Wildfire Hazard Potential']")));

            var checkbox = _driver.FindElement(By.XPath(
                "//span[normalize-space(text())='Wildfire Hazard Potential']/preceding-sibling::input"));
            checkbox.Click();
        }



        [Then(@"shelter markers should appear on the map")]
        public void ThenShelterMarkersShouldAppearOnTheMap()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            bool found = wait.Until(driver =>
            {
                var markers = _driver.FindElements(By.CssSelector(".shelter-marker"));
                return markers.Count > 0;
            });

            Assert.That(found, "Shelter markers did not appear after enabling Emergency Shelters.");
        }

        [Then(@"a popup should appear with the shelter’s name, address, and status")]
        public void ThenAPopupShouldAppearWithTheShelterNameAddressAndStatus()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Look for the Leaflet popup container
            var popup = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("leaflet-popup-content")));

            var content = popup.Text;
            Console.WriteLine("Popup Content: " + content);

            Assert.That(content, Does.Contain("Address").IgnoreCase, "Popup missing address info.");
            Assert.That(content, Does.Contain("Status").IgnoreCase, "Popup missing status info.");
            Assert.That(content.Length, Is.GreaterThan(10), "Popup seems too empty to be valid.");
        }

        [Then(@"the fire hazard layer should be displayed on the map")]
        public void ThenTheFireHazardLayerShouldBeDisplayedOnTheMap()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            // Wait until at least one tile is present and fully loaded (complete == true)
            wait.Until(driver =>
            {
                var tiles = driver.FindElements(By.CssSelector("img.leaflet-tile"));
                return tiles.Any(tile =>
                {
                    var isComplete = (bool)((IJavaScriptExecutor)driver)
                        .ExecuteScript("return arguments[0].complete && arguments[0].naturalWidth > 0;", tile);
                    return isComplete;
                });
            });

            var allTiles = _driver.FindElements(By.CssSelector("img.leaflet-tile"));
            Assert.That(allTiles.Count, Is.GreaterThan(0), "Fire hazard tiles exist but none are fully loaded.");
        }


        [Then(@"the toggle button should reflect the active state")]
        public void ThenTheToggleButtonShouldReflectTheActiveState()
        {
            var checkbox = _driver.FindElement(By.XPath(
                "//span[normalize-space(text())='Wildfire Hazard Potential']/preceding-sibling::input"));
            Assert.That(checkbox.Selected, "Fire hazard layer toggle should be checked.");
        }

        [Then(@"the fire hazard layer should be hidden")]
        public void ThenTheFireHazardLayerShouldBeHidden()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // ✅ STEP 1: Debug the map and layer state
            var result = (Dictionary<string, object>)((IJavaScriptExecutor)_driver).ExecuteScript(@"
                const map = window._leaflet_map || document.querySelector('.leaflet-container')._leaflet_map;
                return {
                    fireHazardDefined: typeof window.fireHazardLayer !== 'undefined',
                    fireHazardAttached: map && window.fireHazardLayer ? map.hasLayer(window.fireHazardLayer) : null
                };
            ");

            Console.WriteLine($"🔥 fireHazardLayer debug: defined={result["fireHazardDefined"]}, attached={result["fireHazardAttached"]}");

            // ✅ STEP 2: Wait for tiles to be invisible (this still works if needed)
            bool isDetached = wait.Until(driver =>
            {
                return (bool)((IJavaScriptExecutor)driver).ExecuteScript(@"
                    const map = window._leaflet_map;
                    return map && window.fireHazardLayer && !map.hasLayer(window.fireHazardLayer);
                ");
            });

            Assert.That(isDetached, "Fire hazard layer is still attached to the map after toggle off.");
        }

        [Then(@"the toggle button should reflect the inactive state")]
        public void ThenTheToggleButtonShouldReflectTheInactiveState()
        {
            var checkbox = _driver.FindElement(By.XPath(
                "//span[normalize-space(text())='Wildfire Hazard Potential']/preceding-sibling::input"));
            Assert.That(!checkbox.Selected, "Fire hazard layer toggle should be unchecked.");
        }
    }
}


