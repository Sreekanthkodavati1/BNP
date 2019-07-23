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
    public class BTA235_CDIS_IsMemberRewardRedeemed : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed_Positive")]
        [TestMethod]
        public void BTA235_SOAP_IsMemberRewardRedeemed()

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
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
                    {
                        reward = r;
                        break;
                    }
                }
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
                stepName = "Verify whether member reward is redeemed using IsMemberRewardRedeemed method";
                bool Value=cdis_Service_Method.IsMemberRewardRedeemed(memberRewardsOut.MemberRewardID+"", out time);
                testStep.SetOutput("The response from the IsMemberRewardRedeemed method is [" + Value + "] which means memberRewardID : "+ memberRewardsOut.MemberRewardID + " has not been redeemed");
                Logger.Info("Member Reward Redeemed: " + Value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the redeemed status";
                Assert.AreEqual("False", Value.ToString(), "Expected Value is :False and Actual Value is : " + Value);
                testStep.SetOutput("IsMemberRewardRedeemed method returns : [" + Value + "]");
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
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed_Positive")]
        [TestMethod]
        public void BTA235_SOAP_IsMemberRewardRedeemedAfterRedemption()

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
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName); ;
                Logger.Info("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with GetRewardCatalog service";
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
                    {
                        reward = r;
                        break;
                    }

                }
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
                stepName = "Redeem the memberreward  with RedeemMemberReward service";
                cdis_Service_Method.RedeemMemberReward(memberRewardsOut.MemberRewardID + "");
                testStep.SetOutput("The Members Reward is Redeemed and its corresponding memberRewardId is : " + memberRewardsOut.MemberRewardID);
                Logger.Info("Reward is Redeemed for reward id" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify whether member reward is redeemed using IsMemberRewardRedeemed method";
                bool Value = cdis_Service_Method.IsMemberRewardRedeemed(memberRewardsOut.MemberRewardID + "", out time);
                testStep.SetOutput("The response from the IsMemberRewardRedeemed method is [" + Value + "] which means memberRewardID : " + memberRewardsOut.MemberRewardID + " has been redeemed");
                Logger.Info("Member Reward Redeemed: " + Value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the redeemed status";
                Assert.AreEqual("True", Value.ToString(), "Expected Value is :True and Actual Value is : " + Value);
                testStep.SetOutput("IsMemberRewardRedeemed method returns : [" + Value + "]");
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
        [TestCategory("API_SOAP_IsMemberRewardRedeemed")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed_Positive")]
        [TestMethod]
        public void BTA1243_ST1353_SOAP_IsMemberRewardRedeemed_MemberRewardRedeemedForExistingMemberRewardId()
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
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
                    {
                        reward = r;
                        break;
                    }
                }
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
                stepName = "Verify whether member reward is redeemed along with the elapsed time";
                bool Value=cdis_Service_Method.IsMemberRewardRedeemed(memberRewardsOut.MemberRewardID+"", out time);
                testStep.SetOutput("The response from the IsMemberRewardRedeemed method is [" + Value + "] which means memberRewardID : "+ memberRewardsOut.MemberRewardID + " has not been redeemed and the elapsed time is : " + time);
                Logger.Info("Member Reward Redeemed: " + Value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate the redeemed status";
                Assert.AreEqual("False", Value.ToString(), "Expected Value is :False and Actual Value is : " + Value);
                testStep.SetOutput("IsMemberRewardRedeemed method returns : [" + Value + "]");
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
        [TestCategory("API_SOAP_IsMemberRewardRedeemed")]
        [TestCategory("API_SOAP_IsMemberRewardRedeemed_Negative")]
        [TestMethod]
        public void BTA1243_ST1354_SOAP_IsMemberRewardRedeemed_PassInvalidRewardId_Negative()
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
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
                    {
                        reward = r;
                        break;
                    }
                }
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
                stepName = "Verify whether the member reward is redeemed or not by passing Invalid RewardID";
                string error = (string)cdis_Service_Method.IsMemberRewardRedeemedIDNegative();
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

