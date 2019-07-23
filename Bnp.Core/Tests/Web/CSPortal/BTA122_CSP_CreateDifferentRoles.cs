using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// User Story BTA-122 : Customer Service Portal - Create Different Roles
    /// </summary>
    [TestClass]
    public class BTA122_CSP_Create_Different_Roles : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// Test BTA-122 : Customer Service Portal - Create Different Roles(AdminRole, SrAdmin and JrAdmin) if it already does not exists
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA122_CSP_CreateDifferentRoles()
        {
            ProjectBasePage basePages = new ProjectBasePage(this.driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            bool stepstatus;
            string stepName = "";

            #region Object Initialization
            var homePage = new CSPortal_HomePage(DriverContext);
            var userAdministrationPage = new CSPortal_UserAdministration(DriverContext);
            var loginPage = new CSPortal_LoginPage(DriverContext);
            var rolePage = new CSPortal_UserAdministration_RolePage(DriverContext);
            #endregion
            try
            {
                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                loginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3 : Navigate to User Administration                
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = homePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4 : Navigate to Roles                
                stepName = "Navigate to Roles";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = userAdministrationPage.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Roles);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5 : Create AdminRole if it does not exists
                stepName = "Create AdminRole if it does not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                var role = RoleValue.Admin;
                var pointAwardLimit = RoleValue.AdminRole_PointAwardLimit;
                stepstatus = rolePage.CreateNewRole(role, pointAwardLimit, out string stat);
                testStep.SetOutput(stat);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6 : Create SrAdminRole if it does not exists
                stepName = "Create SrAdminRole if it does not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                role = RoleValue.SrAdmin;
                pointAwardLimit = RoleValue.SrAdminRole_PointAwardLimit;
                stepstatus = rolePage.CreateNewRole(role, pointAwardLimit, out stat);
                testStep.SetOutput(stat);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7 : Create JrAdminRole if it does not exists
                stepName = "Create JrAdminRole if it does not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                role = RoleValue.JrAdmin;
                pointAwardLimit = RoleValue.JrAdminRole_PointAwardLimit;
                stepstatus = rolePage.CreateNewRole(role, pointAwardLimit, out stat);
                testStep.SetOutput(stat);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8 : Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                homePage.LogoutCSPortal();
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
