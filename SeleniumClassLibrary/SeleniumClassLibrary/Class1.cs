using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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