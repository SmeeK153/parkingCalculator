using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.PageObjects;

namespace seleniumTestAutomation
{
    class ParkingCalculation
    {
        //Object page references
        private IWebDriver driver;
        private SelectElement lotList;
        private IWebElement entryTime;
        private IWebElement entryAM;
        private IWebElement entryPM;
        private IWebElement entryDate;
        private IWebElement exitTime;
        private IWebElement exitAM;
        private IWebElement exitPM;
        private IWebElement exitDate;
        private IWebElement costAmount;
        private IWebElement durationLength;
        private IWebElement errorMessage;
        private IWebElement submitButton;

        //Constructor
        public ParkingCalculation()
        {
            //Chrome web driver reference
            driver = new ChromeDriver(@"C:\Users\Eric\Desktop\Selenium DLLs\chromedriver_win32");

            //Navigate the NEW object to the desired page
            driver.Navigate().GoToUrl("http://adam.goucher.ca/parkcalc/index.php");

            //Set the page references to their respective objects, after the page loads
            SetPageReferences();
        }

        //Set the page references
        public void SetPageReferences()
        {
            //Set reference to the parking lot type control
            lotList = new SelectElement(driver.FindElement(By.Name("Lot")));

            //Set reference to the entry time control
            entryTime = driver.FindElement(By.Name("EntryTime"));

            //Set reference to the entry time AM radio control
            entryAM = driver.FindElements(By.Name("EntryTimeAMPM")).ElementAt(0);

            //Set reference to the entry time PM radio control
            entryPM = driver.FindElements(By.Name("EntryTimeAMPM")).ElementAt(1);

            //Set reference to the entry date control
            entryDate = driver.FindElement(By.Name("EntryDate"));

            //Set reference to the exit time control
            exitTime = driver.FindElement(By.Name("ExitTime"));
            
            //Set reference to the exit time AM radio control
            exitAM = driver.FindElements(By.Name("ExitTimeAMPM")).ElementAt(0);

            //Set reference to the exit time PM radio control
            exitPM = driver.FindElements(By.Name("ExitTimeAMPM")).ElementAt(1);

            //Set reference to the exit date control
            exitDate = driver.FindElement(By.Name("ExitDate"));
            
            //Set the references to the parking cost & stay duration OR error message, depending upon the outcome
            costAmount = driver.FindElements(By.ClassName("SubHead")).ElementAt(1);
            errorMessage = driver.FindElements(By.ClassName("SubHead")).ElementAt(1);

            if (driver.FindElements(By.TagName("tbody")).ElementAt(0).FindElements(By.TagName("span")).Count > 1)
            {
                durationLength = driver.FindElements(By.TagName("tbody")).ElementAt(0).FindElements(By.TagName("span")).ElementAt(1);
            } 
            else
            {
                durationLength = null;
            }

            //Set the reference to the submit button
            submitButton = driver.FindElement(By.Name("Submit"));
        }

        //Set the value of the parking lot type via the displayed text the user sees
        public void ParkingLot(string parkingLotType)
        {
            lotList.SelectByText(parkingLotType.ToString());
        }

        //Return the currently selected parking lot type
        public string ParkingLot()
        {
            return lotList.SelectedOption.Text.ToString();
        }

        //Set the value of the entry time
        public void EntryTime(string time)
        {
            entryTime.Clear();
            entryTime.Click();
            entryTime.SendKeys(time.ToString());
        }

        //Return the current entry time value
        public string EntryTime()
        {
            return entryTime.GetAttribute("value").ToString();
        }

        //Set the entry time to either AM or PM
        public void EntryAMPM(Boolean am)
        {
            if (am)
            {
                entryAM.Click();
            } 
            else
            {
                entryPM.Click();
            }
        }

        //Return the current entry time AM/PM status
        public string EntryAMPM()
        {
            if (entryAM.Selected)
            {
                return "AM";
            }
            else
            {
                return "PM";
            }
        }

        //Set the value of the entry date
        public void EntryDate(string date)
        {
            entryDate.Clear();
            entryDate.Click();
            entryDate.SendKeys(date.ToString());
        }

        //Return the current entry date value
        public string EntryDate()
        {
            return entryDate.GetAttribute("value").ToString();
        }

        //Set the value of the exit time
        public void ExitTime(string time)
        {
            exitTime.Clear();
            exitTime.Click();
            exitTime.SendKeys(time.ToString());
        }

        //Return the current exit time value
        public string ExitTime()
        {
            return exitTime.GetAttribute("value").ToString();
        }

        //Set the exit time to either AM or PM
        public void ExitAMPM(Boolean am)
        {
            if (am)
            {
                exitAM.Click();
            }
            else
            {
                exitPM.Click();
            }
        }

        //Return the current exit time AM/PM status
        public string ExitAMPM()
        {
            if (exitAM.Selected)
            {
                return "AM";
            }
            else
            {
                return "PM";
            }
        }

        //Set the value of the entry date
        public void ExitDate(string date)
        {
            exitDate.Clear();
            exitDate.Click();
            exitDate.SendKeys(date.ToString());
        }

        //Return the current exit date value
        public string ExitDate()
        {
            return exitDate.GetAttribute("value").ToString();
        }

        //Return if the calculation is yielding an error
        public Boolean YieldsError()
        {
            return !DefaultState() && (errorMessage.Text.Length >= 5) && (errorMessage.Text.ToString().Substring(0, 5) == "ERROR");
        }

        //Return whether the page is still in its default result state
        public Boolean DefaultState()
        {
            return (driver.FindElements(By.TagName("tbody")).ElementAt(0).FindElements(By.TagName("span")).Count > 1 && costAmount.Text.ToString() == "$ 0");
        }

        //Return if the calculation is yielding an error AND matches the specific error
        public Boolean ErrorMessage(string message)
        {
            //Check if the returned message is an error, and then if it is the requested error type
            return YieldsError() && !DefaultState() && (errorMessage.Text.ToString() == message.ToString());
        }

        //Return the calculation error, if the calculation is yielding an error
        public string ErrorMessage()
        {
            //Check if the returned message is an error, and then return the error message
            if (YieldsError())
            {
                return errorMessage.Text.ToString();
            }
            else
            {
                return "";
            }
        }

        //Return the cost value, if the calculation is NOT yielding an error
        public string FinalCost()
        {
            if (!YieldsError() && !DefaultState())
            {
                return costAmount.Text.ToString();
            }
            else
            {
                return "";
            }
        }

        //Return if the calculation is NOT yielding an error AND matches the specific cost string
        public Boolean FinalCost(string cost)
        {
            return !YieldsError() && !DefaultState() && costAmount.Text.ToString() == cost.ToString();
        }

        //Return the duration string, if the calculation is NOT yielding an error
        public string ParkingDuration()
        {
            if (durationLength != null && !YieldsError() && !DefaultState())
            {
                return durationLength.Text.ToString().Substring(8);
            }
            else
            {
                return "";
            }
        }

        //Return if the calculation is NOT yielding an error AND matches the specific duration string
        public Boolean ParkingDuration(string days, string hours, string minutes)
        {
            string durationTest = "(" + days + " Days, " + hours + " Hours, " + minutes + " Minutes)";

            return (!YieldsError() && !DefaultState() && durationLength.Text.ToString() == durationTest.ToString());
        }

        //Submit the calculation request to the webpage
        public void Submit()
        {
            //Click the Submit button
            submitButton.Click();

            //Reset the page references after the page updates
            SetPageReferences();
        }

        //Return the current test page URL
        public string CurrentURL()
        {
            return driver.Url;
        }
    }

    class TestFramework
    {
        static void Main(string[] args)
        {
            TestFramework testInstance = new TestFramework();

            ////Run each of the tests
            //Console.WriteLine("Test 1: " + testInstance.Test1());
            //Console.WriteLine("Test 2: " + testInstance.Test2());
            //Console.WriteLine("Test 3: " + testInstance.Test3());
            //Console.WriteLine("Test 4: " + testInstance.Test4());
            //Console.WriteLine("Test 5: " + testInstance.Test5());
            //Console.WriteLine("Test 6: " + testInstance.Test6());
            //Console.WriteLine("Test 7: " + testInstance.Test7());
            //Console.WriteLine("Test 8: " + testInstance.Test8());
           
            testInstance.Test0();
        }

        void Test0()
        {
            //Open up web page to testing URL
            ParkingCalculation calculation = new ParkingCalculation();

            //Set parking type to 'Economy Parking'
            calculation.ParkingLot("Economy Parking");

            //Set entry time to '8:00'
            calculation.EntryTime("8:00");

            //Set entry time to PM
            calculation.EntryAMPM(false);

            //Set entry date to '12/12/2012'
            calculation.EntryDate("12/12/2012");

            //Set exit time to '9:00'
            calculation.ExitTime("9:00");

            //Set exit time to PM
            calculation.ExitAMPM(false);

            //Set exit date to '12/12/2012'
            calculation.ExitDate("12/12/2012");

            //Pre-submit debug
            Debug.WriteLine(calculation.ParkingLot());
            Debug.WriteLine(calculation.EntryTime() + " " + calculation.EntryAMPM() + " " + calculation.EntryDate());
            Debug.WriteLine(calculation.ExitTime() + " " + calculation.ExitAMPM() + " " + calculation.ExitDate());
            Debug.WriteLine("Yields Error: " + calculation.YieldsError());
            Debug.WriteLine("Error message: " + calculation.ErrorMessage() + " Final Cost: " + calculation.FinalCost() + " Parking Duration: " + calculation.ParkingDuration());


            //Submit the calculation request
            calculation.Submit();

            ////After submit debug
            Debug.WriteLine(calculation.ParkingLot());
            Debug.WriteLine(calculation.EntryTime() + " " + calculation.EntryAMPM() + " " + calculation.EntryDate());
            Debug.WriteLine(calculation.ExitTime() + " " + calculation.ExitAMPM() + " " + calculation.ExitDate());
            Debug.WriteLine("Yields Error: " + calculation.YieldsError());
            Debug.WriteLine("Error message: " + calculation.ErrorMessage() + " Final Cost: " + calculation.FinalCost() + " Parking Duration: " + calculation.ParkingDuration());
        }

        //private Boolean Test1()
        //{
        //    //Navigate to http://adam.goucher.ca/parkcalc/index.php
        //    //Select the Short­Term Parking​option from the Choose a Lot d​ropdown
        //    //Enter 10:00​and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
        //    //Select the PM ​option in the Choose Entry Date and Time ​section
        //    //Enter 11:00​and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
        //    //Select the PM ​option in the Choose Leaving Date and Time s​ection
        //    //Click Calculate
        //    //Check that the COST ​is equal to $ 2.00
        //    //Check that the duration of stay is equal to(0 Days, 1 Hours, 0 Minutes)

        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation("STP",'01-01-2014', '10:00', false,'01-01-2014', '11:00', false);

        //    //

        //    //Submit the calculation parameters
        //    calculation.Submit();

        //    //Return the calculation results
        //    return (calculation.ExpectedCost() && calculation.ExpectedDuration());
        //}

        //private Boolean Test2()
        //{
        //    //Navigate to http://adam.goucher.ca/parkcalc/index.php
        //    //Select the Long­Term Surface Parking​option from the Choose a Lot d​ropdown
        //    //Click on the Calendar Icon ​in the Choose Entry Date and Time s​ection
        //    //Select 01 / 01 / 2014 ​in the new window that appears
        //    //Click on the Calendar Icon ​in the Choose Leaving Date and Time s​ection
        //    //Select 02 / 01 / 2014 ​in the new window that appears
        //    //Click Calculate
        //    //Check that the COST ​is equal to $ 270.00
        //    //Check that the duration of stay is equal to(31 Days, 0 Hours, 0 Minutes)

        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}

        //private Boolean Test3()
        //{
        //    //Navigate to http://adam.goucher.ca/parkcalc/index.php
        //    //Select the Short­Term Parking​option from the Choose a Lot d​ropdown
        //    //Leave the Entry Time​unchanged
        //    //Enter 01 / 02 / 2014 ​in the Choose Entry Date and Time s​ection
        //    //Leave the Leaving Time​unchanged
        //    //Enter 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
        //    //Click Calculate
        //    //Check that the following error message appears: ERROR!YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME

        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}

        //private Boolean Test4()
        //{
        //    //Check to see if the page is re-usuable after running at least one calculation.
        //    //Run multiple calculations and see if the outcome deteriorates from True to False, or remains the same throughout (i.e., True to True, False to False)
        //    //Use the input from Test 1 for the first calculation result, Test 2 for the second calculation result

        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    //Submit the calculation parameters
        //    calculation.Submit();

        //    //Store the first calculation result
        //    Boolean firstResult = (calculation.ExpectedCost() && calculation.ExpectedDuration());

        //    //Update the calculation parameters
        //    //calculation.Update();

        //    //Submit the calculation parameters again
        //    calculation.Submit();

        //    //Store the second calculation result
        //    Boolean secondResult = (calculation.ExpectedCost() && calculation.ExpectedDuration());

        //    //Return the results if both runs remain the same
        //    return firstResult == secondResult;
        //}

        //private Boolean Test5()
        //{
        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}

        //private Boolean Test6()
        //{
        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}

        //private Boolean Test7()
        //{
        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}

        //private Boolean Test8()
        //{
        //    //Create new test instance
        //    ParkingCalculation calculation = new ParkingCalculation();

        //    return false;
        //}
    }
}
