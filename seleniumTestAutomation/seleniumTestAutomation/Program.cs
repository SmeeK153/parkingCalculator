using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;


namespace seleniumTestAutomation
{
    class Program
    {
        //Setup the Google Chrome browser object
        IWebDriver driver = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");


        static void Main(string[] args)
        {}

        [Test]
        public void UnitTest1()
        {

        }


        [SetUp]
        public void Startup()
        {
            //Navigate to the calculator page
            driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/");
        }

        [TearDown]
        public void FinishRun()
        {
            driver.Close();
        }

    }

    
       
}
