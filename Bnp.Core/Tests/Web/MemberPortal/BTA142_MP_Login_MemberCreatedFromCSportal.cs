﻿using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using BnPBaseFramework.Web;
using System;
using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Bnp.Core.WebPages.MemberPortal;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// BTA-142 Login to Member Portal with the user created from CS Portal
    /// </summary>
    
    [TestClass]
    public class BTA142_MP_Login_MemberCreatedFromCSportal : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps;
        public TestStep testStep;

        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA142_MP_LoginUserCreatedFromCsPortalInMemberPortal()
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
                var MP_LoginPage = new MemberPortal_LoginPage(DriverContext);
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

                #region Step9:Save Registration 
                stepName = "Save Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                testStep.SetOutput("Registration Completed Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout from CSPortal
                stepName = "Logout from CSPortal";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout from CSPortal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Launch Member Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_LoginPage.LaunchMemberPortal(login.MemberPortal_url,out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                # region Step12: Login with the user Created from CSPortal
                stepName = "Login with the user Created from CSPortal";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_LoginPage.LoginMemberPortal(member.Username, member.Password, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Login to Member Portal should be successful
                stepName = "Verify Login to Member Portal should be successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                Step_Output = MP_LoginPage.VerifyMemberPortalLoginSuccessfulForUser(member.FirstName, member.LastName);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Logout from Member Portal
                stepName = "Logout from Member Portal";                
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_LoginPage.LogoutMPPortal();
                testStep.SetOutput("Logout from Member Portal is Successful");
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