using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-266 : Verify Active And InActive Status Of Agents
    /// </summary>
    [TestClass]
    public class BTA266_CSP_Verify_Active_and_InActive_status_of_Agents : ProjectTestBase
    {
        Login login = new Login();
        AttributeSet attribute = new AttributeSet();

        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// BTA-266 : Verify Active And InActive Status Of Agents
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA266_CSP_VerifyActiveandInActiveStatusofAgents()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            AgentRegistration agent_admin = new AgentRegistration();
            AgentRegistration agent_Sradmin = new AgentRegistration();

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var agentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
                var userAdministration = new CSPortal_UserAdministration(DriverContext);
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
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

                #region Step3:Edit AdminAgent status Active to Inactive
                stepName = "Edit AdminAgent status Active to Inactive";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out string output);
                userAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                agent_admin.FistName = "AdminFirstName";
                agent_admin.LastName = "AdminLastName";
                agent_admin.Role = RoleValue.Admin.ToString();
                agent_admin.UserName = AgentValues.AdminAgent;
                agent_admin.Password = AgentValues.Agentpassword;
                agent_admin.Status = AgentRegistration.AgentStatus.Active.ToString();
                var status = agentPage.EditAgentStatus(agent_admin, AgentRegistration.AgentStatus.InActive.ToString(), out string result);
                testStep.SetOutput(result);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Edit SrAdminAgent status Active to Inactive
                stepName = "Edit SrAdminAgent status Active to Inactive";
                testStep = TestStepHelper.StartTestStep(testStep);
                agent_Sradmin.FistName = "SrFirstName";
                agent_Sradmin.LastName = "SrLastName";
                agent_Sradmin.Role = RoleValue.SrAdmin.ToString();
                agent_Sradmin.UserName = AgentValues.SrAdminAgent;
                agent_Sradmin.Password = AgentValues.Agentpassword;
                agent_Sradmin.Status = AgentRegistration.AgentStatus.Active.ToString();
                status = agentPage.EditAgentStatus(agent_Sradmin, AgentRegistration.AgentStatus.InActive.ToString(), out result);
                testStep.SetOutput(result);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify AdminAgent login in inactive state.
                stepName = "Verify AdminAgent login in Inactive state";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.AdminAgent;
                login.Password = AgentValues.Agentpassword;

                var testResult = agentPage.VerifyLogin(login, AgentRegistration.AgentStatus.InActive.ToString(), out output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, testResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify SrAdminAgent login in inactive state.
                stepName = "Verify SrAdminAgent login in Inactive state";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.SrAdminAgent;
                login.Password = AgentValues.Agentpassword;
                testResult = agentPage.VerifyLogin(login, AgentRegistration.AgentStatus.InActive.ToString(), out output);testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, testResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit AdminAgent status Inactive to Active
                stepName = "Edit AdminAgent status Inactive to Active";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out output);
                userAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                status = agentPage.EditAgentStatus(agent_admin, AgentRegistration.AgentStatus.Active.ToString(), out result);
                testStep.SetOutput(result);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Edit SrAdminAgent status Inactive to Active
                stepName = "Edit SrAdminAgent status Inactive to Active";
                testStep = TestStepHelper.StartTestStep(testStep);
                agent_Sradmin.Status = AgentRegistration.AgentStatus.Active.ToString();
                status = agentPage.EditAgentStatus(agent_Sradmin, AgentRegistration.AgentStatus.Active.ToString(), out result);
                testStep.SetOutput(result);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify AdminAgent login in active state. 
                stepName = "Verify AdminAgent login in active state";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.AdminAgent;
                login.Password = AgentValues.Agentpassword;
                testResult = agentPage.VerifyLogin(login, AgentRegistration.AgentStatus.Active.ToString(), out result);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, testResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify SrAdminAgent login in active state. 
                stepName = "Verify SrAdminAgent login in active state";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.SrAdminAgent;
                login.Password = AgentValues.Agentpassword;
                testResult = agentPage.VerifyLogin(login, AgentRegistration.AgentStatus.Active.ToString(), out output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, testResult, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Logout
                stepName = "Logout";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
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