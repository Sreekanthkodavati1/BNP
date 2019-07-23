using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnp.Core.WebPages.Navigator.FrameworkConfig;
using BnPBaseFramework.Web;
using System.IO;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using System.Collections.Generic;
using System;
using System.Configuration;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.Admin;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA83_Navigator_Generate_FrameworkConfigFiles : ProjectTestBase
    {
        public string OrderTest_Status = "";
        public BTA83_Navigator_Generate_FrameworkConfigFiles()
        {
        }
        public BTA83_Navigator_Generate_FrameworkConfigFiles(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;       

        [TestMethod]
        [TestCategory("Navigator_Temp")]
        public void BTA83_Navigator_GenerateFrameworkCfgFile()
        {
            #region Object declaration            
            string configPath = BnPBaseFramework.Web.Helpers.FilesHelper.GetFolder(BaseConfiguration.DownloadFolder, Directory.GetCurrentDirectory());
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            string FrameworkCfgFile = basePages.ConfigDownloadPath + @"\Framework.cfg";
            basePages.DeleteExistedFile(FrameworkCfgFile);
            testStep = new TestStep();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            #endregion
            try
            {
                #region OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion 

                #region Object initilization
                var loginPage = new Navigator_LoginPage(DriverContext);
                var manageOrganizationPage = new Navigator_Orgnizations_FrameworkConfigurationPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(driverContext);
                var organizationPage = new Navigator_Admin_OrganizationsPage(driverContext);
                #endregion

                #region Step1: Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login to navigator using ADMIN
                stepName = "Login to navigator using ADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.AdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                loginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Navigate to organization page
                stepName = "Navigate to organization page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization); testStep.SetOutput("Successfully navigated to Organization page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Select Organization and Environment
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                manageOrganizationPage.DrillDownOrg(out string Message);
                manageOrganizationPage.SelectEnvironment(out string Message1); testStep.SetOutput(Message + ";" + Message1);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Navigate to Framework Configuration tab
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                organizationPage.NavigateToOrganizationsTabs(Navigator_Admin_OrganizationsPage.OrganizationsTabs.FrameworkConfiguration, out Message); testStep.SetOutput(Message + ";" + Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Update changes to the framework configuration page and file
                stepName = "Update changes to the framework configuration page and file";
                testStep = TestStepHelper.StartTestStep(testStep);
                manageOrganizationPage.ExportFrameworkCfgFile(FrameworkCfgFile);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Copy framework configuration file to configuration path
                stepName = "Copy framework configuration file to configuration path";
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.CreateOrVerfiyConfigFolder();
                File.Copy(FrameworkCfgFile, basePages.ConfigUploadPath + @"\Framework.cfg", true);
                basePages.VerifyExistedorDownloadedFile(basePages.ConfigUploadPath + @"\Framework.cfg", "Framework.cfg File uploaded Successfully in config path:" + basePages.ConfigUploadPath, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout from USER page
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Passed");
            }
            catch (Exception e)
            {
ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Failed");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                if (!OrderTest_Status.Contains("true"))
                {
                    Assert.Fail(); testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());

                }
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