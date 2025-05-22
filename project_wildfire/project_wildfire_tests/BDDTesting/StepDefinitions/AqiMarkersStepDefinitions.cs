using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public sealed class AqiMarkersStepDefinitions
{
    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;

        _driver.Navigate().GoToUrl("http://localhost:5205"); // Adjust as needed
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Given(@"I am on the map page")]
    public void GivenIAmOnTheMapPage()
    {
        // Navigation is already done in BeforeScenario, but this doesn't hurt.
        _driver.Navigate().GoToUrl("http://localhost:5205/");
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }

[When(@"the AQI layer is loaded")]
public void WhenTheAQILayerIsLoaded()
{
    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

    // First hover to reveal the layer toggle panel
    var hoverElement = wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("leaflet-control-layers-toggle")));
    new Actions(_driver).MoveToElement(hoverElement).Click().Perform();

    // Wait for the AQI checkbox specifically by its label text
    var aqiCheckbox = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(
        "//label[span/span[text()=' AQI Stations']]/span/input[@type='checkbox']"
    )));

    // Click it if not already selected
    if (!aqiCheckbox.Selected)
    {
        aqiCheckbox.Click();
    }
}



[Then(@"I should see AQI station markers on the map")]
public void ThenIShouldSeeAQIStationMarkersOnTheMap()
{
    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
    bool markersVisible = wait.Until(driver =>
    {
        var paths = driver.FindElements(By.CssSelector("path.leaflet-interactive"));
        Console.WriteLine($"Found {paths.Count} AQI marker(s).");
        return paths.Count > 0;
    });

    var aqiMarkers = _driver.FindElements(By.CssSelector("path.leaflet-interactive"));
    Assert.That(aqiMarkers.Count, Is.GreaterThan(0), "No AQI station markers (SVG paths) were found on the map.");
}

}
