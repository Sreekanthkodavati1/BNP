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
    public class BTA234_CDIS_RedeemMemberReward : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_RedeemMemberReward")]
        [TestCategory("API_SOAP_RedeemMemberReward_Positive")]
        [TestMethod]
        public void BTA234_SOAP_RedeemMemberReward()

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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Reward to Member using AddMemberRewards service";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
                Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeem the member reward  with RedeemMemberReward service";
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "");
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
                Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Members Rewards with GetMemberRewards service";
                MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
                    ";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID + 
                    ";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID+
                    "; and the redemption date is : "+ memberRewards[0].MemberRewardInfo[0].RedemptionDate);
                Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating memberRewardID and RedemptionDate with data from DB";
                List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID+"");
                MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
                foreach(MemberRewardOrderStruct k in memberRewards)
                {
                    if(k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
                    {
                        memberRewardInfoStruct = k.MemberRewardInfo[0];
                        break;
                    }
                }
                Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
                testStep.SetOutput("The Currency Balance:; " + dbresponse[0]+ " ;and the redemption date is : "+ dbresponse[1]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1286_SOAP_RedeemMemberReward_NegativeAvailableBalance()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward =  cdis_Service_Method.getReward();
				testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward with Negative Available Balance";
                decimal negValue = Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2));
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal("-"+negValue), System.DateTime.Today.AddYears(5), System.DateTime.Now.AddDays(-1), out time);
				testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID + " and the elapsed time is: " + time);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1287_SOAP_RedeemMemberReward_NullAvailableBalance()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
				testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward with Null Avaialable Balance";
                //cdis_Service_Method.RedeemMemberRewardWithNullAvailableBalance(memberRewardsOut.MemberRewardID + "", out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", null, System.DateTime.Today.AddYears(5), System.DateTime.Now.AddMilliseconds(100.00), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID + " and the elapsed time is: " + time);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1288_SOAP_RedeemMemberReward_PositiveAvailableBalance()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward with Positive Avaialable Balance";
				cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3)+"."+ common.RandomNumber(2)),System.DateTime.Today.AddYears(5), System.DateTime.Now.AddMilliseconds(100.00), out time);
				testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID );
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1289_SOAP_RedeemMemberReward_DecimalAvailableBalance()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward with Decimal Avaialable Balance";
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3)+"."+ common.RandomNumber(2)), System.DateTime.Today.AddYears(5), System.DateTime.Now.AddMilliseconds(100.00), out time);
               // cdis_Service_Method.RedeemMemberRewardWithDecimalAvailableBalance(memberRewardsOut.MemberRewardID + "", out time);
				testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID + " and the elapsed time is: " + time);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1293_SOAP_RedeemMemberReward_ExpirationDateIsCurrentDate()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 10, 100);
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in a ExpirationDate as the current date";
				cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3)+"."+ common.RandomNumber(2)), System.DateTime.Today, System.DateTime.Now.AddMilliseconds(100.00), out time);
				testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1294_SOAP_RedeemMemberReward_ExpirationDateIsInFuture()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,10, 100);
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in a ExpirationDate as the future date";
			//	cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddDays(7), System.DateTime.Now.AddDays(-1), out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddDays(7), System.DateTime.Now.AddMilliseconds(100.00), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1295_SOAP_RedeemMemberReward_ExpirationDateIsInPast()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 10, 100);
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in a ExpirationDate as the Past date";
                //cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddDays(-7), System.DateTime.Now.AddDays(-1), out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddDays(-7), System.DateTime.Now.AddMilliseconds(100.00), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1296_SOAP_RedeemMemberReward_RedemptionDateIsCurrentDate()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in Redemption Date as the Current date";
			//	cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddYears(5), System.DateTime.Now, out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddDays(7), System.DateTime.Now.AddMilliseconds(100.00), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1297_SOAP_RedeemMemberReward_RedemptionDateIsInFuture()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in Redemption Date as a Future date";
                //cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddYears(5), System.DateTime.Now.AddDays(7), out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddYears(5), System.DateTime.Today.AddDays(7), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1298_SOAP_RedeemMemberReward_RedemptionDateIsInPast_Positive()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in Redemption Date as a Past date";
                //	cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToInt32(common.RandomNumber(3)), System.DateTime.Today.AddYears(5), System.DateTime.Now.AddDays(-7), out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddYears(5), System.DateTime.Today.AddDays(-7), out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Positive")]
		[TestMethod]
		public void BTA1244_ST1299_SOAP_RedeemMemberReward_RedemptionDateAsNull_Positive()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing in Redemption Date as Null";
               // cdis_Service_Method.RedeemMemberRewardWithNullRedemptionDate(memberRewardsOut.MemberRewardID + "", out time);
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "", Convert.ToDecimal(common.RandomNumber(3) + "." + common.RandomNumber(2)), System.DateTime.Today.AddYears(5), null, out time);
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
				Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Members Rewards with GetMemberRewards service";
				MemberRewardOrderStruct[] memberRewards = cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The reward has been successfully added to the member and the reward details are:" +
					";Member RewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID +
					";Reward Definition ID is : " + memberRewards[0].MemberRewardInfo[0].RewardDefID +
					"; and the redemption date is : " + memberRewards[0].MemberRewardInfo[0].RedemptionDate);
				Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating memberRewardID and RedemptionDate with data from DB";
				List<string> dbresponse = DatabaseUtility.GetAvailableBalanceDfromDBUsingIdSOAP(memberRewardsOut.MemberRewardID + "");
				MemberRewardInfoStruct memberRewardInfoStruct = new MemberRewardInfoStruct();
				foreach (MemberRewardOrderStruct k in memberRewards)
				{
					if (k.MemberRewardInfo[0].MemberRewardID == memberRewardsOut.MemberRewardID)
					{
						memberRewardInfoStruct = k.MemberRewardInfo[0];
						break;
					}
				}
				Assert.AreEqual(memberRewardInfoStruct.AvailableBalance + "", dbresponse[0], "Expected Balance value  is " + memberRewardInfoStruct.AvailableBalance + " Actual Balance  Value is " + dbresponse[0]);
				testStep.SetOutput("The Available Balance:; " + dbresponse[0] + " ;the redemption date is : " + dbresponse[1] + " ;the expiration date is : " + dbresponse[2]);
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
		[TestCategory("API_SOAP_RedeemMemberReward")]
		[TestCategory("API_SOAP_RedeemMemberReward_Negative")]
		[TestMethod]
		public void BTA1244_ST1300_SOAP_RedeemMemberReward_InvalidRewardId_Negative()
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
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The RewardID from the response of GetRewardCatalog method is : " + reward.RewardID);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				IList<VirtualCard> vc = output.GetLoyaltyCards();
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Reward to Member using AddMemberRewards service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The membersRewardID is: " + memberRewardsOut.MemberRewardID);
				Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Redeem the member reward by passing Invalid RewardID";
				string error = (string)cdis_Service_Method.RedeemMemberRewardNegative();
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3347";
				Assert.AreEqual("3347", errorssplit[1], "Expected value is" + "3347" + "Actual value is" + errorssplit[1]);
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


