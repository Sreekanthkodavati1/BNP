using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Email;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-1584 : Create Email Messages and update and delete the same 
    /// </summary>
    [TestClass]
    public class BTA_1584_Regression_Navigator_Email : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// User Story BTA-225 : Create Email,update and delete the same
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        [TestCategory("Smoke")]
        public void BTA225_Navigator_Email_Create_Update_Delete_EmailMessages()
        {
            #region Objects
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool StepStatus;
            login.UserName = NavigatorUsers.NonAdminUser;
            login.Password = NavigatorUsers.NavigatorPassword;
            NonAdminUserData EmailInfo = new NonAdminUserData(driverContext);
            string EmailMessageName =ProjectBasePage.Orgnization_value + EmailInfo.EmailMessageName+"_" + RandomDataHelper.RandomString(4); ;
            string Description = "Description";
            string DMCCode = EmailInfo.DMCCode;
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

                #region Step3:Navigating Email Page
                stepName = "Navigating Email Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.email);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create new MailingName Messsage, if the MailingName is not existed 
                stepName = "Create new MailingName Messsage, if the MailingName is not existed ";
                testStep = TestStepHelper.StartTestStep(testStep);
                var EmailPage = new Navigator_EmailMessagePage(DriverContext);
                StepStatus = EmailPage.CreateNewEmailMessage(EmailMessageName, Description, DMCCode, out string stepoutput);
                testStep.SetOutput(stepoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Update and Verify if the Mailing Description Updated with new description 
                stepName = "Update and Verify if the Mailing Description Updated with new description";
                testStep = TestStepHelper.StartTestStep(testStep);
                Description = "UpdatedDescription";
                StepStatus = EmailPage.UpdateEmailMessage(EmailMessageName, Description, DMCCode, out string UpdateStatus);
                testStep.SetOutput(UpdateStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Delete and Verify if the Mail deleted successfully or not 
                stepName = "Delete and Verify if the Mail deleted successfully or not";
                testStep = TestStepHelper.StartTestStep(testStep);
                Description = "UpdatedDescription";
                StepStatus = EmailPage.DeleteEmailMessage(EmailMessageName, Description, DMCCode, out string deleteStatus);
                testStep.SetOutput(deleteStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: LogOut
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
