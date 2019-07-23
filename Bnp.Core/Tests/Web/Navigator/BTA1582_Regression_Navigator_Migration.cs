using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Promotion;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Content;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using Bnp.Core.WebPages.Navigator.UsersPage.Website;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA1582_Regression_Navigator_Migration : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_184_LN_Delete_Migration()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 4:Navigate to Migration Page and  Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Test.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 5: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 6:  Delete Migration Set And Verify
                stepName = "Delete Migration Set And Verify";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 7: Logout
                stepName = "Logout from Application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_185_LN_Create_Migration_And_Edititem_Intiate_Approve_Runnow_And_Rollback()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Targeted Promotion";
                stepName = "Create Targeted Promotion";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.MigrationPromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria, out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Promotion.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Promotion.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForPromotionDef(promotions.Name, out _output);
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

                #region Step13: "Verify Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                searchCriteria = "Name";
                result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:RollBack Migration Set
                stepName = "RollBack Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                _MigrationPage.RollbackMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: "Verify Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                searchCriteria = "Name";
                result = navigator_PromotionPage.VerifyPromotionAfterRollback(promotions.Name, searchCriteria, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Logout
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_186_LN_Create_Migrationset_With_Scheduledtime()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            try
            {
                #region
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 4:Navigate to Migration Page and  Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Test.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 5: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output, true);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 6: Logout
                stepName = "Logout from Application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_187_Edititems_And_Verify_Notification_Checkbox()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigate to Migration Page and  Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Test.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Click On Edit Items And Verify Notifications Checkbox
                stepName = "Click On Edit Items And Verify Notifications Checkbox";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ClickOnEditItemsAndVerifyCheckbox(Migration.BuildMigrationSetName, Migration.MigrationCheckBox.Notifications.ToString(), out string OutMsg);
                testStep.SetOutput(OutMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Logout
                stepName = "Logout from Application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_183_LN_Clone_MigrationSet_And_Verify_Edit_And_EditItem_Actions()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create NonTargeted Promotion";
                stepName = "Create NonTargeted Promotion";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.MigrationPromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria, out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Promotion.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Promotion.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForPromotionDef(promotions.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Clone MigrationSet After Generate Items
                stepName = "Clone MigrationSet After Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.CloneMigrationSet(Migration.BuildMigrationSetName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Click On Edit Action And Verify MigrationSet Edit Panel
                stepName = "Click On Edit Action And Verify MigrationSet Edit Panel";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ClickOnEditActionAndVerifyEditPanel(Migration.BuildMigrationSetName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Click On EditItems Action And Verify MigrationSet EditItems Panel
                stepName = "Click On EditItems Action And Verify MigrationSet EditItems Panel";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ClickOnEdiItemsActionAndVerifyEditsItemsPanel(Migration.BuildMigrationSetName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_208_LN_Create_And_Migrate_PushNotification()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                var NotificationPage = new Navigator_Users_Program_eCollateral_NotificationPage(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
                CategoryFields notification = new CategoryFields
                {
                    Name = attributeSetData.Migrate_Notification,
                    StartDate = DateTime.Now.ToString("MM/dd/yyyy"),
                    ExpiryDate = DateTime.Now.AddDays(15).ToString("MM/dd/yyyy")
                };
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create PushNotification";
                stepName = "Create PushNotification";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Notifications);
                NotificationPage.CreatePushNotication(notification, out string outMsg);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_PushNotifications.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForPushNotoficationDef(notification.Name, out _output);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_207_LN_Create_And_Migrate_GooglePay()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                var NotificationPage = new Navigator_Users_Program_eCollateral_NotificationPage(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                var googlePayPage = new Navigator_Users_Program_ECollateralPage_GooglePayPage(DriverContext);
                var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
                GooglePay gpay = new GooglePay
                {
                    Name = attributeSetData.Migrate_GPay + RandomDataHelper.RandomString(3),
                    LogoImage = "TestLogo",
                    ProgramName = "TestProgramName",
                    IssuerName = " TestIssuerName"
                };
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create GooglePay";
                stepName = "Create GooglePay";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.GooglePay);
                ProjectBasePage ProjectBasePage = new ProjectBasePage(driverContext);
                googlePayPage.CreateGooglePay(gpay, out string outMsg);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_GooglePay.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForGooglePayDef(gpay.Name, out _output); testStep.SetOutput(_output);
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
        /// BTA 56 : This method is used to Migrate Product Category, Product and verify the Product Category and Product migrated successfully or not
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_56_LN_Create_And_Migrate_ProductCategory_And_Product()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            Migration Migration = new Migration(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_CreateProduct_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            product.Name = Migration.MigrationSets.Migration_Product.ToString() + RandomDataHelper.RandomString(4);
            string stepName = "";
            bool stepstatus = true;
            #endregion
            try
            {
                #region Object Initialization
                product.SetType = "Product Name";
                var attName = data.AttributeAllContentType;
                #endregion

                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin 
                stepName = "Login As User Admin and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput);
                testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create New Category and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName + RandomDataHelper.RandomString(5);
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateProduct_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.AttributeName = attName + contentType.ToString();
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Product.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Product.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForProductCategoryAndProduct(product.Name, product.CategoryName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Verify the Product category is migrated and displayed on Categories Page
                stepName = "Verify the Product category is migrated and displayed on Categories Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                navigator_CreateProduct_Category.VerifyCategoryExists(product.CategoryName, product.CategoryTypeValue);
                testStep.SetOutput("Category is displayed in Category Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Verify Product Migrated on Product Page
                stepName = "Verify Product Migrated on Product Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                RewardCatalog_ProductsPage.VerifyCreatedProduct(product.SetType, product.Name, product.CategoryName);
                testStep.SetOutput(" Product :" + Migration.MigrationSets.Migration_Product.ToString() + " Migrated Successfully and appeared on Product Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Search and Delete the Migrated product
                stepName = "Deleted Migrated product :" + product.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ProductsPage.DeleteProduct(product, product.SetType, product.Name, product.CategoryName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Search and Delete the Category
                stepName = "Delete Migrated Category : " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateProduct_Category.DeleteCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Switch to Development Environment
                stepName = "Switch to Development Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep.SetOutput("Switched to Development Environment");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Search and Delete the newly created product
                stepName = "Deleted Migrated product :" + product.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                RewardCatalog_ProductsPage.DeleteProduct(product, product.SetType, product.Name, product.CategoryName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18: Search and Delete the newly created Category
                stepName = "Delete Migrated Category : " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateProduct_Category.DeleteCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step19: Logout
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

        /// <summary>
        /// BTA 192 : This method is used to create and migrate Convereted Text Block
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_192_LN_Create_And_Migrate_ConvertedTextBlock()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            TextBlocks textblock = new TextBlocks();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            Migration Migration = new Migration(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ContentPage = new Navigator_Users_ContentHomePage(DriverContext);
            var navigator_Users_TextBlockPage = new Navigator_Users_Content_TextBlocks(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
            string stepName = "";
            #endregion
            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin 
                stepName = "Login As User Admin and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create a new "Converted" text blocks
                stepName = "Create a new Converted text blocks";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                textblock.Name = data.Migration_ConvertedTextBlock;
                textblock.Language = TextBlocks.MultiLanguageSelector.French.ToString();
                textblock.Text = data.Converted_Text;
                navigator_Users_TextBlockPage.CreateTextBlockAndVerify(textblock, out string status);
                testStep.SetOutput(status);
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

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_ConvertedTextBlock.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_ConvertedTextBlock.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForTextBlock(textblock.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify the newly migrated "Converted" text block exists in TextBlocks Page
                stepName = "Verify the newly migrated Converted text block exists in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                bool report = navigator_Users_TextBlockPage.VerifyTextBlockExists(textblock.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Delete the newly migrated "Converted" text block in TextBlocks Page
                stepName = "Delete the newly migrated Converted text block " + textblock + " in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                navigator_Users_TextBlockPage.DeleteTextBlockAndVerify(textblock);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Switch to Development Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep.SetOutput("Switched to Development Environment");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Delete the newly Created "Converted" text block in TextBlocks Page
                stepName = "Delete the newly Created Converted text block "+ textblock + " in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                navigator_Users_TextBlockPage.DeleteTextBlockAndVerify(textblock);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Logout
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

        /// <summary>
        /// BTA 222 : This method is used to create and migrate Non-Convereted Text Block
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_222_LN_Create_And_Migrate_NonConvertedTextBlock()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            TextBlocks textblock = new TextBlocks();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            Migration Migration = new Migration(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ContentPage = new Navigator_Users_ContentHomePage(DriverContext);
            var navigator_Users_TextBlockPage = new Navigator_Users_Content_TextBlocks(DriverContext);
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
            string stepName = "";
            #endregion
            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin 
                stepName = "Login As User Admin and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Create a new "Non-Converted" text blocks
                stepName = "Create a new Converted text blocks";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                textblock.Name = data.Migration_NonConvertedTextBlock;
                textblock.Language = TextBlocks.MultiLanguageSelector.English.ToString();
                textblock.Text = data.NonConverted_Text;
                navigator_Users_TextBlockPage.CreateTextBlockAndVerify(textblock, out string status);
                testStep.SetOutput(status);
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

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_NonConvertedTextBlock.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_NonConvertedTextBlock.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForTextBlock(textblock.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify the newly migrated "Non-Converted" text block exists in TextBlocks Page
                stepName = "Verify the newly migrated Non-Converted text block "+ textblock + " exists in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                bool report = navigator_Users_TextBlockPage.VerifyTextBlockExists(textblock.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Delete the newly migrated "Non-Converted" text block in TextBlocks Page
                stepName = "Delete the newly migrated Non-Converted text block " + textblock + " in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                navigator_Users_TextBlockPage.DeleteTextBlockAndVerify(textblock);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Switch to Development Environment
                stepName = "Switch to Development Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep.SetOutput("Switched to Development Environment");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Delete the newly Created "Non-Converted" text block in TextBlocks Page
                stepName = "Delete the newly Created Non-Converted text block " + textblock + " in TextBlocks Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.content);
                navigator_Users_ContentPage.NavigateToContentTab(Navigator_Users_ContentHomePage.ContentMenuTabs.TextBlocks);
                navigator_Users_TextBlockPage.DeleteTextBlockAndVerify(textblock);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, report, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Logout
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

        /// <summary>
        /// BTA 203 : Create and Migrate MTouchProcessing Module
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_203_LN_Create_And_Migrate_MTouchProcessingModule()
        {

            #region Object Initialization
            bool stepStatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_WebsitePage = new Navigator_Users_WebsitePage(DriverContext);
            var navigator_Users_Website_WebsitesPage = new Navigator_Users_Website_WebsitesPage(DriverContext);
            var navigator_Users_Website_ModulesPage = new Navigator_Users_Website_ModulesPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);
            Migration Migration = new Migration(driverContext);

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            string website = "BTA_Dev_CS";
            string pageName = "Account Summary";
            string moduleName = "MTouch Processing";
            string configName = "Migration_MTouchProcessing";
            string specificMTouch = "Web_MTouchModule" + RandomDataHelper.RandomAlphanumericString(5);
            string moduleArea = "Content";
            string moduleOrder = "3";
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

                #region Step3: Create CS portal website               
                stepName = "Navigate to Website application and Create org_env_CS website";
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                Portal portal = new Portal
                {
                    WebSiteName = WebsiteData.CSPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.CS.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.CSDefaultSkin.ToString()
                };
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(websitePage.Create_CS(portal));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Delete the Module from Websites page if exists in the Dev Env
                stepName = "Delete the Module from Websites page if exists in the Dev Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out string outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Delete Configuration If exists in Modules Page
                stepName = "Delete Configuration If exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CancelConfiguration_IfExists("-- select --", moduleName, configName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create Mtouch Module Configuration If not exists in Modules Page
                stepName = "Create Configuration If not exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CreateNewWebsiteConfigurationForMTouchModuleIfNotExists(website, moduleName, configName, specificMTouch, moduleArea, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Add the module in website page if not exists
                stepName = "Add the module in website page if not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.AddModuleToPageIfNotExists(website, pageName, moduleName, configName, moduleArea, moduleOrder, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_MTouchProcessingModule.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_MTouchProcessingModule.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForWebsiteModule(configName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify migrated module displayed on Website Modules page
                stepName = "Verify migrated module displayed on Website Modules page";
                testStep = TestStepHelper.StartTestStep(testStep); application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.VerifyModulePresentInTheGrid("-- select --", moduleName, configName);
                if (stepStatus)
                    testStep.SetOutput("Module Displayed in the grid.");
                else
                {
                    testStep.SetOutput("Module is not present in the grid.");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                    listOfTestSteps.Add(testStep);
                }
                #endregion

                #region Step16:Switch to Dev Env and Delete the Module from Websites page
                stepName = "Switch to Dev Env and Delete the Module from Websites page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Logout
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


        /// <summary>
        /// BTA 203 : Create and Migrate VisitMap Module
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_204_LN_Create_And_Migrate_VisitMapModule()
        {

            #region Object Initialization
            bool stepStatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_WebsitePage = new Navigator_Users_WebsitePage(DriverContext);
            var navigator_Users_Website_WebsitesPage = new Navigator_Users_Website_WebsitesPage(DriverContext);
            var navigator_Users_Website_ModulesPage = new Navigator_Users_Website_ModulesPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            Migration Migration = new Migration(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            string website = "BTA_Dev_CS";
            string pageName = "Account Summary";
            string moduleName = "Customer Facing - Visit Map";
            string configName = "Migration_VisitMap";
            string specificMTouch = "Web_VisitMap" + RandomDataHelper.RandomAlphanumericString(5);
            string moduleArea = "Content";
            string moduleOrder = "3";
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

                #region Step3: Create CS portal website               
                stepName = "Navigate to Website application and Create org_env_CS website";
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                Portal portal = new Portal
                {
                    WebSiteName = WebsiteData.CSPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.CS.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.CSDefaultSkin.ToString()
                };
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(websitePage.Create_CS(portal));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Delete the Module from Websites page if exists in the Dev Env
                stepName = "Delete the Module from Websites page if exists in the Dev Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out string outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Delete Configuration If exists in Modules Page
                stepName = "Delete Configuration If exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CancelConfiguration_IfExists("-- select --", moduleName, configName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create VisitMap Module Configuration If not exists in Modules Page
                stepName = "Create Configuration If not exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CreateNewWebsiteConfigurationForVisitMapModuleIfNotExists(website, moduleName, configName, moduleArea, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Add the module in website page if not exists
                stepName = "Add the module in website page if not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.AddModuleToPageIfNotExists(website, pageName, moduleName, configName, moduleArea, moduleOrder, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_VisitMap.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_VisitMap.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForWebsiteModule(configName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Verify migrated module displayed on Website Modules page
                stepName = "Verify migrated module displayed on Website Modules page";
                testStep = TestStepHelper.StartTestStep(testStep); application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.VerifyModulePresentInTheGrid("-- select --", moduleName, configName);
                if (stepStatus)
                    testStep.SetOutput("Module Displayed in the grid.");
                else
                {
                    testStep.SetOutput("Module is not present in the grid.");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                    listOfTestSteps.Add(testStep);
                }
                #endregion

                #region Step16:Switch to Dev Env and Delete the Module from Websites page
                stepName = "Switch to Dev Env and Delete the Module from Websites page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Logout
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


        /// <summary>
        /// BTA 205 : Create and Migrate RuleEvents Module
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_205_LN_Create_And_Migrate_RuleEvents()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            Migration Migration = new Migration(driverContext);
            NonAdminUserData promotion = new NonAdminUserData(DriverContext);
            var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
            Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
            Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
            Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
            Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            #region Rule
            Rules Rule = new Rules();
            string RuleEvent = "WebRule";
            Rule.RuleName = "WebRule";

            Rules_Configurations_CreateVirtual_Card RuleConfig = new Rules_Configurations_CreateVirtual_Card();
            RuleConfig.Skipcreation_Onexistingvirtualcard = true;
            RuleConfig.LoyaltyId_GenerationSource = "Generate New ID";
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
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

                #region Step3: Create new Rule Event and Verify
                stepName = "Create new Rule Event and Verify ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage);
                testStep.SetOutput(EventMessage);
                testStep.SetOutput("Create New Rule Event in Rules Page");
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

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_RuleEvent.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_RuleEvent.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForRuleEvent(Rule.RuleName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify Rule Event is Successfully Migrated and available in Rule Events Page
                stepName = "Verify Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                _RuleEvent.VerifyRuleInRuleEventPage(RuleEvent);
                testStep.SetOutput("Rule Event is migrated Successfully in Rule Events Page");
                testStep.SetOutput("Rule Envent Migrated successfully and avialable in New Rule in Rules Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Delete Rule Event which is Migrated and available in Rule Events Page
                stepName = "Delete newly migrated Rule Event " + RuleEvent + " from Rule Events Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.DeleteEvent(RuleEvent, out EventMessage);
                testStep.SetOutput(EventMessage);
                testStep.SetOutput("Rule Envent Migrated successfully and avialable in New Rule in Rules Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Switch to Development Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep.SetOutput("Switched to Development Environment");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Delete newly created Rule Event from Rule Events Page
                stepName = "Delete newly created Rule Event " + RuleEvent  + " from Rule Events Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.DeleteEvent(RuleEvent, out EventMessage);
                testStep.SetOutput(EventMessage);
                testStep.SetOutput("Rule Envent Migrated successfully and avialable in New Rule in Rules Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Switch to Migration Environment
                stepName = "Switching to Other Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Logout
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_206_LN_Create_And_Migrate_AttributeSet()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_AttributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                AttributeSet attribute = new AttributeSet();
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigating Model and Navigate to Attribute Set page
                stepName = "Navigating Model and Navigate to Attribute Set page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var models_HomePage = new Navigator_ModelHomePage(DriverContext);
                models_HomePage.NavigateToModels_Page(out string Pageoutput); testStep.SetOutput(Pageoutput);
                models_HomePage.NavigatetoToAttributeSet_Page(out Pageoutput); testStep.SetOutput(Pageoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create Attributes set , Add  Attribute sets
                var attributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                attribute.MainAttributeSets = "Member";
                string randomStr = RandomDataHelper.RandomString(4);
                attribute.AttributeSets = "MigrationTest";
                attribute.Attributes = attributeSetData.Attribute;
                stepName = "Create Attributes set" + attribute.MainAttributeSets + "  Add  Attributes to the attribute set:" + attribute.Attributes + " is Successful and Generate Table is successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeSetPage.CreateAttributeSet(attribute.MainAttributeSets, attribute.AttributeSets);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_AttributeSet.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForAttributeSet(attribute.AttributeSets, out _output);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_209_LN_Create_And_Migrate_RefCountry()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_AttributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                AttributeSet attribute = new AttributeSet();
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigating Model and Navigate to Attribute Set page
                stepName = "Navigating Model and Navigate to Attribute Set page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var models_HomePage = new Navigator_ModelHomePage(DriverContext);
                models_HomePage.NavigateToModels_Page(out string Pageoutput); testStep.SetOutput(Pageoutput);
                models_HomePage.NavigatetoToAttributeSet_Page(out Pageoutput); testStep.SetOutput(Pageoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create Attributes set , Add  Attribute sets
                var attributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                attribute.MainAttributeSets = "Reference Data";
                string randomStr = RandomDataHelper.RandomString(4);
                attribute.AttributeSets = "RefCountry";
                attribute.Attributes = "MigrateChildAttributeTest";
                stepName = "Create Attributes set" + attribute.MainAttributeSets + "  Add  Attributes to the attribute set:" + attribute.Attributes + " is Successful and Generate Table is successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeSetPage.CreateAttributes(attribute.MainAttributeSets, attribute.AttributeSets, attribute.Attributes, attribute.Attributes, attribute.Attributes, "String", "1", "20").Contains("Attribute Element is Created Successfully");
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_AttributeSet.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForRefCountry(attribute.AttributeSets, out _output);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_200_LN_Create_And_Migrate_Rollback_RefData()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_AttributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                AttributeSet attribute = new AttributeSet();
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigating Model and Navigate to Attribute Set page
                stepName = "Navigating Model and Navigate to Attribute Set page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var models_HomePage = new Navigator_ModelHomePage(DriverContext);
                models_HomePage.NavigateToModels_Page(out string Pageoutput); testStep.SetOutput(Pageoutput);
                models_HomePage.NavigatetoToAttributeSet_Page(out Pageoutput); testStep.SetOutput(Pageoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create Attributes set , Add  Attribute sets
                var attributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                attribute.MainAttributeSets = "Reference Data";
                string randomStr = RandomDataHelper.RandomString(4);
                attribute.AttributeSets = "ReferenceMigrationTest";
                attribute.Attributes = attributeSetData.Attribute;
                stepName = "Create Attributes set" + attribute.MainAttributeSets + "  Add  Attributes to the attribute set:" + attribute.Attributes + " is Successful and Generate Table is successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeSetPage.CreateAttributeSet(attribute.MainAttributeSets, attribute.AttributeSets);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_AttributeSet.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForReferenceData(attribute.AttributeSets, out _output);
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

                #region Step14:RollBack Migration Set
                stepName = "RollBack Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                _MigrationPage.RollbackMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_199_LN_Create_And_Migrate_RefData()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData attributeSetData = new NonAdminUserData(DriverContext);
                var MigrationPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                var navigator_AttributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                AttributeSet attribute = new AttributeSet();
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigating Model and Navigate to Attribute Set page
                stepName = "Navigating Model and Navigate to Attribute Set page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var models_HomePage = new Navigator_ModelHomePage(DriverContext);
                models_HomePage.NavigateToModels_Page(out string Pageoutput); testStep.SetOutput(Pageoutput);
                models_HomePage.NavigatetoToAttributeSet_Page(out Pageoutput); testStep.SetOutput(Pageoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create Attributes set , Add  Attribute sets
                var attributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                attribute.MainAttributeSets = "Reference Data";
                string randomStr = RandomDataHelper.RandomString(4);
                attribute.AttributeSets = "ReferenceMigrationTest";
                attribute.Attributes = attributeSetData.Attribute;
                stepName = "Create Attributes set" + attribute.MainAttributeSets + "  Add  Attributes to the attribute set:" + attribute.Attributes + " is Successful and Generate Table is successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeSetPage.CreateAttributeSet(attribute.MainAttributeSets, attribute.AttributeSets);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_AttributeSet.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForReferenceData(attribute.AttributeSets, out _output);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_198_LN_Create_And_Migrate_Bscript()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssuePonits Rule
                Rules Rule = new Rules();
                string RuleEvent = "IssuePonits";
                Rule.RuleName = "IssuePonits";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "1";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_Configurations_CreateVirtual_Card RuleConfig = new Rules_Configurations_CreateVirtual_Card();
                RuleConfig.Skipcreation_Onexistingvirtualcard = true;
                RuleConfig.LoyaltyId_GenerationSource = "Generate New ID";
                #endregion
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(driverContext);
                #endregion

                #region stepName 1: Open Navigator URL
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssuePoints, Edit InCase values are not matching and Create BscriptExpression;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                _RuleTriggers.CreateBscriptExpression(Rule, out Output);
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

                #region Step6: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_PushNotifications.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.BuildMigrationSetName, out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                //  _MigrationPage.SelectItemsForPushNotoficationDef(, out _output);
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_201_LN_Migrate_ForgotPasswordModule_For_CSportal()
        {
            #region Object Initialization
            bool stepStatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_WebsitePage = new Navigator_Users_WebsitePage(DriverContext);
            var navigator_Users_Website_WebsitesPage = new Navigator_Users_Website_WebsitesPage(DriverContext);
            var navigator_Users_Website_ModulesPage = new Navigator_Users_Website_ModulesPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            var websitePage = new Navigator_Users_WebsitePage(driverContext);
            Migration Migration = new Migration(driverContext);

            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            string website = "BTA_Dev_CS";
            string pageName = "Account Summary";
            string moduleName = "Forgot Password";
            string configName = "ForgotPassword";
            string moduleArea = "Content";
            string moduleOrder = "3";
            string email = "";
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

                #region Step3: Create CS portal website               
                stepName = "Navigate to Website application and Create org_env_CS website";
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                Portal portal = new Portal
                {
                    WebSiteName = WebsiteData.CSPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.CS.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.CSDefaultSkin.ToString()
                };
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(websitePage.Create_CS(portal));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Delete the Module from Websites page if exists in the Dev Env
                stepName = "Delete the Module from Websites page if exists in the Dev Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out string outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Delete Configuration If exists in Modules Page
                stepName = "Delete Configuration If exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CancelConfiguration_IfExists("-- select --", moduleName, configName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Create ForgotPassword Module Configuration If not exists in Modules Page
                stepName = "Create ForgotPassword Module Configuration If not exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.AddForgotPasswordToPageIfNotExistsAndConfigurareEmail(website, moduleName, configName, email, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Add the module in website page if not exists
                stepName = "Add the module in website page if not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.AddModuleToPageIfNotExists(website, pageName, moduleName, configName, moduleArea, moduleOrder, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_ForgotPassword.ToString() + "_" + DateHelper.GetDate("Current");
                MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_ForgotPassword.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Create New Migration Set
                stepName = "Create New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                MigrationPage.SelectItemsForWebsiteModule(configName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify migrated module displayed on Website Modules page
                stepName = "Verify migrated module displayed on Website Modules page";
                testStep = TestStepHelper.StartTestStep(testStep); application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.VerifyModulePresentInTheGrid("-- select --", moduleName, configName);
                if (stepStatus)
                    testStep.SetOutput("Module Displayed in the grid.");
                else
                {
                    testStep.SetOutput("Module is not present in the grid.");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                    listOfTestSteps.Add(testStep);
                }
                #endregion

                #region Step16:Switch to Dev Env and Delete the Module from Websites page
                stepName = "Switch to Dev Env and Delete the Module from Websites page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Logout
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

        /// <summary>
        /// Test BTA_210 : Method to test Migration of Activation module for member Portal
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_210_LN_Migrate_MigrateMemberPortalActivationModule()
        {
            #region Object Initialization
            bool stepStatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_WebsitePage = new Navigator_Users_WebsitePage(DriverContext);
            var navigator_Users_Website_WebsitesPage = new Navigator_Users_Website_WebsitesPage(DriverContext);
            var navigator_Users_Website_ModulesPage = new Navigator_Users_Website_ModulesPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            Migration Migration = new Migration(driverContext);

            string website = "BTA_Dev_MP";
            string pageName = "My Account";
            string moduleName = "Activation";
            string configName = "Activation_MP_Migration";
            string requiredField = "Username";
            string successPage = "testing_page";
            string MaxAccountAge = RandomDataHelper.RandomNumber(4);
            string PhoneNumberMask = "test";
            string moduleArea = "Content";
            string moduleOrder = "7";

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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Delete the Module from Websites page if exists in the Dev Env
                stepName = "Delete the Module from Websites page if exists in the Dev Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out string outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Delete Configuration If exists in Modules Page
                stepName = "Delete Configuration If exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CancelConfiguration_IfExists("-- select --", moduleName, configName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create Configuration If not exists in Modules Page
                stepName = "Create Configuration If not exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_Website_ModulesPage.CreateActivationConfigurationIfNotExists(website, moduleName, configName, requiredField, successPage, MaxAccountAge, PhoneNumberMask, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Add the module in website page if not exists
                stepName = "Add the module in website page if not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.AddModuleToPageIfNotExists(website, pageName, moduleName, configName, moduleArea, moduleOrder, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Delete the Module from Websites page if exists in the QA Env
                stepName = "Delete the Module from Websites page if exists in the QA Env";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists("BTA_QA_MP", pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Delete Configuration If exists in Modules Page
                stepName = "Delete Configuration If exists in Modules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.CancelConfiguration_IfExists("-- select --", moduleName, configName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Website_Module_Activation.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Website_Module_Activation.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Create New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForWebsiteModule(configName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16:Verify migrated module displayed on Website Modules page
                stepName = "Verify migrated module displayed on Website Modules page";
                testStep = TestStepHelper.StartTestStep(testStep); application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out outMsg);
                stepStatus = navigator_Users_Website_ModulesPage.VerifyModulePresentInTheGrid("-- select --", moduleName, configName);
                if (stepStatus)
                    testStep.SetOutput("Module Displayed in the grid.");
                else
                {
                    testStep.SetOutput("Module is not present in the grid.");
                    testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                    listOfTestSteps.Add(testStep);
                }
                #endregion

                #region Step17:Switch to Dev Env and Delete the Module from Websites page
                stepName = "Switch to Dev Env and Delete the Module from Websites page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                stepStatus = navigator_Users_WebsitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Websites, out outMsg);
                stepStatus = navigator_Users_Website_WebsitesPage.DeleteModuleIfExists(website, pageName, moduleName, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18: Logout
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



