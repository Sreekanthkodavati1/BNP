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
    public  class BTA246_CDIS_MergeMembers : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA246_CDIS_MergeMembers_Positive()
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
                stepName = "Add First member using AddMember method";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ",Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc1 = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "AwardLoyaltyCurrency Balance to first member";
                AwardLoyaltyCurrencyOut loyaltyCurrencyOut = cdis_Service_Method.AwardLoyaltyCurrency(vc1[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints);
                testStep.SetOutput("Loyalty currency balance for firstMember: " + loyaltyCurrencyOut.CurrencyBalance);
                Logger.Info("Balance of Account after awarding loyalty currency" + loyaltyCurrencyOut.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary of first member using GetAccountSummary Method";
                GetAccountSummaryOut AccountSummaryOut = cdis_Service_Method.GetAccountSummary(vc1[0].LoyaltyIdNumber);
                testStep.SetOutput("The following are the details of First Member;" +
                                   "Loyalty Currency Balance is: " + AccountSummaryOut.CurrencyBalance+
                                   ";Tier:  "+AccountSummaryOut.CurrentTierName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add second member using AddMember method";
                Member outputnew = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + outputnew.IpCode + ", Name: " + outputnew.FirstName);
                Logger.Info("IpCode: " + outputnew.IpCode + ", Name: " + outputnew.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vcs = outputnew.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary of second member using GetAccountSummary Method";
                GetAccountSummaryOut AccountSummaryOut1 = cdis_Service_Method.GetAccountSummary(vcs[0].LoyaltyIdNumber);
                testStep.SetOutput("The following are the details of Second Member;" +
                                   "Loyalty Currency Balance is: " + AccountSummaryOut1.CurrencyBalance +
                                   ";Tier:  " + AccountSummaryOut1.CurrentTierName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Merging Members through MergeMember method";
                Member member=cdis_Service_Method.MergeMembers(vc1[0].LoyaltyIdNumber,vcs[0].LoyaltyIdNumber);
                testStep.SetOutput("The second member Ipcode: "+ member.IpCode + " and member status is: "  + member.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vcmerge = member.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary of the second member after MERGE and verify tier and balance information";
                GetAccountSummaryOut AccountSummaryOutmerge = cdis_Service_Method.GetAccountSummary(vcmerge[0].LoyaltyIdNumber);
                testStep.SetOutput("The following are the details of Second Member after MERGE;" +
                       "Loyalty Currency Balance is: " + AccountSummaryOutmerge.CurrencyBalance +
                       ";Tier:  " + AccountSummaryOutmerge.CurrentTierName+
                        "; and Member status is: " +AccountSummaryOutmerge.MemberStatus);
                Assert.AreEqual(AccountSummaryOut1.MemberStatus, AccountSummaryOutmerge.MemberStatus, "Expected value is" + AccountSummaryOut1.MemberStatus + "Actual value is" + AccountSummaryOutmerge.MemberStatus);
                Assert.AreEqual((AccountSummaryOut1.CurrencyBalance+ AccountSummaryOut.CurrencyBalance), AccountSummaryOutmerge.CurrencyBalance, "Expected value is" + (AccountSummaryOut1.CurrencyBalance + AccountSummaryOut.CurrencyBalance) + "Actual value is" + AccountSummaryOutmerge.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Summary of the First member after MERGE and verify tier and balance information";
                GetAccountSummaryOut AccountSummaryOutoneaftermerge = cdis_Service_Method.GetAccountSummary(vc1[0].LoyaltyIdNumber);
                testStep.SetOutput("The following are the details of First Member after MERGE;" +
                       "Loyalty Currency Balance is: " + AccountSummaryOutoneaftermerge.CurrencyBalance +
                       ";Tier:  " + AccountSummaryOutoneaftermerge.CurrentTierName+
                       "; and Member status is: " + AccountSummaryOutoneaftermerge.MemberStatus);
                Assert.AreEqual("Merged", AccountSummaryOutoneaftermerge.MemberStatus, "Expected value is \"Merged\" and Actual value is" + AccountSummaryOutoneaftermerge.MemberStatus);
                Assert.AreEqual("0", AccountSummaryOutoneaftermerge.CurrencyBalance.ToString(), "Expected value is \"0\" and Actual value is" + AccountSummaryOutoneaftermerge.CurrencyBalance);
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

