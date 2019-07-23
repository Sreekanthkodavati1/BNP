using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-270 : Change Password of CS Portal user and Verify Login with change Password
    /// </summary>
    [TestClass]
    public class BTA270_CSP_ChangePassword : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        /// <summary>
        /// BTA-270 : Change Password of CS Portal user and Verify Login with change Password
        /// Detailed Description: Use Exclusive agent to Change Password , if user is not existed create new. Change Password using 
        /// csadmin and Change password by Dashboard menu option
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        [TestCategory("CSPortal-Smoke")]
        public void BTA270_CSP_Change_Password()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string StepOutput;
            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
                var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
                AgentRegistration agent = new AgentRegistration();

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

                #region Step3:Navigate to User Administration         
                stepName = "Navigate to UserAdministration Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out StepOutput); testStep.SetOutput(StepOutput);
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
                agent.FistName = AgentValues.ChangePasswordTestAgent;
                agent.LastName = AgentValues.ChangePasswordTestAgent;
                agent.Role = RoleValue.Admin.ToString();
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                agent.UserName = AgentValues.ChangePasswordTestAgent;
                stepName = "Create New Agent if user is not existed";
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Change Password for Test Agent
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password for Test Agent";
                stepstatus = CSPortal_UserAdministrationAgentPage.ChangeAgentPassword(agent.UserName, agent.Password, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Logout As Test User 
                stepName = "Logout from CS Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as csadmin");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Login As Test User 
                stepName = "Login As User, User Name:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LoginCSPortal(agent.UserName, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Verify First Name and Last Name
                stepName = "Verify First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(CSP_HomePage.VerifyFirstNameAndLastName(agent.FistName, agent.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Navigate to Change Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Navigate to Change Password";
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.ChangePassword, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept11: Change Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password for Test Agent";
                var cSP_ChangePassword = new CSPortal_ChangePassword(DriverContext);
                string agen_OldPassword = agent.Password;
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                cSP_ChangePassword.EnterPasswordDetails(agen_OldPassword, agent.Password, agent.Password, out string ValidationMessage);
                cSP_ChangePassword.SavePassword();
                stepstatus = cSP_ChangePassword.VerifySuccessMessage(agen_OldPassword, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Logout As Test User 
                stepName = "Logout from CS Portal User:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as User:" + agent.UserName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Login As Test User 
                stepName = "Login As User, User Name:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LoginCSPortal(agent.UserName, agent.Password,out StepOutput); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                #endregion

                #region Step14:Verify First Name and Last Name
                stepName = "Verify First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(CSP_HomePage.VerifyFirstNameAndLastName(agent.FistName, agent.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Logout As Test User 
                stepName = "Logout from CS Portal User:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as User:" + agent.UserName);
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testStep.SetOutput(e.Message);
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