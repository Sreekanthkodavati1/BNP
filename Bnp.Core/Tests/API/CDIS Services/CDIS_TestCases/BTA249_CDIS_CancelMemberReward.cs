using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
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
    public class BTA249_CDIS_CancelMemberReward : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_CancelMemberReward")]
        [TestCategory("API_SOAP_CancelMemberReward_Positive")]
        [TestMethod]
        public void BTA249_SOAP_CancelMemberReward()
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
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Award Loyalty Balance";
                AwardLoyaltyCurrencyOut loyaltyCurrencyOut = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The loyalty currency balance of the member is: " + loyaltyCurrencyOut.CurrencyBalance);
                Logger.Info("Balance of Account after awarding loyalty currency" + loyaltyCurrencyOut.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Activity Summary through GetAccountSummary method";
                GetAccountSummaryOut AccountSummaryOutBeforeReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The loyalty Currency of the member before adding the reward is  " + AccountSummaryOutBeforeReward.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 100, 100);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 100)
                    {
                        reward = r;
                        break;
                    }
                }
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID+
                                     " and the reward name is: "+reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                // IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Reward to Member using AddMemberRewards service";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
                Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Activity Summary through GetAccountSummary method";
                GetAccountSummaryOut AccountSummaryOutAfterReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The  loyalty Currency of the member after adding reward is  " + AccountSummaryOutAfterReward.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Cancelling the member reward with CDIS service";
                decimal value = cdis_Service_Method.CancelMemberReward(memberRewardsOut.MemberRewardID + "", out time);
                testStep.SetOutput("The Loyalty currency balance from the CancelMemberReward response is :" + value);
                Logger.Info("Value from the service:" + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Account Activity Summary through GetAccountSummary method after cancelling the reward";
                AccountSummaryOutBeforeReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The  loyalty Currency of the member after cancelling the reward is  " + AccountSummaryOutBeforeReward.CurrencyBalance);
                Assert.AreEqual(AccountSummaryOutBeforeReward.CurrencyBalance, value, "Expected Value is " + AccountSummaryOutBeforeReward.CurrencyBalance + " Actual Value is " + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the reward status with data from the DB";
                string dbresponse = DatabaseUtility.GetMemberRewardStatusfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                testStep.SetOutput("The Reward status from database : " + dbresponse);
                Assert.AreEqual("Cancelled", dbresponse, "Expected Value is " + "Cancelled" + " Actual Value is " + dbresponse);
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
		[TestCategory("API_SOAP_CancelMemberReward")]
		[TestCategory("API_SOAP_CancelMemberReward_Positive")]
		[TestMethod]
		public void BTA1241_ST1325_SOAP_CancelMemberReward_CancelRewardIssuedToMember()
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
				stepName = "Adding member with CDIS service";
				Member output = cdis_Service_Method.GetCDISMemberGeneral();
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Award Loyalty Balance";
				AwardLoyaltyCurrencyOut loyaltyCurrencyOut = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The loyalty currency balance of the member is: " + loyaltyCurrencyOut.CurrencyBalance);
				Logger.Info("Balance of Account after awarding loyalty currency" + loyaltyCurrencyOut.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Account Activity Summary through GetAccountSummary method";
				GetAccountSummaryOut AccountSummaryOutBeforeReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The loyalty Currency of the member before adding the reward is  " + AccountSummaryOutBeforeReward.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 100, 100);
				RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
				foreach (RewardCatalogSummaryStruct r in rewardCatalog)
				{
					if (r.CurrencyToEarn == 100)
					{
						reward = r;
						break;
					}
				}
				testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID +
									 " and the reward name is: " + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
								
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Account Activity Summary through GetAccountSummary method";
				GetAccountSummaryOut AccountSummaryOutAfterReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The  loyalty Currency of the member after adding reward is  " + AccountSummaryOutAfterReward.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Cancelling the member reward with CDIS service";
				decimal value = cdis_Service_Method.CancelMemberReward(memberRewardsOut.MemberRewardID + "", out time);
				testStep.SetOutput("The Loyalty currency balance from the CancelMemberReward response is : " + value + " the elapsed time is : " + time);
				Logger.Info("Value from the service:" + value);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Account Activity Summary through GetAccountSummary method after cancelling the reward";
				AccountSummaryOutBeforeReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The  loyalty Currency of the member after cancelling the reward is  " + AccountSummaryOutBeforeReward.CurrencyBalance);
				Assert.AreEqual(AccountSummaryOutBeforeReward.CurrencyBalance, value, "Expected Value is " + AccountSummaryOutBeforeReward.CurrencyBalance + " Actual Value is " + value);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the reward status with data from the DB";
				string dbresponse = DatabaseUtility.GetMemberRewardStatusfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
				testStep.SetOutput("The Reward status from database : " + dbresponse);
				Assert.AreEqual("Cancelled", dbresponse, "Expected Value is " + "Cancelled" + " Actual Value is " + dbresponse);
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
		[TestCategory("API_SOAP_CancelMemberReward")]
		[TestCategory("API_SOAP_CancelMemberReward_Negative")]
		[TestMethod]
		public void BTA1241_ST1326_SOAP_CancelMemberReward_InvalidMemberRewardId_Negative()
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
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Award Loyalty Balance";
				AwardLoyaltyCurrencyOut loyaltyCurrencyOut = cdis_Service_Method.AwardLoyaltyCurrency(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The loyalty currency balance of the member is: " + loyaltyCurrencyOut.CurrencyBalance);
				Logger.Info("Balance of Account after awarding loyalty currency" + loyaltyCurrencyOut.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Account Activity Summary through GetAccountSummary method";
				GetAccountSummaryOut AccountSummaryOutBeforeReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The loyalty Currency of the member before adding the reward is  " + AccountSummaryOutBeforeReward.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 100, 100);
				RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
				foreach (RewardCatalogSummaryStruct r in rewardCatalog)
				{
					if (r.CurrencyToEarn == 100)
					{
						reward = r;
						break;
					}
				}
				testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID +
									 " and the reward name is: " + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Account Activity Summary through GetAccountSummary method";
				GetAccountSummaryOut AccountSummaryOutAfterReward = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The  loyalty Currency of the member after adding reward is  " + AccountSummaryOutAfterReward.CurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Cancelling the member reward with CDIS service by providing invalid member reward id";
				string error = (string)cdis_Service_Method.CancelMemberRewardNegative();
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3356";
				Assert.AreEqual("3356", errorssplit[1], "Expected value is" + "3356" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errors[1]);
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

