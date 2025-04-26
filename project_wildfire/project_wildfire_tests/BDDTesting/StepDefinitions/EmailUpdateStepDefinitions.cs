using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;
using Reqnroll;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;
[Binding]
public class EmailUpdateStepDefinitions
{
    private static IWebDriver _driver;

    [BeforeScenario]
    public static void BeforeScenario()
    {
        WebDriverFactory.CreateDriver();
        _driver = WebDriverFactory.Driver;
    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Given(@"a user is logged in")]
    public void GivenAUserIsLoggedIn()
    {
        // Navigate to the base URL
        _driver.Navigate().GoToUrl("http://localhost:5205");

        // Wait for the base page to load
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        wait.Until(d => d.FindElement(By.TagName("body")));

        // Navigate to the login page
        _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Login");

        // Wait for the login page to stabilize
        wait.Until(d => d.FindElement(By.Id("login-email-field")));

        // Wait for a short duration to ensure the page does not disappear
        System.Threading.Thread.Sleep(2000);

        // Enter email
        var emailInput = _driver.FindElement(By.Id("login-email-field"));
        emailInput.Clear();
        emailInput.SendKeys("testuser@test.com");

        // Enter password
        var passwordInput = _driver.FindElement(By.Id("login-password-field"));
        passwordInput.Clear();
        passwordInput.SendKeys("Password123$");

        // Submit the form
        var loginButton = _driver.FindElement(By.Id("login-submit"));
        loginButton.Click();
    }

    [Given(@"the user is on the email update page")]
    public void GivenTheUserIsOnTheEmailUpdatePage()
    {
        // Navigate to the email update page
        _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/Email");

        // Wait for the page to fully load
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        wait.Until(d => d.FindElement(By.TagName("body")));
    }


}
