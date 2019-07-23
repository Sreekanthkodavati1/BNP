using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Admin;
using Bnp.Core.WebPages.Navigator.Database;
using Bnp.Core.WebPages.Navigator.Keys;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using JsonParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-846 : Create Admin users, login with created admin users and Edit admin User Roles
    /// </summary>
    [TestClass]
    public class BTA839_Regression_Navigator_AdminUsers_UserManagementPage : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;


        /// <summary>
        /// User Story BTA-846 : Create Admin users and Edit admin User Roles with Loyalty Navigator Admin
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_25_LN_EditUserRole()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string WebuserName = adminPage.GetUserName(NavigatorUsers.WebUser, Users.AdminRole.WEB.ToString());
                WebuserName =ProjectBasePage. GetUserInfo("RegressionUser", "username");
                //WebuserName + "_" + RandomDataHelper.RandomString(5);
                string DbauserName = adminPage.GetUserName(NavigatorUsers.DBAUser, Users.AdminRole.DBA.ToString());
                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using LWAdminUser
                stepName = "Login to Navigator using LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.AdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create WEBAdmin user and assign role
                stepName = "Verify Regression user and assign role as Web Admin user";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user = adminPage.UserDetails(WebuserName, password);
                testStep.SetOutput(adminPage.Create_AdminUsers(user, Users.AdminRole.WEB.ToString()));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Logout from LWAdminUser
                stepName = "Logout from LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using WEBAdmin
                stepName = "Login to Navigator using Regression User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = WebuserName;
                login.Password = password;
                navigator_LoginPage.Login(login, Users.AdminRole.WEB.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from WEBAdmin
                stepName = "Logout from WEBAdmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage1); testStep.SetOutput(LaunchMessage1);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Login to Navigator using LWAdminUser
                stepName = "Login to Navigator using LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.AdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out string stroutput1); testStep.SetOutput(stroutput1);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Update WEBAdmin user to DBAAdmin user 
                stepName = "Update Regression user to DBAAdmin user";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                var user1 = adminPage.UserDetails(WebuserName, password);
                adminPage.ClickEditUser_And_AssignRoles(WebuserName, Users.AdminRole.DBA.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Logout from LWAdminUser
                stepName = "Logout from LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Login to Navigator using BTADBAADMIN
                stepName = "Login to Navigator using Regression User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = DbauserName;
                login.Password = password;
                navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Logout from BTADBAADMIN
                stepName = "Logout from BTADBAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Create and Delete ADMIN user 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_27_Navigator_LN_DeleteUser()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                string DbauserName = adminPage.GetUserName(NavigatorUsers.DBAUser, Users.AdminRole.DBA.ToString());

                DbauserName = ProjectBasePage.GetUserInfo("RegressionUser", "username");
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                #endregion

                #region Step1:Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Navigator using LWAdminUser
                stepName = "Login to Navigator using LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = password;
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

                #region Step4:Delete Newely Created User
                stepName = "Delete Newely Created User";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(adminPage.Delete_AdminUsers(user));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Logout from LWAdminUser
                stepName = "Logout from LWAdminUser";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Login to Navigator using Deleted User and Verify
                stepName = "Login to Navigator using Deleted User and Verify";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = DbauserName;
                login.Password = password;
                navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out stroutput); testStep.SetOutput(stroutput);
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


        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_8_LN_ValidationsOnKeysPage()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            try
            {
                
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As Key Admin User 
                stepName = "Login As Key Admin User and Navigate to Keys Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_KeyUser_HomePage = new Navigator_KeysHomePage(DriverContext);
                var navigator_MangeKeysPage = new Navigator_ManageKeysPage(DriverContext);
                login.UserName = NavigatorUsers.KEYUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.KEY.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                navigator_KeyUser_HomePage.NavigatetoMangeKeys_Page(out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select Organization and Environment 
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_DBDevHomePage = new Navigator_DBFramework_Page(DriverContext);
                string Org_Output; string Env_Output;
                navigator_DBDevHomePage.DrillDownTestOrg(out Org_Output);
                navigator_DBDevHomePage.SelectTestEnvironment(out Env_Output); testStep.SetOutput(Org_Output + ";" + Env_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Validate KeyPassword and ConfirmKeyPassword fields
                stepName = "Validate KeyPassword and ConfirmKeyPassword fields";
                testStep = TestStepHelper.StartTestStep(testStep);
                var output=navigator_MangeKeysPage.VerifyKeyGeneratedOrNot(); 
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, output, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: LogOut
                stepName = "Logout as KEYADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);

            }
            catch (Exception e)
            {
                testStep.SetOutput(stepName + e);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_7_LN_ChangeKeys()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            string stepOutput = "";
            try
            {

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As Key Admin User 
                stepName = "Login As Key Admin User and Navigate to Keys Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_KeyUser_HomePage = new Navigator_KeysHomePage(DriverContext);
                var navigator_MangeKeysPage = new Navigator_ManageKeysPage(DriverContext);
                login.UserName = NavigatorUsers.KEYUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.KEY.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                navigator_KeyUser_HomePage.NavigatetoMangeKeys_Page(out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select Organization and Environment 
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_DBDevHomePage = new Navigator_DBFramework_Page(DriverContext);
                string Org_Output; string Env_Output;
                navigator_DBDevHomePage.DrillDownTestOrg(out Org_Output);
                navigator_DBDevHomePage.SelectTestEnvironment(out Env_Output); testStep.SetOutput(Org_Output + ";" + Env_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:ChangeKey 
                stepName = "ChangeKey";
                testStep = TestStepHelper.StartTestStep(testStep);
                var output = navigator_MangeKeysPage.ChangeKey("Password1*", "512", out stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, output, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: LogOut
                stepName = "Logout as KEYADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);

            }
            catch (Exception e)
            {
                testStep.SetOutput(stepName + e);
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

        /// <summary>
        /// 26_LN_Create any ADMIN user and edit the admin role information and verify the same
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_26_LN_EditUserInformationAndPassword()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object Initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);

                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string DbauserName = ProjectBasePage.GetUserInfo("RegressionUser", "username");
                #endregion

                #region Step1 - Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2 - Login to Navigator using  LoyaltyWare Administrator
                stepName = "Login to Navigator using  LoyaltyWare Administrator";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = password;
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

                #region Step 4 - Logout from  LoyaltyWare Administrator 
                stepName = "Logout from  LoyaltyWare Administrator";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Login to Navigator using BTADBAADMIN
                stepName = "Login to Navigator using BTADBAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = DbauserName;
                login.Password = password;
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

                #region Step7 - Login to Navigator using  LoyaltyWare Administrator
                stepName = "Login to Navigator using  LoyaltyWare Administrator";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = password;
                navigator_LoginPage.Login(login, Users.AdminRole.LWADM.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8 - Click the Edit User action for the newly-created user and verify the user loaded in edit mode
                stepName = "Click the Edit User action for the newly-created user and verify the user loaded in edit mode";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                testStep.SetOutput(adminPage.ClickEditUser_And_VerifyTheEditModeAndRolesTab(user));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9 - Update user information and save
                stepName = "Update user information and save";
                testStep = TestStepHelper.StartTestStep(testStep);
                adminPage.EditUser(user);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 10 - Logout from  LoyaltyWare Administrator 
                stepName = "Logout from  LoyaltyWare Administrator";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Login to Navigator using updated BTADBAADMIN credentials
                stepName = "Login to Navigator using updated BTADBAADMIN credentials";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = user.UserName;
                login.Password = user.Password;
                navigator_LoginPage.Login(login, Users.AdminRole.DBA.ToString(), out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Logout from BTADBAADMIN
                stepName = "Logout from BTADBAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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