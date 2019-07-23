using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1231_SOAP_GetTierNames : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetTierNames")]
        [TestCategory("API_SOAP_GetTierNames_Positive")]
        [TestMethod]
        public void BTA1231_ST1390_SOAP_GetTierNames_VerifyExistingTierNameRecords()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            double elapsedTime = 0;
            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get all the TierNames from GetTierNames call";
                string[] tierNames = cdis_Service_Method.GetTierNames(out elapsedTime);
                testStep.SetOutput("Tier names from response are: " + string.Join(";",tierNames));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get all TierNames from the DB";
                List<string> tierNamesFromDb = DatabaseUtility.GetTierNamesFromDBCDIS();
                testStep.SetOutput("Tier names from database are: " + string.Join(";", tierNamesFromDb));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the Tier names from the response with database";
                IEnumerable<string> tierNamesCompare = tierNamesFromDb.Except(tierNames);
                if (!tierNamesCompare.Any())
                {
                    testStep.SetOutput("All the TierNames returned by GETTIERNAME method are present in DB and have been validated");
                    Logger.Info("All the TierNames returned by GETTIERNAME method are present in DB and have been validated");
                }
                else
                {
                    throw new Exception("TierNames from response are different from the DB");
                }
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
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
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
