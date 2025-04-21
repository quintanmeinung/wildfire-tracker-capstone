using Reqnroll;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

[Binding]
public sealed class LoginStepDefinitions
{
    
    private readonly FirefoxDriver _driver;
    
    public LoginStepDefinitions()
    {
        new DriverManager().SetUpDriver(new FirefoxConfig());
        
        var options = new FirefoxOptions();
        options.AddArguments("--headless");
        
        _driver = new FirefoxDriver(options);
        _driver.Navigate().GoToUrl("http://localhost:5205");
    } 

    [AfterScenario]
    public void AfterScenario()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Given(@"I navigate to the login form")]
    public void GivenINavigateToTheLoginForm()
    {
        Console.WriteLine("Navigating to login form...");
    }

    [When(@"I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        Console.WriteLine("Entering valid credentials...");
    }

    [When(@"I enter invalid credentials")]
    public void WhenIEnterInvalidCredentials()
    {
        Console.WriteLine("Entering invalid credentials...");
    }

    [Then(@"I should be logged in")]
    public void ThenIShouldBeLoggedIn()
    {
        Console.WriteLine("Checking if logged in...");
    }

    [Then(@"I should see an error message")]
    public void ThenIShouldSeeAnErrorMessage()
    {
        Console.WriteLine("Checking for error message...");
    }

}