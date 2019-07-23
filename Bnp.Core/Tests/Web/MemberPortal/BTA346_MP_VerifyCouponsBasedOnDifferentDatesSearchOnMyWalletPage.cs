using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Bnp.Core.WebPages.CSPortal;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    [TestClass]
    public class BTA346_MP_VerifyCouponsBasedOnDifferentDatesSearchOnMyWalletPage: ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        private Common common;

        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA346_MPVerifyCouponsBasedOnDifferentDatesSearchOnMyWalletPage()
        {          
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            try
            {
                #region  Object Initialization
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                common = new Common(DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                var cSPortal_CustomerAppeasementsPage = new CSPortal_CustomerAppeasementsPage(DriverContext);
                var MPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                ProjectBasePage basePages = new ProjectBasePage(driverContext);
                var memberPortal_MyWalletPage = new MemberPortal_MyWalletPage(DriverContext);
                var memberPortal_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
                #endregion

                #region Precondtion:Create Member
                cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service and Adding Coupon to the Member";
                Member output = basePages.CreateMemberThroughCDIS();
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();                
                long memberCouponId = cdis_Service_Method.AddMemberCoupon(basePages.GetLoyaltyNumber(output), def.CouponDefinition[0].Id);
                testStep.SetOutput("Member  UserName: " + output.Username+" Added Successfully with Coupon : " + def.CouponDefinition[0].Name);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step1:Launch Member Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var MemberPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                MemberPortal_LoginPage.LaunchMemberPortal(login.MemberPortal_url,out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As Member
                stepName = "Login As Member User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = output.Username;
                login.Password = "Password1*";
                string MemberLoyaltyNumber = DatabaseUtility.GetLoyaltyID(output.IpCode.ToString());
                MemberPortal_LoginPage.LoginMemberPortal(login.UserName, login.Password,out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigate to Mywallet page
                stepName = "Navigate to Mywallet page";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out string message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify My Coupons section
                stepName = "Verify My Coupons section";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_MyWalletPage.VerifyMyCouponSection(def.CouponDefinition[0].Name, out string Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify My Coupons VIEW,Send To My Wallet,PRINT,DONE and Share Button Options
                stepName = "Verify My Coupons VIEW,Send To My Wallet,PRINT,DONE and Share Button Options";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = memberPortal_MyWalletPage.VerifyMyCouponsButtonOptions(out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion  

                #region Step6:Logout from member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                MemberPortal_LoginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            } 
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                Assert.Fail(e.Message);
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
