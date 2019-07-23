using System;
using System.Collections.Generic;
using System.Configuration;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA123_CSP_CreateDifferentAgents : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA123_CSPCreateDifferentAgent()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            bool stepstatus;
            string stepName = "";
            #region Object Declaration
            var CSP_HomePage = new CSPortal_HomePage(DriverContext);
            var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
            var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
            AgentRegistration agent = new AgentRegistration();
            #endregion

            try
            {
                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(this.DriverContext);
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

                #region Step3:Navigate to User Administration                
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create AdminAgent if it does not exists
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create AdminAgent if it does not exists";
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                agent.FistName = "AdminFirstName";
                agent.LastName = "AdminLastName";
                agent.Role = RoleValue.Admin.ToString();
                agent.UserName = AgentValues.AdminAgent;
                agent.Password = AgentValues.Agentpassword;
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify Login As Admin Agent Verify Dashboard Links and Logout
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Login As AdminAgent Verify Dashboard Links and Logout";
                CSP_HomePage.LogoutCSPortal();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.VerifyDashboard_Links(agent.UserName));
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create SrAdminAgent if it does not exists
                testStep = TestStepHelper.StartTestStep(testStep);
                agent.FistName = "SrFirstName";
                agent.LastName = "SrLastName";
                agent.Role = RoleValue.SrAdmin.ToString();
                agent.UserName = AgentValues.SrAdminAgent;
                agent.Password = AgentValues.Agentpassword;
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepName);
                stepName = "Create SrAdminAgent if it does not exists";
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Login As Sr.Admin Agent Verify Dashboard Links and Logout
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Login As SrAdmin Verify Dashboard Links and Logout";
                CSP_HomePage.LogoutCSPortal();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.VerifyDashboard_Links(agent.UserName));
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create JrAdminAgent if it does not exists
                testStep = TestStepHelper.StartTestStep(testStep);
                agent.FistName = "JrFirstName";
                agent.LastName = "JrLastName";
                agent.Role = RoleValue.JrAdmin.ToString();
                agent.UserName = AgentValues.JrAdminAgent;
                agent.Password = AgentValues.Agentpassword;
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepName);
                stepName = "Create JrAdminAgent if it does not exists";
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Verify Login As Jr Admin Agent Verify Dashboard Links and Logout
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Login As JrAdminAgent Verify Dashboard Links and Logout";
                CSP_HomePage.LogoutCSPortal();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.VerifyDashboard_Links(agent.UserName));
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
