using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Class to test BTA-1219  : Migration of Coupons
    /// </summary>
    [TestClass]
    public class BTA1219_Navigator_Migrate_Coupons : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// This method tests Migration of a coupon with IsGlobal value checked in source environment gets migrated properly and isGlobal value is checked in the destination environment
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA_50_Navigator_Migrate_Coupon_VerifyCouponWithIsGlobalFalgCheckedMigratedCorrectly()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);

            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            string CouponName = Migration.MigrationCouponame;
            string randomStr = RandomDataHelper.RandomString(4);
            coupon.Name = CouponName + randomStr;
            coupon.CouponCode = "";
            coupon.CategoryName = Migration.MigrationCouponCategoryName;

            coupon.StartDate = DateHelper.GetDate("Current");
            coupon.ExpiryDate = DateHelper.GetDate("Future");
            coupon.UsesAllowed = RandomDataHelper.RandomNumber(2).ToString();
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();
            coupon.IsGlobal = "unchecked";
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

                #region Step3:Create Category with Coupon if not Exists in Dev environment
                stepName = "Create new Category for " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.VerifyCategory_IfNotExistedCreateNew(ProjectBasePage.Env_value, coupon, CategoryFields.CategoryType.Coupon.ToString(), out string stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with Is Global selected in Dev  environment
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue + " and Is Global selected. ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithGLobalValue(coupon));
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

                #region Step6:Create Category with Coupon if not Exists in QA environment
                stepName = "Create new Category as " + coupon.CategoryTypeValue + ", if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool CategoryAlreadyMigrated = true;
                if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    CategoryAlreadyMigrated = false;
                }
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Coupon_Default.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Coupon_Default.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create  New Migration Set
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
                //_MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value,DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForCouponDef(CategoryAlreadyMigrated, coupon.CategoryName, coupon.Name, out _output);
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

                #region Step13:Verify Coupon Migrated  on Coupon Page
                stepName = "Verify Coupon Migrated on Coupon Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.VerifyCreatedCoupon(coupon.SetType, coupon.Name, coupon.CategoryName);
                testStep.SetOutput(" Coupon :" + coupon.Name + " Migrated Successfully and Appeared on Coupon Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Verify the Is Global checkbox is checked
                stepName = "Verify the Is Global checkbox is checked";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool stepStatus = navigator_Users_Program_eCollateral_CouponsPage.VerifyISGlobalCheckedorNot(coupon.Name);
                testStep.SetOutput("Global checkbox is checked for the coupon: "+ coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify Coupon Category Migrated  on Categories Page
                stepName = "Verify Coupon Category Migrated  on Categories Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    throw new Exception("Failed to Migrate Category:" + Migration.MigrationCouponCategoryName);
                }
                testStep.SetOutput(stepoutput);
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

        /// <summary>
        /// This method tests Migration of a coupon associated with Push notification in source environment gets migrated properly and is associated with the same push notification in the destination environment
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA_51_Navigator_Migrate_Coupon_VerifyThePushNotificationOfCouponAfterMigration()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            CategoryFields notification = new CategoryFields();
            Migration Migration = new Migration(driverContext);

            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);

            string CouponName = Migration.MigrationCouponame;
            string randomStr = RandomDataHelper.RandomString(4);
            coupon.Name = CouponName + randomStr;
            coupon.CouponCode = "";
            coupon.CategoryName = Migration.MigrationCouponCategoryName;
            coupon.StartDate = DateHelper.GetDate("Current");
            coupon.ExpiryDate = DateHelper.GetDate("Future");
            coupon.UsesAllowed = RandomDataHelper.RandomNumber(2).ToString();
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();
            coupon.PushNotification = couponData.Migrate_Notification;
            coupon.IsGlobal = "unchecked";

            notification.Name = couponData.Migrate_Notification;
            notification.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            notification.ExpiryDate = DateTime.Now.AddDays(15).ToString("MM/dd/yyyy");
           
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

                #region Step3:Create Category with Coupon if not Exists in Dev environment
                stepName = "Create new Category for " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                bool stepStatus = basePages.VerifyCategory_IfNotExistedCreateNew(ProjectBasePage.Env_value, coupon, CategoryFields.CategoryType.Coupon.ToString(), out string stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus , DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create Notification if not Exists in Dev environment
                stepName = "Create new notification, if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = basePages.VerifyNotification_IfNotExistsCreateNew(ProjectBasePage.Env_value, notification, out stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create new Coupon with a push notification in Dev  environment
                stepName = "Create Coupon with a category and with a push notification " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithPushNotification(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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

                #region Step7:Create Category with Coupon if not Exists in QA environment
                stepName = "Create new Category as " + coupon.CategoryTypeValue + ", if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool CategoryAlreadyMigrated = true;
                if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    CategoryAlreadyMigrated = false;
                }
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create Notification if not Exists in QA environment
                stepName = "Create new notification, if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = basePages.VerifyNotification_IfNotExistsCreateNew(Migration.MigrationEnvironment, notification, out stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Coupon_Default.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Coupon_Default.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Create  New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                //_MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value,DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForCouponDef(CategoryAlreadyMigrated, coupon.CategoryName, coupon.Name, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify Coupon Migrated  on Coupon Page
                stepName = "Verify Coupon Migrated on Coupon Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.VerifyCreatedCoupon(coupon.SetType, coupon.Name, coupon.CategoryName);
                testStep.SetOutput(" Coupon :" + coupon.Name + " Migrated Successfully and Appeared on Coupon Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16:Verify the selected push notification is displayed in the coupon defination
                stepName = "Verify the selected push notification is displayed in the coupon defination";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepStatus = navigator_Users_Program_eCollateral_CouponsPage.VerifyThePushNotificationInCouponDefination(coupon, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17:Verify Coupon Category Migrated  on Categories Page
                stepName = "Verify Coupon Category Migrated  on Categories Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    throw new Exception("Failed to Migrate Category:" + Migration.MigrationCouponCategoryName);
                }
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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

        /// <summary>
        /// This method tests Migration of a coupon with IsGlobal value unchecked in source environment gets migrated properly and isGlobal value is unchecked in the destination environment
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA_52_Navigator_Migrate_Coupon_VerifyDeselectOfIsGlobalFalgMigratedCorrectly()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);

            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            string CouponName = Migration.MigrationCouponame;
            string randomStr = RandomDataHelper.RandomString(4);
            coupon.Name = CouponName + randomStr;
            coupon.CouponCode = "";
            coupon.CategoryName = Migration.MigrationCouponCategoryName;

            coupon.StartDate = DateHelper.GetDate("Current");
            coupon.ExpiryDate = DateHelper.GetDate("Future");
            coupon.UsesAllowed = RandomDataHelper.RandomNumber(2).ToString();
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();
            coupon.IsGlobal = "unchecked";
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

                #region Step3:Create Category with Coupon if not Exists in Dev environment
                stepName = "Create new Category for " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                basePages.VerifyCategory_IfNotExistedCreateNew(ProjectBasePage.Env_value, coupon, CategoryFields.CategoryType.Coupon.ToString(), out string stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon in Dev  environment
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Switch to Migration Environment
                stepName = "Switching to Migration Environment :"+ Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment,Migration.MigrationOrderId,out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create Category with Coupon if not Exists in QA environment
                stepName = "Create new Category as " + coupon.CategoryTypeValue + ", if not exists.";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool CategoryAlreadyMigrated = true;
                if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    CategoryAlreadyMigrated = false;
                }
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Coupon_Default.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists (Migration.MigrationSets.Migration_Coupon_Default.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Create  New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out  output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                //_MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value,DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.EditItems_All(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForCouponDef(CategoryAlreadyMigrated, coupon.CategoryName, coupon.Name,out _output);
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

                #region Step13:Verify Coupon Migrated  on Coupon Page
                stepName = "Verify Coupon Migrated on Coupon Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.VerifyCreatedCoupon(coupon.SetType, coupon.Name, coupon.CategoryName);
                testStep.SetOutput(" Coupon :"+coupon.Name+" Migrated Successfully and Appeared on Coupon Page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Verify the Is Global checkbox is unchecked
                stepName = "Verify the Is Global checkbox is unchecked";
                testStep = TestStepHelper.StartTestStep(testStep);
                if(!navigator_Users_Program_eCollateral_CouponsPage.VerifyISGlobalCheckedorNot(coupon.Name))
                testStep.SetOutput("Global checkbox is unchecked for the coupon: " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify Coupon Category Migrated  on Categories Page
                stepName = "Verify Coupon Category Migrated  on Categories Page";
                testStep = TestStepHelper.StartTestStep(testStep);
               if (!basePages.VerifyCategory_IfNotExistedCreateNew(Migration.MigrationEnvironment, coupon, CategoryFields.CategoryType.Coupon.ToString(), out stepoutput, true))
                {
                    throw new Exception("Failed to Migrate Category:" + Migration.MigrationCouponCategoryName);
                }
                testStep.SetOutput(stepoutput);
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
    }
}