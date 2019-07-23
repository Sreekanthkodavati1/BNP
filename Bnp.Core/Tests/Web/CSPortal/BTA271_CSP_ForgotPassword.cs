using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using System.Configuration;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-271 Verify Forgot Password Functionality in CS Portal
    /// </summary>
    [TestClass]
    public class BTA271_CSP_ForgotPassword : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps;
        public TestStep testStep;

        /// <summary>
        /// This test case need to be Executed in VDI, as we are reading Log data
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA271__CSP_ForgotPassword()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            bool stepstatus = false;
            string BTA_DEV_CS_LogPath = CsPortalData.BTA_DEV_CS_LogPath;

            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
                var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
                var CSP_ForgotPassword = new CSPortal_ForgotPassword(DriverContext);
                AgentRegistration agent = new AgentRegistration();

                #region Step1:Launch CSPortal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
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

                #region Step3:Navigate to User Administration         
                stepName = "Navigate to UserAdministration Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigate to Agent Page      
                stepName = "Navigate to Agent Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("Navigate to Agent Page is Successful");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create new TestAgent user as per config file,if not existed
                testStep = TestStepHelper.StartTestStep(testStep);
                agent.FistName = AgentValues.ForgotPasswordTestAgent;
                agent.LastName = AgentValues.ForgotPasswordTestAgent;
                agent.Role = RoleValue.Admin.ToString();
                agent.UserName = AgentValues.ForgotPasswordTestAgent;
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                stepName = "Create New Agent if user is not existed";
                agent.Password =RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout As Test User 
                stepName = "Logout from CS Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("Logout is Successful as csadmin");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Launch CSPortal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep); ;
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out stepOutput); testStep.SetOutput(stepOutput);
                CSP_ForgotPassword.GetInitialWordCountFromLogFile();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Click on Forgot Password
                stepName = "Click on Forgot Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_LoginPage.ClickForgotPassword();
                testStep.SetOutput("Clicked on Forgot Password Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Enter valid user name and click Submit button
                stepName = "Enter a valid user name and click Submit button";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_ForgotPassword.EnterUserName(agent.UserName);
                CSP_ForgotPassword.ClickSubmitButton();
                testStep.SetOutput("Entered User name: " + agent.UserName + " and clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region  Step10: Select Email option to receive reset code
                stepName = "Select Email option to receive reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_ForgotPassword.SelectEmailOption();
                testStep.SetOutput("Selected Email Option to receive reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Click on Send my reset code
                stepName = "Click on Send my reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_ForgotPassword.ClickSendResetCodeButton();
                testStep.SetOutput("Clicked on Send my reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Select I already have a reset code option
                stepName = "Select I already have a reset code option";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_ForgotPassword.SelectAlreadyHaveResetCode();
                testStep.SetOutput("Selected I already have a rest code option");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Click on Send my reset code
                stepName = "Click on Send my reset code";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_ForgotPassword.ClickSendResetCodeButton();
                testStep.SetOutput("Clicked on Send my reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region  Step14: Enter the reset code received in the Email/Log
                stepName = "Enter the reset code received in the Email/Log";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_ForgotPassword.EnterResetCodeFromLogFile(BTA_DEV_CS_LogPath,out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Click on Submit button
                stepName = "Click on Submit button";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_ForgotPassword.ClickSubmitButton();
                testStep.SetOutput("Clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Create new Password for Test User
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create new Passwod for Test User";
                agent.Password =RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                stepstatus = CSP_ForgotPassword.CreateNewPassword(agent.UserName, agent.Password, agent.Password, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Return back to login page
                stepName = "Return back to login page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus=CSP_ForgotPassword.ReturnToLoginPage(out stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput(stepOutput);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18: Login with the newly created Password
                stepName = "Login with User, User Name:" + agent.UserName+ "with newly reset Password:"+agent.Password;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LoginCSPortal(agent.UserName, agent.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step19: Verify First Name and Last Name
                stepName = "Verify First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(CSP_HomePage.VerifyFirstNameAndLastName(agent.FistName, agent.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step20: Navigate to Change Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Navigate to Change Password";
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.ChangePassword, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept21: Change Password  for Test Agent to default
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password  for Test Agent to default";
                var cSP_ChangePassword = new CSPortal_ChangePassword(DriverContext);
                string agen_OldPassword = agent.Password;
                string agent_DefaultPassword = AgentValues.Agentpassword; 
                cSP_ChangePassword.EnterPasswordDetails(agen_OldPassword, agent_DefaultPassword, agent_DefaultPassword, out string ValidationMessage);
                cSP_ChangePassword.SavePassword();
                stepstatus = cSP_ChangePassword.VerifySuccessMessage(agen_OldPassword, agent_DefaultPassword, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step22: Logout As Test User 
                stepName = "Logout from CS Portal User:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("Logout is Successful as User:" + agent.UserName);
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
