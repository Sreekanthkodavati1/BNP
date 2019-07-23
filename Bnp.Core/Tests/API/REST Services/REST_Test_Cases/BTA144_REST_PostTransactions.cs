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
    public class BTA144_REST_PostTransactions : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA144_REST_PT_AddTransactionToMember_WithAllFields_UsingVCKEY_POSITIVE()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);
            JObject response;
            List<string> ats_txnheader = new List<string>();

            try
            {

                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member through rest service";
                JObject member = (JObject)rest_Service_Method.PostMembergeneral();
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + "; Name=" + member["data"].Value<string>("firstName") + "; VC_KEY=" + member["data"]["cards"][0].Value<string>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                }
                else
                {
                    testStep.SetOutput(member.Value<string>("responseCode") + member.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep); 
                    Logger.Info(member.Value<string>("responseCode") + member.Value<string>("developerMessage"));
                    Logger.Info("Test  failed");
                    throw new Exception(member.Value<string>("developerMessage"));

                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding transaction through rest service to a member with all fields by providing the VCKEY";
                response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithAllFields(member["data"]["cards"][0].Value<string>("id"));
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("Transaction Rowkey: " + response["data"]["transactions"].Value<string>("id") + "; TransactionID: "+response["data"]["transactions"].Value<string>("transactionId")+"; TransactionAmount: " + response["data"]["transactions"].Value<string>("amount"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate data from DB";
                    ats_txnheader = DatabaseUtility.GetVCkeyfromDBUsingROWKEYREST(response["data"]["transactions"].Value<string>("id"));
                    testStep.SetOutput("The Following details are verified in DB:"+
                                        "; VCKEY is: "+ ats_txnheader[0]+
                                        "; TransactionID is: " + ats_txnheader[1] +
                                        "; Transaction Amt is: " + ats_txnheader[2]);
                    Assert.AreEqual(member["data"]["cards"][0].Value<string>("id"), ats_txnheader[0], "Expected value is " + member["data"]["cards"][0].Value<string>("id") + " and actual value is " + ats_txnheader[0]);
                    Assert.AreEqual(response["data"]["transactions"].Value<string>("transactionId"), ats_txnheader[1], "Expected value is " + response["data"]["transactions"].Value<string>("transactionId") + " and actual value is " + ats_txnheader[1]);
                    Assert.AreEqual(response["data"]["transactions"].Value<string>("amount"), ats_txnheader[2], "Expected value is " + response["data"]["transactions"].Value<string>("amount") + " and actual value is " + ats_txnheader[2]);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
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
        public void BTA144_REST_PT_AddTransactionToMember_WithRequiredFields_UsingVCKey_POSITIVE()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);
            JObject response;
            List<string> ats_txnheader = new List<string>();

            try
            {

                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member through rest service";
                JObject member = (JObject)rest_Service_Method.PostMembergeneral();
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName") + "; VC_KEY=" + member["data"]["cards"][0].Value<string>("id"));
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
                stepName = "Adding transaction through rest service by providing the VCKey required Fields";
                response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithRequiredFields(member["data"]["cards"][0].Value<String>("id"));
                //Logger.Info("test  passed");
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("Transaction Rowkey: " + response["data"]["transactions"].Value<string>("id") + "; TransactionID: " + response["data"]["transactions"].Value<string>("transactionId") + "; TransactionAmount: " + response["data"]["transactions"].Value<string>("amount"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate data from DB";
                    ats_txnheader = DatabaseUtility.GetVCkeyfromDBUsingROWKEYREST(response["data"]["transactions"].Value<string>("id"));
                    testStep.SetOutput("The Following details are verified in DB:" +
                                        "; VCKEY is: " + ats_txnheader[0] +
                                        "; TransactionID is: " + ats_txnheader[1] +
                                        "; Transaction Amt is: " + ats_txnheader[2]);
                    Assert.AreEqual(member["data"]["cards"][0].Value<string>("id"), ats_txnheader[0], "Expected value is " + member["data"]["cards"][0].Value<string>("id") + " and actual value is " + ats_txnheader[0]);
                    Assert.AreEqual(response["data"]["transactions"].Value<string>("transactionId"), ats_txnheader[1], "Expected value is " + response["data"]["transactions"].Value<string>("transactionId") + " and actual value is " + ats_txnheader[1]);
                    Assert.AreEqual(response["data"]["transactions"].Value<string>("amount"), ats_txnheader[2], "Expected value is " + response["data"]["transactions"].Value<string>("amount") + " and actual value is " + ats_txnheader[2]);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(response.Value<String>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(response.Value<String>("developerMessage"));
                }
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding transaction through rest service by providing the VCKey for All Fields";
                response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithAllFields(member["data"]["cards"][0].Value<String>("id"));
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
        public void BTA144_REST_PT_AddTransactionUsingInvalidVCKey_NEGATIVE()
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
                stepName = "Posting transaction to a member by providing Invalid VcKey";
                response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithRequiredFields(common.RandomNumber(12));
                if (response.Value<string>("responseCode") == "10508")
                {
                    testStep.SetOutput("The following are the error details when we add transaction using Invalid VCKEY:; Response Code: " + response.Value<string>("responseCode") + "; and corresponding DeveloperMessage: " + response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("The following are the error details when we add transaction using Invalid VCKEY: Response Code: " + response.Value<string>("responseCode") + " and corresponding DeveloperMessage: " + response.Value<string>("developerMessage"));
                    Logger.Info("Test Method Passed");
                }
                else
                {
                    testStep.SetOutput("Response Code=" + response.Value<string>("responseCode") + ", Message =" + response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(false);
                    Logger.Info("Test Method Failed");
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
    }
}
