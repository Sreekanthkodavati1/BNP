using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class BTA220_CDIS_GetCouponDefinitions : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;

        [TestMethod]
        public void BTA220_CDIS_GetCouponDefinitions_Positive()
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
                stepName = "Getting Coupon Definitions from Service";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                testStep.SetOutput("First Coupon Name : " + def.CouponDefinition[0].Name);
                Logger.Info("First Coupon Name : " + def.CouponDefinition[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating data with data from database";
                string dbresponse = DatabaseUtility.GetCouponNameFromDBCDIS(def.CouponDefinition[0].Id + "");
                Assert.AreEqual(def.CouponDefinition[0].Name + "", dbresponse, "Expected Value is" + def.CouponDefinition[0].Name + "OutPut Db values is" + dbresponse);
                testStep.SetOutput("Expected Value is: " + def.CouponDefinition[0].Name + " and OutPut DB value is: " + dbresponse);
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

        [TestMethod]
        public void BTA220_CDIS_GetCouponDefinitionsWithAttributes_Positive()
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
                List<string> contentnames = new List<string>();
                List<string> AttributeNameList = new List<string>();
                List<string> AttributeValueList = new List<string>();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitionsWithAttributes();

                for (int i = 0; i < def.CouponDefinition[0].ContentAttributes.Length; i++)
                {
                    AttributeNameList.Add(def.CouponDefinition[0].ContentAttributes[i].AttributeName);
                }
                AttributeNameList = AttributeNameList.Select(s => s ?? "NULL").ToList();

                for (int i = 0; i < def.CouponDefinition[0].ContentAttributes.Length; i++)
                {
                    AttributeValueList.Add(def.CouponDefinition[0].ContentAttributes[i].AttributeValue);
                }
                AttributeValueList = AttributeValueList.Select(s => s ?? "NULL").ToList();

                Logger.Info("First Coupon Attribute Name : " + def.CouponDefinition[0].ContentAttributes[0].AttributeName.ToString());
                testStep.SetOutput("First Coupon Name is: " + def.CouponDefinition[0].Name + " and Content attributes Names are ## : " + string.Join(": ", AttributeNameList) + " and content attribute values are ##:" + string.Join(": ", AttributeValueList));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating Content attribute names with data from database";
                string dbresponse = DatabaseUtility.GetCouponAttributesValues(def.CouponDefinition[0].Id + "");
                Assert.AreEqual(def.CouponDefinition[0].ContentAttributes.Length + "", dbresponse, "Expected Value is" + def.CouponDefinition[0].ContentAttributes.Length + "OutPut Db values is" + dbresponse);
                contentnames = DatabaseUtility.GetCouponAttributeNamesfromDBCDIS();
                for (int i = 0; i < contentnames.Count; i++)
                {
                    Assert.AreEqual(AttributeNameList[i], contentnames[i], "Expected Value is" + AttributeNameList[i] + "OutPut Db values is" + contentnames[i]);

                }

                testStep.SetOutput("Content attribute count from DB response is : " + dbresponse + " and " +
                    "the content arribute names fected from DB for the coupon :" + def.CouponDefinition[0].Name + ": are :" + string.Join(": ", contentnames) + " which matches with expected value");
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1575_SOAP_GetCouponDefinitions_ExistingChannel()
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
                stepName = "Getting Coupon Definitions from Service with an existing channel";
                string channel = DatabaseUtility.GetFromSoapDB("Lw_Channeldef", string.Empty, string.Empty, "NAME", string.Empty);
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", channel, null, null, true, 1, 5, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating  Coupon definitions response data with database";
                var couponDefinationsFromDb = DatabaseUtility.GetCouponsDefFromDB();
                IEnumerable<string> dataComparison = couponDefinations.Except(couponDefinationsFromDb);
                if (!dataComparison.Any())
                {
                    testStep.SetOutput("Coupon definitions data is validated successfully with database");
                }
                else
                {
                    throw new Exception("Coupon definitions from the response are different from database");
                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1576_SOAP_GetCouponDefinitions_ExistingLanguage()
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
                stepName = "Getting Coupon Definitions from Service with an existing language";
                string language = DatabaseUtility.GetFromSoapDB("Lw_Languagedef", string.Empty, string.Empty, "CULTURE", string.Empty);
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions(language, "Web", null, null, true, 1, 5, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating  Coupon definitions response data with database";
                string dbresponse = DatabaseUtility.GetCouponNameFromDBCDIS(def.CouponDefinition[0].Id + "");
                var couponDefinationsFromDb = DatabaseUtility.GetCouponsDefFromDB();
                IEnumerable<string> dataComparison = couponDefinations.Except(couponDefinationsFromDb);
                if (!dataComparison.Any())
                {
                    testStep.SetOutput("Coupon definitions data is validated successfully with database");
                }
                else
                {
                    throw new Exception("Coupon definitions from the response are different from database");

                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1577_SOAP_GetCouponDefinitions_ReturnAttributesFalse()
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
                stepName = "Getting Coupon Definitions from Service with return attributes False";
                bool returnAttributes = false;
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, null, returnAttributes, 1, 5, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate associated attributes are null when returnAttributes=false";
                List<ContentAttributesStruct[]> associteedAttributes = new List<ContentAttributesStruct[]>();
                List<string> associteedAttributesValues = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    associteedAttributes.Add(def.CouponDefinition[i].ContentAttributes);
                }
                foreach (var associteedAttribute in associteedAttributes)
                {
                    if (associteedAttribute != null)
                    {
                        associteedAttributesValues.Add(associteedAttribute[0].AttributeName);
                        associteedAttributesValues.Add(associteedAttribute[1].AttributeName);
                    }
                }
                if (!associteedAttributesValues.Any())
                {
                    testStep.SetOutput("associate attributes are null when returnAttributes=false");
                }
                else
                {
                    throw new Exception("associate attributes are not null when returnAttributes=false");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating  Coupon definitions response data with database";
                string dbresponse = DatabaseUtility.GetCouponNameFromDBCDIS(def.CouponDefinition[0].Id + "");
                var couponDefinationsFromDb = DatabaseUtility.GetCouponsDefFromDB();
                IEnumerable<string> dataComparison = couponDefinations.Except(couponDefinationsFromDb);
                if (!dataComparison.Any())
                {
                    testStep.SetOutput("Coupon definitions data is validated successfully with database");
                }
                else
                {
                    throw new Exception("Coupon definitions from the response are different from database");
                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1578_SOAP_GetCouponDefinitions_ReturnAttributesTrue()
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
                stepName = "Getting Coupon Definitions from Service with return attributes True";
                bool returnAttributes = true;
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, null, returnAttributes, 1, 5, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate associated attributes are returned when returnAttributes=true";
                List<ContentAttributesStruct[]> associteedAttributes = new List<ContentAttributesStruct[]>();
                List<string> associteedAttributesValues = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    associteedAttributes.Add(def.CouponDefinition[i].ContentAttributes);
                }
                foreach (var associteedAttribute in associteedAttributes)
                {
                    associteedAttributesValues.Add(associteedAttribute[0].AttributeName);
                    associteedAttributesValues.Add(associteedAttribute[1].AttributeName);
                }
                if (associteedAttributesValues.Any())
                {
                    testStep.SetOutput("associate attributes are returned and attribute values are: " + string.Join(",", associteedAttributesValues));
                }
                else
                {
                    throw new Exception("associate attributes are not returned when returnAttributes=true");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating  Coupon definitions response data with database";
                string dbresponse = DatabaseUtility.GetCouponNameFromDBCDIS(def.CouponDefinition[0].Id + "");
                var couponDefinationsFromDb = DatabaseUtility.GetCouponsDefFromDB();
                IEnumerable<string> dataComparison = couponDefinations.Except(couponDefinationsFromDb);
                if (!dataComparison.Any())
                {
                    testStep.SetOutput("Coupon definitions data is validated successfully with database");
                }
                else
                {
                    throw new Exception("Coupon definitions from the response are different from database");
                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1609_SOAP_GetCouponDefinitions_InvalidAttributeName()
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
                stepName = "Getting Coupon Definitions from Service with Invalid attribute name and validate the Error";
                ContentSearchAttributesStruct[] contentSearchAttributesStructs = new ContentSearchAttributesStruct[1];
                contentSearchAttributesStructs[0] = new ContentSearchAttributesStruct();
                contentSearchAttributesStructs[0].AttributeName = "test";
                contentSearchAttributesStructs[0].AttributeValue = DatabaseUtility.GetValueNotNullFromDBSOAP("lw_al_contentattribute", "ATTRIBUTEVALUE");
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", contentSearchAttributesStructs, null, false, 1, 10);
                if (errorMsg.Contains("Error code=3362") && errorMsg.Contains("Error Message=No content available that matches the specified criteria"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1610_SOAP_GetCouponDefinitions_InvalidAttributeNameValue()
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
                stepName = "Getting Coupon Definitions from Service with Invalid attribute name,Invalid attribute value and validate the Error";
                ContentSearchAttributesStruct[] contentSearchAttributesStructs = new ContentSearchAttributesStruct[1];
                contentSearchAttributesStructs[0] = new ContentSearchAttributesStruct();
                contentSearchAttributesStructs[0].AttributeName = "test";
                contentSearchAttributesStructs[0].AttributeValue = "test";
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", contentSearchAttributesStructs, null, false, 1, 10);
                if (errorMsg.Contains("Error code=3362") && errorMsg.Contains("Error Message=No content available that matches the specified criteria"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1611_SOAP_GetCouponDefinitions_InvalidAttributeValue()
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
                stepName = "Getting Coupon Definitions from Service with Invalid attribute value and validate the Error";
                ContentSearchAttributesStruct[] contentSearchAttributesStructs = new ContentSearchAttributesStruct[1];
                contentSearchAttributesStructs[0] = new ContentSearchAttributesStruct();
                contentSearchAttributesStructs[0].AttributeName = "AutoAttributeSet";
                contentSearchAttributesStructs[0].AttributeValue = "test";
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", contentSearchAttributesStructs, null, false, 1, 10);
                if (errorMsg.Contains("Error code=3362") && errorMsg.Contains("Error Message=No content available that matches the specified criteria"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1612_SOAP_GetCouponDefinitions_NonExistingChannel()
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
                stepName = "Getting Coupon Definitions from Service with non existing channel and validate the Error";
                string channel = "test";
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", channel, null, null, false, 1, 10);

                if (errorMsg.Contains("Error code=6003") && errorMsg.Contains("Error Message=Specified channel is not defined"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1613_SOAP_GetCouponDefinitions_NonExistingLanguage()
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
                stepName = "Getting Coupon Definitions from Service with non existing language and validate the Error";
                string language = "test";
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid(language, "Web", null, null, false, 1, 10);
                if (errorMsg.Contains("Error code=6002") && errorMsg.Contains("Error Message=Specified language is not defined"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 6003. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1711_SOAP_GetCouponDefinitions_PageNumberNegativeValue()
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
                stepName = "Getting Coupon Definitions from Service where Page number is a negative value and validate the Error";
                int pageNumber = -1;
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", null, null, false, pageNumber, 10);
                if (errorMsg.Contains("Error code=3304") && errorMsg.Contains("Error Message=Invalid PageNumber provided. Must be greater than zero"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3304. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1712_SOAP_GetCouponDefinitions_NoParameters()
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
                stepName = "Getting Coupon Definitions from Service with no parameters and validate the Error";
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid(null, null, null, null, false, null, null);

                if (errorMsg.Contains("Error code=3304") && errorMsg.Contains("Error Message=Invalid PageNumber provided. Must be greater than zero"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3304. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1713_SOAP_GetCouponDefinitions_ResultsPerPageNegativeValue()
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
                stepName = "Getting Coupon Definitions from Service where Results per page is a negative value and validate the Error";
                int resultsPerPage = -1;
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", null, null, false, 1, resultsPerPage);

                if (errorMsg.Contains("Error code=3305") && errorMsg.Contains("Error Message=Invalid ResultsPerPage provided. Must be greater than zero"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3305. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Negative")]
        [TestMethod]
        public void BTA1234_ST1714_SOAP_GetCouponDefinitions_PageNumberGreaterThanTotalCouponsCount()
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
                stepName = "Getting Coupon Definitions from Service where PageNumber > total Coupons count and validate the Error";
                int pagenumber;
                int resultsperpage = 10;
                int couponsCount = Convert.ToInt32(DatabaseUtility.GetCountFromDBSOAP("lw_coupondef"));
                int pageCount = couponsCount / resultsperpage;
                int resultValues = couponsCount % resultsperpage;
                pagenumber = pageCount + 2;
                string errorMsg = cdis_Service_Method.GetCouponDefinitions_Invalid("en", "Web", null, null, false, pagenumber, resultsperpage);

                if (errorMsg.Contains("Error code=3362") && errorMsg.Contains("Error Message=No content available that matches the specified criteria"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3362. Actual error received is" + errorMsg);
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1715_SOAP_GetCouponDefinitions_ResultsPerPageGreaterThanTotalCouponsCount()
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
                stepName = "Getting Coupon Definitions from Service where Pagenumber > total Coupons count and validate the Error";
                int pagenumber;
                int resultsperpage = 10;
                int couponsCount = Convert.ToInt32(DatabaseUtility.GetCountFromDBSOAP("lw_coupondef"));
                int pageCount = couponsCount / resultsperpage;
                int resultValues = couponsCount % resultsperpage;
                pagenumber = pageCount + 1;

                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, null, false, pagenumber, resultsperpage, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }

                if (couponDefinations.Count() == resultValues)
                {
                    testStep.SetOutput("System returned all Coupon Definitions based on filtering");
                }
                else
                {
                    throw new Exception("System returned Coupon Definitions is not matching the filtering");
                }

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1717_SOAP_GetCouponDefinitions_PageNumberLessThanOrEqualToTotalCouponsCount()
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
                stepName = "Getting Coupon Definitions from Service where PageNumber > total Coupons count and validate the Error";
                int pagenumber;
                int resultsperpage = 10;
                int couponsCount = Convert.ToInt32(DatabaseUtility.GetCountFromDBSOAP("lw_coupondef"));
                pagenumber = couponsCount / resultsperpage;
                int resultValues = couponsCount % resultsperpage;
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, null, false, pagenumber, resultsperpage, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }

                if (couponDefinations.Count() == resultsperpage)
                {
                    testStep.SetOutput("System returned all Coupon Definitions based on filtering");
                }
                else
                {
                    throw new Exception("System returned Coupon Definitions is not matching the filtering");
                }



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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1718_SOAP_GetCouponDefinitions_RestrictDateAsFutureDate()
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
                stepName = "Getting Coupon Definitions from Service where Restrict Date to future date";
                ActiveCouponOptionsStruct activecouponoptions = new ActiveCouponOptionsStruct();
                DateTime futureDate = DateTime.Now.AddYears(1);
                activecouponoptions.RestrictDate = futureDate;
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, activecouponoptions, false, 1, 5, out elapsedTime);
                List<DateTime> dates = new List<DateTime>();
                foreach (var listOfCoupons in def.CouponDefinition)
                {
                    DateTime expiryDate = listOfCoupons.ExpiryDate.Value;
                    dates.Add(expiryDate);
                    if (expiryDate < activecouponoptions.RestrictDate)
                    {
                        throw new Exception("Coupon definitions are not displayed whose expiry date falls after the restrict date:" + futureDate + " and expiry dates are " + string.Join(",", dates));
                    }
                }
                testStep.SetOutput("Coupon definitions are displayed whose expiry date falls after the restrict date:" + futureDate + " and expiry dates are " + string.Join(",", dates));
                Logger.Info("Coupon definitions are displayed whose expiry date falls after the restrict date:" + futureDate + " and expiry dates are " + string.Join(",", dates));

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1719_SOAP_GetCouponDefinitions_LocalDateAsPastDate()
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
                stepName = "Getting Coupon Definitions from Service where local date as past date";
                ActiveCouponOptionsStruct activecouponoptions = new ActiveCouponOptionsStruct();
                DateTime pastDate = DateTime.Now.AddYears(-5);
                activecouponoptions.LocalDate = pastDate;
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, activecouponoptions, false, 1, 5, out elapsedTime);
                List<DateTime> dates = new List<DateTime>();
                foreach (var listOfCoupons in def.CouponDefinition)
                {
                    DateTime startDate = listOfCoupons.StartDate;
                    dates.Add(startDate);
                    if (startDate < activecouponoptions.LocalDate)
                    {
                        throw new Exception("Coupon definitions are not returned whose start date falls after the local date: :" + pastDate + "and the dates are:" + string.Join(",", dates));
                    }
                }
                testStep.SetOutput("Coupon definitions are returned whose start date falls after the local date:" + pastDate + "and the dates are:" + string.Join(",", dates));
                Logger.Info("Coupon definitions are returned whose start date falls after the local date:" + pastDate + "and the dates are:" + string.Join(",", dates));

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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1720_SOAP_GetCouponDefinitions_RestrictGloblaCoupons_RestrictGlobal()
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
                stepName = "Getting Coupon Definitions from Service with restrictGlobalCoupons = RestrictGlobal";
                ActiveCouponOptionsStruct activecouponoptions = new ActiveCouponOptionsStruct();
                activecouponoptions.RestrictGlobalCoupons = "RestrictGlobal";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, activecouponoptions, false, 1, 10, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);

                stepName = "Validate Coupon definitions should be returned  with IsGlobal = false";

                List<bool> isGloblaStatus = new List<bool>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    isGloblaStatus.Add(def.CouponDefinition[i].IsGlobal);
                }
                if (isGloblaStatus.Contains(true))
                {
                    throw new Exception("Coupon definitions are Not returned  with IsGlobal = false");
                }
                else
                {
                    testStep.SetOutput("Coupon definitions are returned  with IsGlobal = false");
                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1721_SOAP_GetCouponDefinitions_RestrictGloblaCoupons_RestrictNonGlobal()
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
                stepName = "Getting Coupon Definitions from Service with restrictGlobalCoupons = RestrictNonGlobal";
                ActiveCouponOptionsStruct activecouponoptions = new ActiveCouponOptionsStruct();
                activecouponoptions.RestrictGlobalCoupons = "RestrictNonGlobal";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, activecouponoptions, false, 1, 10, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);

                stepName = "Validate Coupon definitions should be returned  with IsGlobal = false";

                List<bool> isGloblaStatus = new List<bool>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    isGloblaStatus.Add(def.CouponDefinition[i].IsGlobal);
                }
                if (isGloblaStatus.Contains(false))
                {
                    throw new Exception("Coupon definitions are Not returned  with IsGlobal = True");
                }
                else
                {
                    testStep.SetOutput("Coupon definitions are returned  with IsGlobal = True");
                }
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
        [TestCategory("API_SOAP_GetCouponDefinitions")]
        [TestCategory("API_SOAP_GetCouponDefinitions_Positive")]
        [TestMethod]
        public void BTA1234_ST1722_SOAP_GetCouponDefinitions_RestrictGloblaCoupons_None()
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
                stepName = "Getting Coupon Definitions from Service with restrictGlobalCoupons = RestrictNonGlobal";
                ActiveCouponOptionsStruct activecouponoptions = new ActiveCouponOptionsStruct();
                activecouponoptions.RestrictGlobalCoupons = "None";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions("en", "Web", null, activecouponoptions, false, 1, 10, out elapsedTime);
                List<string> couponDefinations = new List<string>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    couponDefinations.Add(def.CouponDefinition[i].Name);
                }
                testStep.SetOutput("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                Logger.Info("Coupon Names from Coupon definitions are : " + string.Join(",", couponDefinations));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);

                stepName = "Validate Coupon definitions should be returned  with IsGlobal = false";

                List<bool> isGloblaStatus = new List<bool>();
                for (var i = 0; i < def.CouponDefinition.Count(); i++)
                {
                    isGloblaStatus.Add(def.CouponDefinition[i].IsGlobal);
                }
                if (isGloblaStatus.Contains(false) || isGloblaStatus.Contains(true))
                {
                    testStep.SetOutput("Coupon definitions are returned  with IsGlobal = True or IsGlobal = False");
                }
                else
                {
                    throw new Exception("Coupon definitions are Not returned  with IsGlobal = True or IsGlobal = False");
                }
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
