using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;

namespace WebdriverTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver d = new ChromeDriver();
            d.Navigate().GoToUrl(“http://techinews24.com”);
              }
    }
}