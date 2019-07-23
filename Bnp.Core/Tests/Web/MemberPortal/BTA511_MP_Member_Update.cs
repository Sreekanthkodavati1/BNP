using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// User Story BTA-511 : MP_Member_Update
    /// </summary>
    [TestClass]
    public class BTA511_MP_Member_Update : ProjectTestBase
    {
        readonly Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// Test Case BTA-511 : MP_Member_Update
        /// </summary>
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA_511_MP_Member_Update()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                var loginPage = new MemberPortal_LoginPage(DriverContext);
                var myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                var myProfilePage = new MemberPortal_MyProfilePage(driverContext);
                #endregion

                #region Step1: Adding member with CDIS service               
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                testStep.SetOutput("Member UserName:" + output.Username + "; First Name: " + output.FirstName + "; Last Name:" + output.LastName + "; Birth Date: " + output.BirthDate + "; Email Address:" + output.PrimaryEmailAddress + ": end");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Generating Test Data to Create new user with loyalty card
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                #endregion

                #region Step2:Launch Member Portal
                stepName = "Launch Member Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.LaunchMemberPortal(login.MemberPortal_url, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3: Login As Member
                stepName = "Login As Member User";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = output.Username;
                login.Password = "Password1*";
                string MemberLoyaltyNumber = DatabaseUtility.GetLoyaltyID(output.IpCode.ToString());
                loginPage.LoginMemberPortal(login.UserName, login.Password, out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigate to My Profile page
                stepName = "Navigate to My Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyProfile, out var Step_Output); var strStatus = Step_Output;
                testStep.SetStatus(myProfilePage.VerifyMyProfilePage(MemberPortal_MyProfilePage.Sections.MyProfile.ToString(), out Step_Output)); strStatus = strStatus + ". " + Step_Output;
                testStep.SetOutput(strStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Update Basic Info
                stepName = "Update Basic Info Details on Member Update Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var stepstatus = myProfilePage.UpdateBasicInfo_Details(member.FirstName, member.LastName, details.Gender, out var stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Update Address
                stepName = "Update  Address Info Details on Member Update Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = myProfilePage.UpdateAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Update ContactInfo
                stepName = "Update  Contact Info Details on Member Update Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = myProfilePage.UpdateContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Select All Opt In/Out Check boxes to Update
                stepName = "Select All Opt In/Out Check boxes to Update";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = myProfilePage.CommunicationPreferences("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Save Update profile and Verify update success 
                stepName = "Save Update profile and Verify update success";
                testStep = TestStepHelper.StartTestStep(testStep);
                myProfilePage.SaveRegistrationAndUpdateSuccess(out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11: Verify updated First Name and Last Name 
                stepName = "Verify updated First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.VerifyMemberPortalLoginSuccessfulForUser(member.FirstName, member.LastName); testStep.SetOutput("Member update successful for First Name: " + member.FirstName + "; and Last Name: " + member.LastName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12: Logout from member portal 
                stepName = "Logout from Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                loginPage.LogoutMPPortal(); testStep.SetOutput("Logout from Member Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
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
