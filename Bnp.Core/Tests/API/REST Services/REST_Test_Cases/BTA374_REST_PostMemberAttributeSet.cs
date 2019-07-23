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
	public class BTA374_REST_PostMemberAttributeSet : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;

		[TestMethod]
		public void BTA374_REST_PostMemberAttributeSet_POSITIVE()
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
				stepName = "Adding attribute set through rest service by providing the member.firstName as parameter";
				response = (JObject)rest_Service_Method.PostMemberAttributeSets(member["data"].Value<String>("firstName"));
				if (response.Value<String>("isError") == "False")
				{
					testStep.SetOutput("Getting the ID for the \"memberDetails\" attribute set from the response:" + response["data"]["memberDetails"].Value<int>("id"));
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep); testStep = TestStepHelper.StartTestStep(testStep);

					stepName = "Validating data with data from database";
					String dbresponse = DatabaseUtility.GetMemberDetailsIdusingIpcodefromDBREST(member["data"].Value<int>("id") + "");
                    Assert.AreEqual(response["data"]["memberDetails"].Value<int>("id") + "", dbresponse, "Expected value is " + response["data"]["memberDetails"].Value<int>("id") + " and actual value is " + dbresponse);
                    testStep.SetOutput("The memberDetails ID for the attributeset added is \""+response["data"]["memberDetails"].Value<int>("id")+"\" and the response from database for A_ROWKEY column in ats_memberdetails table is:" + dbresponse);			
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);
					testCase.SetStatus(true);
				}
				else
				{
					Logger.Info("test failed");
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
		public void BTA374_REST_PostMemberAttributeSet_NEGATIVE()
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
				stepName = "Adding attribute set by providing invalid member.firstName as parameter";
				response = (JObject)rest_Service_Method.PostMemberAttributeSets(common.RandomString(6));
				
				if (response.Value<String>("isError") == "True")
				{
					testStep.SetOutput("when a invalid member name is passed for attributesets call, the response code from the reponse is : "+response.Value<int>("responseCode")+"");
					testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
					listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating developer message from the Service when response code is 10504";
					if (response.Value<int>("responseCode") == 10504)
					{
						testStep.SetOutput("developer message from response is \""+response.Value<string>("developerMessage")+ "\" when response code is 10504");	
						testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
						listOfTestSteps.Add(testStep);
						testCase.SetStatus(true);
					}
					else
					{
						Logger.Info("test failed");
						testStep.SetOutput("response message is"+response.Value<String>("developerMessage")+"and response code is"+ response.Value<int>("responseCode"));
						testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
						listOfTestSteps.Add(testStep);
						throw new Exception("response message is" + response.Value<String>("developerMessage") + "and response code is" + response.Value<int>("responseCode"));
					}
				}
				else
				{
					Logger.Info("test failed");
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
	}
}
