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
    public class BTA371_REST_GetMemberRewards : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA371_REST_GetMemberRewards_Positive()
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
                var k = member.Value<String>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<String>("isError"));
                if (member.Value<String>("isError") == "False")
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
                    stepName = "Validate the rewardid got from response in db";
                    String dbresponse = DatabaseUtility.GetRewardIDfromDBUsingIPCODEREST(response["data"][0].Value<int>("memberId")+"");
                    Assert.AreEqual(response["data"][0].Value<int>("rewardId") + "", dbresponse, "Expected value is " + response["data"][0].Value<int>("rewardId") + " and actual value is " + dbresponse);
                    testStep.SetOutput("RewardID returned from db is " + dbresponse+" which matches with the rewardID from the response: "+ response["data"][0].Value<int>("rewardId"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
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
        public void BTA371_REST_GetMemberRewards_NEGATIVE()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);
            JObject response;

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Fetching Member's rewardID through service using invalid IPCODE";
                response = (JObject)rest_Service_Method.GetMemberRewardsByIpCode(common.RandomNumber(12));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code from GetMemberRewards service for a member with " +
                        "invalid IPCODE is: " + response.Value<int>("responseCode") + "");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep); testStep = TestStepHelper.StartTestStep(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate the developer message for the above response code from the Service";
                    if (response.Value<int>("responseCode") == 10506)
                    {
                        testStep.SetOutput("developerMessage from response is: " + response.Value<string>("developerMessage"));
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep);
                        Logger.Info("test passed");
                        testCase.SetStatus(true);
                    }
                    else
                    {
                        Logger.Info("test failed");
                        testStep.SetOutput("response message is" + response.Value<string>("developerMessage") + "and response code is" + response.Value<int>("responseCode"));
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep);
                        throw new Exception("response message is" + response.Value<string>("developerMessage") + "and response code is" + response.Value<int>("responseCode"));
                    }
                }
                else
                {
                    Logger.Info("test failed");
                    testStep.SetOutput(response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(response.Value<string>("developerMessage"));
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