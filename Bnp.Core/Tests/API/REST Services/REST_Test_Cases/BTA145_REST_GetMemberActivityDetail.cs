using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
namespace Bnp.Core.Tests.API.REST_Services.REST_Test_Cases
{
	[TestClass]
	public class BTA145_REST_GetMemberActivityDetail : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;
		Common common;
		REST_Service_Methods rest_Service_Method;


		[TestMethod]
		public void BTA145_REST_GetMemberActivityDetail_Positive()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			String stepName = "";
			common = new Common(this.DriverContext);
			rest_Service_Method = new REST_Service_Methods(common);

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
					testStep.SetOutput("Member is added through PostMember service and following are the details, IPCODE:= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<String>("firstName")); ;
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
				}
				else
				{
					testStep.SetOutput(member.Value<String>("developerMessage"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
					Logger.Info("test  failed");
					throw new Exception(member.Value<String>("developerMessage"));
				}
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Adding transaction through rest service by providing the VCKey for All Fields";
				JObject response = (JObject)rest_Service_Method.PostTransactionWithVCKeyWithAllFields(member["data"]["cards"][0].Value<String>("id"));
				if (response.Value<String>("isError") == "False")
				{
					testStep.SetOutput("transactios Rowkey:" + response["data"]["transactions"].Value<String>("id"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep); testStep = TestStepHelper.StartTestStep(testStep);
				}
				else
				{
					testStep.SetOutput(response.Value<String>("developerMessage"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
					Logger.Info("test  failed");
					throw new Exception(response.Value<String>("developerMessage"));
				}

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Fetching Account Details through rest service using Transaction Id";
				String id = response["data"]["transactions"].Value<String>("transactionId");
				JObject output = (JObject)rest_Service_Method.GetAccountDetailsByFMS(id);
                //JObject output = (JObject)rest_Service_Method.GetAccountDetailsByFMS("431321139150330186");
                //testStep.SetOutput("saleAmount = " + output["data"][0]["additionalAttributes"][0].Value<string>("attributeValue"));
                testStep.SetOutput("Quantity = " + output["data"][0].Value<double>("quantity"));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                String dbresponse = DatabaseUtility.GetQuantityfromDBUsingVCKeyREST(member["data"]["cards"][0].Value<String>("id"));
                testStep.SetOutput("Response from database" + dbresponse);
                Assert.AreEqual(output["data"][0].Value<double>("quantity")+"", dbresponse, "Expected value is " + output["data"][0].Value<double>("saleAmount") + " and actual value is " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
				Logger.Info("test passed");
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

