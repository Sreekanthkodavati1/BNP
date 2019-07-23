using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Summary description for BTA149_CDIS_ReplaceCard
    /// </summary>
    [TestClass]
    public class BTA150_SOAP_ChangeExpirationDate:ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime = 0;

        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_ChangeExpirationDate")]
        [TestCategory("API_SOAP_ChangeExpirationDate_Positive")]
        [TestMethod]
        public void BTA150_SOAP_ChangeExpirationDate_Positive()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Replace the expiration date for the card";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime response = cdis_Service_Method.ChangeCardExpirationDate(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The LoyaltyCard's: "+ vc[0].LoyaltyIdNumber + " new expiration date is : " + response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the updated expiration date in the DB";
                string dbresponse = DatabaseUtility.GetExpirationDatefromDbSOAP(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(response + "", dbresponse, "Expected value is" + response + "Actual value is" + dbresponse);
                testStep.SetOutput("The new expiration date from DB is :" + dbresponse);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Positive")]
        [TestMethod]
        public void BTA1466_ST1698_SOAP_Regression_ChangeExpirationDate_FutureDate()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime newDate = DateTime.Now.AddYears(1);
                DateTime response = cdis_Service_Method.ChangeCardExpirationDate(vc[0].LoyaltyIdNumber, newDate, out elapsedTime);
                testStep.SetOutput("The LoyaltyCard's: " + vc[0].LoyaltyIdNumber + " new expiration date is : " + response + "Elapsed Time:" + response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the updated expiration date in the DB";
                string dbresponse = DatabaseUtility.GetExpirationDatefromDbSOAP(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(response + "", dbresponse, "Expected value is" + response + "Actual value is" + dbresponse);
                testStep.SetOutput("The new expiration date from DB is :" + dbresponse);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Positive")]
        [TestMethod]
        public void BTA1466_ST1699_SOAP_Regression_ChangeExpirationDate_CurrentDate()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime currentDate = DateTime.Now;
                DateTime response = cdis_Service_Method.ChangeCardExpirationDate(vc[0].LoyaltyIdNumber, currentDate, out elapsedTime);
                testStep.SetOutput("The LoyaltyCard's: " + vc[0].LoyaltyIdNumber + " new expiration date is : " + response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the updated expiration date in the DB";
                string dbresponse = DatabaseUtility.GetExpirationDatefromDbSOAP(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(response + "", dbresponse, "Expected value is" + response + "Actual value is" + dbresponse);
                testStep.SetOutput("The new expiration date from DB is :" + dbresponse);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Positive")]

        [TestMethod]
        public void BTA1466_ST1700_SOAP_Regression_ChangeExpirationDate_WithValidCard()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime dateAftTwoMonths = DateTime.Now.AddMonths(2);
                DateTime response = cdis_Service_Method.ChangeCardExpirationDate(vc[0].LoyaltyIdNumber, dateAftTwoMonths, out elapsedTime);
                testStep.SetOutput("The LoyaltyCard's: " + vc[0].LoyaltyIdNumber + " new expiration date is : " + response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the updated expiration date in the DB";
                string dbresponse = DatabaseUtility.GetExpirationDatefromDbSOAP(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(response + "", dbresponse, "Expected value is" + response + "Actual value is" + dbresponse);
                testStep.SetOutput("The new expiration date from DB is :" + dbresponse);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Positive")]
        [TestMethod]
        public void BTA1466_ST1701_SOAP_Regression_ChangeExpirationDate_WithTransactionPointStatus()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            //double time = 0;
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Updating Member by adding transactions";
                DateTime date = DateTime.Now.AddMilliseconds(1000);
                string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
                testStep.SetOutput("The transaction header of the transcation posted through udpatemember method is  " + txnHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary through Service";
                GetAccountSummaryOut AccountSummaryOut = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The currencybalance for the member is: " + AccountSummaryOut.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                DateTime expiredDate = DateTime.Now.AddMilliseconds(1000);
                DateTime response = cdis_Service_Method.ChangeCardExpirationDate(vc[0].LoyaltyIdNumber, expiredDate, out elapsedTime);
                testStep.SetOutput("The LoyaltyCard's: " + vc[0].LoyaltyIdNumber + " new expiration date is : " + response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary through Service";
                GetAccountSummaryOut AccountSummaryOut1 = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The currencybalance for the member is: " + AccountSummaryOut1.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the Currency Balance of the Card";
                Assert.AreEqual("0", AccountSummaryOut1.CurrencyBalance.ToString(), "Expected value is :0", "Actual value is" + AccountSummaryOut1.CurrencyBalance.ToString());
                testStep.SetOutput("The Currency Balance of the Card :" + AccountSummaryOut1.CurrencyBalance);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Negative")]
        [TestMethod]
        public void BTA1466_ST1702_SOAP_Regression_ChangeExpirationDate_WithPastDate()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime expiredDate = DateTime.Now.AddDays(-2);
                string response = cdis_Service_Method.ChangeCardExpirationDate_Negative(vc[0].LoyaltyIdNumber, expiredDate);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');

                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("1", errorssplit[1], "Expected value is" + "1" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Negative")]
        [TestMethod]
        public void BTA1466_ST1703_SOAP_Regression_ChangeExpirationDate_WithNonExistingCard()
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
                stepName = "Change the Card expiration date for a non exisiting loyalty card";
                string loyaltyNumber = common.RandomNumber(20);
                DateTime expiredDate = DateTime.UtcNow.AddYears(1);
                string response = cdis_Service_Method.ChangeCardExpirationDate_Negative(loyaltyNumber, expiredDate);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput("The Non-Existing LoyaltyCard is: " + loyaltyNumber + " and the response from service is: " + response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("3305", errorssplit[1], "Expected value is" + "3305" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Negative")]
        [TestMethod]
        public void BTA1466_ST1704_SOAP_Regression_ChangeExpirationDate_WithNullCard()
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
                stepName = "Change the Card expiration date by passing null value as loyalty card id";
                string loyaltyNumber = null;
                DateTime expiredDate = DateTime.UtcNow.AddYears(1);
                string response = cdis_Service_Method.ChangeCardExpirationDate_Negative(loyaltyNumber, expiredDate);
                string[] errors = response.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("2003", errorssplit[1], "Expected value is" + "2003" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_ChangeCardExpirationDate")]
        [TestCategory("ChangeCardExpirationDate_Positive")]

        [TestMethod]
        public void BTA1466_ST1705_SOAP_Regression_ChangeExpirationDate_AddTransactionsAfterExpiration()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                DateTime expiredDate = DateTime.Now.AddMilliseconds(2000);
                string response = cdis_Service_Method.ChangeCardExpirationDate_Negative(vc[0].LoyaltyIdNumber, expiredDate);
                testStep.SetOutput(response);
                Logger.Info(response);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change the Card expiration date";
                DateTime expiredDatenew = DateTime.Now.AddDays(-2);
                string responsenew = cdis_Service_Method.ChangeCardExpirationDate_Negative(vc[0].LoyaltyIdNumber, expiredDatenew);
                string[] errors = responsenew.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(responsenew);
                Logger.Info(responsenew);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the error code and the ErrorMessage for the Error Code";
                Assert.AreEqual("1", errorssplit[1], "Expected value is" + "1" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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