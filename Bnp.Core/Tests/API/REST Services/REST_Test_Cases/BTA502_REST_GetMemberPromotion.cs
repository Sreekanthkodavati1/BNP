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
    public class BTA502_REST_GetMemberPromotion : ProjectTestBase
    {

        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA502_REST_GetMemberPromotion_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            REST_Service_Methods rest_Service_Method = new REST_Service_Methods(common);
            JObject response, response1;

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
                stepName = "Add targeted promotion through rest service by providing the member.firstName and options.promotionCode as parameter";
                response = (JObject)rest_Service_Method.PostMemberToTargetedPromotion(member["data"].Value<string>("firstName"), DatabaseUtility.GETRecentPromotionfromDBREST());
                if (response.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("The following promotion with ; Promotion ID : " + response.SelectToken("data.promotion").Value<string>("id") +
                                                                    "; Promotion name: " + response.SelectToken("data.promotion").Value<string>("name") +
                                                                    "; has been added to the member : " + member["data"].Value<string>("firstName")+
                                                                    "; and the memberpromotionID: " + response["data"].Value<int>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching and verifying the member promotions through GetMemberPromotions restservice by providing First Name as parameter";
                    response1 = (JObject)rest_Service_Method.GetMemberPromotions(member["data"].Value<string>("firstName"));
                    testStep.SetOutput("The following are the details ;memberid  " + response1["data"]["memberPromotions"][0].Value<int>("memberId") + 
                                                                     "; has the promotion with promotionId = " + response1["data"]["memberPromotions"][0]["promotion"].Value<int>("id")+
                                                                     ";and membersPromotionID is: "+ response1["data"]["memberPromotions"][0].Value<string>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validate the memberPromotionID in the db";
                    string dbresponse = DatabaseUtility.GetMemberPromotionsUsingIpcodefromDBREST(response1["data"]["memberPromotions"][0].Value<int>("memberId") + "");
                    Assert.AreEqual(response1["data"]["memberPromotions"][0].Value<int>("id") + "", dbresponse, "Expected value is " + response1["data"]["memberPromotions"][0].Value<int>("id") + " and actual value is " + dbresponse);
                    testStep.SetOutput("The memberPromotionID returned from db is " + dbresponse + " matches with the memberPromotionID from the response: " + response1["data"]["memberPromotions"][0].Value<int>("id"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    Logger.Info("test passed");

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
        public void BTA502_REST_GetMemberPromotionWithoutPromotion_Negative()
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

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Fetching and validating  member promotions through rest service by providing First Name as parameter for member without promotions";
                    response = (JObject)rest_Service_Method.GetMemberPromotions(member["data"].Value<string>("firstName"));
                    if (response.Value<string>("isError") == "False")
                    {

                        testStep.SetOutput("The response code from GetMemberPromotion service for a member with out promotion is: " + response.Value<int>("responseCode")
                           + " and the message is: "+ response.Value<string>("userMessage"));
                        Assert.AreEqual(53101, response.Value<int>("responseCode"), "Expected Value is 53101 and the actual value is" + response.Value<int>("responseCode"));
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
                else
                {
                    Logger.Info("test failed");
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
        public void BTA502_REST_GetMemberPromotionInvalidName_NEGATIVE()
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
                stepName = "Fetching Member's rewardID through service using invalid FirstName";
                response = (JObject)rest_Service_Method.GetMemberPromotions(common.RandomString(12));
                if (response.Value<string>("isError") == "True")
                {
                    testStep.SetOutput("The response code from GetMemberRewards service for a member with " +
                        "invalid Name is: " + response.Value<int>("responseCode") + "");
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