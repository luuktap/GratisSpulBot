using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace GratisSpulBot
{
    class TweakersDecemerSelenium
    {
        private IWebDriver driver;

        private void InitDriver()
        {
            driver = new ChromeDriver();
            driver.Url = "https://tweakers.net";
        }
        private void AcceptCookies()
        {
            driver.FindElement(By.CssSelector("button[title='Ja, ik accepteer cookies']")).Click();
        }

        public List<string> GetTodaysLinks()
        {
            InitDriver();
            AcceptCookies();
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var elements = driver.FindElements(By.XPath($"//*[@id='news']/div[@data-date='{today}']/table/tbody/tr[@class='headline promo']/td[3]/a"));
            elements = driver.FindElements(By.XPath($"//a[contains(text(), 'December Prijzen Parade')]"));
            List<string> urls = new List<string>();
            foreach (var element in elements)
            {
                if (element.Text.IndexOf("December Prijzen Parade") != -1)
                {
                    Console.WriteLine(element.Text);
                    Console.WriteLine(element.GetAttribute("href"));
                    urls.Add(element.GetAttribute("href"));
                }
            }
            driver.Quit();
            return urls;
        }
    }
}
