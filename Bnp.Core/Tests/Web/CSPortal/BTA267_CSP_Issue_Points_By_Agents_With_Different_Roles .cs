using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-267 : Issue Points By Agents With Different Roles
    /// </summary>
    [TestClass]
    public class BTA267_CSP_Issue_Points_By_Agents_With_Different_Roles : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        string stepName = "";
        bool stepstatus;
        string stepOutput = "";

        /// <summary>
        ///  Issue Points By AdminAgents
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA267_CSP_Issue_Points_By_Admin_Agent()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            var ProjectBase = new ProjectBasePage(DriverContext);
            try
            {
                #region Objects
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var cSPortal_CustomerAppeasementsPage = new CSPortal_CustomerAppeasementsPage(DriverContext);
                #endregion

                #region Step1:Adding member with CDIS service
                stepName = "Adding member with CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                Member user = ProjectBase.CreateMemberThroughCDIS();
                testStep.SetOutput("Created Member With LoyaltyId  " + ProjectBase.GetLoyaltyNumber(user));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Login As AdminAgent 
                stepName = "Login As AdminAgent User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.AdminAgent;
                login.Password = AgentValues.Agentpassword;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Login As AdminAgent User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(ProjectBase.GetLoyaltyNumber(user), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Adding points to member beyond range
                stepName = "Adding points to member beyond range";
                testStep = TestStepHelper.StartTestStep(testStep);
                var userName = user.FirstName;
                var maxPoints = 200;
                var points = new System.Random().Next(201, 500).ToString(); ;
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out string result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddingPointsToMember(points, maxPoints));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Adding Points To Member
                stepName = "Adding Points To Member";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = user.FirstName;
                points = new System.Random().Next(1,200).ToString(); ;
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out  result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddingPointsToMember(points,maxPoints));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Points in Accountactivity Page
                stepName = "Verify Points in Accountactivity Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = user.FirstName;
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, out result);
                cSPortal_CustomerAppeasementsPage.VerifyPointsInAccountActivityPage(points);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout As AdminAgent 
                stepName = "Logout As AdminAgent";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                stepstatus = false;
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
        ///  Issue Points By SrAdminAgents
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA267_CSP_Issue_Points_By_NonAdmin_Agent()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            try
            {
                #region 
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var cSPortal_CustomerAppeasementsPage = new CSPortal_CustomerAppeasementsPage(DriverContext);
                var basePages = new ProjectBasePage(DriverContext);
                #endregion

                #region Step1:Adding member with CDIS service
                stepName = "Adding member with CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                Member user = basePages.CreateMemberThroughCDIS();
                testStep.SetOutput("Created Member With LoyaltyId  " + basePages.GetLoyaltyNumber(user));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Login As SrAdmin_Agent 
                stepName = "Login As SrAdmin_Agent User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.SrAdminAgent;
                login.Password = AgentValues.Agentpassword;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Login As SrAdminAgent User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(basePages.GetLoyaltyNumber(user), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Adding points to member beyond range 
                stepName = "Adding points to member beyond range";
                testStep = TestStepHelper.StartTestStep(testStep);
                var userName = user.FirstName;
                var maxPoints = 100;
                var points = new System.Random().Next(101, 500).ToString(); ;
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out string result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddingPointsToMember(points,maxPoints));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Adding Points To Member
                stepName = "Adding Points To Member";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = user.FirstName;
                points = new System.Random().Next(1,100).ToString();
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out  result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddingPointsToMember(points,maxPoints));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Points in Accountactivity Page
                stepName = "Verify Points in Accountactivity Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = user.FirstName;
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, out result);
                cSPortal_CustomerAppeasementsPage.VerifyPointsInAccountActivityPage(points);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout As SrAdmin_Agent 
                stepName = "Logout As SrAdmin_Agent";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                stepstatus = false;
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
