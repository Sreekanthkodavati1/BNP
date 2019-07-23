using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Promotion;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA1218_Regression_Navigator_Migration_Promotion : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;



        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void LN_42_Migration_NonTargeted_Promotion_with_None_option()
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

                #region stepName 4:"Create NonTargeted Promotion With EnrollmentTypes As None";
                stepName = "Create NonTargeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.MigrationPromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify NonTargeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
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

                #region stepName13: "Verify Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                searchCriteria = "Name";
                result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria, out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void LN_43_Migration_NonTargeted_Promotion_with_Required_option()
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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.MigrationPromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Required.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
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

                #region stepName13: "Verify Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                searchCriteria = "Name";
                result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void LN_44_Migration_NonTargeted_Promotion_with_Supported_option()
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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.MigrationPromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Supported.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
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

                #region stepName13: "Verify Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                searchCriteria = "Name";
                result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out outMsg );
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
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

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void LN_45_Migration_NonTargeted_Promotion_And_Rollback()
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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
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
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
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
                result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out outMsg);
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
                result = navigator_PromotionPage.VerifyPromotionAfterRollback(promotions.Name, searchCriteria,out outMsg);
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
        public void LN_46_VerifyPromotionRule_Validation()
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
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
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
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region for Test Error Validation Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Test Error Validation" + RandomDataHelper.RandomString(3);
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "19";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.Promotion = promotions.Code;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "2";
                RuleConfig.TierEvaluationRule = "";
                Rule.Rulestatus_ToInactive = true;

                #endregion

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule If Not Exist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output+";No Object Reference error Appeared");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Delete Rule
                stepName = "Delete Rule ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Delete_Rule(Rule, out Output); testStep.SetOutput(Output );
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
        public void LN_47_Migration_VerifyPromotionWith_Rule_Section()
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
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3) + " " + RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
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
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region for Test Error Validation Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Rule_Test_Promotion_Validation" + RandomDataHelper.RandomString(3);
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "19";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.Promotion = promotions.Code;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "2";
                RuleConfig.TierEvaluationRule = "";
                Rule.Rulestatus_ToInactive = true;

                #endregion

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule If Not Exist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output + ";No Object Reference error Appeared");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                _RuleTriggers.ConfigCancel();
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Verify Rule Under Promotion Section
                stepName = "Verify Rule Under Promotion Section: " + promotions.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                //navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria);
                navigator_PromotionPage.VerifyRuleUnderPromotion(promotions, Rule, out Output); testStep.SetOutput(Output);
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

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_RuleWithPromotion.ToString() + "_" + DateHelper.GetDate("Current");
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
                _MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(10));
                _MigrationPage.SelectItemsForPromotionDef_WithRule(Rule.RuleName,promotions.Name,out _output);
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

                #region stepName 6:Verify Rule Under Promotion Section
                stepName = "Verify Rule Under Promotion Section: " + promotions.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                //navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria);
                navigator_PromotionPage.VerifyRuleUnderPromotion(promotions, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Delete Rule
                stepName = "Delete Rule ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                _RuleTriggers.Delete_Rule(Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                #region Step5: Switch to Migration Environment
                stepName = "Switching to Other Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                #region stepName 6:Delete Rule
                stepName = "Delete Rule ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                _RuleTriggers.Delete_Rule(Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                #region Step8: Logout
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
        public void LN_48_VerifyPromotionWithBlankSpace_Under_Rule_Section()
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
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3)+" "+ RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
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
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region for Test Error Validation Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Rule_Test_Promotion_Validation" + RandomDataHelper.RandomString(3);
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "19";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.Promotion = promotions.Code;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "2";
                RuleConfig.TierEvaluationRule = "";
                Rule.Rulestatus_ToInactive = true;

                #endregion

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule If Not Exist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output + ";No Object Reference error Appeared");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                _RuleTriggers.ConfigCancel();
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Verify Rule Under Promotion Section
                stepName = "Verify Rule Under Promotion Section: "+promotions.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                //navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria);
                navigator_PromotionPage.VerifyRuleUnderPromotion(promotions, Rule, out Output);testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region stepName 6:Delete Rule
                stepName = "Delete Rule ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                _RuleTriggers.Delete_Rule(Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
        public void LN_49_VerifyPromotion_Under_Rule_Section()
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
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

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

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(5);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
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
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region for Test Error Validation Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Rule_Test_Promotion_Validation" + RandomDataHelper.RandomString(3);
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "19";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.Promotion = promotions.Code;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "2";
                RuleConfig.TierEvaluationRule = "";
                Rule.Rulestatus_ToInactive = true;

                #endregion

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule If Not Exist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output + ";No Object Reference error Appeared");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                _RuleTriggers.ConfigCancel();
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Verify Rule Under Promotion Section
                stepName = "Verify Rule Under Promotion Section: " + promotions.Name;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                //navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria);
                navigator_PromotionPage.VerifyRuleUnderPromotion(promotions, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region stepName 6:Delete Rule
                stepName = "Delete Rule ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                _RuleTriggers.Delete_Rule(Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Logout
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
