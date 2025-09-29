using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace BankingAutomation
{
    [TestFixture]
    public class CustomerTransactionTests : BaseTest
    {
        [Test]
        public void VerifyTransactionsAndBalance()
        {
            WebDriverWait wait = new WebDriverWait(driver!, TimeSpan.FromSeconds(50));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[text()='Customer Login']")));
            driver.FindElement(By.XPath("//button[text()='Customer Login']")).Click();

            var userSelect = driver.FindElement(By.Id("userSelect"));
            userSelect.FindElement(By.XPath("//option[text()='Hermoine Granger']")).Click();
            driver.FindElement(By.XPath("//button[text()='Login']")).Click();

            var accountSelect = driver.FindElement(By.Id("accountSelect"));
            accountSelect.FindElement(By.XPath("//option[text()='1003']")).Click();

            double runningBalance = 0;
            var transactions = new List<(int Amount, string Type)>
            {
                (50000,"Credit"),
                (3000,"Debit"),
                (2000,"Debit"),
                (5000,"Credit"),
                (10000,"Debit"),
                (15000,"Debit"),
                (1500,"Credit")
            };

            foreach (var t in transactions)
            {
                if (t.Type == "Credit")
                {
                    driver.FindElement(By.XPath("//button[contains(text(),'Deposit')]")).Click();
                    driver.FindElement(By.XPath("//input[@placeholder='amount']")).SendKeys(t.Amount.ToString());
                    driver.FindElement(By.XPath("//button[text()='Deposit']")).Click();
                    runningBalance += t.Amount;
                }
                else
                {
                    driver.FindElement(By.XPath("//button[contains(text(),'Withdrawl')]")).Click();
                    driver.FindElement(By.XPath("//input[@placeholder='amount']")).SendKeys(t.Amount.ToString());
                    driver.FindElement(By.XPath("//button[text()='Withdraw']")).Click();
                    runningBalance -= t.Amount;
                }

                double uiBalance = Convert.ToDouble(
                    driver.FindElement(By.XPath("//strong[preceding-sibling::span[text()='Balance :']]")).Text);
                Assert.AreEqual(runningBalance, uiBalance, "Balance mismatch!");
            }
        }
    }
}
