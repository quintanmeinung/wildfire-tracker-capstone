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
        _driver.Navigate().GoToUrl("http://localhost:5205");

    }

    [AfterScenario]
    public static void AfterScenario()
    {
        WebDriverFactory.QuitDriver();
    }

    [Given(@"a user is logged in")]
    public void GivenAUserIsLoggedIn()
    {
        
        _driver.FindElement(By.Id("login")).Click();

        // Enter email
        var emailInput = _driver.FindElement(By.Id("login-email-field"));
        emailInput.Clear();
        emailInput.SendKeys("testuser@mail.com");

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

    [When(@"the user enters a valid new email ""(.*)""")]

    public void WhenTheUserEntersAValidNewEmail(string newEmail)

    {

       // var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

        var emailInput = _driver.FindElement(By.Id("newEmail"));
        var submitButton = _driver.FindElement(By.Id("email-update-submit"));
        emailInput.SendKeys(newEmail);
        submitButton.Click();

    }

    [Then("the email should be updated successfully")]

    public void ThenTheEmailShouldBeUpdatedSuccessfully()

    {

        try
        {
        var alert = _driver.SwitchTo().Alert();
        Console.WriteLine("Unexpected alert: " + alert.Text);
        alert.Accept();
        }
        catch (NoAlertPresentException)
        {
        // No alert appeared
        }
        var successMessage = _driver.FindElement(By.Id("email-update-success"));
        Assert.That(successMessage.Text, Does.Contain("Your email has been updated successfully!"));

    }

    [Then("a success message should be displayed")]

    public void ThenASuccessMessageShouldBeDisplayed()

    {

        try
        {
        var alert = _driver.SwitchTo().Alert();
        Console.WriteLine("Unexpected alert: " + alert.Text);
        alert.Accept();
        }
        catch (NoAlertPresentException)
        {
        // No alert appeared
        }
        var successMessage = _driver.FindElement(By.Id("email-update-success"));
        Assert.That(successMessage.Text, Does.Contain("Your email has been updated successfully!"));

    }

    
    [When(@"the user enters an invalid email ""(.*)""")]

    public void WhenTheUserEntersAnInvalidNewEmail(string newEmail)

    {

        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

        var emailInput = _driver.FindElement(By.Id("newEmail"));
        var submitButton = _driver.FindElement(By.Id("email-update-submit"));
        emailInput.SendKeys(newEmail);
        submitButton.Click();

    }

    [Then("an error message should be displayed")]

    public void ThenAnErrorMessageShouldBeDisplayed()

    {

        try
        {
            var alert = _driver.SwitchTo().Alert();
            Console.WriteLine("Unexpected alert: " + alert.Text);
            alert.Accept();
        }

        catch (NoAlertPresentException)
        {
            // No alert appeared
        }

        var elements = _driver.FindElements(By.Id("email-update-success"));
        Assert.That(elements.Count, Is.EqualTo(0), "Unexpected success message displayed!");

    }


    }


