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
    class DateObjectCollection
    {
        private IWebElement time;
        private IWebElement am;
        private IWebElement pm;
        private IWebElement date;
        private IWebDriver driver;

        //Constructor
        public DateObjectCollection(IWebDriver driver, IWebElement timeControl, IWebElement amControl, IWebElement pmControl, IWebElement dateControl, IWebElement datePicker)
        {
            this.driver = driver;
            time = timeControl;
            am = amControl;
            pm = pmControl;
            date = dateControl;
        }

        //Return the value of the time
        public string Time()
        {
            return time.Text.ToString();
        }

        //Set the value of the time
        public void Time(string time)
        {
            this.time.Clear();
            this.time.Click();
            this.time.SendKeys(time);
        }

        //Set the time AM/PM status
        public void AMPM(string ampm)
        {
            if (ampm.ToString() == "am")
            {
                this.am.Click();
            }
            if (ampm.ToString() == "pm")
            {
                this.pm.Click();
            }
            
        }

        //Return the time AM/PM status
        public string AMPM()
        {
            if (this.am.Selected)
            {
                return "AM";
            }
            else
            {
                return "PM";
            }
        }

        //Set the value of the date
        public void Date(string date)
        {
            this.date.Clear();
            this.date.Click();
            this.date.SendKeys(date);
        }

        //Set the value of the date via data picker
        public void Date(int day, int month, int year)
        {
            //Save the main page reference
            string mainWindowHandle = driver.WindowHandles.First();

            //Save the date picker reference
            string datePickerPopup = driver.WindowHandles.Last();

            //Create new DatePickerObject
            DatePickerObject datePicker = new DatePickerObject(driver.SwitchTo().Window(datePickerPopup));

            //Select the desired date
            datePicker.SelectDate(day, month, year);

            //Need to return control to original window handle?
        }

        //Return the current date value
        public string Date()
        {
            return this.date.Text.ToString();
        }

    }

    class DatePickerObject
    {
        //Object page references
        IWebDriver driver;

        public DatePickerObject(IWebDriver datePickerHandle)
        {
            //Set the reference to the date picker
            driver = datePickerHandle;
        }

        private void ChangeYear(int objectiveYear)
        {
            while (objectiveYear != int.Parse(driver.FindElements(By.TagName("font")).ElementAt(1).Text.ToString()))
            {
                if (objectiveYear > int.Parse(driver.FindElements(By.TagName("font")).ElementAt(1).Text.ToString()))
                {
                    //Decrease the year if it is higher than desired
                    driver.FindElements(By.TagName("font")).ElementAt(2).Click();
                    
                }
                else
                {
                    //Increase the year if it is lower than desired
                    driver.FindElements(By.TagName("font")).ElementAt(0).Click();
                }
            }
        }

        private void ChangeMonth(int monthIndex)
        {
            //Create object reference to month selector
            SelectElement monthPick = new SelectElement(driver.FindElement(By.Name("MonthSelector")));

            //Select the desired month
            monthPick.SelectByIndex(monthIndex - 1);
        }

        public void SelectDate(int day, int month, int year)
        {
            //Select the desired year
            ChangeYear(year);

            //Select the desired month
            ChangeMonth(month);
            
            //Find the correct day to click on
            foreach (IWebElement indDate in driver.FindElements(By.TagName("a")))
            {
                if (indDate.GetAttribute("href").Contains("'" + month + "/" + day + "/" + year + "'"))
                {
                    //Click on the desired day
                    indDate.Click();

                    //Switch focus back to the main window
                    driver.SwitchTo().Window(driver.WindowHandles.First());

                    //Leave the function
                    return;
                }

            }
        }
    }

    class ParkingCalculation
    {
        //Object page references
        private IWebDriver driver;
        private SelectElement lotList;
        private IWebElement costAmount;
        private IWebElement durationLength;
        private IWebElement errorMessage;
        private IWebElement submitButton;
        public DateObjectCollection entryDetails;
        public DateObjectCollection leavingDetails;

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

            //Set the reference to the Entry DateTime Object
            entryDetails = new DateObjectCollection(driver, driver.FindElement(By.Name("EntryTime")), driver.FindElements(By.Name("EntryTimeAMPM")).ElementAt(0), driver.FindElements(By.Name("EntryTimeAMPM")).ElementAt(1), driver.FindElement(By.Name("EntryDate")), driver.FindElements(By.TagName("a")).ElementAt(0));

            //Set the reference to the Leaving DateTime Object
            leavingDetails = new DateObjectCollection(driver, driver.FindElement(By.Name("ExitTime")), driver.FindElements(By.Name("ExitTimeAMPM")).ElementAt(0), driver.FindElements(By.Name("ExitTimeAMPM")).ElementAt(1), driver.FindElement(By.Name("ExitDate")), driver.FindElements(By.TagName("a")).ElementAt(1));
            
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
                return durationLength.Text.ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        //Return if the calculation is NOT yielding an error AND matches the specific duration string
        public Boolean ParkingDuration(string length)
        {
            Debug.WriteLine(length);
            return (!YieldsError() && !DefaultState() && durationLength.Text.Trim().ToString() == length.ToString());
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
            
            Debug.WriteLine("Test 1: " + testInstance.Test1());   //PASSED = Successful Verification
            //Debug.WriteLine("Test 2: " + testInstance.Test2());   //PASSED = Successful Verification
            //Debug.WriteLine("Test 3: " + testInstance.Test3());   //PASSED = Successful Verification
            //Debug.WriteLine("Test 4: " + testInstance.Test4());   //FAILED = Successful Verification
            //Debug.WriteLine("Test 5: " + testInstance.Test5());   //FAILED = Successful Verification
            //Debug.WriteLine("Test 6: " + testInstance.Test6());   //FAILED = Successful Verification
            //Debug.WriteLine("Test 7: " + testInstance.Test7());   //FAILED = Successful Verification
            //Debug.WriteLine("Test 8: " + testInstance.Test8());   //FAILED = Successful Verification
        }

        private Boolean Test1()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter 10:00​ and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("10:00");
            calculation.entryDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 11:00​ and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("11:00");
            calculation.leavingDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Check that the COST ​is equal to $ 2.00
            //Check that the duration of stay is equal to (0 Days, 1 Hours, 0 Minutes)
            return (calculation.ParkingDuration("(0 Days, 1 Hours, 0 Minutes)") && calculation.FinalCost("$ 2.00"));
        }

        private Boolean Test2()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Long­ Term Surface Parking ​option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Long-Term Surface Parking");

            //Click on the Calendar Icon ​in the Choose Entry Date and Time s​ection
            //Select 01 / 01 / 2014 ​in the new window that appears                                                           
            calculation.entryDetails.Date(1, 1, 2014);

            //Click on the Calendar Icon ​in the Choose Leaving Date and Time s​ection
            //Select 02 / 01 / 2014 ​in the new window that appears
            calculation.leavingDetails.Date(1, 2, 2014);

            //Click Calculate
            calculation.Submit();

            //Check that the COST ​is equal to $ 270.00
            //Check that the duration of stay is equal to (31 Days, 0 Hours, 0 Minutes)
            return (calculation.ParkingDuration("(31 Days, 0 Hours, 0 Minutes)") && calculation.FinalCost("$ 270.00"));
        }

        private Boolean Test3()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short-­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Leave the Entry Time​ unchanged
            //Enter 01 / 02 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Date("01/02/2014");

            //Leave the Leaving Time​ unchanged
            //Enter 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Date("01/01/2014");

            //Click Calculate
            calculation.Submit();

            //Check that the following error message appears: ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME
            return calculation.ErrorMessage("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME");
        }

        private Boolean Test4()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter 10:00​ and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("10:00");
            calculation.entryDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 11:00​ and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("11:00");
            calculation.leavingDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Check to make sure both AM/PM radio buttons remained selected
            return (calculation.entryDetails.AMPM().ToString() == "PM" && calculation.leavingDetails.AMPM().ToString() == "PM" && calculation.entryDetails.Time().ToString() == "10:00" && calculation.leavingDetails.Time().ToString() == "11:00");
        }

        private Boolean Test5() 
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter 10:00​ and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("10:00");
            calculation.entryDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 25:00​ and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("25:00");
            calculation.leavingDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Make sure this input yields an ERROR
            return calculation.YieldsError();
        }

        private Boolean Test6()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter 11:00​ and 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("11:00");
            calculation.entryDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 10:00​ and 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("10:00");
            calculation.leavingDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Check that the following error message appears: ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME
            return calculation.ErrorMessage("ERROR! YOUR EXIT DATE OR TIME IS BEFORE YOUR ENTRY DATE OR TIME");
        }

        private Boolean Test7()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter "AS:DF" ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("AS:DF");

            //Enter 01 / 01 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 10:00​ in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("10:00");

            //Enter 01 / 01 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Date("01/01/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Make sure this input yields an ERROR
            return calculation.YieldsError();
        }

        private Boolean Test8()
        {
            //Navigate to http://adam.goucher.ca/parkcalc/index.php
            ParkingCalculation calculation = new ParkingCalculation();

            //Select the Short­Term Parking​ option from the Choose a Lot d​ropdown
            calculation.ParkingLot("Short-Term Parking");

            //Enter 10:00​ and 01 / 40 / 2014 ​in the Choose Entry Date and Time s​ection
            calculation.entryDetails.Time("10:00");
            calculation.entryDetails.Date("01/40/2014");

            //Select the PM ​option in the Choose Entry Date and Time ​section
            calculation.entryDetails.AMPM("pm");

            //Enter 11:00​ and 01 / 40 / 2014 ​in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.Time("11:00");
            calculation.leavingDetails.Time("01/40/2014");

            //Select the PM ​option in the Choose Leaving Date and Time s​ection
            calculation.leavingDetails.AMPM("pm");

            //Click Calculate
            calculation.Submit();

            //Make sure this input yields an ERROR
            return calculation.YieldsError();
        }
    }
}
