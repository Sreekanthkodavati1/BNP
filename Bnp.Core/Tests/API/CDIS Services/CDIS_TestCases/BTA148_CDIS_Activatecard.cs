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
	public class BTA148_CDIS_Activatecard : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;
		IList<VirtualCard> vc;
        double elapsetime;

        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA148_SOAP_ActivatecardForMember_Positive()
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
				stepName = "Deactivating card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
				string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "Automation - Deactivate Card");
                Assert.AreEqual("pass", message, "Card: "+ vc[0].LoyaltyIdNumber+" has been not deactivated");
				testStep.SetOutput("Card is deactivated: " + vc[0].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
				testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the response from database";
				string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("Response from database: LoyaltyCardID  NewStatus is \"" + dbresponse + "\" which means the card is : " + (LoyaltyCard_Status)Int32.Parse(dbresponse));
				string value = (LoyaltyCard_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
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
		[TestCategory("API_SOAP_ActivateCard")]
		[TestCategory("API_SOAP_ActivateCard_Positive")]
		[TestMethod]
		public void BTA1030_ST1091_SOAP_Activate_InActiveCard()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			String stepName = "";
            string value = string.Empty;

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
				stepName = "Deactivating card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
                // cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                //cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, DateTime.Now, string.Empty);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP-Automation");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                string deactivateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(deactivateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                testStep.SetOutput("The loyaltyCard: "+ vc[0].LoyaltyIdNumber + " is in inactive status (status from DB): " + value);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member and validate status from DB";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                string activateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(activateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput(msg + " and the status from DB is: " + activateStatus + "(" + value + ")");
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA1030_ST1092_SOAP_Activate_InActiveCard_WhereDateIsNull()
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
				stepName = "Deactivating card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
                // Usage: cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, null, string.Empty); If DateTime is null or else pass DateTime.Now
                //cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, null, string.Empty);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP-Automation");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                string deactivateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                string value = (LoyaltyCard_Status)Int32.Parse(deactivateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is in inactive status (status from DB): " + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member where date is null";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                string activateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(activateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput(msg + " and the status from DB is: " + activateStatus+ "("+ value + ")");
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA1030_ST1093_SOAP_Activate_InActiveCard_WhereUpdateStatusReasonIsNull()
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
				stepName = "Deactivating card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
                //cdis_Service_Method.DeActivateCardUsingLoyaltyIDWhereUpdateStatusReasonIsNull(vc[0].LoyaltyIdNumber);
                // Usage: cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, DateTime.Now, null); If status reason is null or else pass string.Empty
                //cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, DateTime.Now, null);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, null);
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                string deactivateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                string statusReason = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUSCHANGEREASON", string.Empty);
                string value = (LoyaltyCard_Status)Int32.Parse(deactivateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                Assert.AreEqual("", statusReason, "Expected value is :[] and the Actual value is" + statusReason);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is in inactive status (status from DB): " + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member where updateStatus reason is Null";
                string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                string activateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(activateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput(msg + " and the status from DB is: " + activateStatus + "(" + value + ")");
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Negative")]
        [TestMethod]
		public void BTA1030_ST1095_SOAP_ActivateCard_Of_An_InactiveMember()
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
				vc = output.GetLoyaltyCards();
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Deactivating the member and validate the memberstatus from DB";
				cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                string value = (Member_Status)Int32.Parse(dbMemberStatus) + "";
                Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
                testStep.SetOutput("Member detail's: IPCODE: " + vc[0].IpCode + " and member status from DB is:" + dbMemberStatus);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating deactivated member";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("Throws an expection with the " + msg);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				string[] errors = msg.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3314";
				Assert.AreEqual("3314", errorssplit[1], "Expected value is" + "3314" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]+ "and the errorcode is: "+ errorssplit[1]);
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Negative")]
        [TestMethod]
		public void BTA1030_ST1115_SOAP_Activate_CancelledCard()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			String stepName = "";
			GetAccountSummaryOut pointBalanceBeforeCancelCard, pointBalanceAfterCancelCard;

			try
			{
				Logger.Info("Test Method Started");
				Common common = new Common(this.DriverContext);
				CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

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
				stepName = "Cancelling Card through CancelCard method";
				cdis_Service_Method.CancelCard(vc[0].LoyaltyIdNumber,out elapsetime);
				testStep.SetOutput("The following card has Cancelled :" + vc[0].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validate the card status from database";
				string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber + "");
				pointBalanceAfterCancelCard = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				Assert.AreEqual("0", pointBalanceAfterCancelCard.CurrencyBalance.ToString(), "Expected value is \"0\" and the Actual value is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
				Assert.AreEqual("Cancelled", (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + "", "Expected value is Cancelled and the Actual value is" + (LoyaltyCard_Status)Convert.ToInt32(dbresponse));
				testStep.SetOutput("The card status value from db is : " + dbresponse + " which means card status is : " + (LoyaltyCard_Status)Convert.ToInt32(dbresponse) + " and Points balance after" +
					"card cancellation is: " + pointBalanceAfterCancelCard.CurrencyBalance.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating cancelled member";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("Throws an expection with the " + msg);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				string[] errors = msg.Split(';');
				string[] errorssplit = errors[0].Split('=');
            
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Negative")]
        [TestMethod]
		public void BTA1030_ST1116_SOAP_Activate_A_ReplacedCard()
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
				stepName = "Replacing card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
                Member response = cdis_Service_Method.ReplaceCard(vc[0].LoyaltyIdNumber, common.RandomNumber(7), true, System.DateTime.Now, out time);
                vc = response.GetLoyaltyCards();
				testStep.SetOutput("Member \"" + output.FirstName + "\" card with LoyaltyCardId : " + vc[0].LoyaltyIdNumber + " has been replaced with new LoyaltyCardId: " + vc[1].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Asserting the Response";
				Assert.AreEqual(vc[0].Status.ToString(), "Replaced");
				Assert.AreEqual(vc[1].Status.ToString(), "Active");
				testStep.SetOutput("Old card : \"" + vc[0].LoyaltyIdNumber + "\" status is : \"" + vc[0].Status.ToString() + "\" and the new Card : \"" + vc[1].LoyaltyIdNumber + "\" status is : \"" + vc[1].Status.ToString() + "\"");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the response from database";
				string dbresponse = DatabaseUtility.GetLoyaltyCardCountfromDbSOAP(response.IpCode + "");
				Assert.AreEqual(vc.Count + "", dbresponse, "Expected value is" + vc.Count.ToString() + "Actual value is" + dbresponse);
				testStep.SetOutput("Total number of active and inactive cards for the current member are : " + dbresponse);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating replaced member";
				string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("Throws an expection with the " + msg);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				string[] errors = msg.Split(';');
				string[] errorssplit = errors[0].Split('=');

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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Negative")]
        [TestMethod]
		public void BTA1030_ST1117_SOAP_Activate_NonExistingCard()
		{
			testCase = new TestCase(TestContext.TestName);
			listOfTestSteps = new List<TestStep>();
			testStep = new TestStep();
			Common common = new Common(this.DriverContext);
			CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
			String stepName = "";
			try
			{
				Logger.Info("Test Method Started: " + testCase.GetTestCaseName());

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activate the card by passing non existing Card identity";
                string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(common.RandomNumber(18));
                testStep.SetOutput("Throws an expection with the " + msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                string[] errors = msg.Split(';');
                string[] errorssplit = errors[0].Split('=');

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 3307";
                Assert.AreEqual("3305", errorssplit[1], "Expected value is" + "3305" + "Actual value is" + errorssplit[1]);
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
				Logger.Info("Test Failed: " + testCase.GetTestCaseName() + "Reason: " + e.Message);
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA1030_ST1094_SOAP_ActivateAnExpiredCardHavingTransactions()
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

				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Updating Member by adding transactions";
				DateTime date = DateTime.Now.AddDays(-5);
				string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
				testStep.SetOutput("The transaction header of the transcation posted through udpate member method is  " + txnHeaderId);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the response from database";
				string dbresponse = DatabaseUtility.GetTxnHeaderIdusingVCKeyDBSoap(vc[0].VcKey + "");
				Assert.AreEqual(txnHeaderId, dbresponse, "Expected value is" + txnHeaderId + "Actual value is" + dbresponse);
				testStep.SetOutput("TXNheaderID value from db is: " + dbresponse + " and TXNheaderID from service response is: " + txnHeaderId);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Change the expiration date for the card";
				//IList<VirtualCard> vc = output.GetLoyaltyCards();
				DateTime response = cdis_Service_Method.ChangeCardExpirationDateToNow(vc[0].LoyaltyIdNumber);
				testStep.SetOutput("The LoyaltyCard's: " + vc[0].LoyaltyIdNumber + " new expiration date is : " + response);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Verify the updated expiration date in the DB";
				string dbresponse1 = DatabaseUtility.GetExpirationDatefromDbSOAP(vc[0].LoyaltyIdNumber);
				Assert.AreEqual(response + "", dbresponse1, "Expected value is" + response + "Actual value is" + dbresponse1);
				testStep.SetOutput("The new expiration date from DB is :" + dbresponse1);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member";
			    string msg = cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the response from database";
			//	string dbresponse2 = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                string dbresponse2 = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                if (string.Empty == dbresponse2)
                {
                    dbresponse2 = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUS", string.Empty);
                }
                testStep.SetOutput("Response from database: LoyaltyCardID  NewStatus is \"" + dbresponse2 + "\" which means the card is : " + (LoyaltyCard_Status)Int32.Parse(dbresponse2));
				string value = (LoyaltyCard_Status)Int32.Parse(dbresponse2) + "";
				Assert.AreEqual(LoyaltyCard_Status.Expired.ToString(), value, "Expected value is " + LoyaltyCard_Status.Expired.ToString() + " and Actual value is " + value);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the response from database";
				string dbresponse3 = DatabaseUtility.GetTxnHeaderIdusingVCKeyDBSoap(vc[0].VcKey + "");
				Assert.AreEqual(txnHeaderId, dbresponse3, "Expected value is" + txnHeaderId + "Actual value is" + dbresponse3);
				testStep.SetOutput("TXNheaderID value from db is: " + dbresponse3 + " and TXNheaderID from service response is: " + txnHeaderId);
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
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA1030_ST1126_SOAP_Activate_InactiveCard_OfMember_Having_Transactions()
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
				IList<VirtualCard> vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Updating Member by adding transactions";
				DateTime date = DateTime.Now.AddDays(-5);
				string txnHeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(output, date);
				testStep.SetOutput("The transaction header of the transaction posted through update member method is  " + txnHeaderId);
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
				stepName = "Deactivating card for the member";
                // Usage: cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, null, string.Empty); If DateTime is null or else pass DateTime.Now
                //cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, null, string.Empty);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, null, "SOAP-Automation");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                string deactivateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                string value = (LoyaltyCard_Status)Int32.Parse(deactivateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is " + LoyaltyCard_Status.Inactive.ToString() + "Actual value is " + value);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is in inactive status (status from DB): " + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member";
				cdis_Service_Method.ActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber);
                string activateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(activateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is Activated again (status from DB): " + value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Verify the Loyalty currency balance of a member with GetAccountSummary method";
				var currencyBalance = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
				Assert.AreEqual("0", loyalCurrencyBalance.ToString(), "Expected value is 0 and Actual value is " + loyalCurrencyBalance);
				testStep.SetOutput("LoyaltyCurrencyBalance value from GetAccountSummary is: " + currencyBalance.CurrencyBalance.ToString() + " and from GetLoyaltyCurrencyBalance method is: " + loyalCurrencyBalance);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validate member loyaltycurrency in DB";
				string dbresponse = DatabaseUtility.GetLoyalityCurrencieBalancesfromDBUsingIdSOAP(vc[0].VcKey + "");
				Assert.AreEqual("0", dbresponse, "Expected value is" + loyalCurrencyBalance + "Actual value is " + dbresponse);
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

        [TestCategory("Regression")]
        [TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_ActivateCard")]
        [TestCategory("API_SOAP_ActivateCard_Positive")]
        [TestMethod]
		public void BTA1030_ST1118_SOAP_Activate_InActiveCard_VerifyElapsedTime()
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
				stepName = "Deactivating card for the member";
				IList<VirtualCard> vc = output.GetLoyaltyCards();
                //cdis_Service_Method.DeActivateCard(vc[0].LoyaltyIdNumber, DateTime.Now, string.Empty);
                string message = cdis_Service_Method.DeActivateCardUsingLoyaltyID(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP-Automation");
                Assert.AreEqual("pass", message, "Card: " + vc[0].LoyaltyIdNumber + " has been not deactivated");
                string deactivateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                string value = (LoyaltyCard_Status)Int32.Parse(deactivateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is Deactivated and its status from DB is: "+ value);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Activating card for the member";
				cdis_Service_Method.ActivateCardUsingLoyaltyIDToVerifyElapsedTime(vc[0].LoyaltyIdNumber, out time);
                string activateStatus = DatabaseUtility.GetFromSoapDB("lw_virtualcard", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                value = (LoyaltyCard_Status)Int32.Parse(activateStatus) + "";
                Assert.AreEqual(LoyaltyCard_Status.Active.ToString(), value, "Expected value is" + LoyaltyCard_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput("The loyaltyCard: " + vc[0].LoyaltyIdNumber + " is Activated again (status from DB): " + value + " and the Elapsed time is:"+ time+"ms");
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