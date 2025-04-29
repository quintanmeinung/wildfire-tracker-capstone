using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using System.Collections.ObjectModel;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public class MapMarkersStepDefinitions
{
    private IWebDriver _driver;
    private List<(double lat, double lon)> wildfireCoords = new();

    // Changed [BeforeScenario] to a constructor for driver setup.
    // Also added a headless option for the Chrome driver.
    public MapMarkersStepDefinitions()
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
        // This version uses the last added test case to determine the mode
        string url = wildfireCoords.Count switch
        {
            0 => "http://localhost:5205/?test=no-data",
            1 => "http://localhost:5205/?test=single",
            _ => "http://localhost:5205/?test=multiple"
        };

        _driver.Navigate().GoToUrl("http://localhost:5205/?test=true");
        Thread.Sleep(5000);
        var js = (IJavaScriptExecutor)_driver;
        string debugScript = @"
        const markers = document.querySelectorAll('path.wildfire-marker');
        return Array.from(markers).map(el => ({
            lat: el.getAttribute('data-lat'),
            lon: el.getAttribute('data-lon'),
            outerHTML: el.outerHTML
        }));
        ";
        var debugResults = js.ExecuteScript(debugScript);

        Console.WriteLine("🔍 Dumping all wildfire-marker elements found:");
        Console.WriteLine(debugResults.ToString());
    }

    [Then(@"a marker should be placed at \((.*), (.*)\)")]
    public void ThenAMarkerShouldBePlaced(double expectedLat, double expectedLon)
    {
        var js = (IJavaScriptExecutor)_driver;

        string script = @"
            return Array.from(document.querySelectorAll('path.wildfire-marker')).map(el => ({
                lat: el.getAttribute('data-lat'),
                lon: el.getAttribute('data-lon')
            }));
        ";

        bool matchFound = false;
        int attempts = 0;

        while (!matchFound && attempts < 20)
        {
            Thread.Sleep(250);

            var results = (ReadOnlyCollection<object>)js.ExecuteScript(script);
            Console.WriteLine($"JS Attempt {attempts + 1}: Found {results.Count} wildfire markers.");

            foreach (var result in results)
            {
                var marker = (Dictionary<string, object>)result;

                if (double.TryParse(marker["lat"]?.ToString(), out double lat) &&
                    double.TryParse(marker["lon"]?.ToString(), out double lon))
                {
                    if (Math.Abs(lat - expectedLat) < 0.0001 && Math.Abs(lon - expectedLon) < 0.0001)
                    {
                        matchFound = true;
                        Console.WriteLine($"Matched marker at ({lat}, {lon})");
                        break;
                    }
                }
            }

            attempts++;
        }

        Assert.That(matchFound, Is.True, $"No wildfire marker found at ({expectedLat}, {expectedLon}) after {attempts} attempts.");
    }

    
    [Then(@"no markers should be placed")]
    public void ThenNoMarkersShouldBePlaced()
    {
        var markers = _driver.FindElements(By.ClassName("leaflet-marker-icon"));
        Console.WriteLine($"Marker Count Found: {markers.Count}");
        foreach (var marker in markers)
        {
            Console.WriteLine($"Marker HTML: {marker.GetAttribute("outerHTML")}");
        }

        Assert.That(markers.Count, Is.EqualTo(0), "Markers were found, but none should be present.");
    }
}



