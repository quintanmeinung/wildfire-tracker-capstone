using Reqnroll;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using project_wildfire_tests.BDDTesting.Drivers;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [Binding]
    public class AuthenticationSteps
    {
        private readonly IWebDriver _driver = WebDriverFactory.Driver;

        [Given(@"a user is logged in")]
        public void GivenAUserIsLoggedIn()
        {
            _driver.Navigate().GoToUrl("http://localhost:5205");
            _driver.FindElement(By.Id("login")).Click();
            _driver.FindElement(By.Id("login-email-field")).SendKeys("testuser@mail.com");
            _driver.FindElement(By.Id("login-password-field")).SendKeys("Password123$");
            _driver.FindElement(By.Id("login-submit")).Click();
        }
    }
}