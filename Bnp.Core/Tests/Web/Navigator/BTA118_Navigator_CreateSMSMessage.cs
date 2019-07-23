using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.SMS;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA118_Navigator_CreateSMSMessage : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        public string OrderTest_Status = "";

        public BTA118_Navigator_CreateSMSMessage()
        {
        }
        public BTA118_Navigator_CreateSMSMessage(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA118_Navigator_CreateSMSMessages()

        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData SmsData = new NonAdminUserData(driverContext);
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = ""; bool StepStatus;
            login.UserName = NavigatorUsers.NonAdminUser;
            login.Password = NavigatorUsers.NavigatorPassword;
            string SMSMessageName = ProjectBasePage.Orgnization_value + SmsData.SMSMessageName;
            string Description = "Description";
            string DMCCode = SmsData.SMSDMCCode;


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
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigating SMS Page
                stepName = "Navigating SMS Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.sms);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify if the SMS Exists, if the SMS is not existed Create new SMS Messsage
                stepName = "Verify if the SMS Exists, if the SMS is not existed Create new SMS Messsage";
                testStep = TestStepHelper.StartTestStep(testStep);
                var SMSPage = new Navigator_SMSMessagePage(DriverContext);
                StepStatus = SMSPage.CreateNewSMSMessage(SMSMessageName, Description, DMCCode, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: LogOut
                stepName = "Logout as USER Admin With All roles";
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

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, false, DriverContext.SendScreenshotImageContent("WEB"));
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
