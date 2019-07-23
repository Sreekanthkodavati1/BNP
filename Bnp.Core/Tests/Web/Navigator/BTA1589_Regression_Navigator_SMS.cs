using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.SMS;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-1589 : Create SMS Message and Delete SMS Message
    /// </summary>
    [TestClass]
    public class BTA1589_Regression_Navigator_SMS : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// User Story BTA-163 : Create SMS Message and Delete SMS Message
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA163_Navigator_CreateSMSMessages()

        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData SmsData = new NonAdminUserData(driverContext);
            testCase = new TestCase(TestContext.TestName);
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
        /// User Story BTA-164 : Delete SMS Message
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA164_Navigator_DeleteSMSMessages()
        {
            #region Objects
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData SmsData = new NonAdminUserData(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = ""; bool StepStatus;
            login.UserName = NavigatorUsers.NonAdminUser;
            login.Password = NavigatorUsers.NavigatorPassword;
            string SMSMessageName = "Regression_"+ProjectBasePage.Orgnization_value + SmsData.SMSMessageName;
            string Description = "Description";
            string DMCCode = SmsData.SMSDMCCode;
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

                #region Step4:Delete and Verify SMS Exists in SMS List
                stepName = "Delete and Verify SMS Exists in SMS List";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool status=SMSPage.DeleteSMSAndVerify(SMSMessageName,DMCCode,out string DeleteStatus);
                testStep.SetOutput(DeleteStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
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
