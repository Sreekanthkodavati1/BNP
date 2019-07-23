using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using Bnp.Core.WebPages.Navigator.Promotion;
using Bnp.Core.WebPages.Navigator.UsersPage;
using BnPBaseFramework.Web.Helpers;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA995_Regression_Navigator_Promotions :ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void LN_30_Create_Non_Targeted_Promotion_with_Supported_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Non-Targeted Promotion With EnrollmentType As Supported";
                stepName = "Create Non-Targeted Promotion With EnrollmentType As Supported";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Supported.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6: "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName8:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void LN_33_Create_Targeted_Promotion_with_Supported_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Targeted Promotion With Enrollmenttype As None";
                stepName = "Create Targeted Promotion With Enrollmenttype As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Supported.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.Targeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName6: "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion.

                #region Step7:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.Targeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName8:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void LN_31_Create_Non_Targeted_Promotion_with_Required_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Non-Targeted Promotion With Enrollmenttype As Required";
                stepName = "Create Non-Targeted Promotion With Enrollmenttype As Required";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Required.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName6: "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName8:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void LN_34_Create_Targeted_Promotion_with_Required_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Targeted Promotion With Enrollmenttype As Required";
                stepName = "Create Targeted Promotion With Enrollmenttype As Required";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.Required.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.Targeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName6: "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion.

                #region Step7:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.Targeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName8:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void LN_29_Create_NonTargeted_Promotion_with_None_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName6: "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion.

                #region Step7:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName8:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void LN_32_Create_Targeted_Promotion_with_None_option()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";

            try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                #endregion

                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:"Create Targeted Promotion With EnrollmentTypes As None";
                stepName = "Create Targeted Promotion With EnrollmentTypes As None";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                promotions.Enrollmenttype = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.Targeted.ToString(), promotions, promotions.Enrollmenttype));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName5: "Verify Non-Targeted Promotion Details In Grid";
                stepName = "Verify Non-Targeted Promotion Details In Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                var searchCriteria = "Name";
                var result = navigator_PromotionPage.VerifyPromotionDetailsInGrid(promotions, searchCriteria,out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5: "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion.

                #region Step5:Get the data from DB
                stepName = "Searching Promotion in the LW_Promotion Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = navigator_PromotionPage.VerifyPromotionDetailsInDatabese(promotions.Name, promotions.Description,promotions.Enrollmenttype, Navigator_PromotionsPage.PromotionTypes.Targeted.ToString());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName6:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
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
        public void BTA_35_LN_CreatePromotion_With_Certificates_And_Generate_Promotion()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = ""; try
            {
                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                Promotions promotions = new Promotions();
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                promotions.StartDate = DateHelper.GetDate("Current");
                promotions.EndDate = DateHelper.GetDate("Future");
                string CertNumberFormat = promotion.CertNumberFormat;
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

                #region stepName3:"Create Targeted Promotion";
                stepName = "Create Targeted Promotion";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "value for" + promotions.Name;
                string enrollmentType = Navigator_PromotionsPage.EnrollmentTypes.None.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.Targeted.ToString(), promotions, enrollmentType));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Search promotion and Generate Certificate for Promotion
                stepName = "Search promotion and Generate Certificate for Promotion";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_PromotionPage.SearchPromotion(promotions.Name, "Name");
                testStep.SetOutput(navigator_PromotionPage.GeneratePromotionCertificatesAndVerify(CertNumberFormat, 10, promotions.StartDate, promotions.EndDate));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region stepName5:"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion

            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(e.Message);
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
