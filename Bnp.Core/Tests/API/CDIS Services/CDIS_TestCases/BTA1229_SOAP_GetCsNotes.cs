using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1229_SOAP_GetCsNotes: ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetCsNotes")]
        [TestCategory("API_SOAP_GetCsNotes_Positive")]
        [TestMethod]
        public void BTA1229_ST1553_SOAP_GetCsNotes_PassMandatoryFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            long value;
            double elapsedTime = 0;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create CS Note for an Existing member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                string csNote = "CSNOTES SOAP AUTOMATION";
                value = cdis_Service_Method.CreateCsNote(vc[0].LoyaltyIdNumber, csNote, out elapsedTime);
                testStep.SetOutput("Created CS Note successfully for the member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: "+csNote);
                Logger.Info("Created CS Note successfully for an existing member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: "+csNote);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get CSNotes for an Existing member who already has CSNotes";
                var getCSNotes = cdis_Service_Method.GetCsNotes(vc[0].LoyaltyIdNumber, null, null, out elapsedTime);
                testStep.SetOutput("CSNotes for an Existing member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: "+getCSNotes[0].Note);
                Logger.Info("CSNotes for an existing member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: "+getCSNotes[0].Note);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate CSNOTE with the DB value for the numericalID received from the response";
                string note = DatabaseUtility.GetFromSoapDB("Lw_Csnote", "Id", value.ToString(), "NOTE", string.Empty);
                testStep.SetOutput("Created CS Note successfully for the member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: "+getCSNotes[0].Note);
                Assert.AreEqual(getCSNotes[0].Note, note, "The Expected value is : "+getCSNotes[0].Note + " and the Actual value is: " + note);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetCsNotes")]
        [TestCategory("API_SOAP_GetCsNotes_Negative")]
        [TestMethod]
        public void BTA1229_ST1555_SOAP_GetCsNotes_PassEndDateEarlierThanStartDate()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            long value;
            double elapsedTime = 0;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create CS Note for an Existing member";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                string csNote = "CSNOTES SOAP AUTOMATION";
                value = cdis_Service_Method.CreateCsNote(vc[0].LoyaltyIdNumber, csNote, out elapsedTime);
                testStep.SetOutput("Created CS Note successfully for the member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: " + csNote);
                Logger.Info("Created CS Note successfully for an existing member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: " + csNote);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get CSNotes for an Existing member by passing enddate earlier than start date";
                string error = cdis_Service_Method.getCsNotesNegative(vc[0].LoyaltyIdNumber, DateTime.Now, DateTime.Now.AddDays(-1));
                if (error.Contains("Error code=3204"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3204. Actual error received is" + error);
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetCsNotes")]
        [TestCategory("API_SOAP_GetCsNotes_Negative")]
        [TestMethod]
        public void BTA1229_ST1554_SOAP_GetCsNotes_PassInvalidMemberIdentity()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get CSNotes with an invalid member identity";
                string error = cdis_Service_Method.getCsNotesNegative(common.RandomNumber(12), DateTime.Now, DateTime.Now.AddDays(20));
                if (error.Contains("Error code=3302"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3302. Actual error received is" + error);
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }
    }
}
