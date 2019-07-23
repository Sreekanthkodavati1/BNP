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
	public class BTA143_REST_GetMemberById : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;

        [TestMethod]
        public void BTA143_REST_GetMemberByID_Positive()
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
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<String>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching member through rest service using IpCode";
                    JObject response = (JObject)rest_Service_Method.GetMemberDetailsByIpCode(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("IPCODE=" + response["data"].Value<int>("id") + ";Name=" + response["data"].Value<String>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating the response through assertions";
                    Assert.AreEqual(member["data"].Value<String>("firstName"), response["data"].Value<String>("firstName"), "Expected value is " + member["data"].Value<String>("firstName") + " and actual value is " + response["data"].Value<String>("firstName"));
                    Assert.AreEqual(member["data"]["cards"][0].Value<String>("cardNumber"), response["data"]["cards"][0].Value<String>("cardNumber"), "Expected value is " + member["data"]["cards"][0].Value<String>("cardNumber") + " and actual value is " + response["data"]["cards"][0].Value<String>("cardNumber"));
                    Assert.AreEqual(member["data"].Value<String>("username"), response["data"].Value<String>("username"), "Expected value is " + member["data"].Value<String>("username") + " and actual value is " + response["data"].Value<String>("username"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    Logger.Info("test  failed");
                    testStep.SetOutput(member.Value<String>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(member.Value<String>("developerMessage"));
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
        public void BTA143_REST_GetMemberByID_Negative()
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
                stepName = "Fetching member through rest service by providing Invalid IpCode";
                response = (JObject)rest_Service_Method.GetMemberDetailsByIpCode(common.RandomString(12));
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(false);
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
