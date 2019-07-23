using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Admin;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-80 Navigator _ Create Organizations and Environments 
    /// </summary>
    [TestClass]
    public class BTA80_Navigator_Create_Organizations_And_Environments : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        public string OrderTest_Status = "";
        public BTA80_Navigator_Create_Organizations_And_Environments()
        {
        }
        public BTA80_Navigator_Create_Organizations_And_Environments(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }

        /// <summary>
        /// Test Method BTA-80 Navigator _ Create Organizations and Environments 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA80_Navigator_CreateOrganizationsAndEnvironments()
        {
            testStep = new TestStep();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region OrderExecution Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion 

                #region Object Initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_AdminPage = new Navigator_Admin_UsersPage(DriverContext);
                var organizationsPage = new Navigator_Admin_OrganizationsPage(DriverContext);
                #endregion

                #region Step1 : Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2 : Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.AdminUser;
                login.Password = NavigatorUsers.AdminUser_Password;
                navigator_LoginPage.Login(login,Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3 : Navigate to Organization page
                var application_Nav_Util_Page = new Application_Nav_Util_Page(driverContext);                
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                #endregion

                #region Step4 : Create Organization with default development envionment if Organization does not exists
                stepName = "Create Organization with default development envionment if it does not exists";
                testStep = TestStepHelper.StartTestStep(testStep);                
                var orgName = ProjectBasePage.Orgnization_value;
                organizationsPage.CreateOrganization(orgName, out string outStatus);
                testStep.SetOutput(outStatus);                
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5 : Create QA envionment if it does not exists
                stepName = "Create QA envionment if it does not exists";
                testStep = TestStepHelper.StartTestStep(testStep);    
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                organizationsPage.CreateQAEnvironment(orgName, out  outStatus);
                testStep.SetOutput(outStatus);               
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6 : Logout from BTAADMIN
                stepName = "Logout from BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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
