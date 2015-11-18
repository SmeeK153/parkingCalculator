using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using NUnit;


namespace seleniumTestAutomation
{

    class ParkingCalculation
    {
        //Setup the Google Chrome browser object
        static IWebDriver driver = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");

        //Setup the lot type
        string lotType;

        //Setup the start date
        DateTime startDate;

        //Setup the end date
        DateTime endDate;

        //Constructor: All parameters
        public ParkingCalculation(string lotType, DateTime startDate, DateTime endDate)
        {
            this.lotType = lotType;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        //Constructor: Page Default Parking Type
        public ParkingCalculation(DateTime startDate, DateTime endDate)
        {
            this.lotType = null;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        //Constructor: All Page Defaults
        public ParkingCalculation()
        {
            this.lotType = null;

        }

        //Assign the requested lot
        public void SetLot()
        {
            //Get the lot dropdown list
            SelectElement parkingLotType = new SelectElement(driver.FindElement(By.Name("Lot")));

            //Select the desired parking option
            parkingLotType.SelectByValue(lotType);
        }

        //Set both of the provided dates
        public void SetDate()
        {

        }

        //Set an alternative date
        public void SetDate(string dateType, string hour, string minute, Boolean am, string day, string month, string year)
        {

        }

        //Submit the calculation
        public void Submit()
        {

        }


        //Assign the requested date
        private void SetParkingDate(Boolean entryDate)
        {
            //Create the date type name prefix
            string dateTypePrefix;

            //Determine if the date is AM or PM
            //AM = 0 and PM = 1
            int amPM = 0;                                                           //Need to capture AM/PM

            //Determine which type of date is being worked with: Entry or Exit
            if (entryDate)
            {
                dateTypePrefix = "Entry";
            }
            else
            {
                dateTypePrefix = "Exit";
            }

            //Set the Time
            driver.FindElement(By.Name(dateTypePrefix + "Time")).SendKeys("a");     //Need to capture the time

            //Set the AM/PM
            driver.FindElements(By.Name(dateTypePrefix + "TimeAMPM")).ElementAt(amPM).Click();

            //Set the Date
            driver.FindElements(By.Name(dateTypePrefix + "TimeAMPM")).ElementAt(amPM).Click();

        }

        public Boolean ExpectedCost()
        {


            return "";
        }

        public Boolean ExpectedDuration()
        {


            return "";
        }
    }

    class TestFramework
    {
        static void Main(string[] args)
        {
            TestFramework testInstance = new TestFramework;

            //Run each of the tests
            Console.WriteLine("Test 1: " + testInstance.Test1());
            Console.WriteLine("Test 2: " + testInstance.Test2());
            Console.WriteLine("Test 3: " + testInstance.Test3());
            Console.WriteLine("Test 4: " + testInstance.Test4());
            Console.WriteLine("Test 5: " + testInstance.Test5());
            Console.WriteLine("Test 6: " + testInstance.Test6());
            Console.WriteLine("Test 7: " + testInstance.Test7());
            Console.WriteLine("Test 8: " + testInstance.Test8());
        }

        private Boolean Test1()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            //Select the Short­Term Parking​option from the Choose a Lot d​ropdown
            //Enter 10:00​and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            //Select the PM ​option in the Choose Entry Date and Time ​section
            //Enter 11:00​and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            //Click Calculate
            //Check that the COST ​is equal to $ 2.00
            //Check that the duration of stay is equal to(0 Days, 1 Hours, 0 Minutes)

            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation("STP",'01-01-2014', '10:00', false,'01-01-2014', '11:00', false);

            //

            //Submit the calculation parameters
            calculation.Submit();

            //Return the calculation results
            return (calculation.ExpectedCost() && calculation.ExpectedDuration());
        }

        private Boolean Test2()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            //Select the Long­Term Surface Parking​option from the Choose a Lot d​ropdown
            //Click on the Calendar Icon ​in the Choose Entry Date and Time s​ection
            //Select 01 / 01 / 2014 ​in the new window that appears
            //Click on the Calendar Icon ​in the Choose Leaving Date and Time s​ection
            //Select 02 / 01 / 2014 ​in the new window that appears
            //Click Calculate
            //Check that the COST ​is equal to $ 270.00
            //Check that the duration of stay is equal to(31 Days, 0 Hours, 0 Minutes)

            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }

        private Boolean Test3()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            //Select the Short­Term Parking​option from the Choose a Lot d​ropdown
            //Leave the Entry Time​unchanged
            //Enter 01 / 02 / 2014 ​in the Choose Entry Date and Time s​ection
            //Leave the Leaving Time​unchanged
            //Enter 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            //Click Calculate
            //Check that the following error message appears: ERROR!YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME

            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }

        private Boolean Test4()
        {
            //Check to see if the page is re-usuable after running at least one calculation.
            //Run multiple calculations and see if the outcome deteriorates from True to False, or remains the same throughout (i.e., True to True, False to False)
            //Use the input from Test 1 for the first calculation result, Test 2 for the second calculation result

            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            //Submit the calculation parameters
            calculation.Submit();

            //Store the first calculation result
            Boolean firstResult = (calculation.ExpectedCost() && calculation.ExpectedDuration());

            //Update the calculation parameters
            //calculation.Update();

            //Submit the calculation parameters again
            calculation.Submit();

            //Store the second calculation result
            Boolean secondResult = (calculation.ExpectedCost() && calculation.ExpectedDuration());

            //Return the results if both runs remain the same
            return firstResult == secondResult;
        }

        private Boolean Test5()
        {
            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }

        private Boolean Test6()
        {
            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }

        private Boolean Test7()
        {
            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }

        private Boolean Test8()
        {
            //Create new test instance
            ParkingCalculation calculation = new ParkingCalculation();

            return false;
        }
    }

    //class AutomationTest
    //{
    //    //Setup the Google Chrome browser object
    //    static IWebDriver driver = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");




    //    static void Main(string[] args)
    //    {
    //        NewTestEnvironment();
    //        SetLot("VP");
    //    }

    //    static void NewTestEnvironment()
    //    {
    //        //Navigate to the calculator page
    //        driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/");
    //    }

    //    static void SetLot(string lotType)
    //    {
    //        //Get the lot dropdown list
    //        SelectElement parkingLotType = new SelectElement(driver.FindElement(By.Name("Lot")));

    //        //Select the desired parking option
    //        parkingLotType.SelectByValue(lotType);
    //    }

    //    static void SetDate(int hour, int minute, Boolean am, int day, int month, int year, Boolean entryDate)
    //    {
    //        //Determine the type of date being referenced
    //        string dateType;
    //        if (entryDate)
    //        {
    //            dateType = "Entry";
    //        }
    //        else
    //        {
    //            dateType = "Exit";
    //        }


    //    }

    //}
}
