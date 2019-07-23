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
    public class BTA1232_CDIS_GetTiers : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetTiers")]
        [TestCategory("API_SOAP_GetTiers_Positive")]
        [TestMethod]
        public void BTA1232_ST1390_SOAP_GetTiers_ExistingTiers()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            double elapsedTime;
            List<string> tierNames = new List<string>();
            List<string> tierAttributes = new List<string>();

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Tiers from CDIS service";
                TierDefStruct[] tiers = cdis_Service_Method.GetTiers(out elapsedTime);

                foreach (var tierName in tiers)
                {
                    tierNames.Add(tierName.Name);
                }
                //for (var i = 0; i < tiers.Count(); i++)
                //{
                //    tierAttributes.Add(tiers[0].ContentAttributes[0].AttributeName);
                //    tierAttributes.Add(tiers[0].ContentAttributes[1].AttributeName);
                //}

                testStep.SetOutput("Tiers from CDIS service are: " + string.Join("\n", tierNames)+
                    "\r\n Content Attributes  from CDIS service are: " + string.Join("\n ", tierAttributes));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Tiers from database";
                List<string> tierNamesFromDb = DatabaseUtility.GetTierNamesFromDBCDIS();
                testStep.SetOutput("Tier names from database are: " + string.Join("\n", tierNamesFromDb));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
              

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the Tiers  from the response with database";
                IEnumerable<string> tierNamesCompare = tierNamesFromDb.Except(tierNames);
                if (!tierNamesCompare.Any()) // && tierAttributes.Any())
                {
                    testStep.SetOutput("All configured Tier names from response have been validated with tiernames in the DB");
                    Logger.Info("All configured Tier names from response are validate with database");
                }
                else
                {
                    throw new Exception("All configured Tier names from response are different from database");
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
