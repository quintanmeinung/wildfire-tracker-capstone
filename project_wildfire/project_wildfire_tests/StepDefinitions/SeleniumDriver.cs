using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace project_wildfire_tests.Drivers
{
    public class SeleniumDriver : IDisposable
    {
        public IWebDriver Driver { get; private set; }

        public SeleniumDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless"); 
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1920,1080");

            Driver = new ChromeDriver(options); // ✅ Create the driver here
        }

        public void Dispose()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
