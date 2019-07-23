using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// User Story BTA-141 : MP_Register Member and verify
    /// </summary>
    [TestClass]
    public class BTA141_MP_RegisterMemberAndVerify : ProjectTestBase
    {      
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// Test Case BTA-141 : Create new member through member registration page
        /// </summary>
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA141_MP_RegisterMemberAndVerifyPage()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region  Object Initialization
                Login login = new Login();
                var MP_LoginPage = new MemberPortal_LoginPage(driverContext);
                MP_Registration memberData = new MP_Registration(driverContext);
                MemberPortal_LoginPage loginPage=new MemberPortal_LoginPage(driverContext);
                MemberPortal_MyAccountPage myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                MemberPortal_MyProfilePage myProfilePage = new MemberPortal_MyProfilePage(driverContext);
                MemberPortal_MyWalletPage myWalletPage = new MemberPortal_MyWalletPage(driverContext);
                MemberPortal_AccountActivityPage accountActivityPage = new MemberPortal_AccountActivityPage(driverContext);
                #endregion

                #region Step1:Launch Member Service Portal URL
                stepName = "Launch Member Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);               
                MP_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Navigate to Member Registration page 
                stepName = "Navigate to Member Registration page ";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(MP_LoginPage.NavigateToMemberRegistrationPage());
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create new member
                stepName = "Create new member";
                testStep = TestStepHelper.StartTestStep(testStep);
                var memberRegistrationPage = new MemberPortal_MemberRegistrationPage(DriverContext);
                memberRegistrationPage.CreateNewMember(memberData,out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify successful navigation to Member Portal home page
                stepName = "Verify successful navigation to Member Portal home page";
                testStep = TestStepHelper.StartTestStep(testStep);
                Step_Output = loginPage.VerifyMemberPortalLoginSuccessfulForUser(memberData.FirstName, memberData.LastName);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify My Profile page
                stepName = "Verify My Profile page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyProfile,out Step_Output); var strStatus = Step_Output;
                testStep.SetStatus(myProfilePage.VerifyMyProfilePage(MemberPortal_MyProfilePage.Sections.MyProfile.ToString(), out Step_Output)); strStatus = strStatus + ". " + Step_Output;
                testStep.SetOutput(strStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify My Wallet page
                stepName = "Verify My Wallet page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out Step_Output); strStatus = Step_Output;
                testStep.SetStatus(myWalletPage.VerifyMyWalletPage(out Step_Output)); strStatus = strStatus + ". " + Step_Output;
                testStep.SetOutput(strStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Account Activity page
                stepName = "Verify Account Activity page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.AccountActivity, out Step_Output);strStatus = Step_Output;                
                testStep.SetStatus(accountActivityPage.VerifyAccountActivityPage(out Step_Output)); strStatus = strStatus + ". " + Step_Output;
                testStep.SetOutput(strStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout from member portal 
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