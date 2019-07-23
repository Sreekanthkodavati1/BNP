using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Website;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// User Story BTA-574 : MP_Verify Social Media for Rewards and Coupon
    /// </summary>
    [TestClass]
    public class BTA574_MP_Verify_SocialMedia_For_Rewards_And_Coupon : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        private Common common;

        /// <summary>
        /// Test Case BTA-574 : MP_Verify Social Media for Rewards and Coupon
        /// </summary>
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA574_MP_Verify_SocialMediaForRewardsAndCoupon()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object Initialization
                var userName = NavigatorUsers.NonAdminUser;               
                var password = NavigatorUsers.NavigatorPassword;
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var websitePage = new Navigator_Users_WebsitePage(DriverContext);
                var modulesPage = new Navigator_Users_Website_ModulesPage(DriverContext);
                common = new Common(DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                var MPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                var MPortal_MyWalletPage = new MemberPortal_MyWalletPage(DriverContext);
                var MPortal_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
                #endregion

                #region Part1: Navigator portal-Enable social media for rewards and coupons
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User with All roles User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = userName;
                login.Password = password;
                navigator_LoginPage.Login(login,Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3 : Select organization and environment on USER Home page"
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);                
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4: Navigate to Website > Modules page 
                stepName = "Navigate to Website > Modules page";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules,out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5: Enable Social Media for Coupons
                stepName = "Enable Social Media for Coupons";
                testStep = TestStepHelper.StartTestStep(testStep);
                var webSiteName = WebsiteData.MemberPortal_WebSiteName;
                var moduleType = EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.CouponsListView);
                var configName = Navigator_Users_Website_ModulesPage.ConfigNames.CFCoupons_Config.ToString();                
                modulesPage.EnableSocialMedia(moduleType, webSiteName, configName, out stroutput); testStep.SetOutput(stroutput);               
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Enable Social Media for Rewards
                stepName = "Enable Social Media for Rewards";
                testStep = TestStepHelper.StartTestStep(testStep);
                moduleType= EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.RewardHistoryListView);
                configName = Navigator_Users_Website_ModulesPage.ConfigNames.CFRewardsHistory_Config.ToString();
                moduleType = "Reward History - List View";
                configName = "CFRewardsHistory_Config";               
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg);
                modulesPage.EnableSocialMedia(moduleType, webSiteName, configName, out stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Bounce the Member portal App pool
                stepName = "Bounce the Member Portal App pool";
                testStep = TestStepHelper.StartTestStep(testStep);
                var WebsiteManagement = new Navigator_Users_WebsiteManagementPage(DriverContext);
                WebsiteManagement.Navigator_Website_Select_WebsiteManagementTab();
                WebsiteManagement.BounceAppPool("MemberPortal");
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
                #endregion

                #region Part2: Creating members using CDIS service and adding rewards and coupons
                #region Step9: Adding member with CDIS service               
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                testStep.SetOutput("Member UserName:" + output.Username + "; Member First Name:" + output.FirstName);               
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10: Getting Coupon Definitions from Service
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Getting Coupon Definitions from Service";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                testStep.SetOutput("First Coupon Name : " + def.CouponDefinition[1].Name);                
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Adding Coupon to  member from Service
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding Coupon to  member from Service";
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(vc[0].LoyaltyIdNumber, def.CouponDefinition[0].Id);
                testStep.SetOutput("MemberCoupon Added to the user : " + def.CouponDefinition[0].Name);               
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Get Recent Reward Catalog with CDIS service               
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Reward Catalog with CDIS service";
                RewardCatalogSummaryStruct[] rewardCatalog = cdis_Service_Method.GetRecentRewardCatalog(0,0,0);
                RewardCatalogSummaryStruct reward = new RewardCatalogSummaryStruct();
                foreach (RewardCatalogSummaryStruct r in rewardCatalog)
                {
                    if (r.CurrencyToEarn == 0)
                    {
                        reward = r;
                        break;
                    }
                }
                testStep.SetOutput("RewardID:" + reward.RewardID + " and the reward name is :" + reward.RewardName);                
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13: Add Members to Reward Catalog with CDIS service                 
                vc = output.GetLoyaltyCards();
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Add Members to Reward Catalog with CDIS service";
                AddMemberRewardsOut memberRewardsOut = (AddMemberRewardsOut)cdis_Service_Method.AddMemberRewards(vc[0].LoyaltyIdNumber, vc[0].LoyaltyIdNumber, reward);
                testStep.SetOutput("The reward with RewardID:" + memberRewardsOut.MemberRewardID + " has been added to the member with IPCODE: " + output.IpCode);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion
                #endregion

                #region Part3: Verifying the social media share options on Member Portal                
                #region Step14:Launch Member Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);                
                MPortal_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15: Login As Member
                stepName = "Login As Member User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = output.Username;
                login.Password = "Password1*";                
                MPortal_LoginPage.LoginMemberPortal(login.UserName, login.Password, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16: Navigate to Mywallet page
                stepName = "Navigate to Mywallet page";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out string message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17: Verify My Reward Social media share options
                stepName = "Verify My Reward Social Media Share Options";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_MyWalletPage.VerifySociaMediaOptionsForRewardsAndCoupons("rewards",out var Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step18: Verify My Coupons Social media share options
                stepName = "Verify My Coupons Social Media Share Options";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out message);
                var stepstatus = MPortal_MyWalletPage.VerifySociaMediaOptionsForRewardsAndCoupons("coupons",out  Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion  

                #region Step19: Logout from member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPortal_LoginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
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
