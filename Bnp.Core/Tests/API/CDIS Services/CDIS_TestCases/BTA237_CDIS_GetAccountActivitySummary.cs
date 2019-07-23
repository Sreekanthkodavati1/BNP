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
    [TestClass]
    public class BTA237_CDIS_GetAccountActivitySummary : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA237_CDIS_GetAccountActivitySummary_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Post transacation to a Member using UpdateMember method";
                DateTime date = DateTime.Now.AddDays(-5);
                string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
                testStep.SetOutput("The txnHeaderID of the transaction is  " + txnHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "GetAccountActivitySummary of the above member and verify transaction details";
                GetAccountActivitySummaryOut AccountActivitySummaryOut = cdis_Service_Method.GetAccountActivitySummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(txnHeaderId, AccountActivitySummaryOut.AccountActivitySummary[0].TxnHeaderId, "Expected value is" + txnHeaderId + "Actual value is" + AccountActivitySummaryOut.AccountActivitySummary[0].TxnHeaderId);      
                testStep.SetOutput("The following are details from the response, " +
                    ";The txnAmount is: " + AccountActivitySummaryOut.AccountActivitySummary[0].TxnAmount+
                    ";and txnHeader is: "+ AccountActivitySummaryOut.AccountActivitySummary[0].TxnHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating txnAmount from the DB";
                string dbresponse = DatabaseUtility.GetTxnAmountusingVCKeyDBSoap(txnHeaderId);
                Assert.AreEqual(AccountActivitySummaryOut.AccountActivitySummary[0].TxnAmount + "", dbresponse, "Expected value is" + AccountActivitySummaryOut.AccountActivitySummary[0].TxnAmount + "Actual value is" + dbresponse);
                testStep.SetOutput("The txnAmount from the db is: " + dbresponse + " and the txnAmount from service is: " + AccountActivitySummaryOut.AccountActivitySummary[0].TxnAmount);
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
