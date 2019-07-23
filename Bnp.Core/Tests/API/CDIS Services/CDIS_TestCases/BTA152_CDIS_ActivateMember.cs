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
    public class BTA152_CDIS_ActivateMember : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        String stepName = "";
        IList<VirtualCard> vc;
        double elapsedTime;
        GetAccountSummaryOut getAccountSummary = null;

        [TestCategory("Smoke")]
        [TestCategory("API_SOAP_Smoke")]
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA152_SOAP_ActivateMember_Positive()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Locking the member";
                vc = output.GetLoyaltyCards();
                //cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                testStep.SetOutput("Member is Locked ");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activating the member";
                cdis_Service_Method.ActivateMember(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member is Reactivated");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                testStep.SetOutput("Response from database: " + dbresponse + "   Member Status: " + (Member_Status)Int32.Parse(dbresponse));
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Active.ToString(), value, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + value);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST862_SOAP_ActivateMemberWithAllFieldsForActiveMember()
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
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName+ "and Member Status is: " + output.MemberStatus);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName + "and Member Status is: "+output.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing all the fields for a member whose status is Active";
                cdis_Service_Method.ActivateMember(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Activate member performs no action on active member, Member Status after ActivateMember is: " + output.MemberStatus);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST886_SOAP_ActivateMemberWithAllFieldsForLockedMember()
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
                stepName = "Locking the member";
                //cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after DeActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus + " and Loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing all the fields (Required and Optional) of a member whose status is Locked";
                cdis_Service_Method.ActivateMember(vc[0].LoyaltyIdNumber);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after ActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus + " and Loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Memberstatus and updated member status reason in DB ";
                string value = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                string statusChangeReason = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "STATUSCHANGEREASON", string.Empty);
                Assert.AreEqual("SOAP_Automation", statusChangeReason, "status change reason is updated as expected" + statusChangeReason);
                string dbMemberStatus = (Member_Status)Int32.Parse(value) + "";
                Assert.AreEqual(Member_Status.Active.ToString(), dbMemberStatus, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + dbMemberStatus);
                testStep.SetOutput("Member staus is in Active and Status change reason is updated successfully in database and the reason is " + statusChangeReason);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST886_SOAP_ActivateMemberWithAllFieldsForInactiveMember()
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
                stepName = "Inactivating the member";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                //getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member status after DeActivateMember call by using GetAccountSummary is : " + dbMemberStatus + " and Loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing all the fields (Required and Optional) of a member whose status is Inactive";
                cdis_Service_Method.ActivateMember(vc[0].LoyaltyIdNumber);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after ActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus + " and Loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Memberstatus and updated member status reason in DB ";
                string value = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                string statusChangeReason = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "STATUSCHANGEREASON", string.Empty);
                Assert.AreEqual("SOAP_Automation", statusChangeReason, "status change reason is updated as expected" + statusChangeReason);
                string dbMemberStatusNew = (Member_Status)Int32.Parse(value) + "";
                Assert.AreEqual(Member_Status.Active.ToString(), dbMemberStatusNew, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + dbMemberStatusNew);
                testStep.SetOutput("Member staus is in Active and Status change reason is updated successfully in database and the reason is "+ statusChangeReason);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST863_SOAP_ActivateMemberWithMandatoryFieldsForActiveMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();

            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName+ "and Member Status is: " + output.MemberStatus);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing mandatory fields of a member whose status is Active";
                cdis_Service_Method.ActivateMemberWithMandatoryFields(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Activatemember performs no action on active member, Member Status after ActivateMember is: " + output.MemberStatus);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST887_SOAP_ActivateMemberWithMandatoryFieldsForInactiveMember()
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
                stepName = "Inactivating the member";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                // getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member status after DeActivateMember call by using GetAccountSummary is : " + dbMemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing mandatory fileds of a member whose status is Inactive";
                cdis_Service_Method.ActivateMemberWithMandatoryFields(vc[0].LoyaltyIdNumber);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("Active", getAccountSummary.MemberStatus, "Expected value is Active and Actual value is" + getAccountSummary.MemberStatus);
                testStep.SetOutput("Member is Reactivated and Loyalty Id is : " + vc[0].LoyaltyIdNumber+ "and his member status is:" + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Memberstatus as Active in DB ";
                string value = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                string dbMemberStatusNew = (Member_Status)Int32.Parse(value) + "";
                Assert.AreEqual(Member_Status.Active.ToString(), dbMemberStatusNew, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + dbMemberStatusNew);
                testStep.SetOutput("Member staus is in Active in database");
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST887_SOAP_ActivateMemberWithMandatoryFieldsForLockedMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Locking the member";
                //cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after DeActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus+ " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing mandatory fields of a member whose status is Locked";
                cdis_Service_Method.ActivateMemberWithMandatoryFields(vc[0].LoyaltyIdNumber);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after ActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus + " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
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
        [TestCategory("API_SOAP_ActivateMemberr")]
        [TestCategory("ActivateMember_Positive")]
        [TestMethod]
        public void BTA841_ST885_SOAP_ActivatePreEnrolledMemberWithAllFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service as Pre Enrolled";
                Member output = cdis_Service_Method.GetCDISMemberPreEnrolled();
                vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validate Memberstatus in DB as Pre-Enrolled";
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
               // string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "NewStatus", string.Empty);
                Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
                testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing all the fields for a member whose status is Pre-Enrolled";
                cdis_Service_Method.ActivateMember(vc[0].LoyaltyIdNumber);
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member status after ActivateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validate the member status from database as Active";
                string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Active.ToString(), value, "Expected value is" + Member_Status.Active.ToString() + "Actual value is" + value);
                testStep.SetOutput("The Member status from database is : " + value);
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
        [TestCategory("API_SOAP_ActivateMember")]
        [TestCategory("ActivateMember_Negative")]
        [TestMethod]
        public void BTA841_ST888_SOAP_ActivateMemberWithPassingNullValuesToFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            String stepName = "";
            try
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing null values to fields";
                string output = cdis_Service_Method.ActivateMemberWithNullFields();
                testStep.SetOutput(output);
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
        [TestCategory("API_SOAP_ActivateMember")]
        [TestCategory("ActivateMember_Negative")]
        [TestMethod]
        public void BTA841_ST889_SOAP_ActivateMember_TerminatedMember()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            try
            {
                Logger.Info("Test Method Started");
                Common common = new Common(this.DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("IpCode: " + output.IpCode + " , Name: " + output.FirstName+"and memberstatus is: "+output.MemberStatus);
                Logger.Info("IpCode: " + output.IpCode + " , Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Terminating the member";
                //cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber);
                string actualMessage = cdis_Service_Method.TerminateMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", String.Empty, out elapsedTime);
                Assert.AreEqual("pass", actualMessage, "Member with loyality id number : " + vc[0].LoyaltyIdNumber + " is not terminated");
                var getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                Assert.AreEqual("Terminated", getAccountSummary.MemberStatus, "Expected value is Terminated and Actual value is" + getAccountSummary.MemberStatus);
                testStep.SetOutput("Member status after TerminateMember call by using GetAccountSummary is : " + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing member identity of a member whose status is Terminated";
                string outputMsg = cdis_Service_Method.ActivateMemberWithTerminatedStatus(vc[0].LoyaltyIdNumber);
                testStep.SetOutput(outputMsg);
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
        [TestCategory("API_SOAP_ActivateMember")]
        [TestCategory("ActivateMember_Negative")]
        [TestMethod]
        public void BTA841_ST890_SOAP_ActivateMemberWhoseStatusIsMerged()
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
                stepName = "Add First member using AddMember method";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ",Name: " + output.FirstName);
                Logger.Info("IpCode: " + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc1 = output.GetLoyaltyCards();


                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add second member using AddMember method";
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
                testStep.SetOutput("The second member Ipcode: " + member.IpCode + " and member status is: " + member.MemberStatus);
                testStep.SetOutput("Merging of the two Members is successfull and the merged member identity is " + vc1[0].LoyaltyIdNumber+" and his member status is: "+getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing member identity of a member whose status is Merged";
                string outputMsg = cdis_Service_Method.ActivateMemberWhoseStatusIsMerged(vc1[0].LoyaltyIdNumber);
                testStep.SetOutput(outputMsg);
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
        [TestCategory("API_SOAP_ActivateMember")]
        [TestCategory("ActivateMember_Negative")]
        [TestMethod]
        public void BTA841_ST891_SOAP_ActivateMember_With_InvalidMemberIdentity()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            try
            {
                Logger.Info("Test Method Started: " + testCase.GetTestCaseName());

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Activate the member by passing invalid Member identity";
                string output = cdis_Service_Method.ActivateMemberInvalidMemberIdentity();
                testStep.SetOutput(output);
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
    }
}