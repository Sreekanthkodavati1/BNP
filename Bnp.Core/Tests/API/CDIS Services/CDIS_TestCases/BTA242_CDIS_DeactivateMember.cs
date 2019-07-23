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
    public class BTA242_CDIS_DeactivateMember : ProjectTestBase
    {
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        string stepName = "";
        IList<VirtualCard> vc;
        GetAccountSummaryOut getAccountSummary;
        double elapsedTime;

        [TestCategory("Smoke")]
        [TestCategory("SOAP_Smoke")]
        [TestMethod]
        public void BTA242_CDIS_DeActivateMember_Positive()
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
                vc = output.GetLoyaltyCards();
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member is Deativated");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the Member status for Deactivated member from database";
                string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string value = (Member_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Member_Status.Disabled.ToString(), value, "Expected value is" + Member_Status.Disabled.ToString() + "Actual value is" + value);
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Positive")]
        [TestMethod]
        public void BTA844_ST977_CDIS_DeactivateMemberWithPassingAllMandatoryNonMandatoryFields()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Common common = new Common(this.DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            String stepName = "";

            try
            {
                Logger.Info("Test Method Started");
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.GetCDISMemberGeneral();

                testStep.SetOutput("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                Logger.Info("IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivate the member by passing All Mandatory Non MandatoryFields";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                // getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member has been deactivated: IPCODE: " + vc[0].IpCode + " and member status is: " + dbMemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response from database";
                string dbresponse = DatabaseUtility.GetMemberStatusfromDbSOAP(output.IpCode + "");
                string cardStatusCode = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                if (string.Empty == cardStatusCode)
                {
                    cardStatusCode = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUS", string.Empty);
                }
                string value = (Enums.Member_Status)Int32.Parse(dbresponse) + "";
                string cardStatus = (LoyaltyCard_Status)Int32.Parse(cardStatusCode) + "";
                testStep.SetOutput("Member StatusCode from DB is: " + dbresponse + " Member status is: " + value + " and his loyaltycard status is: " + cardStatus);
                Assert.AreEqual(value, Enums.Member_Status.Disabled.ToString(), "Expected value is" + value + "Actual value is" + Enums.Member_Status.Disabled.ToString());
                Assert.AreEqual(LoyaltyCard_Status.Inactive.ToString(), cardStatus, "Expected value is" + LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + cardStatus);
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
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST978_CDIS_DeactivateMemberWithPassingMemberIdentityNull()
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
                stepName = "Deactivate the member by passing member identity as null";
                string output = cdis_Service_Method.DeactivateMemberwithMemberIdentityNull();
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST980_CDIS_DeactivatingMembereWhoseStatusIsTerminated()
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
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been Terminated: IPCODE: " + vc[0].IpCode + " member status is:" + getAccountSummary.MemberStatus + " and the loyalty Id is : " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivate the member by passing member identity of a member whose status is Terminated";
                string outputMsg = cdis_Service_Method.DeactivateMemberWithTerminatedStatus(vc[0].LoyaltyIdNumber);
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST981_CDIS_DeactivateMemberWhoseStatusIsMerged()
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
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc1[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been Merged: IPCODE: " + vc1[0].IpCode + " member status is: " + getAccountSummary.MemberStatus + " and the loyalty id is " + vc1[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivate the member by passing member identity of a member whose status is Merged";
                string outputMsg = cdis_Service_Method.DeactivateMemberWhoseStatusIsMerged(vc1[0].LoyaltyIdNumber);
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST982_CDIS_DeactivateMemberWhoseMemberEntityIsNonMember()
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
                //string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "NewStatus", string.Empty);
                Assert.AreEqual(Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)), output.MemberStatus.ToString(), "Expected value is" + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + "Actual value is" + output.MemberStatus.ToString());
                testStep.SetOutput("The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
                Logger.Info("TestStep: " + stepName + " ##Passed## The Memberstatus from DB is: " + Enum.GetName(typeof(Member_Status), Convert.ToInt32(dbMemberStatus)) + " and the memberstatus from AddMember method response is: " + output.MemberStatus.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member whose status is Non Member";
                string outputMsg = (string)cdis_Service_Method.DeactivateMemberWhoseStatusIsNonMember(vc[0].LoyaltyIdNumber);
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST983_CDIS_DeactivateMemberWithUpdateMemberStatusReasonMoreThan250char()
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
                testStep.SetOutput("IpCode: " + output.IpCode);
                Logger.Info("IpCode: " + output.IpCode);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                IList<VirtualCard> vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Updating Member Status Reason with More Than 250 Characters through Deactivate member method";

                string StatusReason = common.RandomString(270);
                string outputnew = (string)cdis_Service_Method.DeactivateMember_negative(vc[0].LoyaltyIdNumber, StatusReason);
                string[] errors = outputnew.Split(';');
                string[] errorssplit = errors[0].Split('=');
                string[] errorsnew = errors[1].Split('=');
                testStep.SetOutput(outputnew);
                Logger.Info(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating the response for Error Code as 2000";
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Positive")]
        [TestMethod]
        public void BTA844_ST1005_CDIS_DeactivateMemberWithUpdateMemberStatusReasonWith250char()
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
                testStep.SetOutput("IpCode: " + output.IpCode);
                Logger.Info("IpCode: " + output.IpCode);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Updating Member Status Reason with 250 Characters through UpdateMember method";
                string StatusReason = common.RandomString(250);
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber, StatusReason);
                //getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("Member is deactivated successfully with Update member status reason with 250 characters and member status is:" + dbMemberStatus);
                Logger.Info(output);
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
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST988_CDIS_DeActivateMemberWithDeActivateActivecardsAsTrue()
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
                cdis_Service_Method.DeactivateMember_negative(vc[0].LoyaltyIdNumber, true);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                // getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been deactivated: IPCODE: " + vc[0].IpCode + " and member status is: " + (Member_Status)Int32.Parse(dbMemberStatus) + "");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the Card status for Deactivated member from database";
               // string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                string dbresponse = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                if (string.Empty == dbresponse)
                {
                    dbresponse = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUS", string.Empty);
                }
                string value = (Enums.LoyaltyCard_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Enums.LoyaltyCard_Status.Inactive.ToString(), value, "Expected value is" + Enums.LoyaltyCard_Status.Inactive.ToString() + "Actual value is" + value);
                testStep.SetOutput("The cardstatus code from database for disabled member with deaactivateactivecards as true is : " + dbresponse+ " and card status is: "+ LoyaltyCard_Status.Inactive.ToString());
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
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST_CDIS_DeActivateMemberWithDeActivateActivecardsAsFalse()
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
                cdis_Service_Method.DeactivateMember_negative(vc[0].LoyaltyIdNumber, false);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been deactivated: IPCODE: " + vc[0].IpCode + " and member status is: " + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "validating the Card status for Deactivated member from database";
                // string dbresponse = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc[0].LoyaltyIdNumber);
                string dbresponse = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "NEWSTATUS", string.Empty);
                if (string.Empty == dbresponse)
                {
                    dbresponse = DatabaseUtility.GetFromSoapDB("lW_VIRTUALCARD", "LOYALTYIDNUMBER", vc[0].LoyaltyIdNumber, "STATUS", string.Empty);
                }

                string value = (Enums.LoyaltyCard_Status)Int32.Parse(dbresponse) + "";
                Assert.AreEqual(Enums.LoyaltyCard_Status.Cancelled.ToString(), value, "Expected value is" + Enums.LoyaltyCard_Status.Cancelled.ToString() + "Actual value is" + value);
                testStep.SetOutput("The cardstatus code from database for disabled member with deaactivateactivecards as false is : " + dbresponse + " and card status is: " + LoyaltyCard_Status.Cancelled.ToString());
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
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Negative")]
        [TestMethod]
        public void BTA844_ST979_CDIS_DeactivateMemberWithPassingMemberIdentityNotExistInDB()
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
                stepName = "Deactivate the member by passing member identity not exists in DB";
                string output = cdis_Service_Method.DeactivateMemberWithMemberIdentityNotExistsInDb();
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
        [TestCategory("SOAP_Regression")]
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Positive")]
        [TestMethod]
        public void BTA844_ST_CDIS_DeactivateMember_DiasbledMember()
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
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member whose status is Active";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                string dbMemberStatus = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                // getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been deactivated: IPCODE: " + vc[0].IpCode + " and member status is: " + (Member_Status)Int32.Parse(dbMemberStatus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member whose status is Disabled";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                // getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                string dbMemberStatusNew = DatabaseUtility.GetFromSoapDB("LW_LoyaltyMember", "IPCODE", output.IpCode.ToString(), "MemberStatus", string.Empty);
                testStep.SetOutput("IPCODE: " + vc[0].IpCode + " and member status is: " + (Member_Status)Int32.Parse(dbMemberStatusNew)+"");
                Assert.AreEqual("Disabled", (Member_Status)Int32.Parse(dbMemberStatusNew)+"", "Expected value is Disabled and the Actual value is" + (Member_Status)Int32.Parse(dbMemberStatusNew)+"");
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
        [TestCategory("DeactivateMember")]
        [TestCategory("DeactivateMember-Positive")]
        [TestMethod]
        public void BTA844_ST_CDIS_DeactivateMember_LockedMember()
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
                Member output = cdis_Service_Method.GetCDISMemberGeneral();
                testStep.SetOutput("IpCode: " + output.IpCode + ", Name: " + output.FirstName + " and the member status is: " + output.MemberStatus); ;
                Logger.Info("TestStep: " + stepName + " ##Passed## IpCode:" + output.IpCode + ", Name: " + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                vc = output.GetLoyaltyCards();

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member whose status is Active";
                //cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber);
                string message = cdis_Service_Method.LockDownMember(vc[0].LoyaltyIdNumber, DateTime.Now, "SOAP_Automation", string.Empty, out elapsedTime);
                Assert.AreEqual("pass", message, "Member with loyalty id: " + vc[0].LoyaltyIdNumber + " has not been locked");
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("Member has been Locked: IPCODE: " + vc[0].IpCode + " and member status is: " + getAccountSummary.MemberStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);

                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Deactivating the member whose status is Locked";
                cdis_Service_Method.DeactivateMember(vc[0].LoyaltyIdNumber);
                getAccountSummary = cdis_Service_Method.GetAccountSummary(vc[0].LoyaltyIdNumber);
                testStep.SetOutput("IPCODE: " + vc[0].IpCode + " and member status is: " + getAccountSummary.MemberStatus);
                Assert.AreEqual("Locked", getAccountSummary.MemberStatus, "Expected value is Locked and the Actual value is" + getAccountSummary.MemberStatus);
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