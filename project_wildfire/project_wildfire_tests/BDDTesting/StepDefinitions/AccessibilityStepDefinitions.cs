/*
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace project_wildfire_tests.BDDTesting.StepDefinitions;

public class AccessibilityLogicTests
{
    private IJavaScriptExecutor js;
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

    // All [Test] needs to be changed to [Given(@"...")] or [When(@"...")] for BDD
    [Test]
    public void UserSelectsFontSizeFromDropdown()
    {
        driver.Navigate().GoToUrl("http://localhost:5240/");
        Thread.Sleep(1000);

        var dropdown = driver.FindElement(By.Id("fontSize"));
        var select = new SelectElement(dropdown);
        select.SelectByValue("xlarge");

        Thread.Sleep(500); // Let the JS apply changes

        var fontSize = js.ExecuteScript("return window.getComputedStyle(document.body).fontSize;");

        Assert.That(AreEqual("22px", fontSize));
    }

    [Test]
    public void UserClicksContrastToggle()
    {
        driver.Navigate().GoToUrl("http://localhost:5240/");
        Thread.Sleep(1000);

        var contrastBtn = driver.FindElement(By.Id("contrastToggle"));
        contrastBtn.Click();
        Thread.Sleep(300);

        var hasClass = (bool)js.ExecuteScript("return document.body.classList.contains('high-contrast');");
        Assert.That(hasClass);
    }

    [Test]
    public void UserClicksTextToSpeech()
    {
        driver.Navigate().GoToUrl("http://localhost:5240/");
        Thread.Sleep(1000);

        var speechBtn = driver.FindElement(By.Id("speechToggle"));
        speechBtn.Click();
        Thread.Sleep(1000); // Let speech synthesis start

        var isSpeaking = (bool)js.ExecuteScript("return window.speechSynthesis.speaking;");
        Assert.That(isSpeaking);

        js.ExecuteScript("window.speechSynthesis.cancel();"); // Clean up speaking
    }
}
*/