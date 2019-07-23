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
    public class BTA233_GetMemberRewardsSummary : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA233_CDIS_GetMemberRewardsSummary_PositiveCase()

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

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get \"Reward Definition\" from GetRewardCatalog method";
                //RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                //RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                //foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                //{
                //    if (r.CurrencyToEarn == 0)
                //    {
                //        reward = r;
                //        break;
                //    }
                //}
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The following reward is fetched from GetRewardCatalog method; RewardID: " + reward.RewardID + ";Rewardname: "+ reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Reward to the above Member";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut) cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Reward added sucessfully and the; membersRewardID : " + memberRewardsOut.MemberRewardID);
                Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify MemberRewardID, RewardDefID and RewardName from response of the GetMemberRewardsSummary method matches with the reward added through AddMemberreward method";
                MemberRewardSummaryStruct[] memberRewardSummaryStruct= cdis_Service_Method.GetMemberRewardsSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(memberRewardSummaryStruct[0].MemberRewardID, memberRewardsOut.MemberRewardID, "Expected Value is " + memberRewardSummaryStruct[0].MemberRewardID + " Actual Value is " + memberRewardsOut.MemberRewardID);
                Assert.AreEqual(memberRewardSummaryStruct[0].RewardDefID, reward.RewardID, "Expected Value is " + memberRewardSummaryStruct[0].RewardDefID + " Actual Value is " + reward.RewardID);
                Assert.AreEqual(memberRewardSummaryStruct[0].RewardName, reward.RewardName, "Expected Value is " + memberRewardSummaryStruct[0].RewardName + " Actual Value is " + reward.RewardName);
                testStep.SetOutput("GetMemberRewardsSumamry's  MemberRewardID: " + memberRewardSummaryStruct[0].MemberRewardID+ " and the AddMemberReward's MemberRewardsID  : " + memberRewardsOut.MemberRewardID+ " are matching");
                Logger.Info("Member RewardID:" +memberRewardSummaryStruct[0].MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate membersrewardID in lw_memberrewards table";
                string dbresponse = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                Assert.AreEqual(memberRewardSummaryStruct[0].MemberRewardID + "", dbresponse, "Expected Value is " + memberRewardSummaryStruct[0].MemberRewardID + " Actual Value is " + dbresponse);
                testStep.SetOutput("Response from database [MemberRewardID]: " + dbresponse);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Reward Catalog Item with CDIS service";
                RewardCatalogItemStruct rewardCatalogItem = cdis_Service_Method.GetRewardCatalogItem(reward.RewardID);
                testStep.SetOutput("The reward details from RewardCatalogItem for the Reward with ID "+ reward.RewardID+ 
                                   " are: ; Reward Name: " + rewardCatalogItem.RewardName+
                                   "; Reward ID: " + rewardCatalogItem.RewardID);
                Logger.Info("Reward ID: " + rewardCatalogItem.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate reward details of RewardCatalogItem response with GetMemberRewardsSumamry method details";
                Assert.AreEqual(memberRewardSummaryStruct[0].RewardDefID + "", rewardCatalogItem.RewardID + "", "Expected Value is " + memberRewardSummaryStruct[0].RewardDefID + " Actual Value is " + rewardCatalogItem.RewardID);
                Assert.AreEqual(memberRewardSummaryStruct[0].RewardName + "", rewardCatalogItem.RewardName, "Expected Value is " + memberRewardSummaryStruct[0].RewardName + " Actual Value is " + rewardCatalogItem.RewardName);
                testStep.SetOutput("The Reward details from GetRewardCatalogItem are " +
                                    "; RewardID: " + rewardCatalogItem.RewardID +
                                    ";RewardName: "+ rewardCatalogItem.RewardName +
                    "; and Reward details from GetMemberRewardsSumamry are: "+
                    "; RewardID : " + memberRewardSummaryStruct[0].RewardDefID +
                    "; RewardName: " + memberRewardSummaryStruct[0].RewardName);
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

   
