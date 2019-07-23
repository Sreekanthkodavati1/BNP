using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA268_CSP_SearchPurchaseHistoryWithSpecificDateRange : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA268_CSP_SearchPurchaseHistoryWithSpecificDate_Range()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);           
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepOutput = "";
            string stepName = "";
            bool stepstatus;
            try
            {
                #region Object Declaration
                Common common = new Common(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSPMemberAccountActivityPage = new CSPortal_MemberAccountActivityPage(DriverContext);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                var date = DateTime.Now.AddDays(-1);
                DateTime FromDate = new DateTime(date.Year, date.Month, 1);
                DateTime ToDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(2018, date.Month));
                FromDate.ToString("MMddyyyy");
                ToDate.ToString("MMddyyyy");
                #endregion

                #region Precondtion: Adding Members through CDIS
                stepName = "Adding member through CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                Member member = basePages.CreateMemberThroughCDIS();               
                testStep.SetOutput("Member Added with LoyaltyID " + basePages.GetLoyaltyNumber(member));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As csadmin 
                stepName = "Login As csadmin User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = CsPortalData.csadmin;
                login.Password = CsPortalData.csadmin_password;
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Search Based on Loyalty ID and Select member
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(basePages.GetLoyaltyNumber(member), out stepName);
                string FirstName = member.FirstName;
                CSPSearchPage.Select(FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                              

                #region Step4:Navigate to Account Activity Page
                stepName = "Navigate to Account Activity Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5A: Adding Transaction1 through CDIS 
                stepName = "Adding Transaction1 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member, date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(member) + " with Transaction number "+ HeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5B:Transaction1 Search and Verify Purchase History With Specific Date Range
                stepName = "Transaction1 : Search and Verify Purchase History ";
                testStep = TestStepHelper.StartTestStep(testStep);              
                CSPMemberAccountActivityPage.SelectDate_RC(FromDate, ToDate);
                testStep.SetOutput(CSPMemberAccountActivityPage.VerifyPurchaseHistoryBasedonHeaderId(HeaderId));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6A: Adding Transaction2 through CDIS 
                stepName = "Adding Transaction2 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId1 = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member, date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(member)+ " with Transaction number " + HeaderId1);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step6B:Transaction2 Search and Verify Purchase History With Specific Date Range
                stepName = "Transaction2 : Search and Verify Purchase History ";
                testStep = TestStepHelper.StartTestStep(testStep);                
                CSPMemberAccountActivityPage.SelectDate_RC(FromDate, ToDate);
                testStep.SetOutput(CSPMemberAccountActivityPage.VerifyPurchaseHistoryBasedonHeaderId(HeaderId1));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7A: Adding Transaction3 through CDIS 
                stepName = "Adding Transaction3 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId2 = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member, date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(member) + " with Transaction number " + HeaderId2);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7B:Transaction3 Search and Verify Purchase History With Specific Date Range
                stepName = "Transaction3 : Search and Verify Purchase History ";
                testStep = TestStepHelper.StartTestStep(testStep);               
                CSPMemberAccountActivityPage.SelectDate_RC(FromDate, ToDate);
                testStep.SetOutput(CSPMemberAccountActivityPage.VerifyPurchaseHistoryBasedonHeaderId(HeaderId2));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                //CSP_HomePage.LogoutCSPortal();
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
