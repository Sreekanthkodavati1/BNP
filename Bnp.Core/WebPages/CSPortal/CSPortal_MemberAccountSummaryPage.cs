using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Member Search > Select Member > Select Account Summary Page
    /// </summary
  public  class CSPortal_MemberAccountSummaryPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_MemberAccountSummaryPage(DriverContext driverContext)
        : base(driverContext)
        { }
        #region Element Locators
        public string Table_CustomerServiceNotes = "//h2[contains(text(),'Customer Service Notes')]//following::table[1]//tr";
        #endregion

        /// <summary>
        /// Verify Loyalty ID
        /// </summary>
        /// <param name="LoyaltyID"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyLoyaltyId(string LoyaltyID,out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("LoyaltyId Number:", LoyaltyID, out Message1);
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
        /// Verify AddressLine1
        /// </summary>
        /// <param name="AddressLine1"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyAddressLine1(string AddressLine1, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("Address Line 1:", AddressLine1, out Message1);
        }

        /// <summary>
        /// Verify AddressLine2
        /// </summary>
        /// <param name="AddressLine2"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyAddressLine2(string AddressLine2, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("Address Line 2:", AddressLine2, out Message1);
        }
        /// <summary>
        /// Verify City
        /// </summary>
        /// <param name="City"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyCity(string City, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("City:", City, out Message1);
        }

        /// <summary>
        /// Verify State
        /// </summary>
        /// <param name="State"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyState(string State, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("State:", State, out Message1);
        }

        /// <summary>
        /// Verify Zip Code
        /// </summary>
        /// <param name="Zip Code"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyZipCode(string ZipCode, out string Message1)
        {
            return IsElementPresentBasedon_LabelAndWithExactText("Zip Code:", ZipCode, out Message1);
        }

        /// <summary>
        /// Capture Loyalty ID
        /// </summary>
        /// <param name="LoyaltyID"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public string CaptureLoyaltyId()
        {
            return CaptureTextBasedon_Label("LoyaltyId Number:");
        }

        /// <summary>
        /// Verify Merge Activity
        /// </summary>
        /// <param name="Header"></param>
        /// <param name="Input"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyMergeActivity(string Header,string Input, out string Message1)
        {
            return VerifyTableRowsBasedOnHeader(Table_CustomerServiceNotes, "th//a", Header, Input, out Message1);
        }

        public TestStep VerifyMergeActivity(string Header, string Input, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verifying Merge Account Activity on Account Summary page";
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


        public TestStep VerifyLoyaltyId(string LoyaltyID, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Loyalty Id";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyLoyaltyId(LoyaltyID,out string Messge);
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

        public TestStep VerifyFirstName(string FirstName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify FirstName on Account Summary Page";
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
            string stepName = "Verify Last Name on Account Summary Page";

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
            string stepName = "Verify Primary Email Address on Account Summary Page";
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

        public TestStep VerifyAddressLine1(string AddressLine1, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Address Line 1 on Account Summary Page";

            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyAddressLine1(AddressLine1, out string Messge);
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

        public TestStep VerifyAddressLine2(string AddressLine2, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Address Line 2 on Account Summary Page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyAddressLine2(AddressLine2, out string Messge);
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

        public TestStep VerifyCity(string City, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify City on Account Summary Page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyCity(City, out string Messge);
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

        public TestStep VerifyState(string State, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify State on Account Summary Page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyState(State, out string Messge);
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

        public TestStep VerifyZipCode(string ZipCode, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify 	Zip Code on Account Summary Page";

            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyZipCode(ZipCode, out string Messge);
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


    }
}
