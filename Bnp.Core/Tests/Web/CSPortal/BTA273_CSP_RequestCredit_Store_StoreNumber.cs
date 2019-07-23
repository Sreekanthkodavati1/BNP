using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;

namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA273_CSP_RequestCredit_Store_StoreNumber : ProjectTestBase
    {
        Login login = new Login();
        readonly RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        string Step_Output = "";
        public TestStep testStep;
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA_273_CSP_RequestCredit_Store_StoreNumber()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus = true;
            var CSPortal_RequestCredit = new CSPortal_RequestCredit(DriverContext);
            var CSPMemberAccountActivityPage = new CSPortal_MemberAccountActivityPage(DriverContext);
            var CSP_HomePage = new CSPortal_HomePage(DriverContext);
            var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
            var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);

            try
            {
                #region reading Data from dB
                List<string> TransactionList = new List<string>();
                stepName = "Searching Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                RequestCredit_Search_Criteria.TransactionNumber = TransactionList[0].ToString();
                RequestCredit_Search_Criteria.RegisterNumber = TransactionList[1].ToString();
                RequestCredit_Search_Criteria.TxnAmount = TransactionList[2].ToString();
                RequestCredit_Search_Criteria.TxnDate = TransactionList[3].ToString();
                RequestCredit_Search_Criteria.StoreNumber = TransactionList[4].ToString();
                if(RequestCredit_Search_Criteria.StoreNumber.Equals(""))
                {
                    throw new Exception("Store Number Not Avaialble for Transaction:" + RequestCredit_Search_Criteria.TransactionNumber);
                }
                DateTime Txn_dateformat = DateTime.Parse(RequestCredit_Search_Criteria.TxnDate);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Precondition:Create Member One
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                string LoyaltyNumber_One_Firstname = output.FirstName;
                string LoyaltyNumber_One = basePages.GetLoyaltyNumber(output);
                testStep.SetOutput("LoyaltyNumber:" + LoyaltyNumber_One + ",Name:" + output.FirstName + "Created Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep); ;
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out Step_Output); testStep.SetOutput(Step_Output);
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

                #region Step3:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyNumber_One, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(LoyaltyNumber_One_Firstname);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyNumber_One, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Navigate to Request Credit
                stepName = "Navigate to Request Credit Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.RequestCredit, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Search With Store Number
                stepName = "Search With Store Number:" + RequestCredit_Search_Criteria.StoreNumber;
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPortal_RequestCredit.Search_Store_BasedOnStoreNumber(RequestCredit_Search_Criteria.TransactionNumber, RequestCredit_Search_Criteria.StoreNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Get the data from DB
                stepName = "Searching Transaction in the Txn_Header Table";
                List<string> TransactionList_Header = new List<string>();
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList_Header = ProjectBasePage.GetTransactionDetailsFromTransactionHeaderTableFromDB(RequestCredit_Search_Criteria.TransactionNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Verify the Transactions
                stepName = "Verifying Transaction in Txn_Header table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = basePages.VerifyInputandOutputFromDB("Transaction Number", TransactionList_Header[0].ToString(), RequestCredit_Search_Criteria.TransactionNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
              
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Navigate to Account Activity Page
                stepName = "Navigate to Account ActivityPage";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Search and Verify Purchase History of First Transaction With Specific Date Range
                stepName = "Search and Verify Purchase History ";
                testStep = TestStepHelper.StartTestStep(testStep);
                Txn_dateformat.ToString("MMddyyyy");
                CSPMemberAccountActivityPage.SelectDate_RC(Txn_dateformat, Txn_dateformat);
                testStep.SetOutput(CSPMemberAccountActivityPage.VerifyPurchaseHistoryBasedonHeader(RequestCredit_Search_Criteria.TransactionNumber));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal(); testStep.SetOutput("Logout As csadmin is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }

            catch (Exception e)
            {
                testStep.SetOutput(Step_Output);
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