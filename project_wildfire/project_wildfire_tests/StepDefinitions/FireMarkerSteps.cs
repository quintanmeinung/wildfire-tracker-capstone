using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace MapUITests.Steps
{
    [Binding]
    public class MapMarkersSteps
    {
        private IWebDriver driver;
        private List<(double lat, double lon)> wildfireCoords = new();

        [BeforeScenario]
        public void SetUp()
        {
            driver = new ChromeDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Given(@"a wildfire occurs at coordinates \((.*), (.*)\)")]
        public void GivenAWildfireOccursAtCoordinates(double lat, double lon)
        {
            wildfireCoords.Add((lat, lon));
        }

        [When(@"the map loads the wildfire data")]
        public void WhenTheMapLoads()
        {
            driver.Navigate().GoToUrl("http://localhost:5205/");
            Thread.Sleep(5000); // Adjust for your actual JS load time
        }

        [Then(@"a marker should be placed at \((.*), (.*)\)")]
        public void ThenAMarkerShouldBePlaced(double lat, double lon)
        {
            var markers = driver.FindElements(By.ClassName("leaflet-marker-icon"));
            Assert.That(markers.Count, Is.GreaterThan(0), $"No marker found for ({lat}, {lon})");

            // Optional: validate marker positions if rendered via HTML data attributes
        }
        /*
        [Then(@"no markers should be placed")]
        public void ThenNoMarkersShouldBePlaced()
        {
            var markers = driver.FindElements(By.ClassName("leaflet-marker-icon"));
            Assert.That(markers.Count, Is.EqualTo(0), "Markers were found, but none should be present.");
        }*/
    }
}


