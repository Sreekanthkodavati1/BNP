using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Collections.Generic;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using System.Globalization;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-281 : Create new message if it does not exist in the format <Orgname>AutoMessage
    /// </summary>    
    [TestClass]
    public class BTA281_Navigator_Create_Message : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        public string OrderTest_Status = "";

        public BTA281_Navigator_Create_Message()
        {
        }
        public BTA281_Navigator_Create_Message(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        /// <summary>
        /// Read user name from config file
        /// </summary>
        readonly string userName = NavigatorUsers.NonAdminUser;
        /// <summary>
        /// Read password from config file
        /// </summary>
        readonly string password = NavigatorUsers.NavigatorPassword;

        /// <summary>
        /// Test BTA-281: Create new message if it does not exist
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA281_Navigator_Create_New_Message()
        {
            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var application_Nav_Util_Page = new Application_Nav_Util_Page(driverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var programPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var eCollateralPage = new Navigator_Users_Program_eCollateralPage(driverContext);
                var eCollateralTab = Navigator_Users_Program_eCollateralPage.eCollateralTabs.Messages;
                var messagesPage = new Navigator_Users_Program_eCollateral_MessagesPage(driverContext);
                CategoryFields messageData = new CategoryFields();
                messageData.Name = ProjectBasePage.Orgnization_value + NonAdminUserData.MessageName;
                var date = DateTime.Now;
                date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
                messageData.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
                messageData.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                bool status;
                #endregion               

                #region Step 1 : Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 2 : Login to navigator using USER_WithAllRoles               
                stepName = "Login to navigator using USER_WithAllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = userName;
                login.Password = password;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 3 : Select organization and environment on USER Home page
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 4 : Navigate to program -> eCollateralTab            
                stepName = "Navigate to program -> eCollateralTab";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                programPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                eCollateralPage.NavigateToProgramECollateralTab(eCollateralTab);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 5 : Create new message if message does not exist already              
                testStep = messagesPage.CreateNewMessage(messageData, out string messageStatus);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 6 : Logout             
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
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
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
