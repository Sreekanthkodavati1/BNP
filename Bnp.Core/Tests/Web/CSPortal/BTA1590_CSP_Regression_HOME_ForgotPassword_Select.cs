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
    public class BTA1590_CSP_Regression_HOME_ForgotPassword_Select : ProjectTestBase
    {
        #region Global Object Initialization
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps;
        public TestStep testStep;
        #endregion

        /// <summary>
        /// This Test Case is to Verify Error Message when User Enter Wrong RESET Code
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA11_CSP_ForgotPasswordNegativeScenario()
        {
          
            #region TestData
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;
            AgentRegistration agent = new AgentRegistration();
            agent.FistName = AgentValues.ForgotPasswordTestAgent;
            agent.LastName = AgentValues.ForgotPasswordTestAgent;
            agent.Role = RoleValue.Admin.ToString();
            agent.UserName = AgentValues.ForgotPasswordTestAgent;
            agent.Status = AgentRegistration.AgentStatus.Active.ToString();
            agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
            string ResetCode= RandomDataHelper.RandomAlphanumericStringWithSpecialChars(4);
            string ExpectedErrorMessage = "The reset code you entered is invalid.";
            #endregion

            #region Object Initialization
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            var CSP_HomePage = new CSPortal_HomePage(DriverContext);
            var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
            var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
            var CSP_ForgotPassword = new CSPortal_ForgotPassword(DriverContext);
            var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
            #endregion

            try
            {
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, listOfTestSteps);
                CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents, listOfTestSteps);
                CSPortal_UserAdministrationAgentPage.CreateAgent(agent, listOfTestSteps);
                CSP_HomePage.LogoutCSPortal(listOfTestSteps);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.ClickForgotPassword(listOfTestSteps);
                CSP_ForgotPassword.EnterUserNameandClickOnSubmit(agent.UserName, listOfTestSteps);
                CSP_ForgotPassword.SelectEmailOption(listOfTestSteps);
                CSP_ForgotPassword.ClickSendResetCodeButton(listOfTestSteps);
                CSP_ForgotPassword.SelectAlreadyHaveResetCode(listOfTestSteps);
                CSP_ForgotPassword.ClickSendResetCodeButton(listOfTestSteps);
                CSP_ForgotPassword.EnterResetCodeAnd_ClickOnSubmit(ResetCode, listOfTestSteps);
                CSP_ForgotPassword.VerifyInvalidResetCodeError(ExpectedErrorMessage, listOfTestSteps);
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

        ///// <summary>
        ///// This Test case is to update profile and Verify the Updated Values on Account Summary
        ///// </summary>
        //[TestMethod]
        //[TestCategory("CSPortal")]
        //public void BTA12_CSP_UpdateMember_BasicInfoAndAddress()
        //{
        //    ProjectBasePage basePages = new ProjectBasePage(driverContext);
        //    testCase = new TestCase(TestContext.TestName);
        //    listOfTestSteps = new List<TestStep>();
        //    testStep = new TestStep();
        //    MemberProfile MP_Model = new MemberProfile(DriverContext);
        //    Member member = MP_Model.GenerateMemberBasicInfo();
        //    MemberDetails memberDetails = MP_Model.GenerateMemberDetails();
        //    login.UserName = CsPortalData.csadmin;
        //    login.Password = CsPortalData.csadmin_password;

        //    try
        //    {
        //        var CSP_HomePage = new CSPortal_HomePage(DriverContext);
        //        var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
        //        var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
        //        var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
        //        var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);

        //        basePages.CreateMember_UsingSoap(out member, out string LoyaltyId, listOfTestSteps);
        //        CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
        //        CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
        //        CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyId, listOfTestSteps);
        //        CSPSearchPage.Select(member.FirstName, listOfTestSteps);
        //        CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, listOfTestSteps);
        //        CSP_UpdateProfilePage.UpdateBasicInfo_Details(member, memberDetails, listOfTestSteps);
        //        CSP_UpdateProfilePage.UpdateAddress_Details(memberDetails, listOfTestSteps);
        //        CSP_UpdateProfilePage.SaveUpdateProfile(listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyId, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyFirstName(member.FirstName, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyLastName(member.LastName, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyPrimaryEmail(member.PrimaryEmailAddress, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyAddressLine1(memberDetails.AddressLineOne, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyAddressLine2(memberDetails.AddressLineTwo, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyCity(memberDetails.City, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyState(memberDetails.StateOrProvince, listOfTestSteps);
        //        CSPAccountSummaryPage.VerifyZipCode(memberDetails.ZipOrPostalCode, listOfTestSteps);
        //        CSP_HomePage.LogoutCSPortal(listOfTestSteps);
        //        testCase.SetStatus(true);
        //    }
        //    catch (Exception e)
        //    {
        //        testCase.SetStatus(false);
        //        testCase.SetErrorMessage(e.Message);
        //        testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
        //        Assert.Fail();
        //    }
        //    finally
        //    {
        //        testCase.SetTestCaseSteps(listOfTestSteps);
        //        testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
        //        listOfTestCases.Add(testCase);
        //    }
        //}

        ///// <summary>
        ///// This Test case is to update profile With Maximum and Verify the Updated Values on Account Summary
        ///// </summary>
        //[TestMethod]
        //[TestCategory("CSPortal")]
        //public void BTA13_CSP_UpdateMemberWithMaximumValues()
        //{
        //    ProjectBasePage basePages = new ProjectBasePage(driverContext);
        //    testCase = new TestCase(TestContext.TestName);
        //    listOfTestSteps = new List<TestStep>();
        //    testStep = new TestStep();
        //    login.UserName = CsPortalData.csadmin;
        //    login.Password = CsPortalData.csadmin_password;

        //    try
        //    {
        //        #region Generating Test Data to Create new user with loyalty card
        //        MemberProfile MP_Model = new MemberProfile(DriverContext);
        //        Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
        //        MemberDetails details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();
        //        VirtualCard vc = MP_Model.GenerateVirtualCardAboveMaxValues();
        //        #endregion
        //        var CSP_HomePage = new CSPortal_HomePage(DriverContext);
        //        var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
        //        var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
        //        var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
        //        var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);

        //        basePages.CreateMember_UsingSoap(out member, out string LoyaltyId, listOfTestSteps);
        //        CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
        //        CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
        //        CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyId, listOfTestSteps);
        //        CSPSearchPage.Select(member.FirstName, listOfTestSteps);

        //        CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, listOfTestSteps);

        //        member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
        //        details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();

        //        CSP_UpdateProfilePage.UpdateBasicInfo_Details(member, details, listOfTestSteps);
        //        CSP_UpdateProfilePage.UpdateAddress_Details(details, listOfTestSteps);
        //        CSP_UpdateProfilePage.UpdateContactInfo_Details(details, member, listOfTestSteps);
        //        CSP_UpdateProfilePage.SaveUpdateProfile(listOfTestSteps);
        //        string Member_FirstName = member.FirstName.Substring(0, 50);
        //        CSPAccountSummaryPage.VerifyFirstName(Member_FirstName, listOfTestSteps);

        //        string Member_LastName = member.LastName.Substring(0, 50);
        //        CSPAccountSummaryPage.VerifyLastName(Member_LastName, listOfTestSteps);

        //        string Member_PrimaryEmailAddress = member.PrimaryEmailAddress.Substring(0, 254);
        //        CSPAccountSummaryPage.VerifyPrimaryEmail(Member_PrimaryEmailAddress, listOfTestSteps);

        //        string Member_details_AddressLineOne = details.AddressLineOne.Substring(0, 100);
        //        CSPAccountSummaryPage.VerifyAddressLine1(Member_details_AddressLineOne, listOfTestSteps);

        //        string Member_details_AddressLineTwo = details.AddressLineTwo.Substring(0, 100);
        //        CSPAccountSummaryPage.VerifyAddressLine2(Member_details_AddressLineTwo, listOfTestSteps);

        //        string Member_details_City = details.City.Substring(0, 50);
        //        CSPAccountSummaryPage.VerifyCity(Member_details_City, listOfTestSteps);

        //        CSPAccountSummaryPage.VerifyState(details.StateOrProvince, listOfTestSteps);

        //        string Member_details_ZipCode = details.ZipOrPostalCode.Substring(0, 25);
        //        CSPAccountSummaryPage.VerifyZipCode(Member_details_ZipCode, listOfTestSteps);

        //        CSP_HomePage.LogoutCSPortal(listOfTestSteps);
        //        testCase.SetStatus(true);
        //    }

        //    catch (Exception e)
        //    {
        //        testCase.SetStatus(false);
        //        testCase.SetErrorMessage(e.Message);
        //        testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
        //        Assert.Fail();
        //    }
        //    finally
        //    {
        //        testCase.SetTestCaseSteps(listOfTestSteps);
        //        testCase.SetEndTime(new StringHelper().GetFormattedDateTimeNow());
        //        listOfTestCases.Add(testCase);
        //    }
        //}

        /// <summary>
        /// This Test case is to Navigate to All the Dashboard Links for Member and Verify the Titles
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA14_CSP_Member_Search_Select()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Member member = new Member();
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;

            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSPActivityPage = new CSPortal_MemberAccountActivityPage(DriverContext);

                basePages.CreateMember_UsingSoap(out member, out string LoyaltyId, listOfTestSteps);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyId, listOfTestSteps);
                CSPSearchPage.Select(member.FirstName, listOfTestSteps);
                CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyId, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountActivity, listOfTestSteps);
                CSPActivityPage.VerifyFirstName(member.FirstName, listOfTestSteps);
                CSPActivityPage.VerifyLastName(member.LastName, listOfTestSteps);
                CSPActivityPage.VerifyPrimaryEmail(member.PrimaryEmailAddress, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.ContactHistory, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.RequestCredit, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MergeAccounts, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.CustomerAppeasements, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.AccountSummary, listOfTestSteps);
                CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyId, listOfTestSteps);
                CSPAccountSummaryPage.VerifyFirstName(member.FirstName, listOfTestSteps);
                CSPAccountSummaryPage.VerifyLastName(member.LastName, listOfTestSteps);
                CSPAccountSummaryPage.VerifyPrimaryEmail(member.PrimaryEmailAddress, listOfTestSteps);
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
        /// This Test case is to Navigate to All the Dashboard Links for User and Verify the Titles
        /// </summary>

        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA15_CSP_HomePage()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Member member = new Member();
            login.UserName = CsPortalData.csadmin;
            login.Password = CsPortalData.csadmin_password;

            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, listOfTestSteps);
                CSP_LoginPage.LoginCSPortal(login, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.ChangePassword, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, listOfTestSteps);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, listOfTestSteps);
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
