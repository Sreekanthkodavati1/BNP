using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using RandomNameGenerator;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-138 : Create Member With and Without Loyalty ID and Verify the same on Member search Page
    /// </summary>

    [TestClass]
    public class BTA138_CSP_CreateMemberWithandWithoutLoyaltyID : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// BTA-138 : Create Member With Loyalty ID and Verify the same on Member search Page
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA138_CSP_CreateMemberWithLoyaltyID()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);
                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Launch Customer Service Portal URL is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Login As csadmin User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Select All Opt In/Out Check boxes
                stepName = "Select All Opt In/Out Check boxes";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Save Registration and Verify Loyalty ID 
                stepName = "Save Registration and Verify Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(member.FirstName);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                stepstatus = false;
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                Assert.Fail();
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        /// <summary>
        /// BTA-138 : Create Member Without Loyalty ID and Verify the same on Member search Page
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA138_CSP_CreateMemberWithOutLoyaltyID()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                vc.LoyaltyIdNumber = "";
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Login As csadmin User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Select All Opt In/Out Check boxes
                stepName = "Select All Opt In/Out Check boxes";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(member.FirstName);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                stepstatus = false;
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                Assert.Fail();
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