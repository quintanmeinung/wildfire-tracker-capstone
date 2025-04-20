/* using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace MapUITests
{
    public class MapMarkerTest
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void MarkerAppearsOnMap()
        {
            driver.Navigate().GoToUrl("http://localhost:5205/");

            // Wait for map + JS to fully load
            Thread.Sleep(5000); // You can replace this with WebDriverWait for smarter handling

            // Check for markers (Leaflet uses this class)
            var markers = driver.FindElements(By.ClassName("leaflet-marker-icon"));

            Assert.That(markers.Count > 0, "No wildfire markers were found on the map."); 
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}
 */