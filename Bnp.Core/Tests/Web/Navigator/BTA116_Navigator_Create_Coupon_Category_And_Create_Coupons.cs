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
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA116_Navigator_Create_Coupon_Category_And_Create_Coupons : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        public string OrderTest_Status = "";

        public BTA116_Navigator_Create_Coupon_Category_And_Create_Coupons()
        {
        }
        public BTA116_Navigator_Create_Coupon_Category_And_Create_Coupons(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA_116_Navigator_CreateCouponCategoryWithExtendedAttributeSet()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            string Prereq_testCase1 = "BTA119_Navigator_VerifyAttributes_On_ExtendedAttributesPage";
            ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase1, methodName, testStep);

            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            string CouponName = couponData.CouponName;
            string randomStr = RandomDataHelper.RandomString(4);
            coupon.CouponCode = "";
            coupon.Name = CouponName + randomStr;
            coupon.CategoryName = couponData.CouponCategoryName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();

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

                #region Step3:Create Category with Coupon
                stepName = "Create new Category as " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateCoupon_Category.CreateCategory(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with Attribute Set
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue + " with Attribute Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithAttributeSet(coupon);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion               

                #region Step5:Navigate to Websites and Select Website as BTA_Dev_CS and Module Type as  
                stepName = "Navigate to Websites and Select Website  as BTA_Dev_CS and Module Type as Customer Service - Award Coupons";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.website);
                var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);
                var websitePage = new Navigator_Users_WebsitePage(DriverContext);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg);
                var webSiteName = WebsiteData.CSPortal_WebSiteName;
                var moduleType = EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.CustomerServiceAwardCoupons);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Config Coupon to CS portal
                stepName = "Select CSCouponAppeasement_Config file and Add created Coupon into it";
                testStep = TestStepHelper.StartTestStep(testStep);
                Website_Modules.CSCouponAppeasementConfigurebutton();
                Website_Modules.AddCouponInCSCouponAppeasementConfig(coupon.Name);
                Website_Modules.SaveConfigSetting();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Bounce the CS portal App pool
                stepName = "Bounce the CS Portal App pool";
                testStep = TestStepHelper.StartTestStep(testStep);
                var WebsiteManagement = new Navigator_Users_WebsiteManagementPage(DriverContext);
                WebsiteManagement.Navigator_Website_Select_WebsiteManagementTab();
                WebsiteManagement.BounceAppPool("CSPortal");
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
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Passed");

            }

            catch (Exception e)
            {
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Failed");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                //testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
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

        [TestMethod]
        [TestCategory("Navigator-Smoke")]
        public void BTA_116_Navigator_CreateCouponCategory_Coupon()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);

            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            string CouponName = couponData.CouponName;
            string randomStr = RandomDataHelper.RandomString(4);
            coupon.Name = CouponName + randomStr;
            coupon.CouponCode = "";
            coupon.CategoryName = couponData.CouponCategoryName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();
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

                #region Step3:Create Category with Coupon
                stepName = "Create new Category as " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateCoupon_Category.CreateCategory(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with or without Attribute Set
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue + " with or without Attribute Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion               

                #region Step5:Navigate to Websites and Select Website as BTA_Dev_CS and Module Type as  
                stepName = "Navigate to Websites and Select Website  as BTA_Dev_CS and Module Type as Customer Service - Award Coupons";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.website);
                var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);
                var websitePage = new Navigator_Users_WebsitePage(DriverContext);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg);
                var webSiteName = WebsiteData.CSPortal_WebSiteName;
                var moduleType = EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.CustomerServiceAwardCoupons);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Config Coupon to CS portal
                stepName = "Select CSCouponAppeasement_Config file and Add created Coupon into it";
                testStep = TestStepHelper.StartTestStep(testStep);
                Website_Modules.CSCouponAppeasementConfigurebutton();
                Website_Modules.AddCouponInCSCouponAppeasementConfig(coupon.Name);
                Website_Modules.SaveConfigSetting();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Bounce the CS portal App pool
                stepName = "Bounce the CS Portal App pool";
                testStep = TestStepHelper.StartTestStep(testStep);
                var WebsiteManagement = new Navigator_Users_WebsiteManagementPage(DriverContext);
                WebsiteManagement.Navigator_Website_Select_WebsiteManagementTab();
                WebsiteManagement.BounceAppPool("CSPortal");
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