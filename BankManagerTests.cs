using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingAutomation
{
    [TestFixture]
    public class BankManagerTests : BaseTest
    {
        [Test]
        public void CreateVerifyDeleteCustomers()
        {

            WebDriverWait wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[text()='Bank Manager Login']")));
            driver.FindElement(By.XPath("//button[text()='Bank Manager Login']")).Click();
            driver.FindElement(By.XPath("//button[contains(text(),'Add Customer')]")).Click();

            List<(string First, string Last, string Code)> customers = new()
            {
                ("Christopher","Connely","L789C349"),
                ("Frank","Christopher","A897N450"),
                ("Christopher","Minka","M098Q585"),
                ("Connely","Jackson","L789C349"),
                ("Jackson","Frank","L789C349"),
                ("Minka","Jackson","A897N450"),
                ("Jackson","Connely","L789C349")
            };

            foreach (var c in customers)
            {
                driver.FindElement(By.XPath("//input[@placeholder='First Name']")).SendKeys(c.First);
                driver.FindElement(By.XPath("//input[@placeholder='Last Name']")).SendKeys(c.Last);
                driver.FindElement(By.XPath("//input[@placeholder='Post Code']")).SendKeys(c.Code);
                driver.FindElement(By.XPath("//button[text()='Add Customer']")).Click();
                driver.SwitchTo().Alert().Accept();
            }

            driver.FindElement(By.XPath("//button[contains(text(),'Customers')]")).Click();
            var rows = driver.FindElements(By.XPath("//table/tbody/tr"));

            foreach (var c in customers)
            {
                Assert.IsTrue(rows.Any(r =>
                    r.Text.Contains(c.First) &&
                    r.Text.Contains(c.Last) &&
                    r.Text.Contains(c.Code)
                ), $"Customer {c.First} {c.Last} not found");
            }

            var deleteList = new List<(string, string, string)>
            {
                ("Jackson","Frank","L789C349"),
                ("Christopher","Connely","L789C349")
            };

            foreach (var d in deleteList)
            {
                var row = driver.FindElements(By.XPath("//table/tbody/tr"))
                                .FirstOrDefault(r => r.Text.Contains(d.Item1) &&
                                                     r.Text.Contains(d.Item2) &&
                                                     r.Text.Contains(d.Item3));
                row?.FindElement(By.XPath(".//button[text()='Delete']")).Click();
            }
        }
    }
}
