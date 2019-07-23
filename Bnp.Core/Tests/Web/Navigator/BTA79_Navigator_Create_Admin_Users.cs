using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Admin;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-79 : Create Admin users and login with created admin users
    /// </summary>
    [TestClass]
    public class BTA79_Navigator_Create_Admin_Users : ProjectTestBase
    {
        public string OrderTest_Status = "";
        public BTA79_Navigator_Create_Admin_Users()
        {
        }
        public BTA79_Navigator_Create_Admin_Users(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        Login login = new Login();
        public TestCase testCase;
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Create ADMIN user and login with newly created ADMIN user
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser()
        {
            List<TestStep> listOfTestSteps = new List<TestStep>();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            // testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "");
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string adminPassword = adminPage.GetUserName(NavigatorUsers.AdminUser_Password, Users.AdminRole.LWADM.ToString());
                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using bpadmin
                stepName = "Login to Navigator using bpadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.bpAdminUser;
                login.Password = NavigatorUsers.bpAdminUser_Password;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create LWadmin user and assign role
                stepName = "Create LWadmin user and assign role";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user = adminPage.UserDetails(adminUserName, password);
                testStep.SetOutput(adminPage.Create_AdminUsers(user, Users.AdminRole.LWADM.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout from bpadmin
                stepName = "Logout from bpadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = adminPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from BTAADMIN
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
                    Assert.Fail();
                    testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                }
            }
            finally
            {
                testCase.SetTestCaseSteps(listOfTestSteps);
                testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
                listOfTestCases.Add(testCase);
            }
        }

        /// <summary>
        /// Create DBA ADMIN user and login with newly created DBA ADMIN user
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA79_Navigator_CreateDBAAdminUser_And_Login_With_DBAAdminUser()
        {
            List<TestStep> listOfTestSteps = new List<TestStep>();

            //listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);

            try
            {

                #region:OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion

                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string DbauserName = adminPage.GetUserName(NavigatorUsers.DBAUser, Users.AdminRole.DBA.ToString());
                string DbauserPassword = adminPage.GetUserName(NavigatorUsers.DBAUser_Password, Users.AdminRole.DBA.ToString());
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string adminPassword = adminPage.GetUserName(NavigatorUsers.AdminUser_Password, Users.AdminRole.LWADM.ToString());
                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = adminPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create DBAAdmin user and assign role
                stepName = "Create DBAAdmin user and assign role";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user = adminPage.UserDetails(DbauserName, password);
                testStep.SetOutput(adminPage.Create_AdminUsers(user, Users.AdminRole.DBA.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout from admin
                stepName = "Logout from admin";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using BTADBAADMIN
                stepName = "Login to Navigator using BTADBAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = DbauserName;
                login.Password = DbauserPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from BTADBAADMIN
                stepName = "Logout from BTADBAADMIN";
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

        /// <summary>
        /// Create KEY ADMIN user and login with newly created KEY ADMIN user
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA79_Navigator_CreateKEYAdminUser_And_Login_With_KEYAdminUser()
        {
            testStep = new TestStep();
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);

            List<TestStep> listOfTestSteps = new List<TestStep>();

            string stepName = "";
            try
            {
                #region:OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion

                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string adminPassword = adminPage.GetUserName(NavigatorUsers.AdminUser_Password, Users.AdminRole.LWADM.ToString());

                string KeyuserName = adminPage.GetUserName(NavigatorUsers.KEYUser, Users.AdminRole.KEY.ToString());
                string KeyuserPassword = adminPage.GetUserName(NavigatorUsers.KEYUser_Password, Users.AdminRole.KEY.ToString());

                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = adminPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create KEYAdmin user and assign role
                stepName = "Create KEYAdmin user and assign role";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user = adminPage.UserDetails(KeyuserName, password);
                testStep.SetOutput(adminPage.Create_AdminUsers(user, Users.AdminRole.KEY.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout from admin
                stepName = "Logout from admin";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using BTAKEYADMIN
                stepName = "Login to Navigator using BTAKEYADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = KeyuserName;
                login.Password = KeyuserPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.KEY.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from BTAKEYADMIN
                stepName = "Logout from BTAKEYADMIN";
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

        /// <summary>
        /// Create WEB ADMIN user and login with newly created WEB ADMIN user
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA79_Navigator_CreateWEBAdminUser_And_Login_With_WEBAdminUser()
        {

            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            List<TestStep> listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            try
            {
                #region:OrderExection Requirement
                string Prereq_testCase = "BTA79_Navigator_CreateAdminUser_And_Login_With_AdminUser";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase, methodName, testStep);
                #endregion

                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string adminPassword = adminPage.GetUserName(NavigatorUsers.AdminUser_Password, Users.AdminRole.LWADM.ToString());
                string WebuserName = adminPage.GetUserName(NavigatorUsers.WebUser, Users.AdminRole.WEB.ToString());
                string WebuserPassword = adminPage.GetUserName(NavigatorUsers.WebUser_Password, Users.AdminRole.LWADM.ToString());
                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = adminPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create WEBAdmin user and assign role
                stepName = "Create WEBAdmin user and assign role";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user = adminPage.UserDetails(WebuserName, password);
                testStep.SetOutput(adminPage.Create_AdminUsers(user, Users.AdminRole.WEB.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout from bpadmin
                stepName = "Logout from bpadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using BTAWEBADMIN
                stepName = "Login to Navigator using BTAWEBADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = WebuserName;
                login.Password = WebuserPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.WEB.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from BTAWEBADMIN
                stepName = "Logout from BTAWEBADMIN";
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
