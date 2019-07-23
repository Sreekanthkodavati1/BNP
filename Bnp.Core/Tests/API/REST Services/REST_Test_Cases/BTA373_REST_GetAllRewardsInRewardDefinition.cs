using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Bnp.Core.Tests.API.REST_Services.REST_Test_Cases
{
    [TestClass]
    public class BTA373_REST_GetAllRewardsInRewardDefinition : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA373_REST_GetRewardsInRewardDefinition()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            ArrayList RewardID = new ArrayList();
            ArrayList RewardName = new ArrayList();
            string RwdID = string.Empty;
            string RwdName = string.Empty;

            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Fetching all rewards from rewards definition through rest service ";
                JObject output = (JObject)rest_Service_Method.GetAllRewardsInRewardsDefinition();
                if (output.Value<string>("isError") == "False")
                {
                    JArray RewardIDItems = (JArray)output["data"];
                    foreach (JToken RewardIDItem in RewardIDItems)
                    {
                        RewardID.Add((string)RewardIDItem["id"]);
                        RewardName.Add((string)RewardIDItem["name"]);
                    }
                    RwdID = string.Join(";", RewardID.ToArray());
                    RwdName = string.Join(";", RewardName.ToArray());

                    testStep.SetOutput("Total reward id's fetched by service are; " + RwdID + " ;and the resp Reward Name's are ;" + RwdName);
                    //testStep.SetOutput("First reward id: " + output["data"][0].Value<int>("id") + " and the Reward Name is: " + output["data"][0].Value<string>("name"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating the data from database";
                    string dbresponse = DatabaseUtility.GetRewardNameUsingIDDBREST(output["data"][0].Value<int>("id") + "");
                    Assert.AreEqual(output["data"][0].Value<string>("name"), dbresponse, "Expected value is " + output["data"][0].Value<string>("name") + " and actual value is " + dbresponse);
                    testStep.SetOutput("Reward name from the database is  " + dbresponse+" for the rewardID: "+ output["data"][0].Value<int>("id") + "");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test  passed");

                    testCase.SetStatus(true);
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(output.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(output.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
                Logger.Info("test  failed" + e.Message);
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

