using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Class to test BTA-1224  : Create Tiers
    /// </summary>
    [TestClass]
    public class BTA1224_Regression_Navigator_Program_Components_Tier : ProjectTestBase
    {
        Login login = new Login();
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA_103 : Create a Tier
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_103_LN_Programa_Components_Tier_CreateTier()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_Components_TiersPage = new Navigator_Users_Program_Components_TiersPage(DriverContext);
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select the tier application under components and verify the tiers panel displayed
                stepName = "Select the tier application under components and verify the tiers panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Tiers);
                testStep.SetOutput("Tiers panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create a new tier and verify it is present in the grid
                string TierName = CategoryFields.TierType.Tier_Defaults.ToString()+ RandomDataHelper.RandomAlphanumericString(4);
                stepName = "Create a new tier and verify it is present in the grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = navigator_Users_Program_Components_TiersPage.CreateTierAndVerify(TierName, out string outMessage);
                testStep.SetOutput(outMessage);
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
        /// Test BTA_104 : Create a Tier with multiple content attribute
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_104_LN_Programa_Components_Tier_CreateTierWithMultipleContentAttributes()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_Components_TiersPage = new Navigator_Users_Program_Components_TiersPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select the attributes application under components and verify and create a tier type attribute if not exists
                stepName = "Select the attributes application under components and verify and create a tier type attribute if not exists";
                testStep = TestStepHelper.StartTestStep(testStep);
                var attName = WebsiteData.AttributeAllContentType;
                string attributeName = attName + Navigator_Users_Program_Components_AttributesPage.ContentTypes.Tier.ToString();
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                testStep.SetOutput(attributesPage.CreateNewAttribute(attributeName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Select the tier application under components and verify the tiers panel displayed
                stepName = "Select the tier application under components and verify the tiers panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Tiers);
                testStep.SetOutput("Tiers panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create a new tier with extended attribute and verify it is present in the grid
                string TierName = CategoryFields.TierType.Tier_Defaults.ToString() + RandomDataHelper.RandomAlphanumericString(4);
                string attributeValue = attributeName+ "_Value_" + RandomDataHelper.RandomAlphanumericString(4);
                stepName = "Create a new tier with extended attribute and verify it is present in the grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = navigator_Users_Program_Components_TiersPage.CreateTierWithAttributeAndVerify(TierName, attributeName, attributeValue, out string outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
        /// Test BTA_105 : This method verifies the warning messagge in Default reward page when there is no tier created
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Navigator-Smoke")]
        public void BTA_105_LN_Programa_Components_Tier_VerifyTheWarningMessageInDefaultRewardPageWhenThereIsNoTierCreated()
        {
            #region Object Initialization
            bool stepstatus = true;
            CategoryFields product = new CategoryFields();
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_Components_TiersPage = new Navigator_Users_Program_Components_TiersPage(DriverContext);
            var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
            var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                //navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Select a newly created environment
                stepName = "Select a newly created environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select the program application and verify the program configuration panel displayed
                stepName = "Select the program application and verify the program configuration panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                testStep.SetOutput("Program configuration panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Select the tier application under components and verify the tiers panel displayed
                stepName = "Select the tier application under components and verify the tiers panel displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Tiers);
                testStep.SetOutput("Tiers panel opened successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify there is no tier displayed
                stepName = "Verify there is no tier displayed";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = navigator_Users_Program_Components_TiersPage.VerifyThereISNoTierDisplayed(out string outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Navigate to default rewards page and verify the warning message
                stepName = "Navigate to default rewards page and verify the warning message";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.DefaultRewards);
                stepstatus = navigator_Users_Program_Components_TiersPage.VerifyTheWarningMessageInDefaultRewardPage(out outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
