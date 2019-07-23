using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA244_CDIS_GetLoyaltyEvents:ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA244_CDIS_GetLoyaltyEvents_PositiveCase()

        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                List<string> activities = new List<string>();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get loyalty Events names from GetLoyalityEvents method";
                LoyaltyEventStruct[] loyaltyEventStruct = cdis_Service_Method.GetLoyalityEvents();
                foreach (var loyaltyEvent in loyaltyEventStruct) 
                {
                    activities.Add(loyaltyEvent.Name.ToString());
                }

                testStep.SetOutput("The following are the loyaltyEvent Name's from the GetLoyalityEvents response:; " + string.Join(";", activities.ToArray()));
                Logger.Info("Description of Loyality Currencey:" + loyaltyEventStruct[0].Name + "Points" + loyaltyEventStruct[0].DefaultPoints);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating loyaltyEvent names in DB";
                List<string> dbresponse = DatabaseUtility.GetLoyaltyEventsfromDBUsingIdSOAP();
                for (int i = 0; i < loyaltyEventStruct.Length; i++)
                {
                    Assert.AreEqual(loyaltyEventStruct[i].Name + "", dbresponse[i], "Expected Value is " + loyaltyEventStruct[i].Name + " Actual Value is " + dbresponse[i]);
                }

                testStep.SetOutput("Total rows Count from database : " + dbresponse.Count+ "; and following are the loyaltyevents names from DB:; "+ string.Join(";", dbresponse.ToArray()));
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

