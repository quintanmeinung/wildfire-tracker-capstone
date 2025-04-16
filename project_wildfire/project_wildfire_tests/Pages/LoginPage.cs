using OpenQA.Selenium;

namespace project_wildfire_tests.Pages;
public class LoginPage
{
    private IWebDriver _driver;

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
    }

    public IWebElement loginLink => _driver.FindElement(By.Id("login"));
}