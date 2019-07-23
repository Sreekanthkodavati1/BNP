using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Admin;
using Bnp.Core.WebPages.Navigator.FrameworkConfig;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA1225_Regression_Navigator_Program_Reward_Catalog_Exchange_Rates : ProjectTestBase
    {
        Login login = new Login();
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// BTA 128 : This method is used to filter exchange rates with not equal to predicate value in Rate Page
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_128_LN_Program_RewardCatalog_SearchWithPredicatesInExchangeRates()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_ExchangeRatePage = new Navigator_Users_Program_RewardCatalog_ExchangeRatesPage(DriverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            CategoryFields rate = new CategoryFields();
           
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
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

                #region Step3: Verify that "!=" predicate works for Exchange Rates search functionalily
                stepName = "Verify that selected predicate works for Exchange Rates search functionalily";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.SearchExchangeRateBasedOnNotEqualToPredicateValue(CategoryFields.ExchangeRatePropertyType.ToCurrency.ToString(), CategoryFields.Predicates.Ne.ToString(), rate.ExchangeRateValue, out String status);
                testStep.SetOutput(status);
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
        /// BTA 129 : This method is used to Update Default CurrencyRate and Validate in Rate Page
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_129_LN_Program_RewardCatalog_UpdateDefaultCurrencyRateAndValidateInExchangeRatePage()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_ExchangeRatePage = new Navigator_Users_Program_RewardCatalog_ExchangeRatesPage(DriverContext);
            var manageOrganizationPage = new Navigator_Orgnizations_FrameworkConfigurationPage(DriverContext);
            var organizationPage = new Navigator_Admin_OrganizationsPage(driverContext);
            string configPath = BnPBaseFramework.Web.Helpers.FilesHelper.GetFolder(BaseConfiguration.DownloadFolder, Directory.GetCurrentDirectory());
            string FrameworkCfgFile = basePages.ConfigDownloadPath + @"\Framework.cfg";
            basePages.DeleteExistedFile(FrameworkCfgFile);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            CategoryFields rate = new CategoryFields();
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
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

                #region Step3: Navigate to organization page
                stepName = "Navigate to organization page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                testStep.SetOutput("Successfully navigated to Organization page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Navigate to Framework Configuration tab
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                organizationPage.NavigateToOrganizationsTabs(Navigator_Admin_OrganizationsPage.OrganizationsTabs.FrameworkConfiguration, out string Message); testStep.SetOutput(Message + ";" + Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Set a value to the Default Currency in the framework configuration
                stepName = "Set a value to the Default Currency in the framework configuration";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(manageOrganizationPage.EditDefaultCurrencyValueInFrameworkConfiguration(FrameworkCfgFile, rate.ExchangeRateValue, out string Status));
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that the Program Based Currency is displayed in the Rates page 
                stepName = "Verify ProgramBaseCurrency Value in Exchange Rates Page is displaying Updated Currency Value " + rate.ExchangeRateValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.VerifyProgramBaseCurrencyValueIsDisplayingUpdatedCurrencyValue(rate.ExchangeRateValue, out Status);
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify the From Currency is reflecting the Base currency changes
                stepName = " Verify the From Currency is reflecting the Base currency changes";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Result = RewardCatalog_ExchangeRatePage.VerifyFromCurrencyValueIsDisplayingBaseCurrency(rate.ExchangeRateValue);
                Status = "FromCurrency Value in Exchange Rates Page should display Updated Base Currency Value " + rate.ExchangeRateValue;
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Result , DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Navigate to Framework Configuration tab
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                organizationPage.NavigateToOrganizationsTabs(Navigator_Admin_OrganizationsPage.OrganizationsTabs.FrameworkConfiguration, out Message); testStep.SetOutput(Message + ";" + Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Revert the customized changes done for Default Currency in the framework configuration and verify the updated value
                stepName = "Revert the customized changes done for Default Currency in the framework configuration and verify the updated value as " + rate.DefaultCurrencyValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(manageOrganizationPage.EditDefaultCurrencyValueInFrameworkConfiguration(FrameworkCfgFile, rate.DefaultCurrencyValue, out Status));
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify that the Program Based Currency is updated to default Currency Value in the Rates page 
                stepName = "Verify ProgramBaseCurrency Value in Exchange Rates Page is displaying Updated Currency Value " + rate.ExchangeRateValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.VerifyProgramBaseCurrencyValueIsDisplayingUpdatedCurrencyValue(rate.DefaultCurrencyValue, out Status);
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Logout
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
        /// BTA 226 : This method is used to Updated Default Currency in ConfigFile and Verify SearchFunctionality for different Values in Rates Page with Equal to predicate
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_226_LN_UpdatedDefaultCurrencyInConfigFile_And_VerifySearchFunctionalityForDifferentValuesInExchangeRates()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_ExchangeRatePage = new Navigator_Users_Program_RewardCatalog_ExchangeRatesPage(DriverContext);
            var manageOrganizationPage = new Navigator_Orgnizations_FrameworkConfigurationPage(DriverContext);
            var organizationPage = new Navigator_Admin_OrganizationsPage(driverContext);
            string configPath = BnPBaseFramework.Web.Helpers.FilesHelper.GetFolder(BaseConfiguration.DownloadFolder, Directory.GetCurrentDirectory());
            string FrameworkCfgFile = basePages.ConfigDownloadPath + @"\Framework.cfg";
            basePages.DeleteExistedFile(FrameworkCfgFile);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            CategoryFields rate = new CategoryFields();
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
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

                #region Step3: Navigate to organization page
                stepName = "Navigate to organization page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                testStep.SetOutput("Successfully navigated to Organization page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Navigate to Framework Configuration tab
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                organizationPage.NavigateToOrganizationsTabs(Navigator_Admin_OrganizationsPage.OrganizationsTabs.FrameworkConfiguration, out string Message);
                testStep.SetOutput(Message + ";" + Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Set a value to the Default Currency in the framework configuration
                stepName = "Set a value to the Default Currency in the framework configuration";
                testStep = TestStepHelper.StartTestStep(testStep);
                manageOrganizationPage.EditDefaultCurrencyValueInFrameworkConfiguration(FrameworkCfgFile, rate.ExchangeRateValue, out string Status);
                testStep.SetOutput(Status);
               testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Verify that the Program Based Currency is displayed in the Rates page 
                stepName = "Verify ProgramBaseCurrency Value in Exchange Rates Page is displaying Updated Currency Value " + rate.ExchangeRateValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.VerifyProgramBaseCurrencyValueIsDisplayingUpdatedCurrencyValue(rate.ExchangeRateValue,out Status);
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
              
                #region Step7: Verify that the search functionality works for To Currency Name
                stepName = "Verify that the search functionality works for To Currency Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.SearchExchangeRateBasedOnEqualToPredicateValue(CategoryFields.ExchangeRatePropertyType.ToCurrencyName.ToString(), CategoryFields.Predicates.Eq.ToString(), rate.ToCurrencyName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify that the search functionality works for To Currency
                stepName = "Verify that the search functionality works for To Currency";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.SearchExchangeRateBasedOnEqualToPredicateValue(CategoryFields.ExchangeRatePropertyType.ToCurrency.ToString(), CategoryFields.Predicates.Eq.ToString(), rate.ExchangeRateValue);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Verify that the search functionality works for From Currency Name
                stepName = "Verify that the search functionality works for From Currency Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.SearchExchangeRateBasedOnEqualToPredicateValue(CategoryFields.ExchangeRatePropertyType.FromCurrencyName.ToString(), CategoryFields.Predicates.Eq.ToString(), rate.FromCurrencyName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Verify that the search functionality works for From Currency
                stepName = "Verify that the search functionality works for From Currency";
                testStep = TestStepHelper.StartTestStep(testStep);
                var result = RewardCatalog_ExchangeRatePage.SearchExchangeRateBasedOnEqualToPredicateValue(CategoryFields.ExchangeRatePropertyType.FromCurrency.ToString(), CategoryFields.Predicates.Eq.ToString(), rate.ExchangeRateValue);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Navigate to Framework Configuration tab
                stepName = "Select Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.organization);
                organizationPage.NavigateToOrganizationsTabs(Navigator_Admin_OrganizationsPage.OrganizationsTabs.FrameworkConfiguration, out Message); testStep.SetOutput(Message + ";" + Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Revert the customized changes done for Default Currency in the framework configuration and verify the updated value
                stepName = "Revert the customized changes done for Default Currency in the framework configuration and verify the updated value as " + rate.DefaultCurrencyValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(manageOrganizationPage.EditDefaultCurrencyValueInFrameworkConfiguration(FrameworkCfgFile, rate.DefaultCurrencyValue, out Status));
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Verify that the Program Based Currency is updated to default Currency Value in the Rates page 
                stepName = "Verify ProgramBaseCurrency Value in Exchange Rates Page is displaying Updated Currency Value " + rate.ExchangeRateValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.VerifyProgramBaseCurrencyValueIsDisplayingUpdatedCurrencyValue(rate.DefaultCurrencyValue,out Status);
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout
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
        /// BTA 227 : This method is used to perform all validations while creating new Job in Schedule Job Page
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_227_LN_JobCreationValidationsInSchedulePage()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_ExchangeRatePage = new Navigator_Users_Program_RewardCatalog_ExchangeRatesPage(DriverContext);
            CategoryFields rate = new CategoryFields();
            CategoryFields job = new CategoryFields();
            job.Name = data.JobName + RandomDataHelper.RandomString(5);
            job.FrequencyDropDown = CategoryFields.FrequencyType.Hourly.ToString();
            job.Hour = job.RunEveryHour;
            var date = DateTime.Now;
            job.StartDate = DateHelper.GetDate("Current");
            job.ExpiryDate = DateHelper.GetDate("Future");
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
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

                #region Step3: Verify that Job Name is a required value for creating a new Job
                stepName = "Verify that Job Name is a required value for creating a new Job ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.NavigateToExchangeRatesTab(Navigator_Users_Program_RewardCatalog_ExchangeRatesPage.ExchangeTabs.Schedule);
                RewardCatalog_ExchangeRatePage.VerifyJobNameIsRequiredValueForCreatingNewJob("Job Name is a required value.", out string status);
                testStep.SetOutput(status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify that Frequency is a required value for creating a new Job
                stepName = "Verify that Frequency  is a required value for creating a new Job";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.VerifyThatFrequencyValueIsRequiredValueForCreatingNewJob(job, "A frequency value must be seleted.", out status);
                testStep.SetOutput(status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify that Start Date is a required value for creating a new Job
                stepName = "Verify that Start Date is a required value for creating a new Job";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.VerifyThatStartDateIsRequiredValueForCreatingNewJob(job, "Start date is a required value.", out status);
                testStep.SetOutput(status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Verify that End Date is a required value for creating a new Job
                stepName = "Verify that End Date is a required value for creating a new Job";
                testStep = TestStepHelper.StartTestStep(testStep);
                RewardCatalog_ExchangeRatePage.VerifyThatEndDateIsRequiredValueForCreatingNewJob(job, "End date is a required value.", out status);
                testStep.SetOutput(status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8: Verify that the 'Run At' field does not disappear when you click save button with no 'Run At' value provided for 'One time
                stepName = " Verify that the 'Run At' field does not disappear when you click save button with no 'Run At' value provided for 'One time";
                testStep = TestStepHelper.StartTestStep(testStep);
                job.FrequencyDropDown = CategoryFields.FrequencyType.OneTime.ToString();
                RewardCatalog_ExchangeRatePage.VerifyRunFieldIsRequiredForCreatingNewJobBySelectingOneTimeFrequency(job, "Please enter a date and time or run the job.", out status);
                testStep.SetOutput(status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Logout
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
        /// BTA 228 : This method is used to create new job in schedule Page
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_228_LN_CreateNewJobInSchedulePage()
        {
            #region Object Initialization
            CategoryFields coupon = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
            var RewardCatalog_ExchangeRatePage = new Navigator_Users_Program_RewardCatalog_ExchangeRatesPage(DriverContext);
            CategoryFields rate = new CategoryFields();
            CategoryFields job = new CategoryFields();
            job.Name = data.JobName;
            job.FrequencyDropDown = CategoryFields.FrequencyType.Hourly.ToString();
            job.Hour = job.RunEveryHour;
            var date = DateTime.Now;
            job.StartDate = DateHelper.GetDate("Current");
            job.ExpiryDate = DateHelper.GetDate("Future");
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

            try
            {
                #region Step1: Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage);
                testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2: Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
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

                #region Step9: Create New Job and verify the Job is created successfully
                stepName = "Create New Job and verify the Job is created successfully";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.ExchangeRates);
                RewardCatalog_ExchangeRatePage.NavigateToExchangeRatesTab(Navigator_Users_Program_RewardCatalog_ExchangeRatesPage.ExchangeTabs.Schedule);
                RewardCatalog_ExchangeRatePage.CreateJobAndVerifyCreatedJobInSchedulePage(job);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Logout
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
    }
}
