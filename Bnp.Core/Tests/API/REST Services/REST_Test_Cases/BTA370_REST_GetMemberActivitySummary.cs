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
    public class BTA370_REST_GetMemberActivitySummary : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA370_REST_GetMemberActivitySummary_POSITIVE()
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
                stepName = "Adding member through rest service";
                JObject member = (JObject)rest_Service_Method.PostMembergeneral();
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<string>("firstName") + "; VC_KEY= " + member["data"]["cards"][0].Value<string>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(member.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(member.Value<string>("developerMessage"));
                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding transaction through rest service by providing the VCKey";
                response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithAllFields(member["data"]["cards"][0].Value<string>("id"));
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("transaction rowKey : " + response["data"]["transactions"].Value<string>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testStep = TestStepHelper.StartTestStep(testStep);
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(response.Value<string>("developerMessage"));
                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Fetching Activity Details through rest service using IPCODE";
                JObject output = (JObject)rest_Service_Method.GetMemberActivitySummaryByIpCode(member["data"].Value<int>("id") + "");
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("TransactionsId (TxnHeaderID) is = " + output["data"]["transactionSummaries"][0].Value<string>("transactionId"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate transactionId, transactionDate and transactionAmount from the response";
                    Assert.AreEqual(output["data"]["transactionSummaries"][0].Value<string>("transactionId"), response["data"]["transactions"].Value<string>("transactionId"), "Expected value is " + response["data"]["transactions"].Value<string>("transactionId") + " and actual value is " + output["data"]["transactionSummaries"][0].Value<string>("transactionId"));
                    testStep.SetOutput("TransactionsId (TxnHeaderID) is = " + output["data"]["transactionSummaries"][0].Value<string>("transactionId")+
                                       ", TransactionDate  is = " + output["data"]["transactionSummaries"][0].Value<string>("transactionDate") +
                                       " and TransactionsAmount is = " + output["data"]["transactionSummaries"][0].Value<string>("transactionAmount"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test  Passed");
                    testCase.SetStatus(true);  
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(response.Value<string>("developerMessage"));
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

        [TestMethod]
        public void BTA370_REST_GetMemberActivitySummary_NEGATIVE()
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
                stepName = "Fetching Member summary through \"activitySummary\" service using IPCODE";
                response = (JObject)rest_Service_Method.GetMemberActivitySummaryByIpCode(common.RandomNumber(12));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code from \"activitySummary\" service for a user with " +
                        "with an invalid IPCODE is: " + response.Value<int>("responseCode") + "");
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

