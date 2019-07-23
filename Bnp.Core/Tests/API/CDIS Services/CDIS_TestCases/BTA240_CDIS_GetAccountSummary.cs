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
    public class BTA240_CDIS_GetAccountSummary : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA240_CDIS_GetAccountSummary_Positive()
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
                stepName = "Adding points to members by posting transactions using UpdateMember method";
                DateTime date = DateTime.Now.AddDays(-5);
                var txnHeader = cdis_Service_Method.UpdateMember_PostTransactionRequiredDate(output, date);
                testStep.SetOutput("The transaction header of the ID is  " + txnHeader.TxnHeaderId + "and transaction amount is: "+ txnHeader.TxnAmount);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get memberTier through Service";
                MemberTierStruct[] memberTier = cdis_Service_Method.GetMemberTier(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The members tierName using GetMemberTier Method is " + memberTier[0].TierDef.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary through Service";
                GetAccountSummaryOut AccountSummaryOut = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The currencybalance for the member is: " + AccountSummaryOut.CurrencyBalance);
                //added 5 because of welcome reward which is given to customer when he joins as per rule
                Assert.AreEqual((txnHeader.TxnAmount+5).ToString(), AccountSummaryOut.CurrencyBalance.ToString(), "Expected value is" + (txnHeader.TxnAmount + 5).ToString() + "Actual value is" + AccountSummaryOut.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCurrencyBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(AccountSummaryOut.CurrencyBalance + "", dbresponse, "Expected value is" + AccountSummaryOut.CurrencyBalance + "Actual value is" + dbresponse);
                Assert.AreEqual(output.MemberStatus.ToString(), AccountSummaryOut.MemberStatus, "Expected value is" + output.MemberStatus.ToString() + " and actual value is" + AccountSummaryOut.MemberStatus);
                Assert.AreEqual(memberTier[0].TierDef.Name, AccountSummaryOut.CurrentTierName, "Expected value is" + memberTier[0].TierDef.Name + "Actual value is" + AccountSummaryOut.CurrentTierName);
                testStep.SetOutput("The following are member details which have been verfiied" +
                                   ";Loyalty Currency: " +AccountSummaryOut.CurrencyBalance+
                                    ";Member status: " + AccountSummaryOut.MemberStatus +
                                    ";and MemberTier: " + AccountSummaryOut.CurrentTierName);
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
