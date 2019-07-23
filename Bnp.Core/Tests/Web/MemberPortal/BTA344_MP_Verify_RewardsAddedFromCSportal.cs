using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// User Story BTA-344 : MP_Verify Rewards on My Wallet Page
    /// </summary>

    [TestClass]
    public class BTA344_MP_Verify_RewardsAddedFromCSportal : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        bool stepstatus;
        string stepOutput = "";
        /// <summary>
        /// Test Case BTA-344 : MP_Verify Rewards on My Wallet Page
        /// </summary>
        [TestMethod]
        public void BTA344_Verify_Member_Rewards()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region 
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var common = new Common(DriverContext);
                var cdis_Service_Method = new CDIS_Service_Methods(common);
                var cSPortal_CustomerAppeasementsPage = new CSPortal_CustomerAppeasementsPage(DriverContext);
                var MPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                var MP_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
                var myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                var myWalletPage = new MemberPortal_MyWalletPage(driverContext);
                var Mp_LoginPage = new MemberPortal_LoginPage(DriverContext);
                ProjectBasePage basePages = new ProjectBasePage(driverContext);
                #endregion

                #region Step1:Adding member with CDIS service
                stepName = "Adding member through CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                Member member = basePages.CreateMemberThroughCDIS();
                testStep.SetOutput("Member Added with LoyaltyID " + basePages.GetLoyaltyNumber(member));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Get Recent Reward Catalog with CDIS service
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service";
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(cdis_Service_Method.GetActiveRewardCatalogCount() - 5, 10, 100);
                testStep.SetOutput("RewardName:" + rewardCatalog[0].RewardName);
                Logger.Info("RewardName:" + rewardCatalog[0].RewardName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Launch Customer Service Portal URL is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Login As csadmin 
                stepName = "Login As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Login As AdminAgent User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(basePages.GetLoyaltyNumber(member), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Adding Points To Member
                stepName = "Adding Points To Member";
                testStep = TestStepHelper.StartTestStep(testStep);
                var userName = member.FirstName;
                var maxPoints = 10000;
                var points = new System.Random().Next(100, 500).ToString();
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out string result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddingPointsToMember(points, maxPoints));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);

                #endregion

                #region Step7:Adding rewards to member 
                stepName = "Adding rewards to member";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = member.FirstName;
                var rewardName = MemberPortalData.RewardName;
                CSPSearchPage.Select(userName);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out  result);
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddRewardAppeasement(rewardName, out string message));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep.SetOutput("Logout is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Launch MPPortal Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_LoginPage.LaunchMemberPortal(login.MemberPortal_url,out string Message);
                testStep.SetOutput("Launch Member Portal URL is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Login As Newely Created Member 
                stepName = "Login As Newely Created Member ";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = member.Username;
                login.Password =MemberPortalData.MP_password;
                MPortal_LoginPage.LoginMemberPortal(login.UserName, login.Password, out Message);
                testStep.SetOutput("Login As "+ login.UserName + " User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Navigate to My Wallet page
                stepName = "Verify successful navigation to My Wallet page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out  Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify My Reward sections
                stepName = "Verify My Reward sections";
                testStep = TestStepHelper.StartTestStep(testStep);
                var date = DateTime.Now;
                date = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(date, TimeZoneInfo.Local.Id, "Central Standard Time");
                var FromDate = date.ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                var ToDate = date.AddYears(10).ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                myWalletPage.VerifyMyRewardSection(rewardName, FromDate,ToDate , MemberPortal_MyWalletPage.RewardStatus.Active.ToString(), out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify My Reward buttons
                stepName = "Verify My Reward buttons";
                testStep = TestStepHelper.StartTestStep(testStep);
                myWalletPage.VerifyMyRewardButtonOptions(out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion  

                #region Step14:Logout from member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                Mp_LoginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
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