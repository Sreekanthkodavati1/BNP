using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Member Search > Select Member > Select Merge accounts 
    /// </summary
    public class CSPortal_MergeAccountPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_MergeAccountPage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Element Locators
        public string Table_MergeToMember = "//div[@id='MergeConfirmation']//h3//span[contains(text(),'Merge To Member:')]//parent::h3//following::table[1]//tr";
        public string Table_MergeFromMember = "//div[@id='MergeConfirmation']//h3//span[contains(text(),'Merge From Member:')]//parent::h3//following::table[1]//tr";
        private readonly ElementLocator TextBox_FromMember = new ElementLocator(Locator.XPath, "//span[text()='From Member:']//following-sibling::input");
        private readonly ElementLocator Button_Validate = new ElementLocator(Locator.XPath, "//span[text()='From Member:']//following-sibling::a[text()='Validate']");
        private readonly ElementLocator Button_MergeMembers = MergeAccounts_Button_Custom_ElementLocatorXpath("Merge Members");
        private readonly ElementLocator Message_Success = new ElementLocator(Locator.XPath, "//span[contains(text(),'Accounts successfully merged.')]");
        #endregion Element Locators

        /// <summary>
        /// Navigating to MemberRegistration Board
        /// </summary>
        /// <param name="Member_LoyaltyId_One"></param>
        /// <param name="Member_LoyaltyId_two"></param>
        /// <param name="Message"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool MergeAccounts(string Member_LoyaltyId_One, string Member_LoyaltyId_two, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_FromMember).SendText(Member_LoyaltyId_two);
                Driver.GetElement(Button_Validate).ClickElement();
                if (VerifyTableRowsBasedOnHeader(Table_MergeToMember, "th//span", "Loyalty ID", Member_LoyaltyId_One, out Message) && VerifyTableRowsBasedOnHeader(Table_MergeFromMember, "th//span", "Loyalty ID", Member_LoyaltyId_two, out Message))
                {
                    Driver.GetElement(Button_MergeMembers).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    if (Driver.IsElementPresent(Message_Success, 1))
                    {
                        Message = "Accounts successfully merged and Account " + Member_LoyaltyId_One + " was merged into " + Member_LoyaltyId_two;
                        return true;
                    }
                    else
                    {
                        Message = "Failed to Verify  Merge Accounts and Account " + Member_LoyaltyId_One + " / merged into " + Member_LoyaltyId_two + "is not available";
                        throw new Exception(Message);
                    }
                }

                Message = "Failed to Verify  Merge Accounts and Account " + Member_LoyaltyId_One + " / merged into " + Member_LoyaltyId_two + "is not available";
                throw new Exception(Message);
            }
            catch (Exception e)
            {
                Message = "Failed to Verify  Merge Accounts and Account " + Member_LoyaltyId_One + " / merged into " + Member_LoyaltyId_two + "is Due to"+e.Message;
                throw new Exception(Message);

            }
        }

        /// <summary>
        /// Navigating to MemberRegistration Board
        /// </summary>
        /// <param name="Member_LoyaltyId_One"></param>
        /// <param name="Member_LoyaltyId_two"></param>
        /// <param name="Message"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool MergeAccounts_WithAlreadyMergedUser(string Member_LoyaltyId_One, string Member_LoyaltyId_two,string ErrorMessage, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_FromMember).SendText(Member_LoyaltyId_two);
                Driver.GetElement(Button_Validate).ClickElement();

                if (VerifyTableRowsBasedOnHeader(Table_MergeToMember, "th//span", "Loyalty ID", Member_LoyaltyId_One, out Message) && VerifyTableRowsBasedOnHeader(Table_MergeFromMember, "th//span", "Loyalty ID", Member_LoyaltyId_two, out Message))
                {
                    Driver.GetElement(Button_MergeMembers).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    return ValidateErrorMessage(ErrorPageLevel(ErrorMessage), ErrorMessage, out Message);
                }
     
                Message = "Failed to Verify  Merge Accounts and Account " + Member_LoyaltyId_One + " / merged into " + Member_LoyaltyId_two + "is Failed and Refer Screenshot for more details";
                throw new Exception(Message);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Navigate Member Registration Page Due to:"+e.Message);
            }
        }

        public TestStep MergeAccounts(string Member_LoyaltyId_One, string Member_LoyaltyId_two,  List<TestStep> listOfTestSteps)
        {
            string stepName = "Mergining Two Accounts";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                MergeAccounts(Member_LoyaltyId_One, Member_LoyaltyId_two,out  string Message);
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

        public TestStep MergeAccounts_WithAlreadyMergedUser(string Member_LoyaltyId_One, string Member_LoyaltyId_two, string ErrorMessage,  List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Agent shouldn't be Able to Merge Loaylty Number with  Already Merged Loaylty Number";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                MergeAccounts_WithAlreadyMergedUser(Member_LoyaltyId_One, Member_LoyaltyId_two, ErrorMessage, out string Message);
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
