using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;
using SeleniumExtras.WaitHelpers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;


[Binding]
public sealed class DeleteLocationStepDefinitions
{
    public record Location(double Latitude, double Longitude);
    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;

        // _driver starts on the page
        _driver.Navigate().GoToUrl("http://localhost:5205");
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Given("I am logged in to my account")]
    public void GivenIAmLoggedInToMyAccount()
    {
        // Navigate to the login page
        _driver.FindElement(By.Id("login")).Click();

        // Wait for the login page to load
        WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => d.FindElement(By.Id("login-email-field")).Displayed);

        // Fill in the login form
        var emailField = _driver.FindElement(By.Id("login-email-field"));
        var passwordField = _driver.FindElement(By.Id("login-password-field"));
        var loginButton = _driver.FindElement(By.Id("login-submit"));

        emailField.SendKeys("batman@email.com");
        passwordField.SendKeys("Password1!");
        loginButton.Click();
    }

    [When("I click on the map")]
    public void WhenIClickOnTheMap()
    {
        var locationCoords = new Location
        (
            Latitude: 51.5074,  // London coordinates
            Longitude: -0.1278
        );

        // Execute the JavaScript to simulate a click on the map
        ClickOnMap(locationCoords);
    }

    [Then("I see a popup with a save location button appear on the map")]
    public void ThenISeeAPopupAppearOnTheMap()
    {
        // Verify that the marker is displayed on the map
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var popup = wait.Until(d => d.FindElement(By.Id("leaflet-popup-content")));
        Assert.That(popup.Displayed, Is.True, "Popup should be displayed on the map");

        var saveButton = _driver.FindElement(By.Id("save-location-popup")); // I should have used a different name for this button
        Assert.That(saveButton.Displayed, Is.True, "Save location button should be displayed in the popup");
    }

    [When("I click the save location button")]
    public void WhenIClickTheSaveLocationButton()
    {
        var saveButton = _driver.FindElement(By.Id("save-location-popup"));
        saveButton.Click();
    }

    [Then("I see the save location form")]
    public void ThenISeeTheSaveLocationForm()
    {
        // Verify that the save location form is displayed
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
        var form = wait.Until(d => d.FindElement(By.Id("locationForm"))); // Unfortunately, this ID does not follow convention
        Assert.That(form.Displayed, Is.True, "Save location form should be displayed");
    }

    [When(@"I enter a valid title ""(.*)""")]
    public void WhenIEnterAValidTitle(string title)
    {
        var titleField = _driver.FindElement(By.Id("titleInput"));
        titleField.SendKeys(title);
    }

    [Then(@"I enter a valid address ""(.*)""")]
    public void ThenIEnterAValidAddress(string address)
    {
        var addressField = _driver.FindElement(By.Id("addressInput"));
        addressField.SendKeys(address);
    }

    [When(@"I click the submit button")]
    public void WhenIClickTheSubmitButton()
    {
        var saveButton = _driver.FindElement(By.Id("save-location-button"));
        saveButton.Click();
    }

    [Then(@"I should see a success alert")]
    public void ThenIShouldSeeASuccessAlert()
    {
        // Wait for the alert to appear
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var alert = wait.Until(ExpectedConditions.AlertIsPresent());
        
        // Verify alert text
        Assert.That(alert.Text.Contains("Location saved successfully!"), "Alert should contain success message");
        
        // Accept the alert (clicks OK)
        alert.Accept();
    }

    [Then(@"I should see a marker for ""(.*)"" on the map")]
    public void ThenIShouldSeeAMarkerForOnTheMap(string title)
    {
        // Wait for the map to update and show the new location
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        var marker = wait.Until(d => d.FindElement(By.Id(title)));
        
        Assert.That(marker.Displayed, Is.True, "Marker should be displayed on the map");
    }

    private void ClickOnMap(Location location)
    {
        string leafletClickScript = @"
            if (window.webfireMap) {
                window.webfireMap.fire('click', {
                    latlng: L.latLng(" + location.Latitude + "," + location.Longitude + @") 
                });
                return true;
            }
            return false;";

        var js = (IJavaScriptExecutor)_driver;
        var success = (bool)js.ExecuteScript(leafletClickScript);
        
        if (!success) throw new Exception("Map interaction failed - verify map library is loaded");
    }
}