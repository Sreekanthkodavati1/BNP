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
    public class BTA241_CDIS_CancelCard : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        double elapsedtime;

        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_CancelCard_Positive")]
        [TestMethod]
        public void BTA241_CDIS_CancelCard_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            GetAccountSummaryOut pointBalanceBeforeCancelCard, pointBalanceAfterCancelCard;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber + " and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method";
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber,out elapsedtime);
                testStep.SetOutput("The following card has Canceled :"+vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber + "");
                pointBalanceAfterCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("0", pointBalanceAfterCancelCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + " and Points balance after" +
                    "card cancellation is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Positive")]
        [TestMethod]
        public void BTA1465_ST1672_SOAP_CancelCard_DeactivateMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            GetAccountSummaryOut pointBalanceBeforeCancelCard, pointBalanceAfterCancelCard;
            double elapsedtime;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber + " and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Cancel the card and deactivate member";
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, null, "CDIS Service", true, out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUS", string.Empty);
                pointBalanceAfterCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("0", pointBalanceAfterCancelCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + " and Points balance after" +
                    "card cancellation is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the member is deactivated";
                dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
                testStep.SetOutput("The card status from database for lock down is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Positive")]
        [TestMethod]
        public void BTA1465_ST1673_SOAP_CancelCard_NullStatus()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            GetAccountSummaryOut pointBalanceBeforeCancelCard, pointBalanceAfterCancelCard;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber + " and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method with update status reason as null";
                string updatecardstatusreason = null;
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, null, updatecardstatusreason, false, out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber + "");
                pointBalanceAfterCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("0", pointBalanceAfterCancelCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + " and Points balance after" +
                    "card cancellation is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Positive")]
        [TestMethod]
        public void BTA1465_ST1790_SOAP_CancelCard_VerifyElapsedTime()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method";
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, null, "SOAP_AUTOMATION", false, out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber + "");
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verify elapsed time";
                Assert.AreEqual(elapsedtime > 0, true, "Expected Elapsed time is greater than 0 and actual Elapsed time is : " + elapsedtime);
                testStep.SetOutput("The elapsed time for CancelCard : " + elapsedtime);
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


        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Positive")]
        [TestMethod]
        public void BTA1465_ST1791_SOAP_CancelCard_WithActiveTranscations()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding points to members by posting transactions using UpdateMember method";
                DateTime date = DateTime.Now.AddMilliseconds(2000);
                var txnHeader = cdis_Service_Method.UpdateMember_PostTransactionRequiredDate(output, date);
                var pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The transaction header of the ID is  " + txnHeader.TxnHeaderId +
                    "and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method";
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, null, "SOAP_AUTOMATION", false, out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber + "");
                var pointBalanceAfterCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("0", pointBalanceAfterCancelCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + " and Points balance after" +
                    "card cancellation is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Negative")]
        [TestMethod]
        public void BTA1465_ST1789_SOAP_CancelCard_InactiveCard_And_ActiveMember()
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
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating card for the member";
                //cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message, "Card : "+vc[0].LoyaltyIdNumber+ " has not been deactivated");
                testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the inactive card status from database";
                string inactiveCardStatus = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                string activeMemberStatus = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value1 = (Member_Status)Int32.Parse(activeMemberStatus) + "";
                string value = (LoyaltyCard_Status)Int32.Parse(inactiveCardStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                Assert.AreEqual(Member_Status.Active.ToString(), value1, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + value1);
                testStep.SetOutput("LoyaltyCardId Status from DB is : \"" + inactiveCardStatus + "\" which means the card is : " + (LoyaltyCard_Status)Int32.Parse(inactiveCardStatus) +
                    "MemberStatus from DB: " + activeMemberStatus + "   Member Status: " + (Member_Status)Int32.Parse(activeMemberStatus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Cancel card for an Inactive member and validate the Error:3307";
                string error = cdis_Service_Method.CancelCard_Invalid(vc[0].LoyaltyIdNumber, null, null, false);
                if (error.Contains(" Error code=3307") && error.Contains("Error Message=Invalid card status"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3307. Actual error received is" + error);
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
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Negative")]
        [TestMethod]
        public void BTA1465_ST1674_SOAP_CancelCard_CanceledCard()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            GetAccountSummaryOut pointBalanceBeforeCancelCard;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber + " and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method";
                string updatecardstatusreason = null;
                cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber, null, updatecardstatusreason, false, out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card which is already Canceled and validate the Error:3307";
                string error = cdis_Service_Method.CancelCard_Invalid(vc[0].LoyaltyIdNumber, null, null, false);
                if (error.Contains(" Error code=3307") && error.Contains("Error Message=Invalid card status"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3307. Actual error received is" + error);
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
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Negative")]
        [TestMethod]
        public void BTA1465_ST1675_SOAP_CancelCard_NonExistingCard()
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
                stepName = "Cancel a non existing card and validate the Error:3305";
                string invalidCard = common.RandomNumber(7);
                string error = cdis_Service_Method.CancelCard_Invalid(invalidCard, null, null, false);
                if (error.Contains(" Error code=3305") && error.Contains("Error Message=Unable to find member with card id "))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3305. Actual error received is" + error);
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
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Negative")]
        [TestMethod]
        public void BTA1465_ST1676_SOAP_CancelCard_InactiveMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            String stepName = "";
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            GetAccountSummaryOut pointBalanceBeforeCancelCard;

            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                pointBalanceBeforeCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("The member details are IpCode: " + output.IpCode + " , Name: " + output.FirstName + ", LoyaltyCardID is: "
                    + vc[0].LoyaltyIdNumber + " and Currency Balance is : " + pointBalanceBeforeCancelCard.CurrencyBalance);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member";
                vc = output.GetLoyaltyCards();
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member is Deactivated");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Cancel card for an Inactive member and validate the Error:3314";
                string error = cdis_Service_Method.CancelCard_Invalid(vc[0].LoyaltyIdNumber, null, null, false);
                if (error.Contains(" Error code=3314") && error.Contains("Error Message=Member is not active"))
                {
                    testStep.SetOutput("The Error message from Service is received as expected. " + error);
                    Logger.Info("The Error message from Service is received as expected. " + error);
                }
                else
                {
                    throw new Exception("Error not received as expected error: 3314. Actual error received is" + error);
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
        [TestCategory("API_SOAP_CancelCard")]
        [TestCategory("API_SOAP_CancelCard_Positive")]
        [TestMethod]
        public void BTA1465_ST1841_SOAP_CancelCard_ActiveCard()
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
                stepName = "Getting an Active card from database";
                string loyaltyIdnumber = DatabaseUtility.GetActiveCouponFromDB();
                testStep.SetOutput("Active card from database is :"+ loyaltyIdnumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Canceling Card through CancelCard method";
                cdis_Service_Method.CancelCard(loyaltyIdnumber,out elapsedtime);
                testStep.SetOutput("The following card has Canceled :" + loyaltyIdnumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the card status from database";
                string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(loyaltyIdnumber);
                Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Canceled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
                testStep.SetOutput("Card status is validated as cancelled and the card number is "+ loyaltyIdnumber);
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


