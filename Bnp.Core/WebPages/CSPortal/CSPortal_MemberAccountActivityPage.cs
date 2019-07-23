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
    /// This class handles Customer Service Portal > Member Search > Select Member > Select Account Activity Page
    /// </summary
   public class CSPortal_MemberAccountActivityPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_MemberAccountActivityPage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region Element Locators
        public string Table_CustomerServiceNotes = "//h2[contains(text(),'Customer Service Notes')]//following::table[1]//tr";
        private readonly ElementLocator TextBox_FromDate = new ElementLocator(Locator.XPath, "//input[contains(@name,'dpFromDate$dateInput')]");
        private readonly ElementLocator TextBox_ToDate = new ElementLocator(Locator.XPath, "//input[contains(@name,'dpToDate$dateInput')]");
        private readonly ElementLocator Table_PurchaseHistory = new ElementLocator(Locator.XPath, "//h3[contains(text(),'Purchase History')]//following::table[1]//tr/th[2]");
        private readonly ElementLocator Button_Search = new ElementLocator(Locator.XPath, "//a[text()='Search']");
        private readonly ElementLocator PurchaseHistory = new ElementLocator(Locator.XPath, "//div//h3[contains(text(),'Purchase History')]//following::table//tr");
        private readonly ElementLocator BonusAndAppeasmentTable = new ElementLocator(Locator.XPath, "//div//h3[contains(text(),'Purchase History')]//following::table//tr");

        readonly string PurchaseHistory_table = "//div//h3[contains(text(),'Purchase History')]//following::table//tr";
        #endregion

        /// <summary>
        /// Verify Loyalty ID
        /// </summary>
        /// <param name="LoyaltyID"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyLoyaltyId(string LoyaltyID, out string Message)
        {
              return IsElementPresentBasedon_LabelAndText("LoyaltyId Number:", LoyaltyID, out Message);
        }


        /// <summary>
        /// Verify FirstName
        /// </summary>
        /// <param name="FirstName"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyFirstName(string FirstName, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndText("Name:", FirstName, out Message1);
        }

        /// <summary>
        /// Verify LastName
        /// </summary>
        /// <param name="LastName"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyLastName(string LastName, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndText("Name:", LastName, out Message1);
        }

        /// <summary>
        /// Verify PrimaryEmail
        /// </summary>
        /// <param name="PrimaryEmail"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyPrimaryEmail(string PrimaryEmail, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("Primary Email Address:", PrimaryEmail, out Message1);
        }


        /// <summary>
        /// Verify Merge Activity
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="Input"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyMergeActivity(string Header, string Input, out string Message)
        {
            return VerifyTableRowsBasedOnHeader(Table_CustomerServiceNotes, "th//a", Header, Input, out Message);
        }
              
        /// <summary>
        ///Verify Purchase History
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns>Message with Status (string)</returns>
        public string VerifyPurchaseHistoryBasedonHeader(string HeaderId)
        {
            string message = "";
            try
            {
                if (Driver.IsElementPresent(PurchaseHistory, 5))
                {
                    if( VerifyTableRowsBasedOnHeader(PurchaseHistory_table, "th", "Txn Number", HeaderId, out string Message))
                    {
                            message = HeaderId + " Transaction verfied Successfully in Purchase History ";

                            return message;
                    }
                    else
                    {
                        throw new Exception("Failed to verify Header Details: " + HeaderId);
                    }
                }
                else
                {
                    throw new Exception("Failed to verify Header Details: " + HeaderId);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify transaction in Purchase History " + HeaderId + e);
            }
        }

        /// <summary>
        ///Verify Purchase History based on transaction Header Id
        /// </summary>
        /// <param name="HeaderId"></param>
        /// <returns>Message with Status (string)</returns>
        public string VerifyPurchaseHistoryBasedonHeaderId(string HeaderId)
        {
            string message = "";
            try
            {
                if (Driver.IsElementPresent(PurchaseHistory, 5))
                {
                    if (VerifyTableRowsBasedOnHeaderId(PurchaseHistory_table, "th", "Header Id", HeaderId, out string Message))
                    {
                        message = HeaderId + " Transaction verfied Successfully in Purchase History ";
                        return message;
                    }
                    else
                    {
                        throw new Exception("Failed to verify Header Details: " + HeaderId);
                    }
                }
                else
                {
                    throw new Exception("Failed to verify Header Details: " + HeaderId);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify transaction in Purchase History " + HeaderId);
            }
        }

        public bool VerifyAppeasementsandPoints(string Points,out string Message)
        {
            try
            {
                ElementLocator elm = new ElementLocator(Locator.XPath, "//div//h3[contains(text(),'Appeasements and Bonuses')]//following::table[1]//tr//span[text()='" + Points + "' and contains(@id,'Point')]");
                if (VerifyElementandScrollToElement(elm))
                {
                    Message = "Points Appeared under Appeasments and Bonuses and Points Details:" + Points;
                    return true;
                }
                throw new Exception("Expceted Point Not appeared Points Details are:" + Points);
            }catch(Exception e)
            {
                throw new Exception("Failed to Validate Points under Appeasments and Bonuses" + e.Message);

            }
        }

        /// <summary>
        /// Select specific date to verify purchase history 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        public void SelectDate_RC(DateTime FromDate, DateTime ToDate)
        {
            Driver.GetElement(TextBox_FromDate).SendText(FromDate.ToString());
            Driver.GetElement(TextBox_ToDate).SendText(ToDate.ToString());
            Driver.GetElement(TextBox_ToDate).SendKeys(Keys.Tab);
            Click_OnButton(Button_Search);
        }

        public TestStep VerifyFirstName(string FirstName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify FirstName on Account Activity Page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyFirstName(FirstName, out string Messge);
                testStep.SetOutput(Messge);
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

        public TestStep VerifyLastName(string LastName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Last Name on Account Activity Page";

            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyLastName(LastName, out string Messge);
                testStep.SetOutput(Messge);
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

        public TestStep VerifyPrimaryEmail(string PrimaryEmail, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Primary Email Address on Account Activity Page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyPrimaryEmail(PrimaryEmail, out string Messge);
                testStep.SetOutput(Messge);
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

        public TestStep VerifyMergeActivity(string Header, string Input, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verifying Merge Account Activity on Account Activity page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyMergeActivity(Header, Input, out string Message);
                testStep.SetOutput(Message);
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
