using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public sealed class WildfireMarkerStepDefinitions
{
    private ChromeDriver _driver;
    private List<(double lat, double lon)> wildfireCoords = new();

    // Changed [BeforeScenario] to a constructor for driver setup.
    // Also added a headless option for the Chrome driver.
    public WildfireMarkerStepDefinitions()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());

        var options = new ChromeOptions();
        options.AddArguments("--headless");

        _driver = new ChromeDriver(options);

    }

    [AfterScenario]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Given(@"a wildfire occurs at coordinates \((.*), (.*)\)")]
    public void GivenAWildfireOccursAtCoordinates(double lat, double lon)
    {
        wildfireCoords.Add((lat, lon));
    }

    [Given(@"there are no wildfires")]
    public void GivenThereAreNoWildfires()
    {
        wildfireCoords.Clear();
    }

    [When(@"the map loads the wildfire data")]
    public void WhenTheMapLoads()
    {
        _driver.Navigate().GoToUrl("http://localhost:5205/");
        Thread.Sleep(5000); // Adjust for your actual JS load time
    }

    [Then(@"a marker should be placed at \((.*), (.*)\)")]
    public void ThenAMarkerShouldBePlaced(double lat, double lon)
    {
        var markers = _driver.FindElements(By.ClassName("leaflet-marker-icon"));
        Assert.That(markers.Count, Is.GreaterThan(0), $"No marker found for ({lat}, {lon})");
    }
    
    [Then(@"no markers should be placed")]
    public void ThenNoMarkersShouldBePlaced()
    {
        var markers = _driver.FindElements(By.ClassName("leaflet-marker-icon"));
        Assert.That(markers.Count, Is.EqualTo(0), "Markers were found, but none should be present.");
    }
}



