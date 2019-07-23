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
	public class BTA231_CDIS_AddMemberRewards : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;
        double elapsedTime;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
		public void BTA231_SOAP_AddMemberRewards_PositiveCase()
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
				stepName = "Get Recent Reward Catalog with CDIS service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
				RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
				foreach (RewardCatalogSummaryStruct r in rewardCatalog)
				{
					if (r.CurrencyToEarn == 0 && !r.RewardName.Contains("Welcome"))
					{
						reward = r;
						break;
					}
				}
				testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Members to Reward Catalog with CDIS service";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
				Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Verify the members rewardid in DB and fetch his membersrewardsID";
				string dbresponse = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
				Assert.AreEqual(memberRewardsOut.MemberRewardID + "", dbresponse, "Expected Value is " + memberRewardsOut.MemberRewardID.ToString() + " Actual Value is " + dbresponse);
				testStep.SetOutput("memberrewardid from the database is: " + dbresponse + " for a member with IPCode: " + output.IpCode + " and rewardID: " + reward.RewardID);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
        [TestMethod]
		public void BTA231_SOAP_AddMemberRewards_NegativeCase()
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
				stepName = "Get Recent Reward Catalog with CDIS service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(cdis_Service_Method.GetActiveRewardCatalogCount() - 5, 0 ,100);
				testStep.SetOutput("RewardID:" + rewardCatalog[0].RewardID);
				Logger.Info("RewardID:" + rewardCatalog[0].RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add  Members to Reward Catalog with CDIS service";
				var memberRewardsOut = cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, rewardCatalog[0]);
				testStep.SetOutput("Reward not added to member due to : " + memberRewardsOut.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);
			}
			catch (Exception e)
			{
				testStep.SetOutput(e.Message);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(false);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
		[TestCategory("AddMemberRewards_Positive")]
		[TestMethod]
		public void BTA1033_ST1143_SOAP_AddMemberRewards_To_An_ActiveMember()
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
				stepName = "Validate Memberstatus in DB";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add reward to a member using AddMemberRewardsMethod";
				AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("The reward with RewardID:" + reward.RewardID + " has been added to the member with IPCODE: " + output.IpCode);
				Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Verify the members rewardid in DB and verify it with membersrewardsID in response";
				string dbresponse1 = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
				Assert.AreEqual(memberRewardsOut.MemberRewardID + "", dbresponse1, "Expected Value is " + memberRewardsOut.MemberRewardID.ToString() + " Actual Value is " + dbresponse1);
				testStep.SetOutput("memberrewardid from the database is: " + dbresponse1 + " for a member with IPCode: " + output.IpCode + " and memberrewardid from the reponse is: " + memberRewardsOut.MemberRewardID);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
		public void BTA1033_ST1144_SOAP_AddMemberRewards_To_A_PreEnrolledMember()
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


				Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Adding member with CDIS service";
				Member output = cdis_Service_Method.GetCDISMemberPreEnrolled();
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
				Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validate Memberstatus in DB";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add reward to a member using AddMemberRewardsMethod";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + reward.RewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("RewardID:" + memberRewardsOut.MemberRewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the members rewardid in DB and verify it with membersrewardsID in response";
                string dbresponse1 = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                Assert.AreEqual(memberRewardsOut.MemberRewardID + "", dbresponse1, "Expected Value is " + memberRewardsOut.MemberRewardID.ToString() + " Actual Value is " + dbresponse1);
                testStep.SetOutput("memberrewardid from the database is: " + dbresponse1 + " for a member with IPCode: " + output.IpCode + " and memberrewardid from the reponse is: " + memberRewardsOut.MemberRewardID);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1146_SOAP_AddMemberRewards_To_A_LockedMember()
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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Lock down the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
				//cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message_Locked = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message_Locked, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                testStep.SetOutput("Member is Locked");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the card status for lock down member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Locked.ToString(), value, "Expected value is" + Member_Status.Locked.ToString() + "Actual value is" + value);
				testStep.SetOutput("The card status from database for lockdown is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "AddMemberRewardsfor a LOCKEDDOWN member";
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection as expected and the error is: " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3314";
				Assert.AreEqual("3314", errorssplit[1], "Expected value is" + "3314" + "Actual value is" + errorssplit[1]);
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

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1150_SOAP_AddMemberRewards_To_A_TerminatedMember()
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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Terminate the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
				//cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber);
                string actualMessage = cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", String.Empty, out elapsedTime);
                Assert.AreEqual("pass", actualMessage, "Member with loyality id number : " + vc[0].LoyaltyIdNumber + " is not terminated");
                testStep.SetOutput("Member is Terminated");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the card status for terminated member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Terminated.ToString(), value, "Expected value is" + Member_Status.Terminated.ToString() + "Actual value is" + value);
				testStep.SetOutput("The card status from database for terminate is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
				testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "AddMemberRewards to a disabled member and verify the errorcode";
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3314";
				Assert.AreEqual("3314", errorssplit[1], "Expected value is" + "3314" + "Actual value is" + errorssplit[1]);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1152_SOAP_AddMemberRewards_To_A_DisabledMember()
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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Deactivating the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
				cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("Member is Deactivated");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the card status for disabled member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
				testStep.SetOutput("The card status from database for disabled is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "AddMemberRewards to a disabled member and verify the errorcode";
				string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3314";
				Assert.AreEqual("3314", errorssplit[1], "Expected value is" + "3314" + "Actual value is" + errorssplit[1]);
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
		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1153_SOAP_AddMemberRewardsTo_A_NonMember()
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

				Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Adding non member with CDIS service";
				Member output = cdis_Service_Method.GetCDISMemberNonMember();
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
				Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validate Memberstatus in DB as Non Member";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();

                testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "AddMemberRewards to a non member";
				string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 9969";
				Assert.AreEqual("9969", errorssplit[1], "Expected value is" + "9969" + "Actual value is" + errorssplit[1]);
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

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1154_SOAP_AddMemberRewards_AddNULLReward()
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
				stepName = "AddMemberReward Catalog where Reward Order Info is null";
                var reward = new RewardCatalogSummaryStruct();
                reward = null;
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 2003";
				Assert.AreEqual("2003", errorssplit[1], "Expected value is" + "2003" + "Actual value is" + errorssplit[1]);
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

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Positive")]
		[TestMethod]
		public void BTA1033_ST1155_SOAP_AddMemberRewards_AddReward_WithTypecode()
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
				stepName = "Validate Memberstatus in DB";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getRewardWithTypeCode();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add reward to a member using AddMemberRewardsMethod";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + reward.RewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the members rewardid in DB and verify it with membersrewardsID in response";
                string dbresponse1 = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                Assert.AreEqual(memberRewardsOut.MemberRewardID + "", dbresponse1, "Expected Value is " + memberRewardsOut.MemberRewardID.ToString() + " Actual Value is " + dbresponse1);
                testStep.SetOutput("memberrewardid from the database is: " + dbresponse1 + " for a member with IPCode: " + output.IpCode + " and memberrewardid from the reponse is: " + memberRewardsOut.MemberRewardID);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1159_SOAP_AddMemberRewards_NonExisting_CardID()
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
                stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Member to Reward Catalog with CDIS service where the Card Id is non existing";
				string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, common.RandomNumber(12), reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3306";
				Assert.AreEqual("3306", errorssplit[1], "Expected value is" + "3306" + "Actual value is" + errorssplit[1]);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
		public void BTA1033_ST1167_SOAP_AddMemberRewards_Verify_ElapsedTime()

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

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validate Memberstatus in DB";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service";
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add reward to a member using AddMemberRewardsMethod";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + reward.RewardID + " has been added to the member with IPCODE: " + output.IpCode);
                Logger.Info("member RewardID:" + memberRewardsOut.MemberRewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify the members rewardid in DB and verify it with membersrewardsID in response";
                string dbresponse1 = DatabaseUtility.GetMemberRewardIDfromDBUsingIdSOAP(output.IpCode + "", reward.RewardID + "");
                Assert.AreEqual(memberRewardsOut.MemberRewardID + "", dbresponse1, "Expected Value is " + memberRewardsOut.MemberRewardID.ToString() + " Actual Value is " + dbresponse1);
                testStep.SetOutput("memberrewardid from the database is: " + dbresponse1 + " for a member with IPCode: " + output.IpCode + " and memberrewardid from the reponse is: " + memberRewardsOut.MemberRewardID);
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
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1168_SOAP_AddMemberRewards_NonExisting_RewardName()
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
                stepName = "Get Recent Reward Catalog with CDIS service and add nonexisting reward Name";
                //Sending Non Existing RewardName
                var reward = new RewardCatalogSummaryStruct();
                reward.RewardName = common.RandomString(20);
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
						
				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Member to Reward Catalog with CDIS service where the Reward name is non existing";
              // string error = (string)cdis_Service_Method.AddMemberRewardWhereRewardOrderInfoIsNull(vc[0].LoyaltyIdNumber, reward);
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3342";
				Assert.AreEqual("3342", errorssplit[1], "Expected value is" + "3342" + "Actual value is" + errorssplit[1]);
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

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1169_SOAP_AddMemberRewards_Blank_RewardName()
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
                stepName = "Get Recent Reward Catalog with CDIS service and add blank reward Name";
                //Sending blank RewardName
                var reward = new RewardCatalogSummaryStruct();
                reward.RewardName = "";
                // string error = (string)cdis_Service_Method.AddMemberRewardswithBlankRewardName(vc[0].LoyaltyIdNumber, reward);
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3340";
				Assert.AreEqual("3340", errorssplit[1], "Expected value is" + "3340" + "Actual value is" + errorssplit[1]);
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


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
        [TestMethod]
        public void BTA1033_ST1283_SOAP_AddMemberRewards_Blank_MemberIdentity()
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
                stepName = "Get Recent Reward Catalog with CDIS service and send blank memberidentity";
                var reward = cdis_Service_Method.getReward();
                string error = (string)cdis_Service_Method.AddMemberRewards("", vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Throws an expection with the " + error);
                string[] errors = error.Split(';');
                string[] errorssplit = errors[0].Split('=');
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 2003";
                Assert.AreEqual("2003", errorssplit[1], "Expected value is" + "2003" + "Actual value is" + errorssplit[1]);
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


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
        [TestMethod]
        public void BTA1033_ST1285_SOAP_AddMemberRewards_Invalid_CardStatus()
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
                stepName = "Cancel the card";
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, out elapsedTime);
                testStep.SetOutput("The card has been cancelled: "+ vc[0].LoyaltyIdNumber);
                Logger.Info("The card has been cancelled: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service and send blank memberidentity";
                var reward = cdis_Service_Method.getReward();
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Throws an expection with the " + error);
                string[] errors = error.Split(';');
                string[] errorssplit = errors[0].Split('=');
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 3307";
                Assert.AreEqual("3307", errorssplit[1], "Expected value is" + "3307" + "Actual value is" + errorssplit[1]);
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



        [TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1170_SOAP_AddMemberRewards_Blank_RewardOrderInfo()
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
				//RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();

				testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service and add blank reward order info";
                //Sending blank RewardName
                var reward = new RewardCatalogSummaryStruct();
                reward.RewardName = "blank";
                //string error = (string)cdis_Service_Method.AddMemberRewardswithBlankRewardInfo(vc[0].LoyaltyIdNumber, null);
                string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3340";
				Assert.AreEqual("3340", errorssplit[1], "Expected value is" + "3340" + "Actual value is" + errorssplit[1]);
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

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Negative")]
		[TestMethod]
		public void BTA1033_ST1171_SOAP_AddMemberRewards_NotHavingEnoughPoints()
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
				stepName = "Validate Memberstatus in DB";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
				Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Get Recent Reward Catalog with CDIS service";
				RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 10, 100);
				RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
				foreach (RewardCatalogSummaryStruct r in rewardCatalog)
				{
					if (r.CurrencyToEarn != 0)
					{
						reward = r;
						break;
					}
				}
				testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
				Logger.Info("RewardID:" + reward.RewardID);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Add Member to Reward Catalog with CDIS service where points are not enough";
				string error = (string)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
				testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3354";
				Assert.AreEqual("3354", errorssplit[1], "Expected value is" + "3354" + "Actual value is" + errorssplit[1]);
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
