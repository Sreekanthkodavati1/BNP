using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using System.Configuration;
using Bnp.Core.WebPages.MemberPortal;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// BTA-513 Verify Forgot Password Functionality in Member Portal
    /// </summary>
    [TestClass]
    public class BTA513_MP_ForgotPassword : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps;
        public TestStep testStep;

        /// <summary>
        /// This test case need to be Executed in VDI, as we are reading Log data
        /// </summary>
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA_513_MP_ForgotPassword()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            bool stepstatus = false;
            string BTA_DEV_MP_LogPath = MemberPortalData.BTA_DEV_MP_LogPath;
            try
            {
                var MP_ForgotPassword = new MemberPortal_ForgotPassword(DriverContext);
                var MPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSP_SearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var MP_Profile = new MemberProfile(DriverContext);
                var member = MP_Profile.GenerateMemberBasicInfo();
                var details = MP_Profile.GenerateMemberDetails();
                var myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                var myProfilePage = new MemberPortal_MyProfilePage(driverContext);

                #region Step1:Launch CSPortal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create new member user as per config file,if not existed
                stepName = "Create new member user as per config file,if not existed";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.FirstName = MemberPortalData.ForgotPasswordTestMember;
                member.LastName = MemberPortalData.ForgotPasswordTestMember;
                member.Username = MemberPortalData.ForgotPasswordTestMember;
                member.Password =RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                bool isMemberExists = CSP_SearchPage.VerifyMemberExists(member.Username, out stepOutput);
                testStep.SetOutput(stepOutput);
                if (!isMemberExists)
                {
                    CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                    stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput);
                    stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput);
                    stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput);
                    stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput);
                    CSP_RegistrationPage.SaveRegistration();
                    testStep.SetOutput("Member created successfully with username: " + member.Username);
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout as Csadmin 
                stepName = "Logout from CS Portal User:" + login.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("Logout is Successful as User: " + member.Username);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Launch MPPortal 
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Message);
                MP_ForgotPassword.GetInitialWordCountFromLogFile(BTA_DEV_MP_LogPath);
                testStep.SetOutput("Launch Member Portal URL is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Click on Forgot Password
                stepName = "Click on Forgot Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MPortal_LoginPage.ClickForgotPassword();
                testStep.SetOutput("Clicked on Forgot Password Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Enter valid user name and click Submit button
                stepName = "Enter a valid user name and click Submit button";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_ForgotPassword.EnterUserName(member.Username);
                MP_ForgotPassword.ClickSubmitButton();
                testStep.SetOutput("Entered User name: " + member.Username + " and clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Select Email option to receive reset code
                stepName = "Select Email option to receive reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.SelectEmailOption();
                testStep.SetOutput("Selected Email Option to receive reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Click on Send my reset code
                stepName = "Click on Send my reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.ClickSendResetCodeButton();
                testStep.SetOutput("Clicked on Send my reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Select I already have a reset code option
                stepName = "Select I already have a reset code option";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.SelectAlreadyHaveResetCode();
                testStep.SetOutput("Selected I already have a reset code option");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Click on Send my reset code
                stepName = "Click on Send my reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.ClickSendResetCodeButton();
                testStep.SetOutput("Clicked on Send my reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Enter the reset code received in the Log
                stepName = "Enter the reset code received in the Log";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_ForgotPassword.EnterResetCodeFromLogFile(BTA_DEV_MP_LogPath, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Click on Submit button
                stepName = "Click on Submit button";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.ClickSubmitButton();
                testStep.SetOutput("Clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Create new Password for Test User
                stepName = "Create new Passwod for Test User";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.Password =RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                stepstatus = MP_ForgotPassword.CreateNewPassword(member.Username, member.Password, member.Password, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Return back to login page
                stepName = "Return back to login page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = MP_ForgotPassword.ReturnToLoginPage(out stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput(stepOutput);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16:Login with the newly created Password
                stepName = "Login with User, User Name: " + member.FirstName + " with newly reset Password: " + member.Password;
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_LoginPage.LoginMemberPortal(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17:Verify successful navigation to Member Portal home page
                stepName = "Verify successful navigation to Member Portal home page";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(MPortal_LoginPage.VerifyMemberPortalLoginSuccessfulForUser(member.FirstName, member.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18:Navigate to My Profile page
                stepName = "Navigate to My Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyProfile, out var Step_Output); var strStatus = Step_Output;
                testStep.SetStatus(myProfilePage.VerifyMyProfilePage(MemberPortal_MyProfilePage.Sections.MyProfile.ToString(), out Step_Output)); strStatus = strStatus + ". " + Step_Output;
                testStep.SetOutput(strStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept19:Change Password  for Test Member to default
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password  for Test Member to default";
                string agen_OldPassword = member.Password;
                string agent_DefaultPassword = AgentValues.Agentpassword;
                myProfilePage.ChangePassword(agen_OldPassword, agent_DefaultPassword, agent_DefaultPassword);
                stepstatus = myProfilePage.VerifySuccessMessage(agen_OldPassword, agent_DefaultPassword, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step20:Logout from Member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_LoginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
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
