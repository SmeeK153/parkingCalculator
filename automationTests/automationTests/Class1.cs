using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;


namespace automationTests
{
    public class Class1
    {
        static void Main(string[] args)
        {
            IWebDriver d = new ChromeDriver();
            d.Navigate().GoToUrl("www.google.com");
              }

    }
}
