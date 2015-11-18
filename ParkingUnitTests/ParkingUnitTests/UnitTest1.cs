using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace ParkingUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}

namespace seleniumDemo
{
    [TestClass]
    public class UnitTest1
    {
        static IWebDriver driverGC;

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            driverGC = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");
        }

        [TestMethod]
        public void TestGoogleDrive()
        {
            driverGC.Navigate().GoToUrl("www.google.com");
            //driverGC.FindElement(By.Id("lst-ib")).SendKeys("Selenium");
            //driverGC.FindElement(By.Id("lst-ib")).SendKeys(Keys.Enter);

        }
    }
}