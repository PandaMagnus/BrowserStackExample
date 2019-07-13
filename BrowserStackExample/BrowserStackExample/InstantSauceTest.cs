using System;
using IntelliTect.TestTools.Selenate;
//using NUnit.Framework;
//using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Xunit;
//using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace Web.Tests.OnboardingTests
{
    //    [TestFixture]
    [Trait("Category", "InstantSauceTest")]
    [Trait("Category", "XUnit")]
    [Trait("Category", "Instant")]
    // [Category("InstantSauceTest"), Category("NUnit"), Category("Instant")]
    public class InstantSauceTest : IDisposable
    {
        private IWebDriver _driver;
        private Browser _browser;
        private bool _testPassed = false;
        private string browser = "Safari";
        private string version = "11.1";
        private string os = "macOS 10.13";
        private string deviceName = "";
        private string deviceOrientation = "";

        //[Test]
        [Fact]
        public void ShouldOpenOnSafari()
        {
            /*Easy Option For Sauce Authentication:
              * You can hardcode the values like this example below, but the best practice is to use environment variables
              */
            var sauceUserName = "";
            var sauceAccessKey = "";

            DesiredCapabilities caps = new DesiredCapabilities();
            caps.SetCapability(CapabilityType.BrowserName, browser);
            caps.SetCapability(CapabilityType.Version, version);
            caps.SetCapability(CapabilityType.Platform, os);
            caps.SetCapability("deviceName", deviceName);
            caps.SetCapability("deviceOrientation", deviceOrientation);
            caps.SetCapability("username", sauceUserName);
            caps.SetCapability("accessKey", sauceAccessKey);
            caps.SetCapability("name", "Testing again!");

            _driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), 
                caps,
                TimeSpan.FromSeconds(600));
            _browser = new Browser(_driver);
            //navigate to the url of the Sauce Labs Sample app
            _driver.Navigate().GoToUrl("https://www.saucedemo.com");

            //Create an instance of a Selenium explicit wait so that we can dynamically wait for an element
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            //wait for the user name field to be visible and store that element into a variable
            var userNameField = wait.Until(w => _browser.FindElement(By.CssSelector("[type='text']")));
            //type the user name string into the user name field
            userNameField.SendKeys("standard_user");
            //type the password into the password field
            _driver.FindElement(By.CssSelector("[type='password']")).SendKeys("secret_sauce");
            //hit Login button
            _driver.FindElement(By.CssSelector("[type='submit']")).Click();

            //Synchronize on the next page and make sure it loads
            IWebElement inventoryPageLocator =
              wait.Until(w => _browser.FindElement(By.Id("inventory_container")));
            //Assert that the inventory page displayed appropriately
            Assert.True(inventoryPageLocator.Displayed);
            _testPassed = true;
        }

        /*
          *Below we are performing 2 critical actions. Quitting the driver and passing
          * the test result to Sauce Labs user interface.
          */

        public void Dispose()
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("sauce:job-result=" + (_testPassed ? "passed" : "failed"));
            _driver?.Quit();
        }
    }
}
