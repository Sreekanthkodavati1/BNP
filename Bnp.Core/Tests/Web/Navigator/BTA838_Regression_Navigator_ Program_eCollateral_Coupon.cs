using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using JsonParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Test to test BTA-838  : Create Coupon
    /// </summary>
    [TestClass]
    public class BTA_838_Regression_Navigator__Program_eCollateral_Coupon:ProjectTestBase
    {
        Login login = new Login();
         TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        private Common common;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA_09 : Create Coupon with Global Value and verify in LW_CouponDef table 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_09_LN_CreateCoupon_With_Global_Option_Selected()
        {
            #region Object Initialization
            bool stepstatus = true;
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
            string randomStr = RandomDataHelper.RandomString(5);
            coupon.Name = CouponName + randomStr;
            coupon.CategoryName = couponData.CouponCategoryName;
            coupon.IsGlobal = couponData.GlobalValueSet;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
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

                #region Step4: Create new Coupon 
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithGLobalValue(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step5:Get the data from DB
                stepName = "Searching Coupon in the LW_CouponDef Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Name = ProjectBasePage.GetCouponDetailsFromCouponDefTableFromDB(coupon.Name, coupon.IsGlobal, out string Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Logout
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
        /// Test BTA_11 : Create Coupon with coupon code and generate certificates for the same and verify 
        /// Note: To generate certificates first start IIS Services from DB Server
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_11_LN_CreateCoupon_With_Certificates_And_Generate_Coupons()
        {
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
            coupon.CategoryName = couponData.CouponCategoryName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.PredicateDropDown= CategoryFields.Predicates.Eq.ToString();
            coupon.CouponCode = couponData.CouponCode;
            string randomNumber = JsonFileProcessor.RandomNumber(3);
            coupon.CouponCode = coupon.CouponCode + randomNumber;
            string CertNumberFormat = couponData.CertNumberFormat;
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

                #region Step4: Create new Coupon 
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue ;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 
                
                #region Step5: Search Coupon with Coupon Code and Predicate and Generate Certificate for Coupon
                stepName = "Search Coupon with Coupon Code and Predicate and Generate Certificate for Coupon";
                testStep = TestStepHelper.StartTestStep(testStep);
                coupon.SetType = EnumUtils.GetDescription(CategoryFields.Property.CouponCode);
                navigator_Users_Program_eCollateral_CouponsPage.SearchCouponBasedOnPredicate(coupon.SetType, coupon.PredicateDropDown, coupon.CouponCode);
                coupon.StartDate = date.ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                coupon.ExpiryDate = date.AddYears(1).ToString("dd/MM/yyyy HH:mm:ss", new CultureInfo("en-US"));
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.GenerateCouponCertificatesAndVerify(CertNumberFormat, 100, coupon.StartDate, coupon.ExpiryDate));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step6: Logout
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
        /// Test BTA_14 : Create Coupon with Multi-language as English and channel properties as Mobile generate Coupon Def.xml for the same and verify in Download folder
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_14_LN_Create_Coupon_With_English_Language_Mobile_And_Export()
        {
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
            coupon.CategoryName = couponData.CouponCategoryName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.PredicateDropDown = "Eq";
            coupon.CouponCode = couponData.CouponCode;
            string randomNumber = JsonFileProcessor.RandomNumber(3);
            coupon.CouponCode = coupon.CouponCode + randomNumber;
            string CertNumberFormat = couponData.CertNumberFormat;
            coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
            coupon.ChannelProperties = CategoryFields.Channel.Mobile.ToString();
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepOutput = "";
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

                #region Step4: Create new Coupon 
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Generate and Verify Def.Xml file
                stepName = "Generate and Verify Def.Xml file";
                testStep = TestStepHelper.StartTestStep(testStep);
                string DefFilePath = basePages.ConfigDownloadPath;
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.GenerateAndVerifyDefXMLFile(DefFilePath, "CouponsDefs_*.pdf", out stepOutput));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step6: Logout
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
        /// Create a coupon and verify the max uses per month
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_15_LN_CreateCouponToVerifyMaxUsesPerMonth()
        {
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
            common = new Common(DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            RedeemMemberCouponByIdOut redeemMemberCoupons = new RedeemMemberCouponByIdOut();
            string CouponName = couponData.CouponName;
            string usesPer = "Month";
            string randomStr = RandomDataHelper.RandomString(4);
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with max uses per month
                stepName = "Create new Coupon with max uses per month as " + coupon.MaxUsesPerMonth;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithMaxUses(coupon, usesPer);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create a Member and assign the coupon through CDIS service
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create a Member and assign the coupon through CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), Navigator_Users_Program_eCollateral_CouponsPage.couponId);
                testStep.SetOutput("Member  UserName: " + output.Username + " Added Successfully with Coupon : " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Redeeming the Coupon for the max uses times by using MemberCouponId
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon for the max uses times by using MemberCouponId";
                navigator_Users_Program_eCollateral_CouponsPage.RedeemCouponForMaxUsesTime(redeemMemberCoupons, memberCouponId, Convert.ToInt32(coupon.MaxUsesPerMonth), out string msg);
                testStep.SetOutput(msg);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " and the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verifying the error message by trying to redeem the coupon one more time
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the error message by trying to redeem the coupon one more time";
                bool status = navigator_Users_Program_eCollateral_CouponsPage.VerifyTheErrorMessageForCouponRedemptionForMoreThanMaxUses(redeemMemberCoupons, memberCouponId, "month", out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
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

        /// <summary>
        /// Create a coupon and verify the max uses per week
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_16_LN_CreateCouponToVerifyMaxUsesPerWeek()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            RedeemMemberCouponByIdOut redeemMemberCoupons = new RedeemMemberCouponByIdOut();
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            string CouponName = couponData.CouponName;
            string usesPer = "Week";
            string randomStr = RandomDataHelper.RandomString(4);
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with max uses per week
                stepName = "Create new Coupon with max uses per week as " + coupon.MaxUsesPerWeek;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithMaxUses(coupon, usesPer);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create a Member and assign the coupon through CDIS service
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create a Member and assign the coupon through CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), Navigator_Users_Program_eCollateral_CouponsPage.couponId);
                testStep.SetOutput("Member  UserName: " + output.Username + " Added Successfully with Coupon : " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Redeeming the Coupon for the max uses times by using MemberCouponId
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon for the max uses times by using MemberCouponId";
                navigator_Users_Program_eCollateral_CouponsPage.RedeemCouponForMaxUsesTime(redeemMemberCoupons, memberCouponId, Convert.ToInt32(coupon.MaxUsesPerWeek), out string msg);
                testStep.SetOutput(msg);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " and the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verifying the error message by trying to redeem the coupon one more time
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the error message by trying to redeem the coupon one more time";
                bool status = navigator_Users_Program_eCollateral_CouponsPage.VerifyTheErrorMessageForCouponRedemptionForMoreThanMaxUses(redeemMemberCoupons, memberCouponId, "week", out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));

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

        /// <summary>
        /// Create a coupon and verify the max uses per year
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_17_LN_CreateCouponToVerifyMaxUsesPerYear()
        {
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
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            RedeemMemberCouponByIdOut redeemMemberCoupons = new RedeemMemberCouponByIdOut();
            string CouponName = couponData.CouponName;
            string usesPer = "Year";
            string randomStr = RandomDataHelper.RandomString(4);
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with max uses per Year
                stepName = "Create new Coupon with max uses per year as " + coupon.MaxUsesPerYear;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithMaxUses(coupon, usesPer));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create a Member and assign the coupon through CDIS service
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create a Member and assign the coupon through CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), Navigator_Users_Program_eCollateral_CouponsPage.couponId);
                testStep.SetOutput("Member  UserName: " + output.Username + " Added Successfully with Coupon : " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Redeeming the Coupon for the max uses times by using MemberCouponId
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon for the max uses times by using MemberCouponId";
                navigator_Users_Program_eCollateral_CouponsPage.RedeemCouponForMaxUsesTime(redeemMemberCoupons, memberCouponId, Convert.ToInt32(coupon.MaxUsesPerYear), out string msg);
                testStep.SetOutput(msg);
                testStep.SetOutput("Loyalty Id number of the member is : " + basePages.GetLoyaltyNumber(output) + " and the number of the Usages Completed : " + Convert.ToInt32(coupon.MaxUsesPerYear) + ";left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verifying the error message by trying to redeem the coupon one more time
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the error message by trying to redeem the coupon one more time";
                bool status = navigator_Users_Program_eCollateral_CouponsPage.VerifyTheErrorMessageForCouponRedemptionForMoreThanMaxUses(redeemMemberCoupons, memberCouponId, "year", out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
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

        /// <summary>
        /// Create a coupon and verify the max uses per day
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_18_LN_CreateCouponToVerifyMaxUsesPerDay()
        {
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
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            RedeemMemberCouponByIdOut redeemMemberCoupons = new RedeemMemberCouponByIdOut();
            string CouponName = couponData.CouponName;
            string usesPer = "Day";
            string randomStr = RandomDataHelper.RandomString(4);
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with max uses per Day
                stepName = "Create new Coupon with max uses per day as " + coupon.MaxUsesPerDay;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithMaxUses(coupon, usesPer);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create a Member and assign the coupon through CDIS service
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create a Member and assign the coupon through CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), Navigator_Users_Program_eCollateral_CouponsPage.couponId);
                testStep.SetOutput("Member  UserName: " + output.Username + " Added Successfully with Coupon : " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Redeeming the Coupon for the max uses times by using MemberCouponId
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon for the max uses times by using MemberCouponId";
                navigator_Users_Program_eCollateral_CouponsPage.RedeemCouponForMaxUsesTime(redeemMemberCoupons, memberCouponId, Convert.ToInt32(coupon.MaxUsesPerDay), out string msg);
                testStep.SetOutput(msg);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " and the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verifying the error message by trying to redeem the coupon one more time
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the error message by trying to redeem the coupon one more time";
                bool status = navigator_Users_Program_eCollateral_CouponsPage.VerifyTheErrorMessageForCouponRedemptionForMoreThanMaxUses(redeemMemberCoupons, memberCouponId, "day", out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
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

        /// <summary>
        /// Creating coupon with all interval combinations
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_19_LN_CreateCouponWithAllIntervalCombinations()
        {
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
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            RedeemMemberCouponByIdOut redeemMemberCoupons = new RedeemMemberCouponByIdOut();
            string CouponName = couponData.CouponName;
            string usesPer = "All";
            string randomStr = RandomDataHelper.RandomString(4);
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with all interval combinations
                stepName = "Create new Coupon with all interval combinations";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCouponWithMaxUses(coupon, usesPer);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create a Member and assign the coupon through CDIS service
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create a Member and assign the coupon through CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), Navigator_Users_Program_eCollateral_CouponsPage.couponId);
                testStep.SetOutput("Member  UserName: " + output.Username + " Added Successfully with Coupon : " + coupon.Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Redeeming the Coupon for the max uses times by using MemberCouponId
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Redeeming the Coupon for the max uses times by using MemberCouponId";
                navigator_Users_Program_eCollateral_CouponsPage.RedeemCouponForMaxUsesTime(redeemMemberCoupons, memberCouponId, Convert.ToInt32(coupon.MaxUsesPerYear), out string msg);
                testStep.SetOutput(msg);
                testStep.SetOutput("Loyalty Id number of the member is : " + redeemMemberCoupons.MemberIdentity + " and the number of the Usages left for Redemption: " + redeemMemberCoupons.NumberOfUsesLeft);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verifying the error message by trying to redeem the coupon one more time
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying the error message by trying to redeem the coupon one more time";
                bool status = navigator_Users_Program_eCollateral_CouponsPage.VerifyTheErrorMessageForCouponRedemptionForMoreThanMaxUses(redeemMemberCoupons, memberCouponId, "year", out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
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
        public void BTA_12_LN_Edit_Coupon()
        {
            #region Object Initialization
            bool stepstatus = true;
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
            string randomStr = RandomDataHelper.RandomString(5);
            coupon.Name = CouponName + randomStr;
            coupon.CategoryName = couponData.CouponCategoryName;
            var date = DateTime.Now;
            date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
            coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
            coupon.UsesAllowed = "10";
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.SetType = CategoryFields.Property.Name.ToString();
            coupon.CouponCode = couponData.CouponCode;
            string randomNumber = JsonFileProcessor.RandomNumber(2);
            coupon.CouponCode = coupon.CouponCode + RandomDataHelper.RandomString(3);
            string CertNumberFormat = couponData.CertNumberFormat;

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

                #region Step5:Get the data from DB
                stepName = "Searching Coupon in the LW_CouponDef  Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Name = ProjectBasePage.GetCouponDetailsFromCouponDefTableFromDB(coupon.Name, out string Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Edit the created Coupon
                stepName = "Edit the created Coupon";
                testStep = TestStepHelper.StartTestStep(testStep);
                date = DateTime.Now;
                date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
                coupon.StartDate = date.AddYears(-1).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                coupon.ExpiryDate = date.AddYears(1).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                var output = navigator_Users_Program_eCollateral_CouponsPage.EditCoupon(coupon.SetType, coupon, coupon.CategoryName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, output, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Get the data from DB
                stepName = "Searching Coupon in the LW_CouponDef  Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                output = navigator_Users_Program_eCollateral_CouponsPage.VerifyEditCouponSaveOrNotInDb(coupon);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, output, DriverContext.SendScreenshotImageContent("WEB"));
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
        public void BTA838_13_LN_DeleteCoupon()
        {
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
            string randomStr = RandomDataHelper.RandomString(5);
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
            coupon.CouponCode = couponData.CouponCode;
            string randomNumber = JsonFileProcessor.RandomNumber(2);
            coupon.CouponCode = "";
            string CertNumberFormat = couponData.CertNumberFormat;

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

                #region Step4: Create new Coupon 
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify the Created Coupon details from DB
                stepName = "Verify the Created Coupon details from DB";
                List<string> TransactionList_Header = new List<string>();
                testStep = TestStepHelper.StartTestStep(testStep);
                var Name = ProjectBasePage.GetCouponDetailsFromCouponDefTableFromDB(coupon.Name, out string Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Delete the created Coupon
                stepName = "Delete the created Coupon";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.DeleteCoupon(coupon.SetType, coupon.Name, coupon.CategoryName); 
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Logout
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
        /// Verifying the negative scenarios for create coupon with max uses
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_10_LN_VerifyingNegativeScenarios()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var couponData = new NonAdminUserData(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
            coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
            coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
            coupon.SetType = CategoryFields.Property.Name.ToString();
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

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Click ECollateral tab and then Coupons to verify Coupons page displayed
                stepName = "Click ECollateral tab and then Coupons to verify Coupons page displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Enter zero in Max uses per year, month, week and day then validate the error message
                stepName = "Enter zero in Max uses per year, month, week and day then validate the error message";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.VerifyErrorMessageForNullValueForMaxUses(out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Validate the error message for Uses Allowed field when no input is entered
                stepName = "Validate the error message for Uses Allowed field when no input is entered";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.VerifyRequiredErrorMessageForUsesAllowedField(out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Validate the error message for max uses per year larger than uses allowed
                stepName = "Validate the error message for max uses per year larger than uses allowed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerYear>UsesAllowed", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Validate the error message for max uses per day larger than max uses per year
                stepName = "Validate the error message for max uses per day larger than max uses per year";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerDay>MaxUsesPerYear", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14: Validate the error message for max uses per day larger than max uses per month
                stepName = "Validate the error message for max uses per day larger than max uses per month";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerDay>MaxUsesPerMonth", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Validate the error message for max uses per day larger than max uses per week
                stepName = "Validate the error message for max uses per day larger than max uses per week";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerDay>MaxUsesPerWeek", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18: Validate the error message for max uses per month larger than max uses per year
                stepName = "Validate the error message for max uses per month larger than max uses per year";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerMonth>MaxUsesPerYear", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step19: Validate the error message for max uses per week larger than max uses per year
                stepName = "Validate the error message for max uses per week larger than max uses per year";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerWeek>MaxUsesPerYear", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step20: Click the Create new Coupon button and verify the create coupon panel displayed
                stepName = "Click the Create new Coupon button and verify the create coupon panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(navigator_Users_Program_eCollateral_CouponsPage.ClickNewCouponAndVerifyCouponPanel());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step21: Validate the error message for max uses per week larger than max uses per month
                stepName = "Validate the error message for max uses per week larger than max uses per month";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_Program_eCollateral_CouponsPage.ValidateErrorMessageForCouponUsesCombinations(coupon, "MaxUsesPerWeek>MaxUsesPerMonth", out outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step22: Logout
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
