using System;
using System.Collections.Generic;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    [TestClass]
    public class BTA509_MP_RequestCredit_Online_TransactionAmount : ProjectTestBase
    {
        Login login = new Login();
        readonly RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        string Step_Output = "";
        public TestStep testStep;

        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA_509_MP_RequestCredit_Online_TransactionAmount()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus = true;
            var Mp_RequestCredit = new MemberPortal_RequestCredit(DriverContext);
            var MP_LoginPage = new MemberPortal_LoginPage(DriverContext);
            var Mp_MyAccountPage = new MemberPortal_MyAccountPage(DriverContext);
            var CSPortal_RequestCredit = new CSPortal_RequestCredit(driverContext);
            var MPAccountActivityPage = new MemberPortal_AccountActivityPage(DriverContext);

            try
            {
                #region reading Data from dB
                List<string> TransactionList = new List<string>();
                stepName = "Searching Transaction in the Transaction History Details Table";
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList = ProjectBasePage.GetTransactionDetailsFromTransationHistoryTableFromDB(out Step_Output);
                RequestCredit_Search_Criteria.TransactionNumber = TransactionList[0].ToString();
                RequestCredit_Search_Criteria.RegisterNumber = TransactionList[1].ToString();
                RequestCredit_Search_Criteria.TxnAmount = TransactionList[2].ToString();
                RequestCredit_Search_Criteria.TxnDate = TransactionList[3].ToString();
                RequestCredit_Search_Criteria.StoreNumber = TransactionList[4].ToString();
                if (RequestCredit_Search_Criteria.StoreNumber.Equals(""))
                {
                    throw new Exception("Store Number Not Avaialble for Transaction:" + RequestCredit_Search_Criteria.TransactionNumber);
                }
                DateTime Txn_dateformat = DateTime.Parse(RequestCredit_Search_Criteria.TxnDate);
                string transactionDate = Txn_dateformat.ToString("MM/dd/yyyy");
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, "");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Precondition:Create Member One
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member member = basePages.CreateMemberThroughCDIS();
                testStep.SetOutput("LoyaltyNumber_One:" + basePages.GetLoyaltyNumber(member) + ",Name:" + member.FirstName + "Created Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step1:Launch MPPortal Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Message);
                testStep.SetOutput("Launch Member Portal URL is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As Newely Created Member 
                stepName = "Login As Newely Created Member ";
                testStep = TestStepHelper.StartTestStep(testStep);
                string userName = member.Username;
                login.Password = MemberPortalData.MP_password;
                MP_LoginPage.LoginMemberPortal(userName, login.Password, out Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigate to Request Credit
                stepName = "Navigate to Request Credit Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = Mp_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.RequestCredit, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Search With Transaction Amount
                stepName = "Select Online and Search With Transaction Amount: " + RequestCredit_Search_Criteria.TxnAmount;
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = Mp_RequestCredit.Search_BasedOnTransactionAmount(RequestCredit_Search_Criteria.TransactionNumber, RequestCredit_Search_Criteria.TxnAmount, MemberPortal_RequestCredit.TransactionType.Online.ToString(), out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Get the data from DB
                stepName = "Searching Transaction in the Txn_Header Table";
                List<string> TransactionList_Header = new List<string>();
                testStep = TestStepHelper.StartTestStep(testStep);
                TransactionList_Header = ProjectBasePage.GetTransactionDetailsFromTransactionHeaderTableFromDB(RequestCredit_Search_Criteria.TransactionNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify the Transactions
                stepName = "Verifying Transaction in Txn_Header table";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = basePages.VerifyInputandOutputFromDB("Transaction Number", TransactionList_Header[0].ToString(), RequestCredit_Search_Criteria.TransactionNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Navigate to Account Activity Page
                stepName = "Navigate to Account ActivityPage";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = Mp_MyAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.AccountActivity, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Search and Verify Sales Transactions With Specific Date Range
                stepName = "Search and Verify Sales Transactions";
                testStep = TestStepHelper.StartTestStep(testStep);
                MPAccountActivityPage.SelectDate_RC(Txn_dateformat, Txn_dateformat.AddDays(1));
                MPAccountActivityPage.VerifySalesTransactionsSection("Header Id", RequestCredit_Search_Criteria.TransactionNumber, out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9: Logout from Member Portal
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                MP_LoginPage.LogoutMPPortal();
                testStep.SetOutput("Logout from Member Portal is Successful");
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

