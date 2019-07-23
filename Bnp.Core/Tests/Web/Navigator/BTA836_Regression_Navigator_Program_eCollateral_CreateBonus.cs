using System;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.Navigator.Admin;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Test BTA-836: Create Bonus 
    /// </summary> 
    [TestClass]
    public class BTA836_Regression_Navigator_Program_eCollateral_CreateBonus:ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// Test BTA_01: Create Bonus and Generate PDF and download the bonuses Pdf
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_01_LN_Program_eCollateral_CreateBonus()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields bonus = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var bonusData = new NonAdminUserData(driverContext);
            string stepOutput = "";
            string stepName = "";
            #endregion

            try
            {
                #region Object Initialization
                string BonusName = bonusData.BonusName;
                string randomStr = RandomDataHelper.RandomString(4);
                bonus.Name = BonusName + randomStr;
                bonus.CategoryName = bonusData.BonusCategoryName;
                var date = DateTime.Now;
                bonus.StartDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.Logo_Img_Hero = "Null";
                bonus.CategoryTypeValue = CategoryFields.CategoryType.Bonus.ToString();
                bonus.ValueToSetInAttribute = "ValueGivenForAttribute";
                bonus.SetType = CategoryFields.Property.Name.ToString();
                bonus.MultiLanguage = CategoryFields.Languages.English.ToString();
                bonus.ChannelProperties= CategoryFields.Channel.Web.ToString();
                #endregion

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

                #region Step3:Create New Category with Bonus
                stepName = "Create new Category as " + bonus.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(bonus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create new bonus 
                stepName = "Create bonus with Category " + bonus.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Bonuses);
                testStep.SetOutput(navigator_CreateBonus.CreateBonus(bonus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Generate PDF and Verify in Download folder
                stepName = " Generate PDF and Verify in Download folder";
                testStep = TestStepHelper.StartTestStep(testStep);
                string bonusPDFFilePath = basePages.ConfigDownloadPath;
                testStep.SetOutput(navigator_CreateBonus.GenerateAndVerifyBonusesPDF(bonusPDFFilePath, "Bonuses_*.pdf",out stepOutput));         
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout
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
        /// Test BTA_02: Edit Bonus and Generate PDF and download the bonuses Pdf
        /// </summary> 
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_02_LN_Program_eCollateral_EditBonus()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            CategoryFields bonus = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            var bonusData = new NonAdminUserData(driverContext);
            string stepOutput = "";
            string stepName = "";
            #endregion

            try
            {
                #region Object Initialization
                string BonusName = "Auto_Bonus_";
                string randomStr = RandomDataHelper.RandomString(4);
                bonus.Name = BonusName + randomStr;
                bonus.CategoryName = bonusData.BonusCategoryName;
                var date = DateTime.Now;
                bonus.StartDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.Logo_Img_Hero = "Null";
                bonus.CategoryTypeValue = CategoryFields.CategoryType.Bonus.ToString();
                bonus.ValueToSetInAttribute = "ValueGivenForAttribute";
                bonus.SetType = CategoryFields.Property.Name.ToString();
                bonus.MultiLanguage = CategoryFields.Languages.English.ToString();
                bonus.ChannelProperties = CategoryFields.Channel.Web.ToString();
                #endregion

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

                #region Step3:Create New Category with Bonus
                stepName = "Create new Category as " + bonus.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(bonus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new bonus 
                stepName = "Create bonus with Category " + bonus.CategoryName ;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Bonuses);
                testStep.SetOutput(navigator_CreateBonus.CreateBonus(bonus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Edit Bonus and Verify 
                stepName = "Edit Bonus and Verify";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Bonuses);
                testStep.SetOutput(navigator_CreateBonus.EditBonusAndVerify(bonus.SetType, bonus.Name, bonus.CategoryName, out string message));
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
        /// Test BTA_03 : Validation of Bonus 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_03_LN_Program_eCollateral_ValidaitonsBonus()
        {
            #region Object Declaration
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            var bonusData = new NonAdminUserData(driverContext);
            CategoryFields bonus = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
            var navigator_CreateBonus = new Navigator_Users_Program_eCollateral_BonusesPage(DriverContext);
            string stepOutput = "";
            string stepName = "";
            #endregion

            try
            {
                #region Object Initialization                
                var common = new Common(DriverContext);
                bonus.CategoryName = bonusData.BonusCategoryName;
                var date = DateTime.Now;
                bonus.StartDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bonus.Logo_Img_Hero = "null";
                bonus.CategoryTypeValue = CategoryFields.CategoryType.Bonus.ToString();
                bonus.ValueToSetInAttribute = "ValueGivenForAttribute";
                bonus.SetType = CategoryFields.Property.Name.ToString();
                var organizationsPage = new Navigator_Admin_OrganizationsPage(DriverContext);
                bonus.MultiLanguage = CategoryFields.Languages.English.ToString();
                bonus.ChannelProperties = CategoryFields.Channel.Web.ToString();
                #endregion

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

                #region Step3:Create New Category with Bonus
                stepName = "Create new Category as " + bonus.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(bonus));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Verify Error Message if Bonus Name is Empty
                stepName = "Verify Error Message if Bonus Name is Empty";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Bonuses);
                testStep.SetOutput(navigator_CreateBonus.VerifyErrorMessageForEmptyName());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                //#region Step5: Verify Error Message if Bonus Name is more than 100 Charecters
                //stepName = "Verify Error Message if User Name Name is more than 100 Charecters";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //bonus.Name = organizationsPage.RandomAlphanumericString(new System.Random().Next(101, 105));
                //navigator_CreateBonus.ValidateBonusName(bonus, out string message).ToString();
                //testStep.SetOutput(message);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

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
    }
}
