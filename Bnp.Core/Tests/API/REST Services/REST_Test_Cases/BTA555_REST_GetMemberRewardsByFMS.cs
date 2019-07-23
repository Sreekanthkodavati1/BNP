using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;


namespace Bnp.Core.Tests.API.REST_Services.REST_Test_Cases
{
    [TestClass]
    public class BTA555_REST_GetMemberRewardsByFMS : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA555_REST_GetMemberRewardsByFMS_POSITIVE()
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
                    stepName = "Fetching member rewards through rest service using First Name";
                    JObject firstNameResponse = (JObject)rest_Service_Method.GetMemberRewardsByFMS(MemberSearchIdentity.FirstName, member["data"].Value<string>("firstName") + "");
                    testStep.SetOutput("The member with IPCODE = " + firstNameResponse["data"][0].Value<int>("memberId") +
                        " and; first name: "+ member["data"].Value<string>("firstName") +
                        " has the reward with; rewardID = " + firstNameResponse["data"][0].Value<int>("rewardId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching member rewards through rest service using email";
                    JObject emailResponse = (JObject)rest_Service_Method.GetMemberRewardsByFMS(MemberSearchIdentity.EmailAddress, member["data"].Value<string>("email") + "");
                    testStep.SetOutput("The member with IPCODE = " + emailResponse["data"][0].Value<int>("memberId") +
                        " and; email: " + member["data"].Value<string>("email") +
                        " has the reward with; rewardID = " + emailResponse["data"][0].Value<int>("rewardId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    string CardID = member["data"]["cards"][0].Value<string>("cardNumber");
                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching member rewards through rest service using CARD ID";
                    JObject cardIDResponse = (JObject)rest_Service_Method.GetMemberRewardsByFMS(MemberSearchIdentity.CardID, CardID);
                    testStep.SetOutput("The member with IPCODE = " + cardIDResponse["data"][0].Value<int>("memberId") +
                        " and; CardID: " + CardID +
                        " has the reward with; rewardID = " + cardIDResponse["data"][0].Value<int>("rewardId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate the rewardid got from response in db";
                    string dbresponse = DatabaseUtility.GetRewardIDfromDBUsingIPCODEREST(firstNameResponse["data"][0].Value<int>("memberId") + "");
                    Assert.AreEqual(firstNameResponse["data"][0].Value<int>("rewardId") + "", dbresponse, "Expected value is " + firstNameResponse["data"][0].Value<int>("rewardId") + " and actual value is " + dbresponse);
                    testStep.SetOutput("RewardID returned from db is " + dbresponse + " which matches with the rewardID from the response: " + firstNameResponse["data"][0].Value<int>("rewardId"));
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
        public void BTA555_REST_GetMemberRewardsByFMS_NEGATIVE()
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
                stepName = "Fetching member rewards through rest service using Invalid First Name";
                response = (JObject)rest_Service_Method.GetMemberRewardsByFMS(MemberSearchIdentity.FirstName, common.RandomString(15));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code from GetMemberRewardsByFMS service for a member with " +
                        "invalid First Name is: " + response.Value<int>("responseCode") + "");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep); testStep = TestStepHelper.StartTestStep(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate the developer message for the above response code from the Service";
                    if (response.Value<int>("responseCode") == 10504)
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
