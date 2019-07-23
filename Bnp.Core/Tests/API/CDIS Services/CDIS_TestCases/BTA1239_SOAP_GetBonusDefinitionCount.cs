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
using Newtonsoft.Json.Linq;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1239_SOAP_GetBonusDefinitionCount : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1442_SOAP_GetBonusDefinitionCount_WhenActiveOnlyIsFalse()
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
                stepName = "Getting Bonus Definitions Count from Service when Active Only is false";
                int output = cdis_Service_Method.GetBonusDefinitionCount(false, null, string.Empty, out time);
                testStep.SetOutput("Bonus Count : " + output + " and Elapsed Time is : " + time);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the bonus count with count in the database";
                string dbresponse = DatabaseUtility.GetBonusCountfromDbSOAP();
                testStep.SetOutput("Bonus Count from database:" + dbresponse);

                Assert.AreEqual(output + "", dbresponse, "Expected value is" + output + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1443_SOAP_GetBonusDefinitionCount_WhenActiveOnlyIsNull()
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
                stepName = "Getting Bonus Definitions Count from Service when Active Only is null";
                int output = cdis_Service_Method.GetBonusDefinitionCount(null, null, string.Empty, out time);
                testStep.SetOutput("Bonus Count : " + output);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the bonus count with count in the database";
                string dbresponse = DatabaseUtility.GetBonusCountfromDbSOAP();
                testStep.SetOutput("Bonus Count from database:" + dbresponse);

                Assert.AreEqual(output + "", dbresponse, "Expected value is" + output + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1444_SOAP_GetBonusDefinitionCount_WhenActiveOnlyIsTrue()
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
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Count_Future_Coupons"];

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions Count from Service when Active Only is true";
                int output = cdis_Service_Method.GetBonusDefinitionCount(true, null, string.Empty, out time);
                testStep.SetOutput("Bonus Count : " + output);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the bonus count with count in the database";
                string dbresponse = DatabaseUtility.GetBonusCountfromDbSOAP();
   //             string futureCouponCount = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "COUNT(*)", sqlstmt);
  //              int activeCouponCount = Convert.ToInt32(dbresponse) - Convert.ToInt32(futureCouponCount);
                testStep.SetOutput("Active Bonus Count from database:" + dbresponse);
                Assert.AreEqual(output.ToString(), dbresponse, "Expected value is" + output + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1445_SOAP_GetBonusDefinitionCount_WhenAllParamsAreNull()
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
                stepName = "Getting Bonus Definitions Count from Service when all parameters are null";
                int output = cdis_Service_Method.GetBonusDefinitionCount(null, null, null, out time);
                testStep.SetOutput("Bonus Count : " + output);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetBonusCountfromDbSOAP();
                testStep.SetOutput("Bonus Count from database:" + dbresponse);
                Assert.AreEqual(output + "", dbresponse, "Expected value is" + output + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1446_SOAP_GetBonusDefinitionCount_WhenAllParamsAreValid()
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
                IList<JToken> JsonData = ProjectBasePage.GetJSONData("CommonSqlStatements");
                string sqlstmt = (string)JsonData.FirstOrDefault()["Get_Count_Future_Coupons"];

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Bonus Definitions Count from Service when all parameters are valid";
                int output = cdis_Service_Method.GetBonusDefinitionCount(true, null, null, out time);
                testStep.SetOutput("Bonus Count : " + output);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetBonusCountfromDbSOAP();
 //               string futureCouponCount = DatabaseUtility.GetFromSoapDB(string.Empty, string.Empty, string.Empty, "COUNT(*)", sqlstmt);
  //              int activeCouponCount = Convert.ToInt32(dbresponse) - Convert.ToInt32(futureCouponCount);
                testStep.SetOutput("Active Bonus Count from database:" + dbresponse);
                Assert.AreEqual(output.ToString(), dbresponse, "Expected value is" + output + "Actual value is" + dbresponse);
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
        [TestCategory("API_SOAP_GetBonusDefinitionCount")]
        [TestCategory("API_SOAP_GetBonusDefinitionCount_Positive")]
        [TestMethod]
        public void BTA1239_ST1447_SOAP_GetBonusDefinitionCount_WhenAttributeNameIsNonExisting()
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
                stepName = "Getting Bonus Definitions Count from Service when attribute name is Non Existing";
                ContentSearchAttributesStruct[] contentSearchAttributesStructs = new ContentSearchAttributesStruct[1];
                contentSearchAttributesStructs[0] = new ContentSearchAttributesStruct();
                contentSearchAttributesStructs[0].AttributeName = "NonExisting";
                contentSearchAttributesStructs[0].AttributeValue = common.RandomString(10);

                int output = cdis_Service_Method.GetBonusDefinitionCount(true, contentSearchAttributesStructs, null, out time);
                testStep.SetOutput("Bonus Count : " + output);
                Logger.Info("Bonus Count : " + output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response ";
                Assert.AreEqual(output + "", "0", "Expected value is" + 0 + "Actual value is" + output);
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