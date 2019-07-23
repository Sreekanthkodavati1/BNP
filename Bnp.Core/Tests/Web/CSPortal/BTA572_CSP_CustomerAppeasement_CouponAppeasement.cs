using System;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnpBaseFramework.API.Loggers;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;

namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA572_CSP_CustomerAppeasement_CouponAppeasement : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        bool stepstatus;
        string stepOutput = "";
        /// <summary>
        /// Test Case BTA-510 : CS_Customer Appeasement_Coupon Appeasement
        /// </summary>

        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA_572_CSP_CustomerAppeasement_CouponAppeasement()
        {
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            Common common = new Common(DriverContext);
            try
            {
                #region 
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                var basePages = new ProjectBasePage(DriverContext);
                var cSPortal_CustomerAppeasementsPage = new CSPortal_CustomerAppeasementsPage(DriverContext);
                var MPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                var MP_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
                var myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                var myWalletPage = new MemberPortal_MyWalletPage(driverContext);
                var Mp_LoginPage = new MemberPortal_LoginPage(DriverContext);

                #endregion

                #region Step1:Adding member with CDIS service
                stepName = "Adding member with CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                Member output = basePages.CreateMemberThroughCDIS();
                IList<VirtualCard> vc = output.GetLoyaltyCards();
                testStep.SetOutput("Created Member With LoyaltyId  " + vc[0].LoyaltyIdNumber);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Get Recent Coupons with CDIS service
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Get Recent Coupons with CDIS service";
                GetCouponDefinitionsOut def = cdis_Service_Method.GetCouponDefinitions();
                testStep.SetOutput("CouponsName:" + def.CouponDefinition[0].Name);
                Logger.Info("CouponsName:" + def.CouponDefinition[0].Name);
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
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc[0].LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify Member details displayed in Account Summary page
                stepName = "Verify Member details displayed in Account Summary page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var userName = output.FirstName;
                CSPSearchPage.Select(userName);
                testStep.SetOutput("Member details displayed in Account Summary page");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Navigate to Customer Appeasements page
                stepName = "Navigate to Customer Appeasements page";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, out string result);
                testStep.SetOutput(result);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Adding Coupon Appeasement
                stepName = "Adding Coupon Appeasement";
                testStep = TestStepHelper.StartTestStep(testStep);
                userName = output.FirstName;
                testStep.SetOutput(cSPortal_CustomerAppeasementsPage.AddCouponAppeasement(def.CouponDefinition[0].Name, out string message));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Logout As csadmin 
                stepName = "Logout As csadmin";
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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
