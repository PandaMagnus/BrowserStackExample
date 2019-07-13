using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using Xunit;

namespace BrowserStackExample
{
    public class BrowserStackTest : IDisposable
    {
        protected IWebDriver driver;
        protected string profile;
        protected string environment;
        //private Local browserStackLocal;

        public BrowserStackTest(/*string profile, string environment*/)
        {
            this.profile = "single";
            this.environment = "chrome";
        }

        [Fact]
        public void ExampleBrowserStackTest()
        {
            NameValueCollection caps = ConfigurationManager.GetSection("capabilities/" + profile) as NameValueCollection;
            NameValueCollection settings = ConfigurationManager.GetSection("environments/" + environment) as NameValueCollection;

            DesiredCapabilities capability = new DesiredCapabilities();
            capability.SetCapability("build", "");
            capability.SetCapability("name", "single_test");
            capability.SetCapability("browserstack.debug", "true");
            capability.SetCapability("browser", "safari");

            //foreach (string key in caps.AllKeys)
            //{
            //    capability.SetCapability(key, caps[key]);
            //}

            //foreach (string key in settings.AllKeys)
            //{
            //    capability.SetCapability(key, settings[key]);
            //}

            //String username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            //if (username == null)
            //{
            //    username = ConfigurationManager.AppSettings.Get("user");
            //}

            //String accesskey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            //if (accesskey == null)
            //{
            //    accesskey = ConfigurationManager.AppSettings.Get("key");
            //}

            capability.SetCapability("browserstack.user", "");
            capability.SetCapability("browserstack.key", "");
            capability.SetCapability("name", "Bstack-[xUnit] Sample Test");

      //      if (capability.GetCapability("browserstack.local") != null && capability.GetCapability("browserstack.local").ToString() == "true")
      //      {
      //          browserStackLocal = new Local();
      //          List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
      //  new KeyValuePair<string, string>("key", accesskey)
      //};
      //          browserStackLocal.start(bsLocalArgs);
      //      }

            driver = new RemoteWebDriver(new Uri("http://hub-cloud.browserstack.com/wd/hub/"), capability);

            driver.Navigate().GoToUrl("https://www.google.com/ncr");
            IWebElement query = driver.FindElement(By.Name("q"));
            query.SendKeys("BrowserStack");
            query.Submit();
            System.Threading.Thread.Sleep(5000);
            Assert.Equal("BrowserStack - Google Search", driver.Title);
        }

        public void Dispose()
        {
            driver.Quit();
            //if (browserStackLocal != null)
            //{
            //    browserStackLocal.stop();
            //}
        }
    }
}
