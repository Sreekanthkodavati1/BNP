using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.REST_Services.REST_Test_Cases
{
    [TestClass]
    public class BTA87_REST_AuthToken : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA87_REST_Auth_Token()
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
                stepName = "Getting Authentication Token";
                String authToken = rest_Service_Method.GetAuthToken();
                testStep.SetOutput(authToken);
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
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Positive")]
        [TestMethod]
        public void BTA1569_ST1607_REST_AuthToken_EN_AcceptLanguage()
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
                stepName = "Creating auth token through rest service where Accept Language is English";
                JObject token = (JObject)rest_Service_Method.PostAuthToken();
                var k = token.Values<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(token.Value<string>("isError"));

                if (token.Value<string>("isError") == "False")
                {
                    
                    testStep.SetOutput("Access Token =" + token["data"].Value<string>("accessToken") + " User Message=" + token.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);                    
                }
                else
                {
                    Logger.Info("test  failed");
                    //testStep.SetOutput(token.Value<string>("userMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    throw new Exception("Reponse code: " + token.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + token.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Positive")]
        [TestMethod]
        public void BTA1569_ST1608_REST_AuthToken_JA_AcceptLanguage()
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
                stepName = "Creating auth token through rest service where Accept Language is Japanese";
                JObject token = (JObject)rest_Service_Method.PostAuthTokenLanguage("ja");
                var k = token.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(token.Value<string>("isError"));

                if (token.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("Access Token =" + token["data"].Value<string>("accessToken") + " User Message=" + token.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    Logger.Info("test  failed");
                    //testStep.SetOutput(token.Value<string>("userMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    throw new Exception("Reponse code: " + token.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + token.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1654_REST_AuthToken_EN_InvalidClientIdAndValidSecretId()
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
                stepName = "Fetching Auth Token for invalid clientId and Valid ClinetSecret";
                response = (JObject)rest_Service_Method.PostAuthTokenReg(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 0, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 0, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: "+ response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1655_REST_AuthToken_EN_ValidClientIdAndInValidSecretId()
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
                stepName = "Fetching Auth Token for Valid clientId and Invalid ClinetSecret";
                response = (JObject)rest_Service_Method.PostAuthTokenReg(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 1, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 1, "clientSecret"));

                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1656_REST_AuthToken_EN_InValidClientIdAndInValidSecretId()
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
                stepName = "Fetching Auth Token for Invalid clientId and Invalid ClinetSecret";
                response = (JObject)rest_Service_Method.PostAuthTokenReg(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 2, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 2, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1657_REST_AuthToken_EN_WithoutProvidingClientIdIAndSecretId()
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
                stepName = "Fetching Auth Token without providing clientId and ClinetSecret";
                response = (JObject)rest_Service_Method.PostAuthTokenRegWithoutBodyEN(null, null);
                if (response.Value<Int32>("responseCode") == 10400)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1658_REST_AuthToken_EN_WithoutProvidingClientIdIAndSecretIdExceptBraces()
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
                stepName = "Fetching Auth Token without providing clientId and ClinetSecret except braces";
                response = (JObject)rest_Service_Method.PostAuthTokenReg(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 3, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 3, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1659_REST_AuthToken_JA_InvalidClientIdAndValidSecretId()
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
                stepName = "Fetching Auth Token invalid clientId and valid ClientSecret where accept language is Japanese";
                response = (JObject)rest_Service_Method.PostAuthTokenRegJa(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 4, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 4, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1660_REST_AuthToken_JA_ValidClientIdAndInValidSecretId()
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
                stepName = "Fetching Auth Token for Valid clientId and Invalid ClientSecret where accept language is Japanese";
                response = (JObject)rest_Service_Method.PostAuthTokenRegJa(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 1, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 1, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }


        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1661_REST_AuthToken_JA_InValidClientIdAndInValidSecretId()
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
                stepName = "Fetching Auth Token for Invalid clientId and Invalid ClientSecret where accept language is Japanese";
                response = (JObject)rest_Service_Method.PostAuthTokenRegJa(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 2, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 2, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 11110)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1662_REST_AuthToken_JA_WithoutProvidingClientIdAndSecretId()
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
                stepName = "Fetching Auth Token without providing clientId and ClinetSecret where accept language is Japanese";
                response = (JObject)rest_Service_Method.PostAuthTokenRegWithoutBodyJa(null,null);
                if (response.Value<Int32>("responseCode") == 10400)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1663_REST_AuthToken_JA_WithoutProvidingClentIdAndSecretIdExceptBraces()
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
                stepName = "Fetching Auth Token without providing clientId and ClinetSecret except braces where accept language is Japanese";
                response = (JObject)rest_Service_Method.PostAuthTokenRegJa(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 3, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 3, "clientSecret"));
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }

        }

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_Authtoken")]
        [TestCategory("API_REST_Authtoken_Negative")]
        [TestMethod]
        public void BTA1569_ST1664_REST_AuthToken_ValidClientIdAndValidSecretId_InvalidAcceptLanguage()
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
                stepName = "Fetching Auth Token by providing valid clientId,valid clinetSecret and invalid accept language";
                response = (JObject)rest_Service_Method.PostAuthTokenWithInvalidLanguage(DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 4, "clientId"), DatabaseUtility.GetExpectedColumValue(ProjectTestBase.TEST_DATA_FOR_AUTH_TOKEN, 4, "clientSecret"), "ENGLISH");
                if (response.Value<Int32>("responseCode") == 10502)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<String>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<String>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception("Reponse code: " + response.Value<string>("responseCode") + " ;Error Response Code is NOT matching; Developer Error Message from response is: " + response.Value<string>("developerMessage"));
                }
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message.ToString());
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

    

