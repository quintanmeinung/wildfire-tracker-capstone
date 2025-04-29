using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;
using Reqnroll;
using NUnit.Framework;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class UpdatePhoneNumberStepDefinitions
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

        [Given(@"a user is signed in")]
        public void GivenAUserIsLoggedIn()
        {
           

            // Wait for the base page to load
             var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            _driver.FindElement(By.Id("login")).Click();      
            wait.Until(d => d.FindElement(By.Id("login-email-field")));

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

        [Given(@"the user is on the phone number update page")]
        public void GivenTheUserIsOnThePhoneNumberUpdatePage()
        {
            _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/PhoneNumber");
            
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.TagName("body")));
        }

        [When(@"the user enters a valid new phone number {string}")]
        public void WhenTheUserEntersAValidNewPhoneNumber(string phoneNumber)
        {
          var phoneInput = _driver.FindElement(By.Id("PhoneNumber"));
          var submitButton = _driver.FindElement(By.Id("phone-number-submit"));

          phoneInput.Clear();
          phoneInput.SendKeys(phoneNumber);
          submitButton.Click();

        }

       

        [Then("the phone number should be updated successfully")]
        public void ThenThePhoneNumberShouldBeUpdatedSuccessfully()
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

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var successMessage = _driver.FindElement(By.CssSelector(".alert.alert-success"));
            Assert.That(successMessage.Text, Does.Contain("Your phone number has been updated."));
        }
        
         [When(@"the user enters an invalid phone number {string}")]
        public void WhenTheUserEntersAnInvalidPhoneNumber(string phoneNumber)
        {
           var phoneInput = _driver.FindElement(By.Id("PhoneNumber"));
           var submitButton = _driver.FindElement(By.Id("phone-number-submit"));
           phoneInput.Clear();
           phoneInput.SendKeys(phoneNumber);
           submitButton.Click();
           
        }

        [Then("a phone number error message should be displayed")]
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

            var phoneInput = _driver.FindElement(By.Id("PhoneNumber"));
            Assert.That(phoneInput.Displayed, Is.True, "Phone input field disappeared, form was submitted unexpectedly.");

        }
    }
}