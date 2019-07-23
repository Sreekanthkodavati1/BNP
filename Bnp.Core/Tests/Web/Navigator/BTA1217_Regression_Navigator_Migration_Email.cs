using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Email;
using Bnp.Core.WebPages.Navigator.UsersPage.Email_AWS;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Class to test BTA-1217  : Migration of Emails
    /// </summary>
    [TestClass]
    public class BTA1217_Regression_Navigator_Migration_Email : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// This method is to test the migration of email
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA_263_LN_Migration_Email()
        {
            #region Object Initialization
            Migration Migration = new Migration(driverContext);
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var EmailPage = new Navigator_EmailMessagePage(DriverContext);
            var navigator_Users_EmailAWSPage = new Navigator_AWSEmail_EmailsPage(DriverContext);
            bool StepStatus = true;
            string randomStr = RandomDataHelper.RandomString(4);
            string EmailMessageName = ProjectBasePage.Orgnization_value + WebsiteData.EmailMessageName+ randomStr;
            string Description = "Description";
            string DMCCode = WebsiteData.DMCCode;
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

                #region Step3: Create new email message
                stepName = "Create new email message";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.email);
                StepStatus = EmailPage.CreateNewEmailMessage(EmailMessageName, Description, DMCCode, out string outMessage);
                testStep.SetOutput(outMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Switch to Migration Environment
                stepName = "Switching to Migration Environment :" + Migration.MigrationEnvironment;
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SwitchEnvironment();
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment(Migration.MigrationEnvironment, Migration.MigrationOrderId, out string _output); testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Navigate to Migration Page and Delete Migration Set if any
                stepName = "Navigate to Migration Page and Delete Migration Set if any";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.migration);
                Navigator_MigrationPage _MigrationPage = new Navigator_MigrationPage(DriverContext);
                Migration.BuildMigrationSetName = Migration.MigrationSets.Migration_Email.ToString() + "_" + DateHelper.GetDate("Current");
                _MigrationPage.DeleteIfMigrationSetExists(Migration.MigrationSets.Migration_Email.ToString(), out string output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Create  New Migration Set
                stepName = "Create  New Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.MigrationSetCreation(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Edit Items and Generate Items
                stepName = "Edit Items and Generate Items";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.EditItems(Migration.BuildMigrationSetName, ProjectBasePage.Env_value, DateHelper.GeneratePastTimeStampBasedonMin(2));
                _MigrationPage.SelectItemsForAWSEMail(false, "", EmailMessageName, out _output);
                testStep.SetOutput(_output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Initiate Migration Set
                stepName = "Initiate Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.InitiateMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Approve Migration Set
                stepName = "Approve Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.ApproveMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Run Migration Set
                stepName = "Run Now Migration Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                _MigrationPage.RunNowMigrationSet(Migration.BuildMigrationSetName, out output);
                testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Verify migrated email message displayed on Email Messages page
                stepName = "Verify migrated email message displayed on Email Messages page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.email);
                StepStatus = EmailPage.VerifyMailingNameMessageDetails(EmailMessageName, DMCCode);
                if (StepStatus)
                {
                    testStep.SetOutput(" Email :" + EmailMessageName + " Migrated Successfully and Appeared on Mails Page");
                }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Logout
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
