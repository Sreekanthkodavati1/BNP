using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1228_SOAP_CreateCSNote : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CreateCsNote")]
        [TestCategory("API_SOAP_CreateCsNote_Positive")]
        [TestMethod]
        public void BTA1228_ST1351_SOAP_CreateCsNote_ExistingMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            long value;

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
                value = cdis_Service_Method.CreateCsNote(vc[0].LoyaltyIdNumber, "The customer password was reset", out elapsedTime);
                testStep.SetOutput("Created CS Note successfully for the member with member identity: "+ vc[0].LoyaltyIdNumber +" and the CSNOTE is: The customer password was reset");
                Logger.Info("Created CS Note successfully for an existing member with member identity: " + vc[0].LoyaltyIdNumber+ " and the CSNOTE is: The customer password was reset");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate CSNOTE with the DB value for the numericalID received from the response";
                string note=DatabaseUtility.GetFromSoapDB("Lw_Csnote", "Id", value.ToString(), "NOTE", string.Empty);
                testStep.SetOutput("Created CS Note successfully for the member with member identity: " + vc[0].LoyaltyIdNumber + " and the CSNOTE is: The customer password was reset");
                Assert.AreEqual("The customer password was reset", note, "The Expected value is : <The customer password was reset> and the Actual value is: " + note);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the elapsed time is greater than Zero";
                if (elapsedTime > 0)
                {
                    testStep.SetOutput("Elapsed time is greater than zero and the elapsed time is " + elapsedTime);
                }
                else
                {
                    throw new Exception("Elapsed time is not greater than Zero");
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

        [TestCategory("API_SOAP_CreateCsNote")]
        [TestCategory("API_SOAP_CreateCsNote_Negative")]
        [TestMethod]
        public void BTA1228_ST1352_SOAP_CreateCsNote_InvalidMemberIdentity()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create CS Note with invalid memberidentity";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                string memberIdentity = "";
                memberIdentity = common.RandomNumber(7);
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                string error = cdis_Service_Method.CreateCsNoteInvalid(memberIdentity, "The customer password was reset", out elapsedTime);
                if (error.Contains("Error code=3302") && error.Contains("Error Message=Unable to find member with identity"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3362. Actual error received is" + error);
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
