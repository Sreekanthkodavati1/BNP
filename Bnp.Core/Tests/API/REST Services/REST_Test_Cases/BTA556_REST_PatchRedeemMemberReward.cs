using System;
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
    public class BTA556_REST_PatchRedeemMemberReward : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA556_REST_PatchRedeemMemberReward_POSITIVE()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member through rest service";
                JObject member = (JObject)rest_Service_Method.PostMembergeneral();
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));

                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching member rewards through rest service using IpCode";
                    JObject response = (JObject)rest_Service_Method.GetMemberRewardsByIpCode(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("The member with IPCODE = " + response["data"][0].Value<int>("memberId") + " has the reward with rewardID = " + response["data"][0].Value<int>("rewardId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Redeeming member rewards through rest service using RewardID";
                    response = (JObject)rest_Service_Method.PatchRedeemMemberRewardByUsingRewardId(response["data"][0].Value<int>("id") + "");
                    testStep.SetOutput("The member with IPCODE = " + response["data"].Value<int>("memberId") + " has redeemed the reward with rewardID = " + response["data"].Value<int>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate the redemptiondate got from response with DB";
                    string dbresponse = DatabaseUtility.GetRedemptionDatefromDBUsingRewardIdREST(response["data"].Value<int>("id") + "");
                    DateTime datedb = Convert.ToDateTime(dbresponse).Date;
                    DateTime resp = Convert.ToDateTime(response["data"].Value<DateTime>("redemptionDate")).Date;
                    Assert.AreEqual(resp, datedb, "Expected value is " + resp + " and actual value is " + datedb);
                    testStep.SetOutput("Redemption Date returned from db is " + dbresponse + " which matches with the Redemption Date from the response: " + resp);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test passed");

                    testCase.SetStatus(true);

                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(member.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(member.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
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

        [TestMethod]
        public void BTA556_REST_PatchRedeemMemberReward_NEGATIVE()
        {

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member through rest service";
                JObject member = (JObject)rest_Service_Method.PostMembergeneral();
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));

                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching member rewards through rest service using IpCode";
                    JObject response = (JObject)rest_Service_Method.GetMemberRewardsByIpCode(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("The member with IPCODE = " + response["data"][0].Value<int>("memberId") + " has the reward with rewardID = " + response["data"][0].Value<int>("rewardId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Redeeming member rewards through rest service using RewardID";
                    response = (JObject)rest_Service_Method.PatchRedeemMemberRewardByUsingRewardId(response["data"][0].Value<int>("id") + "");
                    testStep.SetOutput("The member with IPCODE = " + response["data"].Value<int>("memberId") + " has redeemed the reward with rewardID = " + response["data"].Value<int>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    Logger.Info("Test Method Started");
                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Redeeming an already redeemed memberreward using rewardID through rest service";
                    response = (JObject)rest_Service_Method.PatchRedeemMemberRewardByUsingRewardId(response["data"].Value<int>("id") + "");
                    if (response.Value<string>("isError") == "True")
                    {
                        testStep.SetOutput("The response code from PatchRedeemMemberReward service for an already redeemed memberreward " +
                            "with RewardId is " + response.Value<int>("responseCode") + "");
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep); testStep = TestStepHelper.StartTestStep(testStep);

                        testStep = TestStepHelper.StartTestStep(testStep);
                        stepName = "Validate the user message for the above response code from the Service";
                        if (response.Value<int>("responseCode") == 50102)
                        {
                            testStep.SetOutput("userMessage from response is: " + response.Value<string>("userMessage"));
                            testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                            listOfTestSteps.Add(testStep);
                            Logger.Info("test passed");
                            testCase.SetStatus(true);
                        }
                        else
                        {
                            Logger.Info("test failed");
                            testStep.SetOutput("response message is" + response.Value<string>("userMessage") + "and response code is" + response.Value<int>("responseCode"));
                            testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                            listOfTestSteps.Add(testStep);
                            throw new Exception("response message is" + response.Value<string>("userMessage") + "and response code is" + response.Value<int>("responseCode"));
                        }
                    }
                    else
                    {
                        Logger.Info("test failed");
                        testStep.SetOutput(response.Value<string>("userMessage"));
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep);
                        throw new Exception(response.Value<string>("userMessage"));
                    }
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
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
