using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Member Search
    /// </summary
    public class CSPortal_SearchPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_SearchPage(DriverContext driverContext)
       : base(driverContext)
        { }

        #region ElementLoactors
        public string ResultTable_Count = "//th[contains(text(),'Actions')]//ancestor::table[1]";
        public string ResultTable = "//th[contains(text(),'Actions')]//ancestor::table[1]//tr";
        private readonly ElementLocator Button_Clear = MemberSearch_Button_Custom_ElementLocatorXpath("Clear");
        private readonly ElementLocator TextBox_LoyaltyID = MemberSearch_TextBox_Custom_ElementLocatorXpath("LoyaltyID:");
        private readonly ElementLocator TextBox_Email = MemberSearch_TextBox_Custom_ElementLocatorXpath("Email");
        private readonly ElementLocator TextBox_FirstName = MemberSearch_TextBox_Custom_ElementLocatorXpath("First Name");
        private readonly ElementLocator TextBox_LastName = MemberSearch_TextBox_Custom_ElementLocatorXpath("Last Name");
        private readonly ElementLocator Button_Search = MemberSearch_Button_Custom_ElementLocatorXpath("Search");
        #endregion
      

        /// <summary>
        ///Search Based On LoyaltyID
        /// </summary>
        /// <param name="LoyaltyID"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_BasedOnLoyaltyID(string LoyaltyID, out string Message)
        {
            try
            {
                Driver.GetElement(Button_Clear).ClickElement();
                Driver.GetElement(TextBox_LoyaltyID).SendText(LoyaltyID);
                Driver.GetElement(Button_Search).ClickElement();
                if (VerifyNumberOfRowsIncludingHeader(ResultTable_Count, 2))
                {
                    Message = "One Row Appeared in the results with LoyaltyID  " + LoyaltyID;
                    return true;
                }
                else
                {
                    Message = "One Row are not appeared in the results";
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Loyalty ID:" + LoyaltyID);
            }
        }


        /// <summary>
        ///Select Based on Input like firstname , last name or Email
        /// </summary>
        /// <param name="Input"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public void Select(string Input)
        {
            try
            {
                ElementLocator Select_Link = new ElementLocator(Locator.XPath, "//td//span[text()='" + Input + "']//parent::td//following-sibling::td//a[text()='Select']");
                if (Driver.IsElementPresent(Select_Link, .2))
                {
                    Driver.GetElement(Select_Link).ClickElement();
                }

            }
            catch (Exception)
            {
                throw new Exception("Failed to Click on select");
            }
        }

        /// <summary>
        ///Select Based on Input like firstname , last name or Email
        /// </summary>
        /// <param name="Input"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public void Select()
        {
            try
            {
                ElementLocator Select_Link = new ElementLocator(Locator.XPath, "//td//a[text()='Select']");
                if (Driver.IsElementPresent(Select_Link, .2))
                {
                    Driver.GetElement(Select_Link).ClickElement();
                }

            }
            catch (Exception)
            {
                throw new Exception("Failed to Click on select");
            }
        }


        /// <summary>
        ///Search Based On Email
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_EmailID(string Email, out string Message)
        {
            try
            {
                Driver.GetElement(Button_Clear).ClickElement();
                Driver.GetElement(TextBox_Email).SendText(Email);
                Driver.GetElement(Button_Search).ClickElement();
                if (VerifyNumberOfRowsIncludingHeader(ResultTable_Count, 2))
                {
                    return VerifyTableRowsBasedOnHeader(ResultTable, "th", "Email", Email, out Message);
                }
                else
                {
                    Message = "Invalid Output Appeared";
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Email:" + Email);
            }
        }

        /// <summary>
        ///Search Based On FirstName
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_FirstName(string FirstName, out string Message)
        {
            try
            {
                Driver.GetElement(Button_Clear).ClickElement();
                Driver.GetElement(TextBox_FirstName).SendText(FirstName);
                Driver.GetElement(Button_Search).ClickElement();
                return VerifyTableRowsBasedOnHeader(ResultTable, "th", "First Name", FirstName, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search First Name:" + FirstName);
            }
        }

        /// <summary>
        ///Search Based On Last Name
        /// </summary>
        /// <param name="LastName"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_LastName(string LastName, out string Message)
        {
            try
            {
                Driver.GetElement(Button_Clear).ClickElement();
                Driver.GetElement(TextBox_LastName).SendText(LastName);
                Driver.GetElement(Button_Search).ClickElement();
                return VerifyTableRowsBasedOnHeader(ResultTable, "th", "Last Name", LastName, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Last Name:" + LastName);
            }
        }

        /// <summary>
        ///Search Based On no inputs, Hence it return all the records with only email id
        /// </summary>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_WithBlankInputs(out string Message)
        {
            try
            {
                Driver.GetElement(Button_Clear).ClickElement();
                Driver.GetElement(Button_Search).ClickElement();
                return VerifyTableRowsBasedOnHeader_ExlusiveForNoInputs(ResultTable, "Email", "@", out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search With no inputs:");
            }
        }
        /// <summary>
        /// To verify a member exists in CS Portal
        /// </summary>
        /// <param name="member"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool VerifyMemberExists(string member, out string Message)
        {
            try
            {
                Click_OnButton(Button_Clear);
                Driver.GetElement(TextBox_FirstName).SendText(member);
                Click_OnButton(Button_Search);
                if (Driver.IsElementPresent(By.XPath("//td//span[text()='" + member + "']")))
                {
                    Message = "Member already exists :" + member;
                    return true;
                }
                else
                {
                    Message = "Creating a Member :" + member;
                    return false;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search for the Member:" + member);
            }
        }

        public TestStep Search_BasedOnLoyaltyID(string LoyaltyID, List<TestStep> listOfTestSteps)
        {
            string stepName = "Search Based on Loyalty ID";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                Search_BasedOnLoyaltyID(LoyaltyID,out string searchMessage);
                testStep.SetOutput(searchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep Select(string Input, List<TestStep> listOfTestSteps)
        {
            string stepName = "Select User from the output ";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                Select(Input);
                testStep.SetOutput(stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }
    }
}