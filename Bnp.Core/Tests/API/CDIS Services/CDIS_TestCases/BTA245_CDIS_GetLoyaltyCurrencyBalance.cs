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
    public class BTA245_CDIS_GetLoyaltyCurrencyBalance : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA245_CDIS_GetLoyaltyCurrencyBalance_Positive()
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
                stepName = "Posting transaction to a member using UpdateMember method";
                DateTime date = DateTime.Now.AddDays(-5);
                string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
                testStep.SetOutput("The txnHeaderID is  " + txnHeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get the Loyalty currency balance of the member using GetLoyaltyCurrencyBalance";
                decimal loyalCurrencyBalance = cdis_Service_Method.GetLoyaltyCurrencyBalance(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Loyalty currency balance of the member is : " + loyalCurrencyBalance);
                Logger.Info("loyalty Currency Balance for the member is : " + loyalCurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the Loyalty currency balance of GetLoyaltyCurrencyBalance method with GetAccountSummary method";
                var currencyBalance = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(currencyBalance.CurrencyBalance.ToString(), loyalCurrencyBalance.ToString(), "Expected value is" + currencyBalance.CurrencyBalance.ToString() + "Actual value is" + loyalCurrencyBalance);
                testStep.SetOutput("LoyaltyCurrencyBalance value from GetAccountSummary is: " + currencyBalance.CurrencyBalance.ToString() + " and from GetLoyaltyCurrencyBalance method is: " + loyalCurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate member points in DB";
                string dbresponse = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
                Assert.AreEqual(loyalCurrencyBalance+"", dbresponse, "Expected value is" + loyalCurrencyBalance + "Actual value is" + dbresponse);
                testStep.SetOutput("LoyaltyCurrencyBalance value from db is: " + dbresponse + " and LoyaltyCurrencyBalance from GetLoyaltyCurrencyBalance method is: " + loyalCurrencyBalance);
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
