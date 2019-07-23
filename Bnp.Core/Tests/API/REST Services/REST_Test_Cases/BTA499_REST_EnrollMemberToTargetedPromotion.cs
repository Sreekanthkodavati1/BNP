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
    public class BTA499_REST_EnrollMemberToTargetedPromotion : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA499_REST_EnrollMemberToTargetedPromotion_POSITIVE()
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
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("Member is added through PostMember service and following are the details, IPCODE:= " + member["data"].Value<int>("id") + "; Name= " + member["data"].Value<string>("firstName")); ;
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                }
                else
                {
                    testStep.SetOutput("PostMember service call failed with following developer message: \"" + member.Value<string>("developerMessage") + "\"");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test  failed");
                    throw new Exception(member.Value<string>("developerMessage"));
                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding targeted promotion through rest service by providing the member.firstName and options.promotionCode as parameter";
                response = (JObject)rest_Service_Method.PostMemberToTargetedPromotion(member["data"].Value<string>("firstName"), DatabaseUtility.GETRecentPromotionfromDBREST());
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("The following promotion with ; memberpromotionID: " + response["data"].Value<int>("id") +
                                                                     "; Promotion ID : " + response.SelectToken("data.promotion").Value<string>("id") +
                                                                    "; Promotion name: " + response.SelectToken("data.promotion").Value<string>("name") +
                                                                    "; has been added to the member : " + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                }
                else
                {
                    Logger.Info("test failed");
                    testStep.SetOutput(response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    throw new Exception(response.Value<string>("developerMessage"));
                }

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Enrolling targeted promotion through rest service by providing the member.firstName and options.promotionCode as parameter";
                response = (JObject)rest_Service_Method.EnrollMemberToTargetedPromotion(member["data"].Value<string>("firstName"), DatabaseUtility.GETRecentPromotionfromDBREST());
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("The following promotion with ; memberpromotionID: " + response["data"].Value<int>("id") +
                                                                     "; Promotion ID : " + response.SelectToken("data.promotion").Value<string>("id") +
                                                                    "; Promotion name: " + response.SelectToken("data.promotion").Value<string>("name") +
                                                                    "; has been enrolled to the member : " + member["data"].Value<string>("firstName")+
                                                                    "; and enroll status is: "+ response["data"].Value<bool>("enrolled"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate enroll and memberpromotionID status from DB";
                    string dbresponse = DatabaseUtility.GetMemberPromotionsUsingIpcodefromDBREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("The memberpromotionID from DB is:  " + dbresponse
                        + " and the enroll status of the response is: " + response["data"].Value<bool>("enrolled"));
                    Assert.AreEqual(true, response["data"].Value<bool>("enrolled"), "Ëxpected value is true and the actual value is" + response["data"].Value<bool>("enrolled"));
                    Assert.AreEqual(response["data"].Value<int>("id") + "", dbresponse, "Expected value is " + response["data"].Value<int>("id") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
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

        [TestMethod]
        public void BTA499_REST_EnrollMemberToTargetedPromotion_NEGATIVE()
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
                stepName = "Adding invalid promotion by providing the invalid firstName and promotionCode details";
                response = (JObject)rest_Service_Method.EnrollMemberToTargetedPromotion(common.RandomString(6), common.RandomString(4));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code if an invalid details (firstname, promotioncode) are passed is: " + response.Value<int>("responseCode") + "");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating developer message for the response code 10504";
                    if (response.Value<int>("responseCode") == 10504)
                    {
                        testStep.SetOutput("Developer message from response code 10504 is \"" + response.Value<string>("developerMessage") + "\"");
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep);

                        testCase.SetStatus(true);
                    }
                    else
                    {
                        Logger.Info("test failed");
                        testStep.SetOutput("Developer message is" + response.Value<string>("developerMessage") + "and response code is" + response.Value<int>("responseCode"));
                        testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                        listOfTestSteps.Add(testStep);
                        throw new Exception("Developer message is" + response.Value<string>("developerMessage") + "and response code is" + response.Value<int>("responseCode"));
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