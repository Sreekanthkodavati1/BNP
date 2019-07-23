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
    public class BTA1240_SOAP_GetBonusDefinitions : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1427_SOAP_GetBonusDefinitions_InactiveOnly()
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
                stepName = "Getting Bonus Definitions from Service which are Inactive Only";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, false, null, null, 1, 10, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Bonus Id : " + def[0].Id);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Id : " + def[0].Id);
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
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1428_SOAP_GetBonusDefinitions_ActiveOnly()
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
                stepName = "Getting Bonus Definitions from Service which are Active Only";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, null, 1, 10, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Bonus Id : " + def[0].Id);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Id : " + def[0].Id);
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
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1429_SOAP_GetBonusDefinitions_WithAnExistingChannel()
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
                stepName = "Getting Bonus Definitions from Service with an existing channel";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, "Web", true, null, null, 1, 10, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Bonus Id : " + def[0].Id);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Id : " + def[0].Id);
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
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1430_SOAP_GetBonusDefinitions_WithAnExistingLanguage()
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
                stepName = "Getting Bonus Definitions from Service with an existing language";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions("en", null, true, null, null, 1, 10, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Bonus Id : " + def[0].Id);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Id : " + def[0].Id);
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
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1431_SOAP_GetBonusDefinitions_ReturnAttributeIsFalse()
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
                stepName = "Getting Bonus Definitions from Service where Return Attribute is False";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, false, 1, 10, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Bonus Id : " + def[0].Id);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Id : " + def[0].Id);
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
        [TestCategory("API_SOAP_GetBonusDefinitions")]
        [TestCategory("API_SOAP_GetBonusDefinitions_Positive")]
        [TestMethod]
        public void BTA1240_ST1432_SOAP_GetBonusDefinitions_ReturnAttributeIsTrue()
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
                stepName = "Getting Bonus Definitions from Service where Return Attribute is True";
                BonusDefinitionStruct[] def = cdis_Service_Method.GetBonusDefinitions(null, null, true, null, true, 1, 5, string.Empty, out time);
                testStep.SetOutput("First Bonus Name : " + def[0].Name + " First Attribute Name : " + def[0].ContentAttributes[0].AttributeName);
                Logger.Info("First Bonus Name : " + def[0].Name + "First Bonus Name : " + def[0].ContentAttributes[0].AttributeName);
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
    }
}