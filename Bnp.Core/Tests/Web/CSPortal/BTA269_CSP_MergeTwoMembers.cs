using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    /// <summary>
    /// BTA-269 : Create Two members and Merge one to another and Verify the same on Account activity and Account Summary Page
    /// </summary>
    [TestClass]
    public class BTA269_CSP_MergeTwoMembers : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// BTA-269 : Create Two members and Merge one to another and Verify the same on Account activity and Account Summary Page
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA269_CSP_Merge_TwoMembers()
        {
            ProjectBasePage ProjectBasePage = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;

            try
            {
                #region Objects and Test Data
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSP_SearchPage = new CSPortal_SearchPage(DriverContext);
                var CSP_AccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);
                var CSP_AccountActivityPage = new CSPortal_MemberAccountActivityPage(DriverContext);

                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                var CSP_MergAccountPage = new CSPortal_MergeAccountPage(DriverContext);

                #endregion
                ProjectBasePage.CreateMember_UsingSoap(out member, out string MemberOne_LoyaltyNumber, listOfTestSteps);
                string MemberOne_FirstName = member.FirstName.ToString();
                ProjectBasePage.CreateMember_UsingSoap(out member, out string MemberTwo_LoyaltyNumber, listOfTestSteps);
      
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSP_SearchPage.Search_BasedOnLoyaltyID(MemberOne_LoyaltyNumber, listOfTestSteps);
                CSP_SearchPage.Select(MemberOne_FirstName, listOfTestSteps);
                CSP_AccountSummaryPage.VerifyLoyaltyId(MemberOne_LoyaltyNumber, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MergeAccounts, listOfTestSteps);
                CSP_MergAccountPage.MergeAccounts(MemberOne_LoyaltyNumber, MemberTwo_LoyaltyNumber, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountSummary, listOfTestSteps);

                string MergeActivityText = "Account " + MemberTwo_LoyaltyNumber + " was merged into " + MemberOne_LoyaltyNumber + ".";
                CSP_AccountSummaryPage.VerifyMergeActivity("Note", MergeActivityText, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, listOfTestSteps);
                CSP_AccountActivityPage.VerifyMergeActivity("Note", MergeActivityText, listOfTestSteps);
                CSP_HomePage.LogoutCSPortal(listOfTestSteps);
                testCase.SetStatus(true);

            }
            catch (Exception e)
            {
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