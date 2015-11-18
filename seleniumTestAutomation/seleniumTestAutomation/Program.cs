using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace seleniumTestAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create the reference for the browser
            IWebDriver driver = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");

            //Navigate to Google page
            driver.Navigate().GoToUrl("http://www.google.com");

            //

        }
    }
}
