using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace project_wildfire_tests.BDDTesting.StepDefinitions
{
    [TestFixture]
    public class EmailUpdateStepDefinitions
    {
        private IWebDriver _driver;

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }

        [Test]
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
            var emailInput = _driver.FindElement(By.Id("Input_Email"));
            emailInput.Clear();
            emailInput.SendKeys("testuser@test.com");

            // Enter password
            var passwordInput = _driver.FindElement(By.Id("Input_Password"));
            passwordInput.Clear();
            passwordInput.SendKeys("Password123$");

            // Submit the form
            var loginButton = _driver.FindElement(By.Id("login-submit"));
            loginButton.Click();
        }

        [Test]
        public void GivenTheUserIsOnTheEmailUpdatePage()
        {
            // Navigate to the email update page
            _driver.Navigate().GoToUrl("http://localhost:5205/Identity/Account/Manage/Email");

            // Wait for the page to fully load
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            wait.Until(d => d.FindElement(By.TagName("body")));
        }

    
    }
}