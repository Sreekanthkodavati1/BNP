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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.API.CDIS_Services.CDIS_TestCases
{
    [TestClass]
    public class BTA1242_CDIS_GetAllRewardOrderShippingAddressesMemberRewards : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedTime = 0;

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_AddMemberRewards")]
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
        public void BTA1242_ST1561_SOAP_GetAllRewardOrderShippingAddresses_ActiveMember()
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
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members Rewards with CDIS service";
                Hashtable addressData = cdis_Service_Method.SetRewardOrderShippingAddress();
                List<Object> addressList = new List<Object>();
                ICollection key = addressData.Keys;
                foreach (string k in key)
                {
                    addressList.Add(addressData[k]);

                }
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber,
                "testFirst", "testLast", "testemail@gmail.com",
               (string)addressData["addresslineone"], (string)addressData["addresslinetwo"], (string)addressData["addresslinethree"], (string)addressData["addresslinefour"], (string)addressData["city"], (string)addressData["state"], (string)addressData["ziporpostalcode"], (string)addressData["county"], (string)addressData["country"], "Web", "SOAP", reward);
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

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get All the Rewards Orders shipping addresses";
                ShippingAddressStruct[] address = cdis_Service_Method.GetAllRewardOrderShippingAddresses(vc[0].LoyaltyIdNumber);
                List<string> addressFromResponse = new List<string>();
                addressFromResponse.Add(address[0].AddressLineOne);
                addressFromResponse.Add(address[0].AddressLineTwo);
                addressFromResponse.Add(address[0].AddressLineThree);
                addressFromResponse.Add(address[0].AddressLineFour);
                addressFromResponse.Add(address[0].City);
                addressFromResponse.Add(address[0].StateOrProvince);
                addressFromResponse.Add(address[0].ZipOrPostalCode);
                addressFromResponse.Add(address[0].County);
                addressFromResponse.Add(address[0].Country);
                IEnumerable<object> finalData = addressList.Except(addressFromResponse);
                if (!finalData.Any())
                {
                    testStep.SetOutput("Member's reward order shipping addresses is returned successfully");
                }
                else
                {
                    throw new Exception("Member's reward order shipping addresses is different from the actual data");
                }
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
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
        public void BTA1242_ST1562_SOAP_GetAllRewardOrderShippingAddresses_ActiveMemberWithNoAddress()
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
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0, 0 ,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
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
                stepName = "Get All the Rewards Orders shipping addresses";
                ShippingAddressStruct[] address = cdis_Service_Method.GetAllRewardOrderShippingAddresses(vc[0].LoyaltyIdNumber);

                if (address.Length == 0)
                {
                    testStep.SetOutput("Member's reward order shipping addresses is returned successfully with empty address");
                }
                else
                {
                    throw new Exception("Member's reward order shipping addresses is not returned from the response");
                }
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
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
        public void BTA1242_ST1563_SOAP_GetAllRewardOrderShippingAddresses_DisabledMember()
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
                var reward = cdis_Service_Method.getReward();
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);
                Logger.Info("RewardID:" + reward.RewardID);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members Rewards with CDIS service";
                Hashtable addressData = cdis_Service_Method.SetRewardOrderShippingAddress();
                List<Object> addressList = new List<Object>();
                ICollection key = addressData.Keys;
                foreach (string k in key)
                {
                    addressList.Add(addressData[k]);

                }
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber,
                "testFirst", "testLast", "testemail@gmail.com",
               (string)addressData["addresslineone"], (string)addressData["addresslinetwo"], (string)addressData["addresslinethree"], (string)addressData["addresslinefour"], (string)addressData["city"], (string)addressData["state"], (string)addressData["ziporpostalcode"], (string)addressData["county"], (string)addressData["country"], "Web", "SOAP", reward);

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

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member detail's: IPCODE: " + vc[0].IpCode + " and member status is:" + dbMemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the Member status for Deactivated member from database";
                dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
                testStep.SetOutput("The card status from database for deactivated member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get All the Rewards Orders shipping addresses for Disabled ember";
                ShippingAddressStruct[] address = cdis_Service_Method.GetAllRewardOrderShippingAddresses(vc[0].LoyaltyIdNumber);
                List<string> addressFromResponse = new List<string>();

                addressFromResponse.Add(address[0].AddressLineOne);
                addressFromResponse.Add(address[0].AddressLineTwo);
                addressFromResponse.Add(address[0].AddressLineThree);
                addressFromResponse.Add(address[0].AddressLineFour);
                addressFromResponse.Add(address[0].City);
                addressFromResponse.Add(address[0].StateOrProvince);
                addressFromResponse.Add(address[0].ZipOrPostalCode);
                addressFromResponse.Add(address[0].County);
                addressFromResponse.Add(address[0].Country);
                addressFromResponse.Add(address[0].County);
                IEnumerable<object> finalData = addressList.Except(addressFromResponse);
                if (!finalData.Any())
                {
                    testStep.SetOutput("Member's reward order shipping addresses is returned successfully");
                }
                else
                {
                    throw new Exception("Member's reward order shipping addresses is different from the actual data");
                }
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
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
        public void BTA1242_ST1564_SOAP_GetAllRewardOrderShippingAddresses_LockedMember()
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
                stepName = "Add Members Rewards with CDIS service";
                Hashtable addressData = cdis_Service_Method.SetRewardOrderShippingAddress();
                List<Object> addressList = new List<Object>();
                ICollection key = addressData.Keys;
                foreach (string k in key)
                {
                    addressList.Add(addressData[k]);

                }
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber,
                "testFirst", "testLast", "testemail@gmail.com",
               (string)addressData["addresslineone"], (string)addressData["addresslinetwo"], (string)addressData["addresslinethree"], (string)addressData["addresslinefour"], (string)addressData["city"], (string)addressData["state"], (string)addressData["ziporpostalcode"], (string)addressData["county"], (string)addressData["country"], "Web", "SOAP", reward);

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

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Lock down the member";
                //cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                testStep.SetOutput("Member is Locked");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the card status for lock down member from database";
                dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                String value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Locked.ToString(), value, "Expected value is" + Member_Status.Locked.ToString() + "Actual value is" + value);
                testStep.SetOutput("The card status from database for lockdown is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get All the Rewards Orders shipping addresses for Locked member";
                ShippingAddressStruct[] address = cdis_Service_Method.GetAllRewardOrderShippingAddresses(vc[0].LoyaltyIdNumber);
                List<string> addressFromResponse = new List<string>();

                addressFromResponse.Add(address[0].AddressLineOne);
                addressFromResponse.Add(address[0].AddressLineTwo);
                addressFromResponse.Add(address[0].AddressLineThree);
                addressFromResponse.Add(address[0].AddressLineFour);
                addressFromResponse.Add(address[0].City);
                addressFromResponse.Add(address[0].StateOrProvince);
                addressFromResponse.Add(address[0].ZipOrPostalCode);
                addressFromResponse.Add(address[0].County);
                addressFromResponse.Add(address[0].Country);
                addressFromResponse.Add(address[0].County);
                IEnumerable<object> finalData = addressList.Except(addressFromResponse);
                if (!finalData.Any())
                {
                    testStep.SetOutput("Member's reward order shipping addresses is returned successfully");
                }
                else
                {
                    throw new Exception("Member's reward order shipping addresses is different from the actual data");
                }
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
        [TestCategory("AddMemberRewards_Positive")]
        [TestMethod]
        public void BTA1242_ST1565_SOAP_GetAllRewardOrderShippingAddresses_TerminatedMember()
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
                stepName = "Add Members Rewards with CDIS service";
                Hashtable addressData = cdis_Service_Method.SetRewardOrderShippingAddress();
                List<Object> addressList = new List<Object>();
                ICollection key = addressData.Keys;
                foreach (string k in key)
                {
                    addressList.Add(addressData[k]);

                }
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber,
                "testFirst", "testLast", "testemail@gmail.com",
               (string)addressData["addresslineone"], (string)addressData["addresslinetwo"], (string)addressData["addresslinethree"], (string)addressData["addresslinefour"], (string)addressData["city"], (string)addressData["state"], (string)addressData["ziporpostalcode"], (string)addressData["county"], (string)addressData["country"], "Web", "SOAP", reward);

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

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Terminating the member";
                //cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber);
                string actualMessage = cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", String.Empty, out elapsedTime);
                Assert.AreEqual("pass", actualMessage, "Member with loyality id number : "+ vc[0].LoyaltyIdNumber+" is not terminated");
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member detail's: IPCODE: " + vc[0].IpCode + " and member status is:" + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the Member status for Terminated member from database";
                dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Terminated.ToString(), value, "Expected value is" + Member_Status.Terminated.ToString() + "Actual value is" + value);
                testStep.SetOutput("The card status from database for terminated member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get All the Rewards Orders shipping addresses for Terminated member";
                ShippingAddressStruct[] address = cdis_Service_Method.GetAllRewardOrderShippingAddresses(vc[0].LoyaltyIdNumber);
                List<string> addressFromResponse = new List<string>();

                addressFromResponse.Add(address[0].AddressLineOne);
                addressFromResponse.Add(address[0].AddressLineTwo);
                addressFromResponse.Add(address[0].AddressLineThree);
                addressFromResponse.Add(address[0].AddressLineFour);
                addressFromResponse.Add(address[0].City);
                addressFromResponse.Add(address[0].StateOrProvince);
                addressFromResponse.Add(address[0].ZipOrPostalCode);
                addressFromResponse.Add(address[0].County);
                addressFromResponse.Add(address[0].Country);
                addressFromResponse.Add(address[0].County);
                IEnumerable<object> finalData = addressList.Except(addressFromResponse);
                if (!finalData.Any())
                {
                    testStep.SetOutput("Member's reward order shipping addresses is returned successfully");
                }
                else
                {
                    throw new Exception("Member's reward order shipping addresses is different from the actual data");
                }
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
        public void BTA1242_ST1560_SOAP_GetAllRewardOrderShippingAddresses_MemberIdentityIsString()
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
                stepName = "Get All the Rewards Orders shipping addresses with memberIdentiy as String and validate the Error";
                string memberIdentity = "test";
                string errorMsg = cdis_Service_Method.GetAllRewardOrderShippingAddresses_Invalid(memberIdentity);
                if (errorMsg.Contains("Error code=3302") && errorMsg.Contains("Error Message=Unable to find member with identity"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + errorMsg);
                    Logger.Info("The Error message from Service is received as expected. " + errorMsg);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3302. Actual error received is" + errorMsg);
                }
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
