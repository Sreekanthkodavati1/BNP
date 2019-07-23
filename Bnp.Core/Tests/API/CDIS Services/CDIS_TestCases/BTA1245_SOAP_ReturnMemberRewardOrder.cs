using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1245_SOAP_ReturnMemberRewardOrder : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Positive")]
        [TestMethod]
        public void BTA1245_ST1760_SOAP_ReturnMemberRewardOrderFFPOrderNbrFalse()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method";
                decimal? amount = 100;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                GetAccountSummaryOut getInitialAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal initialBalance = getInitialAccountSummary.CurrencyBalance;
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members rewards  with CDIS service";
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                reward.RewardName = DatabaseUtility.GetFromSoapDB("LW_REWARDSDEF", "name", "API_Reward_Balancemorethanzero", "name", string.Empty);
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number";
                string orderNumber = memberRewardsOut.OrderNumber;
                string message = cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                Assert.AreEqual(message, "pass", "Member rewards for the member are not returned with order number: " + orderNumber);
                testStep.SetOutput("Returned member rewards for the member with order number: " + orderNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Member's loyalty currency is returned";
                var getFinalAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal finalBalance = getFinalAccountSummary.CurrencyBalance;
                Assert.AreEqual(initialBalance, finalBalance, "Initial and final Currency balance are not equal. initial balance:" + getInitialAccountSummary, "final balance:" + getFinalAccountSummary);
                testStep.SetOutput("Member's loyalty currency is returned successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Positive")]
        [TestMethod]
        //TODO: Need the exact reward name from DB
        public void BTA1245_ST1778_SOAP_ReturnMemberRewardOrder_OrderNumberPartNumberExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method";
                decimal? amount = 100;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                GetAccountSummaryOut getInitialAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal initialBalance = getInitialAccountSummary.CurrencyBalance;
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members rewards  with CDIS service where Order number and part number exists";
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                reward.RewardName = DatabaseUtility.GetFromSoapDB("LW_PRODUCT", "name", "API_Reward_Product", "name", string.Empty);
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number";
                string orderNumber = memberRewardsOut.OrderNumber;
                cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                Assert.AreEqual(elapsedTime > 0, true, "Elapsed time is not greater tha zero");
                testStep.SetOutput("Returned member rewards for the member with order number: " + orderNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Member's loyalty currency is returned";
                var getFinalAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal finalBalance = getFinalAccountSummary.CurrencyBalance;
                Assert.AreEqual(initialBalance, finalBalance, "Initial and final Currency balance are not equal. initial balance:" + getInitialAccountSummary, "final balance:" + getFinalAccountSummary);
                testStep.SetOutput("Member's loyalty currency is returned successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Positive")]
        [TestMethod]
        public void BTA1245_ST1779_SOAP_ReturnMemberRewardOrder_OnlyOrderNumberExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method";
                decimal? amount = 100;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                GetAccountSummaryOut getInitialAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal initialBalance = getInitialAccountSummary.CurrencyBalance;
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members rewards  with CDIS service where only Order number exists";
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                reward.RewardName = DatabaseUtility.GetFromSoapDB("LW_REWARDSDEF", "name", "API_Reward_Balancemorethanzero", "name", string.Empty);
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number";
                string orderNumber = memberRewardsOut.OrderNumber;
                cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                testStep.SetOutput("Returned member rewards for the member with order number: " + orderNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Member's loyalty currency is returned";
                var getFinalAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal finalBalance = getFinalAccountSummary.CurrencyBalance;
                Assert.AreEqual(initialBalance, finalBalance, "Initial and final Currency balance are not equal. initial balance:" + getInitialAccountSummary, "final balance:" + getFinalAccountSummary);
                testStep.SetOutput("Member's loyalty currency is returned successfully where initial balance is : " + initialBalance + " and final balance is : " + finalBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Negative")]
        [TestMethod]
        public void BTA1245_ST1780_SOAP_ReturnMemberRewardOrder_WithoutOrderNumber()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();
            string errorCode = "2003";
            string expectedMessage = "ReturnMemberRewardOrderIn is a required property.  Please provide a valid value.";

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
              
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number where order number is null";
                string orderNumber = null;
                string message = cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                Assert.AreEqual(message.Contains(errorCode), true, "Did not return error code: " + errorCode + " for the member with order number: " + orderNumber);
                Assert.AreEqual(message.Contains(expectedMessage), true, "Did not return message as : " + expectedMessage + " for the member with order number: " + orderNumber);
                testStep.SetOutput("Returned error code: " + errorCode + " and message as like : " + expectedMessage + " for the member with order number: " + orderNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Negative")]
        [TestMethod]
        //TODO: Need the exact reward name from DB
        public void BTA1245_ST1781_SOAP_ReturnMemberRewardOrder_OrderNumberExistsPartNumberDoesNotExists()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();
            string errorCode = "3355";
            string expectedMessage = "Order number not found.";

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Award LoyaltyCurrency to a member using AwardLoyaltyCurrency method";
                decimal? amount = 100;
                DateTime expirationdate = new DateTime(DateTime.Now.Year, 12, 31);
                DateTime? transactiondate = DateTime.Now;
                AwardLoyaltyCurrencyOut awardLoyaltyCurrency = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber, LoyaltyEvents.PurchaseActivity, LoyaltyCurrency.BasePoints, amount, transactiondate, expirationdate, "Awarding new amounts", "SOAPService");
                GetAccountSummaryOut getInitialAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                decimal initialBalance = getInitialAccountSummary.CurrencyBalance;
                testStep.SetOutput("The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The LoyaltyCurrencyBalance awarded to the member is: " + awardLoyaltyCurrency.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members rewards  with CDIS service";
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                reward.RewardName = DatabaseUtility.GetFromSoapDB("LW_REWARDSDEF", "name", "ZeroBalanceReward", "name", string.Empty);
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number where order number exists and part number does not exists";
                string orderNumber = memberRewardsOut.OrderNumber;
                string message = cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                Assert.AreEqual(message.Contains(errorCode), true, "Error code received is different and is : " + errorCode + " for the member with order number: " + orderNumber);
                Assert.AreEqual(message.Contains(expectedMessage), true, "Error message received is different and is  : " + expectedMessage + " for the member with order number: " + orderNumber);
                testStep.SetOutput("Returned error code: " + errorCode + " and Error message : " + expectedMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ReturnMemberRewardOrder")]
        [TestCategory("API_SOAP_ReturnMemberRewardOrder_Negative")]
        [TestMethod()]
        public void BTA1245_ST1782_SOAP_ReturnMemberRewardOrder_InvalidOrderNumber()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            List<string> CategoryID = new List<string>();
            List<string> DBCategoryID = new List<string>();
            string errorCode = "3355";
            string expectedMessage = "Order number not found.";

            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
             
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Return member rewards based on the Order number where order number is invalid";
                string orderNumber = "LWI-" + common.RandomNumber(4);
                string message = cdis_Service_Method.ReturnMemberRewardOrder(orderNumber, out elapsedTime);
                Assert.AreEqual(message.Contains(errorCode), true, "Error code received is different and is : " + errorCode + " for the member with order number: " + orderNumber);
                Assert.AreEqual(message.Contains(expectedMessage), true, "Error message received is different and is : " + expectedMessage + " for the member with order number: " + orderNumber);
                testStep.SetOutput("Returned Error code: " + errorCode + " and Error message is : " + expectedMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                Logger.Info("###Test Execution Ends### Test Passed: " + testCase.GetTestCaseName());
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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