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
   public class BTA372_REST_UpdateMember : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA372_REST_UpdateMember_POSITIVE()
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
                    testStep.SetOutput("Members details are IPCODE= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<string>("firstName") + "; and Email=" + member["data"]["cards"][0].Value<string>("email"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                }
                else
                {
                    testStep.SetOutput(member.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    Logger.Info("test  failed");
                    listOfTestSteps.Add(testStep);
                    throw new Exception(member.Value<string>("developerMessage"));
                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Updating Member through REST Service";
                response = (JObject)rest_Service_Method.PatchUpdateMemberwithIpcode(member["data"].Value<int>("id")+"");
                if (response.Value<String>("isError") == "False")
                {
                    testStep.SetOutput("The Member with IPcode: " + response["data"].Value<int>("id")+" email address has been updated to : \""+ response["data"].Value<string>("email")+"\" and city from member attribute details has been updated to: \"" + response["data"]["attributeSets"][0]["memberDetails"][0].Value<string>("city")+"\"");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);


                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetEmailfromDBUsingIpcodeREST(response["data"].Value<int>("id"));
                    Assert.AreEqual(response["data"].Value<string>("email"), dbresponse, "Expected value is " + response["data"].Value<string>("email") + " and actual value is " + dbresponse);
                    string dbresponse1 = DatabaseUtility.GetCityusingIPCODEREST(response["data"].Value<int>("id"));
                    Assert.AreEqual(dbresponse1, response["data"]["attributeSets"][0]["memberDetails"][0].Value<string>("city"), "Expected city name is"+dbresponse1+" and the actual city name is" + response["data"]["attributeSets"][0]["memberDetails"][0].Value<string>("city"));
                    testStep.SetOutput("Updated EMAIL-ID  from database is: \"" + dbresponse+"\" and Updated city from DB is \""+ response["data"]["attributeSets"][0]["memberDetails"][0].Value<string>("city")+"\"");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test  passed");

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
        public void BTA372_REST_UpdateMember_NEGATIVE()
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
                stepName = "Updating member using invalid IPCODE";
                response = (JObject)rest_Service_Method.PatchUpdateMemberwithIpcode(common.RandomNumber(12));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code from \"Update Member \" service for a user with " +
                        "invalid IPCODE is: " + response.Value<int>("responseCode") + "");
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