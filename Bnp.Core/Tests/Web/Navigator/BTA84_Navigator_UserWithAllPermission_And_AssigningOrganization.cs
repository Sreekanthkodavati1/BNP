using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Admin;
using Bnp.Core.WebPages.Navigator.UsersPage;
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
    /// User Story BTA-84 : Navigator _ User with all permission and Assigning organization
    /// </summary>
    [TestClass]
    public class BTA84_Navigator_UserWithAllPermission_And_AssigningOrganization : ProjectTestBase
    {
        public string OrderTest_Status = "";


        public BTA84_Navigator_UserWithAllPermission_And_AssigningOrganization()
        {
        }
        public BTA84_Navigator_UserWithAllPermission_And_AssigningOrganization(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }

        Login login = new Login();        
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;
        /// <summary>
        /// Test Method BTA-84 : Navigator _ User with all permission and Assigning organization
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA84_Navigator_CreateUserWithAllPermissions()
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
                #region:OrderExection Requirement
                string Prereq_testCase1 = "BTA81_Navigator_GenerateKeys";
                string Prereq_testCase2 = "BTA82_Navigator_GenerateDBConfig";
                string Prereq_testCase3 = "BTA83_Navigator_GenerateFrameworkCfgFile";
                ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase1, Prereq_testCase2, Prereq_testCase3, methodName, testStep);
                #endregion

                #region Object Initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var adminPage = new Navigator_Admin_UsersPage(driverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                string adminUserName = adminPage.GetUserName(NavigatorUsers.AdminUser, Users.AdminRole.LWADM.ToString());
                string adminPassword= adminPage.GetUserName(NavigatorUsers.AdminUser_Password, Users.AdminRole.LWADM.ToString());

                #endregion

                #region Step1 - Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep,stepName , true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2 - Login to Navigator using BTAADMIN
                stepName = "Login to Navigator using BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = adminUserName;
                login.Password = adminPassword;
                navigator_LoginPage.Login(login,Users.AdminRole.LWADM.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 3 - Create BTAUSER_DEV user and assign role to Org
                stepName = "Create BTAUSER_DEV user and assign role to Org";
                testStep = TestStepHelper.StartTestStep(testStep);                              
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.users);
                string AllRolesUserName = adminPage.GetUserName(NavigatorUsers.NonAdminUser, Users.AdminRole.USER.ToString());
                string AllRolesUserName_Password = adminPage.GetUserName(NavigatorUsers.NonAdminUser_Password, Users.AdminRole.USER.ToString());

                var orgName = ProjectBasePage.Orgnization_value;                  
                var user = adminPage.UserDetails(AllRolesUserName, password);
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(adminPage.CreateUserWithAllRoles(user,orgName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 4 - Logout from bpadmin 
                stepName = "Logout from BTAADMIN";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep,stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 5 - Login with newly created USER
                stepName = "Login with newly created USER";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AllRolesUserName;
                login.Password = AllRolesUserName_Password;
                navigator_LoginPage.Login(login,Users.AdminRole.USER.ToString(), out  stroutput); testStep.SetOutput(stroutput);
                navigator_UsersHomePage.Navigator_Users_ClickHome();
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString().ToString(), "Passed");
            }
            catch (Exception e)
            {
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString().ToString(), "Failed");
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
