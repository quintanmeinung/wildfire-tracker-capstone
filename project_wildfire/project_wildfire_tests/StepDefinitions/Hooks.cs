using Reqnroll;
using project_wildfire_tests.Drivers;
using OpenQA.Selenium;

namespace project_wildfire_tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private SeleniumDriver _seleniumDriver;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _seleniumDriver = new SeleniumDriver();
            _scenarioContext.Set<IWebDriver>(_seleniumDriver.Driver);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_seleniumDriver != null)
            {
                _seleniumDriver.Dispose();
            }
        }
    }
}

