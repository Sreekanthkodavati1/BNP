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
	public class BTA151_CDIS_LockDownMember : ProjectTestBase
	{
		TestCase testCase;
		List<TestStep> listOfTestSteps = new List<TestStep>();
		TestStep testStep;
		IList<VirtualCard> vc;
        double elapsedTime;

		[TestCategory("Smoke")]
		[TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Positive")]
        [TestMethod]
		public void BTA151_SOAP_LockDownMember_Positive()
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
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: "+ vc[0].LoyaltyIdNumber+" has not been locked");
                testStep.SetOutput("Member is Locked");
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the card status for lock down member from database";
				String dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				String value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Locked.ToString(), value, "Expected value is" + Member_Status.Locked.ToString() + "Actual value is" + value);
				testStep.SetOutput("The card status from database for lockdown is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Positive")]
        [TestMethod]
		public void BTA151_ST961_SOAP_LockDown_Already_LockedMember_POSITIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Loyalty Id of a member whose status is Active";
				//cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member's : IPCODE: " + vc[0].IpCode + " member status is: " + getAccountSummary.MemberStatus+ " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				//testStep = TestStepHelper.StartTestStep(testStep);
				//stepName = "validating the card status for lock down member from database";
				//String dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				//String value = (Member_Status)Int32.Parse(dbresponse) + "";
				//Assert.AreEqual(Member_Status.Locked.ToString(), value, "Expected value is" + Member_Status.Locked.ToString() + "Actual value is" + value);
				//testStep.SetOutput("The card status from database for lockdown member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				//testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				//listOfTestSteps.Add(testStep);
				//testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Loyalty Id of a member whose status is Locked";
				//cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message_Locked = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message_Locked, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                var getAccountSummaryLD = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member's details of locked down member : IPCODE: " + vc[0].IpCode + " member status is: " + getAccountSummaryLD.MemberStatus + " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
              //  testStep.SetOutput("Member status remains the same and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the card status for lock down member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(getAccountSummaryLD.MemberStatus, value, "Expected value is" + getAccountSummaryLD.MemberStatus + "Actual value is" + value);
				testStep.SetOutput("The cardstatus Code from database for lockdown member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Positive")]
        [TestMethod]
		public void BTA151_ST962_SOAP_LockDownMember_DisabledMember_POSITIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Deactivating the member";
				cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                // var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member detail's: IPCODE: " + vc[0].IpCode + " and member status is:" + dbMemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the Member status for Deactivated member from database";
                string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
                testStep.SetOutput("The card status from database for deactivated member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down a DisabledMember";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(vc[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                testStep.SetOutput("Throws an expection with the " + error);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
                String[] errors = error.Split(';');
                String[] errorssplit = errors[0].Split('=');
                String[] errorsnew = errors[1].Split('=');

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 3303";
                Assert.AreEqual("3303", errorssplit[1], "Expected value is" + "3303" + "Actual value is" + errorssplit[1]);
                testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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

		//public void BTA967_LockDownMemberWhereMemberStatusIsDeactivated_NEGATIVE()
		//{
		//	testCase = new TestCase(TestContext.TestName);
		//	listOfTestSteps = new List<TestStep>();
		//	testStep = new TestStep();
		//	String stepName = "";

		//	try
		//	{
		//		Logger.Info("Test Method Started");
		//		Common common = new Common(this.DriverContext);
		//		CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

		//		testStep = TestStepHelper.StartTestStep(testStep);
		//		stepName = "Adding member with CDIS service";
		//		Member output = cdis_Service_Method.GetCDISMemberGeneral();
		//		vc = output.GetLoyaltyCards();
		//		testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
		//		Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);

		//		testStep = TestStepHelper.StartTestStep(testStep);
		//		stepName = "Deactivating the member";
		//		cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
		//		testStep.SetOutput("Member is Deactivated");
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);

		//		testStep = TestStepHelper.StartTestStep(testStep);
		//		stepName = "Validating the Member status for Deactivated member from database";
		//		string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
		//		string value = (Member_Status)Int32.Parse(dbresponse) + "";
		//		Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
		//		testStep.SetOutput("The card status from database for deactivated member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);
		//		testCase.SetStatus(true);

		//		testStep = TestStepHelper.StartTestStep(testStep);
		//		stepName = "Locking down the member by passing Loyalty Id of a member whose status is Deactivated";
		//		string error = (string)cdis_Service_Method.LockDownMemberNegative(vc[0].LoyaltyIdNumber);
		//		testStep.SetOutput("Throws an expection with the " + error);

		//		String[] errors = error.Split(';');
		//		String[] errorssplit = errors[0].Split('=');
		//		String[] errorsnew = errors[1].Split('=');
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);

		//		testStep = TestStepHelper.StartTestStep(testStep);
		//		stepName = "Validating the response for Error Code as 3303";
		//		Assert.AreEqual("3303", errorssplit[1], "Expected value is" + "3303" + "Actual value is" + errorssplit[1]);
		//		testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);
		//		testCase.SetStatus(true);

		//	}
		//	catch (Exception e)
		//	{
		//		testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
		//		listOfTestSteps.Add(testStep);
		//		testCase.SetStatus(false);
		//		testCase.SetErrorMessage(e.Message);
		//		Assert.Fail(e.Message);
		//	}
		//	finally
		//	{
		//		testCase.SetTestCaseSteps(listOfTestSteps);
		//		testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
		//		listOfTestCases.Add(testCase);
		//	}
		//}

		[TestCategory("Regression")]
		[TestCategory("API_SOAP_Regression")]
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Positive")]
		[TestMethod]
		public void BTA151_ST968_SOAP_LockDown_TerminatedMember_Positive()
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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Terminating the member";
				//cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber);
                string actualMessage = cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", String.Empty, out elapsedTime);
                Assert.AreEqual("pass", actualMessage, "Member with loyality id number : " + vc[0].LoyaltyIdNumber + " is not terminated");
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member detail's: IPCODE: " + vc[0].IpCode + " and member status is:" + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the Member status for Terminated member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Terminated.ToString(), value, "Expected value is" + Member_Status.Terminated.ToString() + "Actual value is" + value);
				testStep.SetOutput("The card status from database for terminated member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down a Terminated Member";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(vc[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3303";
				Assert.AreEqual("3303", errorssplit[1], "Expected value is" + "3303" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Negative")]
		[TestMethod]
		public void BTA151_ST963_SOAP_LockDownMember_With_StatusReasonMoreThan255Characters_NEGATIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Updated Status Reason of a member with more than 255 characters";
				//string error = (string)cdis_Service_Method.LockDownMemberUpdatedMemberStatusReason(common.RandomString(260), vc[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, common.RandomString(260), string.Empty, out elapsedTime);
                string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep.SetOutput("Throws an expection with the " + error);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 2002";
				Assert.AreEqual("2002", errorssplit[1], "Expected value is" + "2002" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Negative")]
		[TestMethod]
		public void BTA151_ST964_SOAP_LockDownMember_MemberIdentityEqualToNull_NEGATIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing null value in member identity";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(null);
                string error = cdis_Service_Method.LockDownMember(null, DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep.SetOutput("Throws an expection with the " + error);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 2003";
				Assert.AreEqual("2003", errorssplit[1], "Expected value is" + "2003" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Negative")]
		[TestMethod]
		public void BTA151_ST965_SOAP_LockDownMember_IpCodeNotExistsInDB_NEGATIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing an invalid member identity";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(common.RandomNumber(8));
                string error = cdis_Service_Method.LockDownMember(common.RandomNumber(8), DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep.SetOutput("Throws an expection with the " + error);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);


				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3302";
				Assert.AreEqual("3302", errorssplit[1], "Expected value is" + "3302" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
		[TestCategory("SOAP_Regression")]
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Negative")]
        [TestMethod]
		public void BTA151_ST966_SOAP_LockDown_NonMember_NEGATIVE()
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
				Member output = cdis_Service_Method.GetCDISMemberNonMember();
				testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
				Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validate Memberstatus in DB as Non Member";
				string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
               // string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "NewStatus", string.Empty);
                Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
				testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				vc = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Loyalty Id of a member whose status is Non Member";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(vc[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3393";
				Assert.AreEqual("3393", errorssplit[1], "Expected value is" + "3393" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
        [TestCategory("API_SOAP_LockDownMember")]
        [TestCategory("LockDownMember_Negative")]
		[TestMethod]
		public void BTA151_ST969_SOAP_LockDown_MergedMember_NEGATIVE()
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
				stepName = "Adding first member with CDIS service";
				Member output = cdis_Service_Method.GetCDISMemberGeneral();
				vc = output.GetLoyaltyCards();
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc1 = output.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Adding second member using AddMember method";
				Member outputnew = cdis_Service_Method.GetCDISMemberGeneral();
				testStep.SetOutput("IpCode: " + outputnew.IpCode + ", Name: " + outputnew.FirstName);
				Logger.Info("IpCode: " + outputnew.IpCode + ", Name: " + outputnew.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vc2 = outputnew.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Merging Members through MergeMember method";
				Member member = cdis_Service_Method.MergeMembers(vc1[0].LoyaltyIdNumber, vc2[0].LoyaltyIdNumber);
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc1[0].LoyaltyIdNumber);
                testStep.SetOutput("1st Member details : IPCODE: " + vc1[0].IpCode + " and member status is:" + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				IList<VirtualCard> vcmerge = member.GetLoyaltyCards();

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the Member status for Merged member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Merged.ToString(), value, "Expected value is" + Member_Status.Merged.ToString() + "Actual value is" + value);
				testStep.SetOutput("The cardstatus Code from database for merged member is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Loyalty Id of a member whose status is Merged";
				//string error = (string)cdis_Service_Method.LockDownMemberNegative(vc1[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc1[0].LoyaltyIdNumber, DateTime.UtcNow, "CDIS Automation", string.Empty, out elapsedTime);
                testStep.SetOutput("Throws an expection with the " + error);
				string[] errors = error.Split(';');
				string[] errorssplit = errors[0].Split('=');
				string[] errorsnew = errors[1].Split('=');
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Validating the response for Error Code as 3392";
				Assert.AreEqual("3392", errorssplit[1], "Expected value is" + "3392" + "Actual value is" + errorssplit[1]);
				testStep.SetOutput("The ErrorMessage from Service is: " + errorsnew[1]);
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
		[TestCategory("API_SOAP_LockDownMember")]
		[TestCategory("LockDownMember_Positive")]
		[TestMethod]
		public void BTA151_ST1000_SOAP_LockDownMember_StatusReasonIsEqualTo255Characters_POSITIVE()

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
				testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "Locking down the member by passing Updated Status Reason of a member equal to 255 characters";
				//string error = (string)cdis_Service_Method.LockDownMemberUpdatedMemberStatusReason(common.RandomString(255), vc[0].LoyaltyIdNumber);
                string error = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, common.RandomString(255), string.Empty, out elapsedTime);
                Assert.AreEqual("pass", error, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member detail's : IPCODE: " + vc[0].IpCode + " member status is:" + getAccountSummary.MemberStatus+ " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
				testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
				listOfTestSteps.Add(testStep);
				testCase.SetStatus(true);

				testStep = TestStepHelper.StartTestStep(testStep);
				stepName = "validating the card status for lock down member from database";
				string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
				string value = (Member_Status)Int32.Parse(dbresponse) + "";
				Assert.AreEqual(Member_Status.Locked.ToString(), value, "Expected value is" + Member_Status.Locked.ToString() + "Actual value is" + value);
				testStep.SetOutput("The cardstatus code from database for lockdown is : \"" + dbresponse + "\" and the member status: " + (Member_Status)Int32.Parse(dbresponse));
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