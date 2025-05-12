using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
namespace project_wildfire_tests.BDDTesting.Drivers;
public static class WebDriverFactory
{
    private static IWebDriver _driver;

    // This property allows access to the WebDriver instance
    public static IWebDriver Driver
    {
        get
        {   
            // If the driver is not initialized, throw an exception
            if (_driver == null)
            {
                throw new InvalidOperationException("WebDriver not initialized. Call CreateDriver() first.");
            }
            return _driver;
        }
    }

    public static void CreateDriver(bool headless = false)
    {
        // Check if the driver is already initialized
        if (_driver == null)
        {
            // Initialize a driver manager and set up the Chrome driver
            new DriverManager().SetUpDriver(new FirefoxConfig());
            var options = new FirefoxOptions();
            
            // Default behavior is to run in headless mode
            if (headless)
            {
                options.AddArguments("--headless");

                // Window size is specified for headless mode to ensure consistent behavior
                options.AddArguments("--window-size=1920,1080");
            }
            
            _driver = new FirefoxDriver(options);

            // Set the default timeout for implicit waits
            // i.e. if an element is not found, it will wait for 5 seconds before throwing an exception
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
    }

    public static void QuitDriver()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver = null;
        }
    }
}