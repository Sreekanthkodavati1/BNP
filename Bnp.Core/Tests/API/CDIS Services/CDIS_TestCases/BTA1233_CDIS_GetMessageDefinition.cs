using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1233_SOAP_GetMessageDefinition : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetMessageDefinition")]
        [TestCategory("API_SOAP_GetMessageDefinition_Positive")]
        [TestMethod]
        public void BTA1233_ST1405_SOAP_GetMessageDefinition_MandatoryFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            double elapsedTime = 0;
            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition Id from database";
                string messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                testStep.SetOutput("Message definition Id from database is " + messageDefId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition with Mandatory Fields(MessageDefId)";
                MessageDefinitionStruct messageDefData = cdis_Service_Method.GetMessageDefinition(long.Parse(messageDefId), "", "", false, out elapsedTime);
                testStep.SetOutput("messageDefData response is" + " Id: "+messageDefData.Id+" Name: "+messageDefData.Name+" StartDate: "+messageDefData.StartDate+" ExpiryDate: "+messageDefData.ExpiryDate+" Elapsed time: "+ elapsedTime);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                string startDate = messageDefData.StartDate.Value.ToString("dd-MM-yyyy HH:mm:ss");
                string expiryDate = messageDefData.ExpiryDate.Value.ToString("dd-MM-yyyy HH:mm:ss");
            
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate message definition response with database";
                messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                long idFromDb = long.Parse(messageDefId);
                string startDateDb = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "STARTDATE", string.Empty);
                string expiryDateDb = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "EXPIRYDATE", string.Empty);
                DateTime startD =  Convert.ToDateTime(startDateDb);
                DateTime expiryD = Convert.ToDateTime(expiryDateDb);
                startDateDb = startD.ToString("dd-MM-yyyy HH:mm:ss");
                expiryDateDb = expiryD.ToString("dd-MM-yyyy HH:mm:ss");
                if (messageDefData.Id==idFromDb && startDate.Equals(startDateDb) && expiryDate.Equals(expiryDateDb)&&elapsedTime>0)
                {
                    testStep.SetOutput("Message definition response is validated successfully with database");
                }
                else
                {
                    throw new Exception("Message definition response is different from the data in Database");
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetMessageDefinition")]
        [TestCategory("API_SOAP_GetMessageDefinition_Positive")]
        [TestMethod]
        public void BTA1233_ST1406_SOAP_GetMessageDefinition_AllFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            double elapsedTime = 0;
            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition Id from database";
                string messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                testStep.SetOutput("Message definition Id from database is " + messageDefId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition with All fields";
                MessageDefinitionStruct messageDefData = cdis_Service_Method.GetMessageDefinition(long.Parse(messageDefId), "en", "Web", true, out elapsedTime);
                testStep.SetOutput("messageDefData response is" + " Id: " + messageDefData.Id + " Name: " + messageDefData.Name + " StartDate: " + messageDefData.StartDate + " ExpiryDate: " + messageDefData.ExpiryDate + " Elapsed time: " + elapsedTime);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                string startDate = messageDefData.StartDate.Value.ToString("dd-MM-yyyy HH:mm:ss");
                string expiryDate = messageDefData.ExpiryDate.Value.ToString("dd-MM-yyyy HH:mm:ss");

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate message definition response with database";
                messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                long idFromDb = long.Parse(messageDefId);
                string startDateDb = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "STARTDATE", string.Empty);
                string expiryDateDb = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "EXPIRYDATE", string.Empty);
                DateTime startD = Convert.ToDateTime(startDateDb);
                DateTime expiryD = Convert.ToDateTime(expiryDateDb);
                startDateDb = startD.ToString("dd-MM-yyyy HH:mm:ss");
                expiryDateDb = expiryD.ToString("dd-MM-yyyy HH:mm:ss");
                if (messageDefData.Id == idFromDb && startDate.Equals(startDateDb) && expiryDate.Equals(expiryDateDb))
                {
                    testStep.SetOutput("Message definition response is validated successfully with database");
                }
                else
                {
                    throw new Exception("Message definition response is different from the data in Database");
                }

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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetMessageDefinition")]
        [TestCategory("API_SOAP_GetMessageDefinition_Negative")]
        [TestMethod]
        public void BTA1233_ST1393_SOAP_GetMessageDefinition_InvalidLanguage()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started");

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message Definition ID from database";
                string messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                testStep.SetOutput("Message definition Id from database is " + messageDefId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition with invalid language and validate the Error code=6002";
                string language = "test";
                string error = cdis_Service_Method.GetMessageDefinition_Invalid(long.Parse(messageDefId), language, "Web", true);
                if (error.Contains("Error code=6002") && error.Contains("Error Message=Specified language is not defined"))
                {
                    testStep.SetOutput("Invalid Language details: \""+ language + "\" and the Error details received from the response are as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:6002 . Actual error received is" + error);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetMessageDefinition")]
        [TestCategory("API_SOAP_GetMessageDefinition_Negative")]
        [TestMethod]
        public void BTA1233_ST1394_SOAP_GetMessageDefinition_InvalidChannel()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started");

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message Definition ID from database";
                string messageDefId = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", string.Empty, string.Empty, "OBJECTID", string.Empty);
                testStep.SetOutput("Message definition Id from database is " + messageDefId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition with invalid channel and validate the Error code=6003";
                string channel = "test";
                string error = cdis_Service_Method.GetMessageDefinition_Invalid(long.Parse(messageDefId), "en", channel, true);
                if (error.Contains("Error code=6003") && error.Contains("Error Message=Specified channel is not defined"))
                {
                    testStep.SetOutput("Invalid Channel details: \"" + channel + "\" and the Error details received from the response are as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:6003 . Actual error received is" + error);
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
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_GetMessageDefinition")]
        [TestCategory("API_SOAP_GetMessageDefinition_Negative")]
        [TestMethod]
        public void BTA1233_ST1395_SOAP_GetMessageDefinition_InvalidMessageDefId()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started");

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Message definition with Invalid MessageDef and validate the Error code 3370";
                string messageDefId = common.RandomNumber(4);
                string value = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", "OBJECTID", messageDefId, "OBJECTID", string.Empty);
                while (value.Equals(messageDefId))
                {
                    messageDefId = common.RandomNumber(4);
                    value = DatabaseUtility.GetFromSoapDB("Lw_Al_MessageDef", "OBJECTID", messageDefId, "OBJECTID", string.Empty);
                }

                string error = cdis_Service_Method.GetMessageDefinition_Invalid(long.Parse(messageDefId), "en", "Web", true);
                if (error.Contains("Error code=3370") && error.Contains("Error Message=No message found with the provided id"))
                {
                    testStep.SetOutput("Invalid MessageDefID details: \"" + messageDefId + "\" and The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:3370 . Actual error received is" + error);
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