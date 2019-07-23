using System;
using System.Collections.Generic;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.Models;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1031_SOAP_ApplyTxnCredit : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        readonly RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
        string Step_Output = "";
        double elapsedTime=0;
        TestStep testStep;
        IList<VirtualCard> vc;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Positive")]
        [TestMethod]
        public void BTA1031_ST1162_SOAP_ApplyTxnCredit_ToExistingMember()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply existing transaction to a member ";
                string memberIdentity = vc[0].LoyaltyIdNumber;
                string cardid = vc[0].LoyaltyIdNumber;
                decimal pointsEarned = cdis_Service_Method.ApplyTxnCredit(memberIdentity, cardid, transactionHeaderId, "ApplyingTxnCredit_CDIS",out elapsedTime);
                if (pointsEarned == Convert.ToDecimal(txnAmount))
                {
                    testStep.SetOutput("Applied existing transaction to a member with MemberIdentity: " + memberIdentity + " TransactionHeaderId: " + transactionHeaderId + " TransactionAmount: " + txnAmount);
                }
                else
                {
                    throw new Exception("Failed to apply Transaction to a member");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the transaction is added in database ";
                string txnHeaderIdFromDB = DatabaseUtility.GetFromSoapDB("ats_txndetailitem", "A_TXNHEADERID", transactionHeaderId, "A_TXNHEADERID", string.Empty);
                Assert.AreEqual(transactionHeaderId, txnHeaderIdFromDB, "Expected value is" + transactionHeaderId + "Actual Value is" + txnHeaderIdFromDB);
                testStep.SetOutput("Transaction is added successfully in txndetailitem table with transactionHeaderId: " + transactionHeaderId);
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
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Negative")]
        [TestMethod]
        public void BTA1031_ST1160_SOAP_ApplyTxnCredit_MemberIdentityNull()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply Transaction to a member where member identity is null and validate the error code as 2003";
                string cardid = vc[0].LoyaltyIdNumber;
                string error = cdis_Service_Method.ApplyTxnCreditInvalid(null, cardid, transactionHeaderId, "ApplyingTxnCredit_CDIS");
                if (error.Contains("Error code=2003") && error.Contains("Error Message=MemberIdentity of ApplyTxnCreditIn is a required property"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:2003. Actual error received is" + error);
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
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Negative")]
        [TestMethod]
        public void BTA1031_ST1161_SOAP_ApplyTxnCredit_MemberIdentityInvalid()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply Transaction to a member where member identity is Invalid and validate the error code as 3302";
                string memberIdentity = "";
                memberIdentity = common.RandomNumber(7);
                string value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                while (value == memberIdentity)
                {
                    memberIdentity = common.RandomNumber(7);
                    value = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", memberIdentity, "LOYALTYIDNUMBER", string.Empty);
                }
                string cardid = vc[0].LoyaltyIdNumber;
                string error = cdis_Service_Method.ApplyTxnCreditInvalid(memberIdentity, cardid, transactionHeaderId, "ApplyingTxnCredit_CDIS");
                if (error.Contains("Error code=3302") && error.Contains("Error Message=Unable to find member with identity"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error:2003. Actual error received is" + error);
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
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Positive")]
        [TestMethod]
        public void BTA1031_ST1213_SOAP_ApplyTxnCreditToMemberCardIdNull()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply transaction to a member with Cardid as null ";
                string memberIdentity = vc[0].LoyaltyIdNumber;
                decimal pointsEarned = cdis_Service_Method.ApplyTxnCredit(memberIdentity, null, transactionHeaderId, "ApplyingTxnCredit_CDIS", out elapsedTime);
                if (pointsEarned == Convert.ToDecimal(txnAmount))
                {
                    testStep.SetOutput("Applied  transaction to a member where Cardid is null with MemberIdentity: " + memberIdentity + " TransactionHeaderId: " + transactionHeaderId + " TransactionAmount: " + txnAmount);
                }
                else
                {
                    throw new Exception("Failed to apply Transaction to a member");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the transaction is added in database ";
                string txnHeaderIdFromDB = DatabaseUtility.GetFromSoapDB("ats_txndetailitem", "A_TXNHEADERID", transactionHeaderId, "A_TXNHEADERID", string.Empty);
                Assert.AreEqual(transactionHeaderId, txnHeaderIdFromDB, "Expected value is" + transactionHeaderId + "Actual Value is" + txnHeaderIdFromDB);
                testStep.SetOutput("Transaction is added successfully in txndetailitem table with transactionHeaderId: " + transactionHeaderId);
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
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Positive")]
        [TestMethod]
        public void BTA1031_ST1214_SOAP_ApplyTxnCreditToMember_NoteNull()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply transaction to a member with Note=Null ";
                string memberIdentity = vc[0].LoyaltyIdNumber;
                string cardid = vc[0].LoyaltyIdNumber;
                decimal pointsEarned = cdis_Service_Method.ApplyTxnCredit(memberIdentity, cardid, transactionHeaderId,null, out elapsedTime);
                if (pointsEarned == Convert.ToDecimal(txnAmount))
                {
                    testStep.SetOutput("Applied  transaction to a member where Note is null with MemberIdentity: " + memberIdentity + " TransactionHeaderId: " + transactionHeaderId + " TransactionAmount: " + txnAmount);
                }
                else
                {
                    throw new Exception("Failed to apply Transaction to a member");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the transaction is added in database ";
                string txnHeaderIdFromDB = DatabaseUtility.GetFromSoapDB("ats_txndetailitem", "A_TXNHEADERID", transactionHeaderId, "A_TXNHEADERID", string.Empty);
                Assert.AreEqual(transactionHeaderId, txnHeaderIdFromDB, "Expected value is" + transactionHeaderId + "Actual Value is" + txnHeaderIdFromDB);
                testStep.SetOutput("Transaction is added successfully in txndetailitem table with transactionHeaderId: " + transactionHeaderId);
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
        [TestCategory("API_SOAP_ApplyTxnCredit")]
        [TestCategory("API_SOAP_ApplyTxnCredit_Positive")]
        [TestMethod]
        public void BTA1031_ST1215_SOAP_ApplyTxnCreditToMemberCardIdNull_VerifyElapsedTime()
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
                List<string> TransactionList = new List<string>();
                stepName = "Getting Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                string transactionHeaderId = TransactionList[0];
                string txnAmount = TransactionList[2];
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Apply transaction to a member with Cardid as null ";
                string memberIdentity = vc[0].LoyaltyIdNumber;
                decimal pointsEarned = cdis_Service_Method.ApplyTxnCredit(memberIdentity, null, transactionHeaderId, "ApplyingTxnCredit_CDIS",out elapsedTime);
                if (pointsEarned == Convert.ToDecimal(txnAmount))
                {
                    testStep.SetOutput("Applied  transaction to a member where Cardid is null with MemberIdentity: " + memberIdentity + " TransactionHeaderId: " + transactionHeaderId + " TransactionAmount: " + txnAmount);
                }
                else
                {
                    throw new Exception("Failed to apply Transaction to a member");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the transaction is added in database ";
                string txnHeaderIdFromDB = DatabaseUtility.GetFromSoapDB("ats_txndetailitem", "A_TXNHEADERID", transactionHeaderId, "A_TXNHEADERID", string.Empty);
                Assert.AreEqual(transactionHeaderId, txnHeaderIdFromDB, "Expected value is" + transactionHeaderId + "Actual Value is" + txnHeaderIdFromDB);
                testStep.SetOutput("Transaction is added successfully in txndetailitem table with transactionHeaderId: " + transactionHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the elapsed time is greater than Zero";
                if (elapsedTime > 0)
                {
                    testStep.SetOutput("Elapsed time is greater than zero and the elapsed time is " + elapsedTime);
                }
                else
                {
                    throw new Exception("Elapsed time is not greater than Zero");
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
