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
	public class BTA146_REST_GetMemberAccountSummary : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;
		Common common;
		REST_Service_Methods rest_Service_Method;

		[TestMethod]
		public void BTA146_REST_GetAccountSummaryByID_Positive()
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
					testStep.SetOutput("Member is added through PostMember service and following are the details, IPCODE:= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<String>("firstName"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);

					testStep = TestStepHelper.StartTestStep(testStep);
					stepName = "Fetching Account Summary through rest service using IpCode";
					JObject response = (JObject)rest_Service_Method.GetAccountSummaryByIpCode(member["data"].Value<int>("id") + "");
					testStep.SetOutput("members current status and currency balance from reponse are \""+ response["data"].Value<string>("memberStatus")+"\" and \""+response["data"].Value<int>("currencyBalance")+"\"");
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
					testCase.SetStatus(true);
					Logger.Info("test passed");
				}
				else
				{
					testStep.SetOutput(member.Value<String>("developerMessage"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
					Logger.Info("test  failed");
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

