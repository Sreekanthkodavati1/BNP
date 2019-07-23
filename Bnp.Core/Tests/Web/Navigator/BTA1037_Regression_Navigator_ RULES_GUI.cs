using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Promotion;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Email;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Bnp.Core.Tests.Web.Navigator
{
    [TestClass]
    public class BTA1037_Regression_Navigator_RULES_GUI : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        [TestMethod]
        [TestCategory("Navigator-Smoke")]
        public void LN_01_IssueVirtualCardIfNotExist()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberSave";
                Rule.RuleName = "IssueVirtualCardIfNotExist";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Create Virtual Card";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "1";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_Configurations_CreateVirtual_Card RuleConfig = new Rules_Configurations_CreateVirtual_Card();
                RuleConfig.Skipcreation_Onexistingvirtualcard = true;
                RuleConfig.LoyaltyId_GenerationSource = "Generate New ID";
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);             
                _RuleTriggers.CreateRule(Rule,out string Output);testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                
                #region stepName 8:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueVirtualCardIfNotExist(RuleConfig,Rule, out  Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 9: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        public void LN_02_DefaultRewardAppeasementRule()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberOthers";

                Rule.RuleName = "DefaultRewardAppeasementRule";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward from catalog";
                Rule.Invocation = "AfterUpdate";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "1";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_IssueRewardfromcatalog RuleConfig = new Rules_IssueRewardfromcatalog();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.PointsConsumedWhenIssued = "NoAction";
                RuleConfig.ExpirationDate = "AddYear(Date(), 5)";
                RuleConfig.AssignLoyaltyWareCertificate = true;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent,out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueRewardfrom_Catalog(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 9: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Coupon")]
        public void LN_03_IssueCouponAppeasement()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Issue Coupon Appeasement Rule
                Rules Rule = new Rules();
                Rule.RuleName = "Issue Coupon Appeasement";
                Rule.RuleOwner = "CouponAppeasement";
                Rule.RuleType = "Issue Coupon";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "0";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = true;
                Rule.LogExecution = true;
                Rule.QueueruleExecution = false;

                Rules_IssueCoupon RuleConfig = new Rules_IssueCoupon();
                RuleConfig.AssignLoyaltyWareCertificate = true;
                RuleConfig.CouponName= "";
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: "+Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueCoupon(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        public void LN_04_RuleForLW_MESSAGEtable()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "Message";

                Rule.RuleName = "Rule For LW_MESSAGE table";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Message";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "5";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueMessage RuleConfig = new Rules_IssueMessage();
                RuleConfig.AllowMultipleMessages = true;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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
           

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8:Add Configuration Details
                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueMessage(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Evaluate Tier")]
        public void LN_05_EvaluateTier()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "EvaluateTier";

                Rule.RuleName = "Evaluate Tier";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Evaluate Tier";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "8";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_EvaluateTier RuleConfig = new Rules_EvaluateTier();
                RuleConfig.VirtualCardLocationLogic = "AllCards";
                RuleConfig.Tiers_Gold = true;
                RuleConfig.Tiers_Platinum = true;
                RuleConfig.Tiers_Silver = true;
                RuleConfig.Tiers_Standard = true;

                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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


                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_EvaluteTier(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward")]
        public void LN_06_WelcomeReward()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberSave";

                Rule.RuleName = "Welcome Reward";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "9";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueReward RuleConfig = new Rules_IssueReward();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.RewardTypePoints = "Earned";
                RuleConfig.RewardName = "";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.IssuetheMembersRewardChoice = false;
                RuleConfig.AssignLoyaltyWareCertificate = true;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                CategoryFields product = new CategoryFields();
                NonAdminUserData data = new NonAdminUserData(driverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
                var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
                var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
                var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
                var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName;
                product.SetType = "Product Name";
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create new reward and Verify
                CategoryFields reward = new CategoryFields();
                reward.Name = "Welcome Reward";
                RuleConfig.RewardName = reward.Name;
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
                reward.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueReward(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward From Catalog")]
        public void LN_07_Reward_Catalog_Checkout()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberOthers";

                Rule.RuleName = "DefaultRewardAppeasementRule";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward from catalog";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "4";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_IssueRewardfromcatalog RuleConfig = new Rules_IssueRewardfromcatalog();
                RuleConfig.FulfillmentMethod = "Printed";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.ExpirationDate = "";
                
                RuleConfig.AssignLoyaltyWareCertificate = true;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueRewardfrom_Catalog(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 9: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Award Loyalty Currency")]
        public void LN_08_FriscoIsSoAwesome()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberSave";

                Rule.RuleName = "FriscoIsSoAwesome";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "12";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "75";
                RuleConfig.TierEvaluationRule = "";
                RuleConfig.AllowZeroPointAward = true;

                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 9: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Award Loyalty Currency")]
        public void LN_09_PointsFrom3to4()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Points From 3 to 4 Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Points From 3 to 4";
                Rule.RuleOwner = "TxnHeader";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "13";
                Rule.Expression = "{'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'bScript Expression','val':'1==1'}}]}";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "{'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'Yay Bonus','val':'IF((row.TxnDate >= ADDHOUR(GETBEGINNINGOFDAY(row.TxnDate), '15')) & (row.TxnDate <= ADDHOUR(GETBEGINNINGOFDAY(row.TxnDate), '16')), row.TXNAmount * 1, 0)'}}]}";
                RuleConfig.AllowZeroPointAward = true;
                RuleConfig.TierEvaluationRule = "";
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

           

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 7: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Award Loyalty Currency")]
        public void LN_10_TieredPoints()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for TieredPoints Rule
                Rules Rule = new Rules();

                Rule.RuleName = "TieredPoints";
                Rule.RuleOwner = "TxnHeader";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "14";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "row.TxnAmount*2";
                RuleConfig.TierEvaluationRule = "Evaluate Tier";
                RuleConfig.AllowZeroPointAward = false;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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


                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 7: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Award Loyalty Currency")]
        public void LN_11_AwardCurrencyOnJoining()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Award Currency On Joining Rule
                Rules Rule = new Rules();

                Rule.RuleName = "Award Currency On Joining";
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "18";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "5";
                RuleConfig.TierEvaluationRule = "";
                Rule.Rulestatus_ToInactive = true;

                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 7: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Award Loyalty Currency")]
        public void LN_12_AwardPointsBasedonTier()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();

                Rule.RuleName = "AwardPointsBasedonTier";
                Rule.RuleOwner = "TxnHeader";
                Rule.RuleType = "Award Loyalty Currency";
                Rule.Invocation = "AfterInsert";
                Rule.StartDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.EndDate = DateHelper.GeneratePastTimeStampBasedonDay(1);
                Rule.Sequence = "19";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = true;
                Rule.LogExecution = true;
                Rule.QueueruleExecution = false;
                Rule.Rulestatus_ToInactive = true;
                Rules_AwardLoyaltyCurrency RuleConfig = new Rules_AwardLoyaltyCurrency();
                RuleConfig.AccrualExpression = "{'Items':[{'type':'func','Data':{'fname':'IF'},'Items':[{'type':'parameter','Data':{'Name':'Expression 1','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'ISINTIER'},'Items':[{'type':'parameter','Data':{'Name':'TierName','Type':1,'Optional':false},'ManualString':''Standard''},{'type':'parameter','Data':{'Name':'Date','Type':4,'Optional':true}}]}]},{'type':'parameter','Data':{'Name':'Expression 2','Type':32767,'Optional':false},'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'TxnAmount * 0.5','val':'row.TXNAmount * 0.5'}}]},{'type':'parameter','Data':{'Name':'Expression 3','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'IF'},'Items':[{'type':'parameter','Data':{'Name':'Expression 1','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'ISINTIER'},'Items':[{'type':'parameter','Data':{'Name':'TierName','Type':1,'Optional':false},'ManualString':''Silver''},{'type':'parameter','Data':{'Name':'Date','Type':4,'Optional':true}}]}]},{'type':'parameter','Data':{'Name':'Expression 2','Type':32767,'Optional':false},'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'TXNAmount * 0.6','val':'row.TXNAmount * 0.6'}}]},{'type':'parameter','Data':{'Name':'Expression 3','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'IF'},'Items':[{'type':'parameter','Data':{'Name':'Expression 1','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'ISINTIER'},'Items':[{'type':'parameter','Data':{'Name':'TierName','Type':1,'Optional':false},'ManualString':''Gold''},{'type':'parameter','Data':{'Name':'Date','Type':4,'Optional':true}}]}]},{'type':'parameter','Data':{'Name':'Expression 2','Type':32767,'Optional':false},'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'TXNAmount * 0.7','val':'row.TXNAmount * 0.7'}}]},{'type':'parameter','Data':{'Name':'Expression 3','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'IF'},'Items':[{'type':'parameter','Data':{'Name':'Expression 1','Type':32767,'Optional':false},'Items':[{'type':'func','Data':{'fname':'ISINTIER'},'Items':[{'type':'parameter','Data':{'Name':'TierName','Type':1,'Optional':false},'ManualString':''Platinum''},{'type':'parameter','Data':{'Name':'Date','Type':4,'Optional':true}}]}]},{'type':'parameter','Data':{'Name':'Expression 2','Type':32767,'Optional':false},'Items':[{'type':'helper','BuilderId':'expressionBuilder','Data':{'ConditionName':'TXNAmount * 0.8','val':'row.TXNAmount * 0.8'}}]},{'type':'parameter','Data':{'Name':'Expression 3','Type':32767,'Optional':false}}]}]}]}]}]}]}]}]}";
                RuleConfig.TierEvaluationRule = "";
                RuleConfig.AllowZeroPointAward = true;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_AwardLoyaltyCurrency(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 7: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Message")]
        public void LN_13_Issue_Message()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "IssueMessage";
                Rule.RuleName = "Issue Message";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Message";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "6";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;
                Rules_IssueMessage RuleConfig = new Rules_IssueMessage();
                RuleConfig.AllowMultipleMessages = true;

                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify Rule Event;
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueMessage(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step 9: Logout
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward")]
        public void LN_14_RewardAppeasementRule()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberSave";

                Rule.RuleName = "RewardAppeasementRule";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward";
                Rule.Invocation = "AfterUpdate";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "17";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueReward RuleConfig = new Rules_IssueReward();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.RewardTypePoints = "Earned";
                RuleConfig.RewardName = "";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.IssuetheMembersRewardChoice = true;
                RuleConfig.AssignLoyaltyWareCertificate = false;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueReward(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward")]
        public void LN_15_AwardReward()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueVirtualCardIfNotExist Rule
                Rules Rule = new Rules();
                string RuleEvent = "RewardTrigger";

                Rule.RuleName = "AwardReward";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "16";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueReward RuleConfig = new Rules_IssueReward();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.RewardTypePoints = "Earned";
                RuleConfig.RewardName = "";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.IssuetheMembersRewardChoice = true;
                RuleConfig.AssignLoyaltyWareCertificate = false;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueReward(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward")]
        public void LN_16_IssueRewardForPoints()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for IssueRewardForPoints Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberSave";
                Rule.RuleName = "IssueRewardForPoints";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward";
                Rule.Invocation = "AfterUpdate";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "15";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueReward RuleConfig = new Rules_IssueReward();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.RewardTypePoints = "Earned";
                RuleConfig.RewardName = "Test_Reward";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.IssuetheMembersRewardChoice = false;
                RuleConfig.AssignLoyaltyWareCertificate = false;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                CategoryFields product = new CategoryFields();
                NonAdminUserData data = new NonAdminUserData(driverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
                var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
                var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
                var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
                var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName;
                product.SetType = "Product Name";
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create new reward and Verify
                CategoryFields reward = new CategoryFields();
                reward.Name = "Welcome Reward";
                RuleConfig.RewardName = reward.Name;
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
                reward.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueReward(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Reward")]
        public void LN_17_Issue_Reward_For_Member()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Issue Reward For Member	 Rule
                Rules Rule = new Rules();
                string RuleEvent = "MemberOthers";
                Rule.RuleName = "Issue Reward For Member";
                Rule.RuleOwner = RuleEvent;
                Rule.RuleType = "Issue Reward";
                Rule.Invocation = "BeforeInsert";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "11";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueReward RuleConfig = new Rules_IssueReward();
                RuleConfig.FulfillmentMethod = "Electronic";
                RuleConfig.RewardTypePoints = "Earned";
                RuleConfig.RewardName = "";
                RuleConfig.PointsConsumedWhenIssued = "Consume";
                RuleConfig.IssuetheMembersRewardChoice = false;
                RuleConfig.AssignLoyaltyWareCertificate = false;
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                Navigator_RuleEvent _RuleEvent = new Navigator_RuleEvent(DriverContext);
                CategoryFields product = new CategoryFields();
                NonAdminUserData data = new NonAdminUserData(driverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                var Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
                var navigator_CreateBonus_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
                var RewardCatalog_ProductsPage = new Navigator_Users_Program_RewardCatalog_ProductsPage(DriverContext);
                var Program_RewardCatalogPage = new Navigator_Users_Program_RewardCatalogPage(DriverContext);
                var RewardCatalog_RewardsPage = new Navigator_Users_Program_RewardCatalog_RewardsPage(DriverContext);

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region Step3:Create New Category with Product and Verify
                product.CategoryTypeValue = CategoryFields.CategoryType.Product.ToString();
                product.CategoryName = data.ProductCategoryName;
                stepName = "Create new Category as " + product.CategoryName;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateBonus_Category.CreateCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new product and Verify
                var contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Product;
                product.Name = data.ProductName;
                product.SetType = "Product Name";
                product.ValueToSetInAttribute = "ValueGivenForAttributeSetProduct";
                stepName = "Create New Product with Category as " + product.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Products);
                testStep.SetOutput(RewardCatalog_ProductsPage.CreateProduct_With_ProductCategory(product));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Create new reward and Verify
                CategoryFields reward = new CategoryFields();
                reward.Name = "Welcome Reward";
                RuleConfig.RewardName = reward.Name;
                reward.BalanceNeeded = "100";
                reward.CategoryTypeValue = product.Name;
                reward.SetType = "Reward Name";
                var date = DateTime.Now;
                reward.StartDate = date.ToString("MM/dd/yyyy  HH:mm:ss", new CultureInfo("en-US"));
                reward.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                stepName = "Create new reward for product as " + reward.Name + "";
                testStep = TestStepHelper.StartTestStep(testStep);
                reward.ValueToSetInAttribute = "ValueGivenForAttributeSetReward";
                contentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.Reward;
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.RewardCatalog);
                Program_RewardCatalogPage.NavigateToProgramRewardCatalogTab(Navigator_Users_Program_RewardCatalogPage.RewardCatalogTabs.Rewards);
                testStep.SetOutput(RewardCatalog_RewardsPage.CreateReward_With_Product(reward));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:Navigate to Model > Rules Events Page;
                stepName = "Navigate to Model -> Rules Events  Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleEvents);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Verify or Create Rule Event
                stepName = "Verify or Create Rule Event :" + RuleEvent;
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleEvent.CreateNewEvent(RuleEvent, out string EventMessage); testStep.SetOutput(EventMessage);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueReward(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Coupon")]
        public void LN_18_IssueCoupon_On_MemberSave()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Issue Coupon Appeasement Rule
                Rules Rule = new Rules();
                Rule.RuleName = "Issue Coupon On MemberSave";
                Rule.RuleOwner = "MemberSave";
                Rule.RuleType = "Issue Coupon";
                Rule.Invocation = "AfterUpdate";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "10";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = false;
                Rule.ContinueOnError = true;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_IssueCoupon RuleConfig = new Rules_IssueCoupon();
                RuleConfig.AssignLoyaltyWareCertificate = true;
                 #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
                var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);
                var navigator_Users_Program_eCollateral_CouponsPage = new Navigator_Users_Program_eCollateral_CouponsPage(DriverContext);
                var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
                CategoryFields coupon = new CategoryFields();
                var couponData = new NonAdminUserData(driverContext);

                string CouponName = couponData.CouponName;
                coupon.CouponCode = "";
                coupon.Name = CouponName;
                RuleConfig.CouponName = coupon.Name;

                coupon.CategoryName = couponData.CouponCategoryName;
                var date = DateTime.Now;
                date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
                coupon.StartDate = date.ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                coupon.ExpiryDate = date.AddYears(10).ToString("MM/dd/yyyy HH:mm:ss", new CultureInfo("en-US"));
                coupon.UsesAllowed = "10";
                coupon.CategoryTypeValue = CategoryFields.CategoryType.Coupon.ToString();
                coupon.ValueToSetInAttribute = "ValueGivenForAttribute";
                coupon.SetType = CategoryFields.Property.Name.ToString();
                
                coupon.MultiLanguage = CategoryFields.Languages.English.ToString();
                coupon.ChannelProperties = CategoryFields.Channel.Web.ToString();

                #endregion

                #region stepName 1: Open Navigator URL
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

                #region Step3:Create Category with Coupon
                stepName = "Create new Category as " + coupon.CategoryTypeValue;
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                testStep.SetOutput(navigator_CreateCoupon_Category.CreateCategory(coupon));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Create new Coupon with or without Attribute Set
                stepName = "Create Coupon with a category " + coupon.CategoryTypeValue + " with or without Attribute Set";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Coupons);
                navigator_Users_Program_eCollateral_CouponsPage.CreateCoupon(coupon);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion               

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_IssueCoupon(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
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
                testStep.SetOutput(e.ToString());
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
        [TestCategory("Navigator-Smoke")]
        [TestCategory("Issue Coupon")]
        public void LN_19_CFContactUsEmailRule()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region for Issue Coupon Appeasement Rule
                Rules Rule = new Rules();
                Rule.RuleName = "CFContactUsEmailRule";
                Rule.RuleOwner = "MemberOthers";
                Rule.RuleType = "Send Triggered Email";
                Rule.Invocation = "Manual";
                Rule.StartDate = DateHelper.GetDate("Current");
                Rule.EndDate = DateHelper.GetDate("Future");
                Rule.Sequence = "1";
                Rule.Expression = "1==1";
                Rule.Targeted = false;
                Rule.AlwaysRun = true;
                Rule.ContinueOnError = false;
                Rule.LogExecution = false;
                Rule.QueueruleExecution = false;

                Rules_CFContactUsEmailRule RuleConfig = new Rules_CFContactUsEmailRule();
                RuleConfig.TheEmailoftherecipient = "contactus@brierley.com";
               
                RuleConfig.Rule_Version = "1";
                #endregion

                #region
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                Migration Migration = new Migration(driverContext);
                NonAdminUserData promotion = new NonAdminUserData(DriverContext);
                var navigator_Users_ProgramPage = new Application_Nav_Util_Page(DriverContext);
                Navigator_RuleTriggers _RuleTriggers = new Navigator_RuleTriggers(DriverContext);
                Navigator_ModelHomePage _ModelHomePage = new Navigator_ModelHomePage(DriverContext);
                #endregion

                #region stepName 1: Open Navigator URL
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

                #region Step3:Navigating Email Page
                stepName = "Navigating Email Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.email);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify if the Mailing Name Exists, if the MailingName is not existed Create new MailingName Messsage
                testStep = TestStepHelper.StartTestStep(testStep);
                NonAdminUserData EmailInfo = new NonAdminUserData(driverContext);
                string EmailMessageName = ProjectBasePage.Orgnization_value + EmailInfo.EmailMessageName;
                string Description = "Description";
                string DMCCode = EmailInfo.DMCCode;
                RuleConfig.Triggered_Email = EmailMessageName;
                var EmailPage = new Navigator_EmailMessagePage(DriverContext);
                bool StepStatus = EmailPage.CreateNewEmailMessage(EmailMessageName, Description, DMCCode, out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, StepStatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4:Navigate to Model > Rules Triggers Page;
                stepName = "Navigate to Model -> Rules Triggers Pag";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.model);
                _ModelHomePage.NavigateToModelTab(Navigator_ModelHomePage.ModelMenuTabs.RuleTriggers);
                testStep.SetOutput("Navigate to Model->Rules Triggers Page is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5:Create Rule IssueVirtualCardIfNotExist, Edit InCase values are not matching ;
                stepName = "Create Rule: " + Rule.RuleName.ToString();
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.CreateRule(Rule, out string Output); testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:Add Configuration Details

                stepName = "Add Configuration Details ";
                testStep = TestStepHelper.StartTestStep(testStep);
                _RuleTriggers.Configure_TriggerEmail(RuleConfig, Rule, out Output); testStep.SetOutput(Output);
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
                testStep.SetOutput(e.ToString());
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
