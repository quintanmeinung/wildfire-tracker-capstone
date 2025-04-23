using Microsoft.Extensions.Options;
using NUnit.Framework;
using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class EmailUpdateStepDefinitions
    {
        private IWebDriver _driver;

        [BeforeScenario]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-cache");
            options.AddArgument("--disable-application-cache");
            options.AddArgument("--disable-dev-shm-usage");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public void Dispose()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    

        [AfterScenario]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }

        [Given("a user is logged in")]
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
            wait.Until(d => d.FindElement(By.Id("Input_Email")));

            // Wait for a short duration to ensure the page does not disappear
            System.Threading.Thread.Sleep(2000);

            // Enter email
            //var emailInput = _driver.FindElement(By.Id("Input_Email"));
            //emailInput.Clear();
            //emailInput.SendKeys("testuser@test.com");

            _driver.FindElement(By.Id("Input_Email")).SendKeys("tester@test.com");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Password123$");
            _driver.FindElement(By.Id("login-submit")).Click();
            System.Threading.Thread.Sleep(2000);
        }

        [Given("the user is on the email update page")]
        public void GivenTheUserIsOnTheEmailUpdatePage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/Email");

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(100));

            // Suppress wildfire alert popup if it appears
            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                var alert = _driver.SwitchTo().Alert();
                Console.WriteLine("Suppressed alert: " + alert.Text);
                alert.Accept();
            }
            catch (WebDriverTimeoutException)
            {
                // No alert appeared, continue normally
            }

            wait.Until(d => d.FindElement(By.TagName("body")));
        }

        [When(@"the user enters a valid new email ""(.*)""")]
        public void WhenTheUserEntersAValidNewEmail(string newEmail)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var emailInput = _driver.FindElement(By.Id("newEmail"));
            //emailInput.Clear();
            //var wait2 = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            emailInput.SendKeys(newEmail);
           // var wait2 = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        [When("submits the form")]
        public void WhenSubmitsTheForm()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            // Make sure the form is fully loaded
            wait.Until(driver =>
            {
                var form = driver.FindElement(By.Id("update-email-form"));
                return form.Displayed;
            });

            Console.WriteLine("Submitting the form via JavaScript...");
            ((IJavaScriptExecutor)_driver).ExecuteScript("document.getElementById('update-email-form').dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));");

           // System.Threading.Thread.Sleep(3000);
        }

        [Then("the email should be updated successfully")]
        public void ThenTheEmailShouldBeUpdatedSuccessfully()
        {
        //var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

        try
        {
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
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
       // var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        try
        {
           // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
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
            //emailInput.Clear();
            //var wait2 = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));

            emailInput.SendKeys(newEmail);
           // var wait2 = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
        }

        [Then("an error message should be displayed")]
        public void ThenAnErrorMessageShouldBeDisplayed()
        {
       // var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        try
        {
           // wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
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
}




        