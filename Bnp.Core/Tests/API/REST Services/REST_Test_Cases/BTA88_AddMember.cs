using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Bnp.Core.Tests.API.REST_Services.REST_Models.Member_Model;

namespace Bnp.Core.Tests.API.REST_Services.REST_Test_Cases
{
    [TestClass]
    public class BTA88_REST_AddMember: ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;


        [TestMethod]
        public void BTA88_REST_AddMemberGeneral()
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
                Memberm mem = common.GenerateAddMemberForREST();
                JObject member = (JObject)rest_Service_Method.PostMember(mem);
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);


                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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
        public void BTA88_REST_AddMemberMandatory()
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
                Member mem = common.GenerateAddMemberForRESTMandatory();
                JObject member = (JObject)rest_Service_Method.PostMember(mem);
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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
        public void BTA88_REST_AddMemberExistingtVirtualCard()
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
                Memberm mem = common.GenerateAddMemberForRESTExistingVC();
                JObject member = (JObject)rest_Service_Method.PostMember(mem);
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());

                Logger.Info("test  passed");
                if (member.Value<string>("responseCode") == "10512")
                {
                    testStep.SetOutput("Response Code=" + member.Value<string>("responseCode") + ", Message =" + member.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + member.Value<string>("responseCode") + ", Message =" + member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
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

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1710_REST_PostMember_ByProvidingMemberObjectsVCObjectsAttributeSets()
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
                stepName = "Adding member through rest service by providing Member Objects, VC Objects and Attribute Sets";
                Member memberin = common.AddMemberWithAllFields();
                memberin.FirstName= memberin.FirstName.Replace("SOAP","REST");
                memberin.LastName = memberin.LastName.Replace("SOAP", "REST");
                memberin.MiddleName = memberin.MiddleName.Replace("SOAP", "REST");
                memberin.PrimaryEmailAddress = memberin.PrimaryEmailAddress.Replace("SOAP", "REST");
                memberin.Username = memberin.Username.Replace("SOAP", "REST");
                JObject member = (JObject)rest_Service_Method.PostMember(memberin);
               // JObject member = (JObject)rest_Service_Method.PostMembergeneralWithAllFields(memberin);
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1716_REST_PostMember_WithoutProvidingVCKey()
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
                stepName = "Adding member through rest service without providing VC Key";
                Member memberin = common.GenerateAddMemberForSOAPNoVC();
                memberin.FirstName = memberin.FirstName.Replace("SOAP", "REST");
                memberin.LastName = memberin.LastName.Replace("SOAP", "REST");
                memberin.Username = memberin.Username.Replace("SOAP", "REST");
                JObject member = (JObject)rest_Service_Method.PostMember(memberin);
               // JObject member = (JObject)rest_Service_Method.PostMembergeneralWithNoVCKey(memberin);
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName") + ";CardNumber=" + member["data"]["cards"][0].Value<string>("cardNumber"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating virtual card with data from database";
                    string dbresponse = DatabaseUtility.GetFromSoapDB("LW_VIRTUALCARD", "IPCODE", member["data"].Value<string>("id"), "LoyaltyIDNumber", string.Empty);
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"]["cards"][0].Value<string>("cardNumber"), dbresponse, "Expected value is " + member["data"]["cards"][0].Value<string>("cardNumber") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1723_REST_PostMember_ByProvidingStoreLocations()
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
                stepName = "Adding member through rest service by providing Store Locations";
                JObject member = (JObject)rest_Service_Method.PostMemberByProvidingStoreLocations();
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";Name=" + member["data"].Value<string>("firstName"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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
        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1724_REST_PostMember_WithMultipleCards()
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
                stepName = "Adding member through rest service with multiple cards";
                Memberm mem = common.GenerateAddMemberMultipleCardsForREST();
                JObject member = (JObject)rest_Service_Method.PostMember(mem);
                //JObject member = (JObject)rest_Service_Method.PostMembergeneralWithAllFields(common.GenerateAddMemberMultipleCardsForREST());
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";CardNumber1=" + member["data"]["cards"][0].Value<string>("cardNumber") + ";CardNumber1=" + member["data"]["cards"][1].Value<string>("isPrimary") + ";CardNumber2=" + member["data"]["cards"][0].Value<string>("cardNumber") + ";CardNumber2=" + member["data"]["cards"][1].Value<string>("isPrimary"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1725_REST_PostMember_WithAttributeSets()
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
                stepName = "Adding member through rest service with multiple attribute sets";
                Memberm mem = common.GenerateAddMemberMultipleAttributeSetForREST();
                JObject member = (JObject)rest_Service_Method.PostMember(mem);
               // JObject member = (JObject)rest_Service_Method.PostMembergeneralWithAllFields(common.GenerateAddMemberMultipleAttributeSetForREST());
                var k = member.Value<string>("isError");
                Logger.Info(k.GetType());
                Logger.Info(member.Value<string>("isError"));
                if (member.Value<string>("isError") == "False")
                {
                    testStep.SetOutput("IPCODE=" + member["data"].Value<int>("id") + ";CardNumber=" + member["data"]["cards"][0].Value<string>("cardNumber"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);

                    testStep = TestStepHelper.StartTestStep(testStep);
                    stepName = "Validating data with data from database";
                    string dbresponse = DatabaseUtility.GetMemberFNAMEfromDBUsingIdREST(member["data"].Value<int>("id") + "");
                    testStep.SetOutput("Response from database" + dbresponse);
                    Assert.AreEqual(member["data"].Value<string>("firstName"), dbresponse, "Expected value is " + member["data"].Value<string>("firstName") + " and actual value is " + dbresponse);
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                    Logger.Info("test passed");
                }
                else
                {
                    //Logger.Info("test  failed");
                    //testStep.SetOutput(member.Value<string>("developerMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
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

        [TestCategory("Regression")]
        [TestCategory("API_REST_Regression")]
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1733_REST_PostMember_PassInvalidTokenInAuthorization()
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
                stepName = "Add member by passing invalid auth token";
                response = (JObject)rest_Service_Method.PostMemberWithInvalidAuthToken();
                if (response.Value<string>("error") == "invalid_token")
                {
                    testStep.SetOutput("Message =" + response.Value<string>("error_description"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1734_REST_PostMember_PassAlreadyExistingValues()
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
                Member member1 = common.AddMemberWithAllFields();
                IList<VirtualCard> vc = member1.GetLoyaltyCards(); 
                member1.PrimaryEmailAddress = member1.PrimaryEmailAddress.Replace("SOAP", "REST");
                member1.Username = member1.Username.Replace("SOAP", "REST");
                member1.AlternateId = member1.AlternateId.Replace("SOAP", "REST");
                JObject member = (JObject)rest_Service_Method.PostMembergeneralWithAllFields(member1);

                stepName = "Adding member through rest service for existing values";
                Member member2 = common.GenerateAddMemberForSOAPNoVC();
                member2.PrimaryEmailAddress = member1.PrimaryEmailAddress;
                member2.Username = member1.Username;
                member2.AlternateId = member1.AlternateId;
                member2.Add(vc[0]);

                response = (JObject)rest_Service_Method.PostMember(member2);
               // response = (JObject)rest_Service_Method.PostMembergeneralWithAllFields(member2);
                
                if (response.Value<Int32>("responseCode") == 10512)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));

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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1735_REST_PostMember_PassAcceptLanguageEqualsToEnglish()
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
                stepName = "Add member by passing accept language equals to English";
                response = (JObject)rest_Service_Method.PostMemberWithInvalidAcceptLanguage();
                if (response.Value<Int32>("responseCode") == 10502)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1736_REST_PostMember_ProvideInvalidIsEmployee()
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
                stepName = "Add member by passing invalid IsEmployee";
                InValidMember mem = common.GenerateAddMemberForRESTWithInvalidEmployee();
                response = (JObject)rest_Service_Method.PostMember(mem);
               // response = (JObject)rest_Service_Method.PostMemberWithInvalidIsEmployee();
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1737_REST_PostMember_ProvidInvalidZipCode()
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
                stepName = "Add member by passing invalid Zip Code";
                Memberm mem = common.GenerateAddMemberForRESTWithInvalidZipCode();
                response = (JObject)rest_Service_Method.PostMember(mem);
               // response = (JObject)rest_Service_Method.PostMemberWithInvalidZipCode();
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1738_REST_PostMember_ProvidInvalidBirthdate()
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
                stepName = "Add member by passing invalid Birthdate";
                Memberm mem = common.GenerateAddMemberForRESTWithInvalidBirthDate();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1739_REST_PostMember_ValuesExceedingMaximumCharacters()
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
                stepName = "Add member by passing values exceeding maximum characters";
                Memberm mem = common.GenerateAddMemberForRESTWithValuesExceedingMaximumCharacters();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1761_REST_PostMember_ExpirationDateLessThanSysDateInCardsObject()
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
                stepName = "Add member by providing expiration date < sys date in Cards object";
                Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateLessThanSysDate();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 12116)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1762_REST_PostMember_ProvideInvalidRequest()
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
                stepName = "Add member by providing invalid request";
                response = (JObject)rest_Service_Method.PostMemberWithInvalidRequest();
                if (response.Value<Int32>("responseCode") == 10400)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1763_REST_PostMember_ExpirationDateGreaterThanSysDateInCardsObject()
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
                stepName = "Add member by providing expiration date > sys date in Cards object";
                Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateGreaterThanSysDate();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10000)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Positive")]
        [TestMethod]
        public void BTA1570_ST1764_REST_PostMember_ExpirationDateEqualToSysDateInCardsObject()
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
                stepName = "Add member by providing expiration date equal to system date in Cards object";
                Memberm mem = common.GenerateAddMemberForRESTWhereExpirationDateEqualsToSysDate();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10000)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("userMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1765_REST_PostMember_InvalidPhoneNumber()
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
                stepName = "Add member by providing Invalid Phone Number";
                Memberm mem = common.GenerateAddMemberForRESTWithInvalidPhoneNumber();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1766_REST_PostMember_ProvideAlternateIDInQuotes()
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
                stepName = "Add member by providing Alternate ID in quotes";
                Memberm mem = common.GenerateAddMemberForRESTWhereAlternateIdInQuotes();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1767_REST_PostMember_WithoutProvidingPostBody()
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
                stepName = "Add member by providing empty body";
                response = (JObject)rest_Service_Method.PostMemberWithInvalidRequest();
                if (response.Value<Int32>("responseCode") == 10400)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1783_REST_PostMember_ProvideSameUserNameAndPassword()
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
                stepName = "Add member by providing same user name and password";
                Memberm mem = common.GenerateAddMemberForRESTWithSameUsernameAndPassword();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1784_REST_PostMember_ProvidePasswordWithCondition()
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
                stepName = "Add member by providing password with conditions";
                Memberm mem = common.GenerateAddMemberForRESTWithSameUsernameAndPassword();
                response = (JObject)rest_Service_Method.PostMember(mem);
                //response = (JObject)rest_Service_Method.PostMemberWithSameUsernameAndPassword();
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response.Value<string>("developerMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);

                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1785_REST_PostMember_ProvidePasswordAsNumber()
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
                stepName = "Add member by providing password as numbers";
                Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordAsNumber();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1785_REST_PostMember_ProvidePasswordLessThan7Characters()
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
                stepName = "Add member by providing password less than 7 characters";
                Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordLessThan7Chars();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1787_REST_PostMember_ProvidePasswordOnly()
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
                stepName = "Add member by providing password only";
                Memberm mem = common.GenerateAddMemberForRESTByProvidingPasswordOnly();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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
        [TestCategory("API_REST_PostMember")]
        [TestCategory("API_REST_PostMember_Negative")]
        [TestMethod]
        public void BTA1570_ST1788_REST_PostMember_ProvideUsernameOnly()
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
                stepName = "Add member by providing username only";
                Memberm mem = common.GenerateAddMemberForRESTByProvidingUsernameOnly();
                response = (JObject)rest_Service_Method.PostMember(mem);
                if (response.Value<Int32>("responseCode") == 10501)
                {
                    testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                    listOfTestSteps.Add(testStep);
                    testCase.SetStatus(true);
                }
                else
                {
                    //testStep.SetOutput("Response Code=" + response.Value<Int32>("responseCode") + ", Message =" + response["errors"][0].Value<string>("errorMessage"));
                    //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                    //listOfTestSteps.Add(testStep);
                    //testCase.SetStatus(false);
                    throw new Exception(response.Value<string>("developerMessage"));
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