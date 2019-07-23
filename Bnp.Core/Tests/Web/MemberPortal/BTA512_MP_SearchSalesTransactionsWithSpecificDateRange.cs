using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    [TestClass]
    public class BTA512_MP_SearchSalesTransactionsWithSpecificDateRange : ProjectTestBase
    {
        Login login = new Login();
        readonly RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        string Step_Output = "";
        public TestStep testStep;
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA_512_MP_Search_SalesTransactionsWithSpecificDateRange()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus = true;
            Common common = new Common(DriverContext);

            try
            {
                #region  Object Initialization
                common = new Common(DriverContext);
                var memberPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                var memberPortal_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
                var memberPortal_AccountActivityPage = new MemberPortal_AccountActivityPage(DriverContext);
                var date = DateTime.Now.AddDays(-1);
                DateTime FromDate = new DateTime(date.Year, date.Month, 1);
                DateTime ToDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(2018, date.Month));
                #endregion   

                #region Precondtion: Adding Members through CDIS
                stepName = "Adding member through CDIS service";
                testStep = TestStepHelper.StartTestStep(testStep);
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                Member user = basePages.CreateMemberThroughCDIS();
                Member[] member = cdis_Service_Method.GetCDISMemberByCardId(basePages.GetLoyaltyNumber(user));
                testStep.SetOutput(" Member Added with LoyaltyID: " + basePages.GetLoyaltyNumber(user));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step1:Launch Member Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var MemberPortal_LoginPage = new MemberPortal_LoginPage(DriverContext);
                MemberPortal_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As Member
                stepName = "Login As Member User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = user.Username;
                login.Password = "Password1*";
                string MemberLoyaltyNumber = DatabaseUtility.GetLoyaltyID(user.IpCode.ToString());
                MemberPortal_LoginPage.LoginMemberPortal(login.UserName, login.Password, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigate to Account Activity Page
                stepName = "Navigate to Account ActivityPage";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.AccountActivity, out string message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep); ;
                #endregion

                #region Step4A: Adding Transaction1 through CDIS 
                stepName = "Adding Transaction1 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member[0], date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(user) + " with Transaction number " + HeaderId);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4B:Transaction1 Search and Verify Sales Transactions With Specific Date Range
                stepName = "Transaction1 : Search and Verify Sales Transactions";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_AccountActivityPage.SelectDate_RC(FromDate, ToDate);
                memberPortal_AccountActivityPage.VerifySalesTransactionsSection("Header Id", HeaderId, out message);
                testStep.SetOutput(message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5A: Adding Transaction2 through CDIS 
                stepName = "Adding Transaction2 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId1 = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member[0], date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(user) + " with Transaction number " + HeaderId1);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step5B:Transaction2 Search and Verify Sales Transactions With Specific Date Range
                stepName = "Transaction2 : Search and Verify Sales Transactions ";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_AccountActivityPage.SelectDate_RC(FromDate, ToDate);
                memberPortal_AccountActivityPage.VerifySalesTransactionsSection("Header Id", HeaderId1, out message);
                testStep.SetOutput(message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6A: Adding Transaction3 through CDIS 
                stepName = "Adding Transaction3 through CDIS";
                testStep = TestStepHelper.StartTestStep(testStep);
                string HeaderId2 = cdis_Service_Method.UpdateMember_AddTransactionRequiredDate(member[0], date);
                testStep.SetOutput("Updated Transaction for the member LoyaltyID " + basePages.GetLoyaltyNumber(user) + " with Transaction number " + HeaderId2);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6B:Transaction3 Search and Verify Sales Transactions With Specific Date Range
                stepName = "Transaction3 : Search and Verify Sales Transactions ";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_AccountActivityPage.SelectDate_RC(FromDate, ToDate);
                memberPortal_AccountActivityPage.VerifySalesTransactionsSection("Header Id", HeaderId2, out message);
                testStep.SetOutput(message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Logout from member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                memberPortal_LoginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                testStep.SetOutput(Step_Output);
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
