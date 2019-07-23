using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA228_CDIS_GetRewardCatalog : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA228_CDIS_GetRewardCatalog_PositiveCase()

        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            string RwdID = string.Empty;
            string RwdName = string.Empty;
            ArrayList RewardID = new ArrayList();
            ArrayList RewardName = new ArrayList();


            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Reward Catalog with CDIS service";
                RewardCatalogSummaryStruct[] rewardCatalogs = cdis_Service_Method.GetRewardCatalog();
                foreach (var rewardcatalog in rewardCatalogs)
                {
                    RewardID.Add(rewardcatalog.RewardID);
                    RewardName.Add(rewardcatalog.RewardName);
                }
                RwdID = string.Join(";", RewardID.ToArray());
                RwdName = string.Join(";", RewardName.ToArray());
                testStep.SetOutput("The RewardID from the reponse are: ;" + RwdID + "; and there corresponding reward names are: ; "+ RwdName);
                Logger.Info("RewardID: " + RwdID+ " and Reward names are: "+ RwdName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetRewardCatalogfromDBUsingIdSOAP(rewardCatalogs[0].RewardID + "");
                Assert.AreEqual(rewardCatalogs[0].RewardID.ToString(), dbresponse, "Expected Value is " + rewardCatalogs[0].RewardID + " Actual Value is " + dbresponse);
                testStep.SetOutput("RewardID from database for the first reward catalog item is : " + dbresponse);
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
