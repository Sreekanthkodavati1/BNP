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
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;

namespace Bnp.Core.Tests.Web.CSPortal
{
    /// <summary>
    /// BTA-140 : Create Member and Update Member With valid details
    /// </summary>

    [TestClass]
    public class BTA140_CSP_UpdateMemberAndVerifyMember : ProjectTestBase
    {
        Login login = new Login();
        
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// BTA-140 : Create Member and Update Member With valid details
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        [TestCategory("CSPortal-Smoke")]
        public void BTA140_CSP_UpdateMemberAnd_VerifyMember()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            MemberProfile MP_Model = new MemberProfile(DriverContext);

            try
            {
                #region Precondtion:Create Members
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();            
                testStep.SetOutput("IpCode:" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Generating Test Data to Create new user with loyalty card
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
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
                testStep.SetOutput("Login As csadmin User is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(basePages.GetLoyaltyNumber(output), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(output.FirstName);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(basePages.GetLoyaltyNumber(output), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Navigate to Member Update 
                stepName = "Navigate to Member Update Profile";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UpdateProfile, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Update Basic Info
                stepName = "Update Basic Info Details on Member  Update Profile page";
                var CSP_UpdateProfilePage = new CSPortal_MemberUpdateProfilePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_UpdateProfilePage.UpdateBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Update Address
                stepName = "Update  Address Info Details on Member  Update Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_UpdateProfilePage.UpdateAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Update ContactInfo
                stepName = "Update  Contact Info Details on Member  Update Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_UpdateProfilePage.UpdateContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
         
                #region Step9:Select All Opt In/Out Check boxes to Update
                stepName = "Select All Opt In/Out Check boxes to Update";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_UpdateProfilePage.UpdateOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Save Update profile  and Verify Loyalty ID 
                stepName = "Update profile and Verify Loyalty ID on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_UpdateProfilePage.SaveRegistration();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(basePages.GetLoyaltyNumber(output), out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Verify FirstName on Account Summary Page
                stepName = "Verify FirstName on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyFirstName(member.FirstName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify Last tName on Account Summary Page
                stepName = "Verify Last Name on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyLastName(member.LastName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify Primary Email Address on Account Summary Page
                stepName = "Verify Primary Email Address on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                 stepstatus = CSPAccountSummaryPage.VerifyPrimaryEmail(member.PrimaryEmailAddress, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Verify Address Line 1 on Account Summary Page
                stepName = "Verify Address Line 1 on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                 stepstatus = CSPAccountSummaryPage.VerifyAddressLine1(details.AddressLineOne, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Verify Address Line 2 on Account Summary Page
                stepName = "Verify Address Line 2 on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyAddressLine2(details.AddressLineTwo, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16:Verify City on Account Summary Page
                stepName = "Verify City on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyCity(details.City, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17:Verify State on Account Summary Page
                stepName = "Verify State on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyState(details.StateOrProvince, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18:Verify 	Zip Code on Account Summary Page
                stepName = "Verify 	Zip Code on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPAccountSummaryPage.VerifyZipCode(details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step19:Logout As csadmin 
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName + e, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
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