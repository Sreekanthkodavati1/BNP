using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using System.Configuration;
using BnPBaseFramework.Web.Helpers;
using Bnp.Core.Tests.API.Validators;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-1590  Verify Forgot Password and Select option Regression Test Casesin CS Portal
    /// </summary>
    [TestClass]
    public class BTA1592_CSP_Regression_CustomerAppeasment_MergeAccounts_UpdateProfile : ProjectTestBase
    {
        #region Global Object Initialization
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps;
        public TestStep testStep;
        #endregion

       
        /// <summary>
        /// This Test case is to update profile and Verify the Updated Values on Account Summary
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA12_CSP_UpdateMember_BasicInfoAndAddress()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            MemberProfile MP_Model = new MemberProfile(DriverContext);
            Member member = MP_Model.GenerateMemberBasicInfo();
            MemberDetails memberDetails = MP_Model.GenerateMemberDetails();
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;

            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);

                basePages.CreateMember_UsingSoap(out member, out string LoyaltyId, listOfTestSteps);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyId, listOfTestSteps);
                CSPSearchPage.Select(member.FirstName, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, listOfTestSteps);
                CSP_UpdateProfilePage.UpdateBasicInfo_Details(member, memberDetails, listOfTestSteps);
                CSP_UpdateProfilePage.UpdateAddress_Details(memberDetails, listOfTestSteps);
                CSP_UpdateProfilePage.SaveUpdateProfile(listOfTestSteps);
                CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyId, listOfTestSteps);
                CSPAccountSummaryPage.VerifyFirstName(member.FirstName, listOfTestSteps);
                CSPAccountSummaryPage.VerifyLastName(member.LastName, listOfTestSteps);
                CSPAccountSummaryPage.VerifyPrimaryEmail(member.PrimaryEmailAddress, listOfTestSteps);
                CSPAccountSummaryPage.VerifyAddressLine1(memberDetails.AddressLineOne, listOfTestSteps);
                CSPAccountSummaryPage.VerifyAddressLine2(memberDetails.AddressLineTwo, listOfTestSteps);
                CSPAccountSummaryPage.VerifyCity(memberDetails.City, listOfTestSteps);
                CSPAccountSummaryPage.VerifyState(memberDetails.StateOrProvince, listOfTestSteps);
                CSPAccountSummaryPage.VerifyZipCode(memberDetails.ZipOrPostalCode, listOfTestSteps);
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

        /// <summary>
        /// This Test case is to update profile With Maximum and Verify the Updated Values on Account Summary
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA13_CSP_UpdateMemberWithMaximumValues()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;

            try
            {
                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                MemberDetails details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();
                VirtualCard vc = MP_Model.GenerateVirtualCardAboveMaxValues();
                #endregion
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);

                basePages.CreateMember_UsingSoap(out member, out string LoyaltyId, listOfTestSteps);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyId, listOfTestSteps);
                CSPSearchPage.Select(member.FirstName, listOfTestSteps);

                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, listOfTestSteps);

                member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();

                CSP_UpdateProfilePage.UpdateBasicInfo_Details(member, details, listOfTestSteps);
                CSP_UpdateProfilePage.UpdateAddress_Details(details, listOfTestSteps);
                CSP_UpdateProfilePage.UpdateContactInfo_Details(details, member, listOfTestSteps);
                CSP_UpdateProfilePage.SaveUpdateProfile(listOfTestSteps);
                string Member_FirstName = member.FirstName.Substring(0, 50);
                CSPAccountSummaryPage.VerifyFirstName(Member_FirstName, listOfTestSteps);

                string Member_LastName = member.LastName.Substring(0, 50);
                CSPAccountSummaryPage.VerifyLastName(Member_LastName, listOfTestSteps);

                string Member_PrimaryEmailAddress = member.PrimaryEmailAddress.Substring(0, 254);
                CSPAccountSummaryPage.VerifyPrimaryEmail(Member_PrimaryEmailAddress, listOfTestSteps);

                string Member_details_AddressLineOne = details.AddressLineOne.Substring(0, 100);
                CSPAccountSummaryPage.VerifyAddressLine1(Member_details_AddressLineOne, listOfTestSteps);

                string Member_details_AddressLineTwo = details.AddressLineTwo.Substring(0, 100);
                CSPAccountSummaryPage.VerifyAddressLine2(Member_details_AddressLineTwo, listOfTestSteps);

                string Member_details_City = details.City.Substring(0, 50);
                CSPAccountSummaryPage.VerifyCity(Member_details_City, listOfTestSteps);

                CSPAccountSummaryPage.VerifyState(details.StateOrProvince, listOfTestSteps);

                string Member_details_ZipCode = details.ZipOrPostalCode.Substring(0, 25);
                CSPAccountSummaryPage.VerifyZipCode(Member_details_ZipCode, listOfTestSteps);

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
        /// <summary>
        /// BTA-269 : Create Two members and Merge one to another and Verify the same on Account activity and Account Summary Page
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA32_CSP_Merge_TwoMembers()
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
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                var CSP_MergAccountPage = new CSPortal_MergeAccountPage(DriverContext);

                #endregion
                ProjectBasePage.CreateMember_UsingSoap(out member, out string MemberOne_LoyaltyNumber, listOfTestSteps);
                string MemberOne_IPCODE = member.IpCode.ToString();
                string MemberOne_FirstName = member.FirstName.ToString();
                ProjectBasePage.CreateMember_UsingSoap(out member, out string MemberTwo_LoyaltyNumber, listOfTestSteps);
                string MemberTwo_IPCODE = member.IpCode.ToString();
                string MemberTwo_FirstName = member.FirstName.ToString();
                ProjectBasePage.CreateMember_UsingSoap(out member, out string MemberThree_LoyaltyNumber, listOfTestSteps);
                string MemberThree_IPCODE = member.IpCode.ToString();
                string MemberThree_FirstName = member.FirstName.ToString();


                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSP_SearchPage.Search_BasedOnLoyaltyID(MemberOne_LoyaltyNumber, listOfTestSteps);
                CSP_SearchPage.Select(MemberOne_FirstName, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MergeAccounts, listOfTestSteps);
                CSP_MergAccountPage.MergeAccounts(MemberOne_LoyaltyNumber, MemberTwo_LoyaltyNumber, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountSummary, listOfTestSteps);

                string MergeActivityText = "Account " + MemberTwo_LoyaltyNumber + " was merged into " + MemberOne_LoyaltyNumber + ".";
                CSP_AccountSummaryPage.VerifyMergeActivity("Note", MergeActivityText, listOfTestSteps);

                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, listOfTestSteps);
                CSP_SearchPage.Search_BasedOnLoyaltyID(MemberThree_LoyaltyNumber, listOfTestSteps);
                CSP_SearchPage.Select(MemberThree_FirstName, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MergeAccounts, listOfTestSteps);
                string ExpectedError_Message = "Invalid status Merged of member with ipcode " + MemberTwo_IPCODE + ".";
                CSP_MergAccountPage.MergeAccounts_WithAlreadyMergedUser(MemberThree_LoyaltyNumber, MemberTwo_LoyaltyNumber, ExpectedError_Message, listOfTestSteps);

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
