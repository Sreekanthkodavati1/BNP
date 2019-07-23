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
    public class BTA232_CDIS_GetMemberRewards : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        public void BTA232_CDIS_GetMemberRewards_PositiveCase()

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
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("The following reward is fetched from GetRewardCatalog method; RewardID: " + reward.RewardID + ";Rewardname: " + reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Reward to the above Member";
                AddMemberRewardsOut memberRewardsOut =(AddMemberRewardsOut) cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Reward added sucessfully and the; membersRewardID : " + memberRewardsOut.MemberRewardID);
                Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Members Rewards with CDIS service";
                MemberRewardOrderStruct[] memberRewards =cdis_Service_Method.GetMemberRewards(vc[0].LoyaltyIdNumber);
                Assert.AreEqual(output.FirstName, memberRewards[0].FirstName, "Expected Value is " + output.FirstName + " Actual Value is " + memberRewards[0].FirstName);
                Assert.AreEqual(memberRewardsOut.MemberRewardID, memberRewards[0].MemberRewardInfo[0].MemberRewardID, "Expected Value is " + memberRewardsOut.MemberRewardID + " Actual Value is " + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
                Assert.AreEqual(reward.RewardID, memberRewards[0].MemberRewardInfo[0].RewardDefID, "Expected Value is " + reward.RewardID + " Actual Value is " + memberRewards[0].MemberRewardInfo[0].RewardDefID);
                testStep.SetOutput("The member (IPCODE: "+ output.IpCode +", FirstName: "+ output.FirstName+ ") rewards details are: " +
                    "; MemberRewardID: " + memberRewards[0].MemberRewardInfo[0].MemberRewardID+
                    "; and the Reward Definition ID is : "+ memberRewards[0].MemberRewardInfo[0].RewardDefID);
                Logger.Info("Member RewardID:" + memberRewards[0].MemberRewardInfo[0].MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate \"MemberRewardID\" for the member in DB";
                string dbresponse = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                testStep.SetOutput("MemberRewardID from database : " + dbresponse);
                Assert.AreEqual(memberRewards[0].MemberRewardInfo[0].MemberRewardID + "", dbresponse, "Expected Value is " + memberRewards[0].MemberRewardInfo[0].MemberRewardID + " Actual Value is " + dbresponse);
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
