using System;
using System.Collections.Generic;
using System.Linq;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1238_SOAP_GetBonusDefinition : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Positive")]
        [TestMethod]
        public void BTA1238_ST1407_SOAP_GetBonusDefinition_ByPassingBonusDefId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;
            try
            {

                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing BonusDefId";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                BonusDefinitionStruct def1 = cdis_Service_Method.GetBonusDefinition(def[0].Id.ToString(),null, null, null, string.Empty, time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name);
                Logger.Info("First Bonus Name : " + def[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetBonusNameFromDBCDIS(def[0].Id + "");
                Assert.AreEqual(def[0].Name + "", dbresponse, "Expected Value is" + def[0].Name + "Output Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def[0].Name + " and OutPut DB value is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Positive")]
        [TestMethod]
        public void BTA1238_ST1408_SOAP_GetBonusDefinition_ByPassingBonusDefIdAndChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing BonusDefId and Channel";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                BonusDefinitionStruct def1 = cdis_Service_Method.GetBonusDefinition(def[0].Id.ToString(), "Web", null, null, string.Empty, time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name);
                Logger.Info("First Bonus Name : " + def[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetBonusNameFromDBCDIS(def[0].Id + "");
                Assert.AreEqual(def[0].Name + "", dbresponse, "Expected Value is" + def[0].Name + "Output Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def[0].Name + " and Output DB value is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Positive")]
        [TestMethod]
        public void BTA1238_ST1409_SOAP_GetBonusDefinition_ByPassingBonusDefIdChannelAndLanguage()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing BonusDefId,Channel and Language";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                BonusDefinitionStruct def1 = cdis_Service_Method.GetBonusDefinition(def[0].Id.ToString(), "Web", "en", null, string.Empty, time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name);
                Logger.Info("First Bonus Name : " + def[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetBonusNameFromDBCDIS(def[0].Id + "");
                Assert.AreEqual(def[0].Name + "", dbresponse, "Expected Value is" + def[0].Name + "Output Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def[0].Name + " and Output DB value is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Positive")]
        [TestMethod]
        public void BTA1238_ST1410_SOAP_GetBonusDefinition_ByPassingReturnAttributeAsFalse()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing ReturnAttribute as False";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                BonusDefinitionStruct def1 = cdis_Service_Method.GetBonusDefinition(def[0].Id.ToString(), null, null, false, string.Empty, time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name);
                Logger.Info("First Bonus Name : " + def[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetBonusNameFromDBCDIS(def[0].Id + "");
                Assert.AreEqual(def[0].Name + "", dbresponse, "Expected Value is" + def[0].Name + "Output Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def[0].Name + " and Output DB value is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Positive")]
        [TestMethod]
        public void BTA1238_ST1411_SOAP_GetBonusDefinition_ByPassingReturnAttributeAsTrue()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            double time = 0;

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing ReturnAttribute as True";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                BonusDefinitionStruct def1 = cdis_Service_Method.GetBonusDefinition(def[0].Id.ToString(), null, null, true, string.Empty, time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name);
                Logger.Info("First Bonus Name : " + def[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetBonusNameFromDBCDIS(def[0].Id + "");
                Assert.AreEqual(def[0].Name + "", dbresponse, "Expected Value is" + def[0].Name + "Output Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def[0].Name + " and Output DB value is: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testCase.SetStatus(true);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinition")]
        [TestCategory("API_SOAP_GetBonusDefinition_Negative")]
        [TestMethod]
        public void BTA1238_ST1412_SOAP_GetBonusDefinition_ByPassingNullValuesForAllFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
           
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions from Service by passing Null Values for all Fields";
                string error = (string)cdis_Service_Method.GetBonusDefinitionsNegative();
                testStep.SetOutput("Throws an expection with the " + error);
                string[] errors = error.Split(';');
                string[] errorssplit = errors[0].Split('=');
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 3369";
                Assert.AreEqual("3369", errorssplit[1], "Expected value is" + "3369" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errors[1]);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

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



