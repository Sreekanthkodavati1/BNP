using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// This calss is to test Create, Edit and Dete
    /// </summary>
    [TestClass]
    public class BTA1587_Regression_Navigator_Store : ProjectTestBase
    {
        Login login = new Login();
        TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA_224 : Create new store
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_224_LN_Regression_Navigator_Store_CreateAndEditStore()
        {
            #region Object Initialization
            bool stepstatus = true;
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var StorePage = new Navigator_Users_Program_Components_StoresPage(driverContext);
            Store store = new Store
            {
                StoreNumber = RandomDataHelper.RandomNumber(4),
                StoreName = "Auto_Store" + RandomDataHelper.RandomString(4),
                PhoneNumber = RandomDataHelper.RandomNumber(10),
                City = "TestCity",
                State = "TestState",
                Country = "TestCountry",
                Address = "TestAddress"
            };

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

                #region Step4: Create new Store             
                stepName = "Create new Store";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.CreateStore(store, out string msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify the presence of newly created store in the grid
                stepName = "Verify the presence of newly created store in the grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                StorePage.VerifyTheExistanceOfStore(store, out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Edit and Verify the updated store details
                stepName = "Edit and Verify the updated store details";
                testStep = TestStepHelper.StartTestStep(testStep);
                StorePage.EditAndVerify_StoreDetails(store, out msg);
                testStep.SetOutput(msg);
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
        /// Test BTA_114 : Delete and verify the existance of store
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_114_LN_Regression_Navigator_Store_DeleteStore()
        {
            #region Object Initialization
            bool stepstatus = true;
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var StorePage = new Navigator_Users_Program_Components_StoresPage(driverContext);
            Store store = new Store
            {
                StoreNumber = RandomDataHelper.RandomNumber(4),
                StoreName = "Auto_Store" + RandomDataHelper.RandomString(4),
                PhoneNumber = RandomDataHelper.RandomNumber(10),
                City = "TestCity",
                State = "TestState",
                Country = "TestCountry",
                Address = "TestAddress"
            };

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

                #region Step4: Create new Store             
                stepName = "Create new Store";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.CreateStore(store, out string msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Verify the presence of newly created store in the grid
                stepName = "Verify the presence of newly created store in the grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.VerifyTheExistanceOfStore(store, out msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Delete and Verify the existance of store in the grid
                stepName = "Delete and Verify the existance of store in the grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Stores);
                StorePage.DeleteStoreAndVerifyItsExistance(store, out msg);
                testStep.SetOutput(msg);
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
    }
}
