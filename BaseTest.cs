using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;

namespace BankingAutomation
{
    public class BaseTest
    {
        protected IWebDriver? driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();

            // Headless mode is required on GitHub runners
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            // Create a unique temporary user-data-dir to avoid conflicts
            string tempProfile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            options.AddArgument($"--user-data-dir={tempProfile}");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login");
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Quit();
        }
    }
}
