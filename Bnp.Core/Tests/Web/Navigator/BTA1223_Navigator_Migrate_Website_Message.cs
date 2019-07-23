using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using Bnp.Core.WebPages.Navigator.UsersPage.Website;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA1223_Navigator_Migrate_Website_Message : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// BTA_165 Migrate Website with 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_165_Navigator_Migrate_Website()
        {
            #region Object Initialization
            Migration Migration = new Migration(driverContext);
            var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3 :Create Member portal website if it does not exists              
                stepName = "Navigate to Website application and Create org_env_MP website if it does not exists  ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                Portal portalMP = new Portal
                {
                    WebSiteName = WebsiteData.MemberPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.MP.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.MemberFacing.ToString()
                };
                testStep.SetOutput(websitePage.Create_MP(portalMP));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Search and Drag Account Status Attribute name into CFUpdateProfile_Config Configuration file 
                stepName = "Search and Drag Account Status Attribute name into CFUpdateProfile_Config Configuration file ";
                testStep = TestStepHelper.StartTestStep(testStep);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg);
                var webSiteName = WebsiteData.MemberPortal_WebSiteName;
                var moduleType = EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.MemberProfile);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                AttributeSet attribute = new AttributeSet();
                string source = "Account Status", target = "SmsOptIn";
                testStep.SetOutput(Website_Modules.AddAttributeInCFUpdateProfile_Config(source, target));
                Website_Modules.SaveConfigSetting();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify Website Module In QA Env and Delete existing 
                stepName = "Verify Website Module In QA Env and Delete existing";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg_Outpu);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                string stepOutput = Website_Modules.VerifyAttributeInQAEnv(source);
                testStep.SetOutput(stepOutput);
                if (stepOutput.Equals(source + " is exists in CFUpdateProfile_Config file"))
                { Website_Modules.DeleteAttributeFromFile(source); }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set isf any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Website_Set.ToString();
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Website_Set.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                string ModuleName = "BrierleyPortal:MemberProfile:CFUpdateProfile_Config";
                _MigrationPage.SelectItemsForWebsiteModuleMigration(ModuleName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify Website Module Migrated with Added attribute name in CFUpdateProfile_Config file
                stepName = "Verify Website Module Migrated  with Added attribute name in CFUpdateProfile_Config file";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg1);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                testStep.SetOutput(Website_Modules.VerifyAttributeInQAEnv("Account Status"));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Logout
                stepName = "Logout from USER page";
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
        /// BTA_119 Migrate Messages with push notification to Upper Env
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_118_Navigator_Migrate_Message_With_PushNotification()
        {
            #region Object Initialization
            Migration Migration = new Migration(driverContext);
            var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var messagesPage = new Navigator_Users_Program_eCollateral_MessagesPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            var attributeSetData = new NonAdminUserData(driverContext);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields messageData = new CategoryFields();
            messageData.Name = "Migration_"+ProjectBasePage.Orgnization_value + NonAdminUserData.MessageNameWithPushNotification;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            messageData.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
            messageData.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            CategoryFields notification = new CategoryFields
            {
                Name = attributeSetData.Migrate_Notification,
                StartDate = DateTime.Now.ToString("MM/dd/yyyy"),
                ExpiryDate = DateTime.Now.AddDays(15).ToString("MM/dd/yyyy")
            };

            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create Notification if not Exists in Dev environment
                stepName = "Create new notification, if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool stepStatus = basePages.VerifyNotification_IfNotExistsCreateNew(ProjectBasePage.Env_value, notification, out string stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create new message if message does not exist already              
                stepName = "Create new message if message does not exist already";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                testStep = messagesPage.CreateNewMessageWithPushNotification(messageData,notification, out string messageStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step4: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify and Delete Message if available in QA Env
                stepName = "Verify and Delete Message if available in QA Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                bool OutputStr = messagesPage.DeleteIfMessageExists(messageData.Name, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Navigate to Migration Page and Delete Message Migration Set if any
                stepName = "Navigate to Migration Page and Delete Message Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_MessageWithPushNotification_Set.ToString();
                bool status = _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion              

                #region Step7:Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_MessageWithPushNotification_Set.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GetDate("Current"));
                _MigrationPage.SelectItemsForMessage(messageData.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify Migrated Message is available in QA Env
                stepName = "Verify Migrated Message is available in QA Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                bool Out_status = messagesPage.VerifyMessageExists(messageData.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Logout
                stepName = "Logout from USER page";
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
        /// BTA_119 Migrate Messages to Upper Env
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_119_Navigator_Migrate_Message()
        {
            #region Object Initialization
            Migration Migration = new Migration(driverContext);
            var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var messagesPage = new Navigator_Users_Program_eCollateral_MessagesPage(driverContext);
            string randomStr = RandomDataHelper.RandomString(4);
            var attributeSetData = new NonAdminUserData(driverContext);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields messageData = new CategoryFields();
            messageData.Name = ProjectBasePage.Orgnization_value + NonAdminUserData.MessageName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            messageData.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
            messageData.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create new message if message does not exist already              
                stepName = "Create new message if message does not exist already";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                testStep = messagesPage.CreateNewMessage(messageData, out string messageStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step4: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify and Delete Message if available in QA Env
                stepName = "Verify and Delete Message if available in QA Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                bool OutputStr = messagesPage.DeleteIfMessageExists(messageData.Name, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Navigate to Migration Page and Delete Message Migration Set if any
                stepName = "Navigate to Migration Page and Delete Message Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Message_Set.ToString();
                bool status = _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion              

                #region Step7:Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Message_Set.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GetDate("Current"));
                _MigrationPage.SelectItemsForMessage(messageData.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify Migrated Message is available in QA Env
                stepName = "Verify Migrated Message is available in QA Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages);
                bool Out_status = messagesPage.VerifyMessageExists(messageData.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Logout
                stepName = "Logout from USER page";
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