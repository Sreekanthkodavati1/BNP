using Bnp.Core.WebPages.MemberPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.MemberPortal
{
    /// <summary>
    /// User Story BTA-347 : MP_Verify Loyalty ID Cards on My Wallet Page
    /// </summary>
    [TestClass]
    public class BTA347_MP_VerifyLoyaltyIDCardsOnMyWalletPage : ProjectTestBase
    {      
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        /// <summary>
        /// Test Case BTA-347 : MP_Verify Loyalty ID Cards on My Wallet Page
        /// </summary>
        [TestMethod]
        [TestCategory("MemberPortal")]
        public void BTA347_MP_VerifyLoyaltyIDCardsAndViewOptions()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region  Object Initialization                
                var MP_LoginPage = new MemberPortal_LoginPage(driverContext);                
                MemberPortal_LoginPage loginPage=new MemberPortal_LoginPage(driverContext);
                MemberPortal_MyAccountPage myAccountPage = new MemberPortal_MyAccountPage(driverContext);
                MemberPortal_MyWalletPage myWalletPage = new MemberPortal_MyWalletPage(driverContext);
                #endregion

                #region Precondition:Create Member with Loyalty ID
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with Loyalty ID from CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                Login login = new Login
                {
                    UserName = output.Username,
                    Password = MemberPortalData.MP_password
                };
                testStep.SetOutput("Loyalty ID:" + basePages.GetLoyaltyNumber(output) + ", Name:" + output.FirstName + " Created Successfully");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step1:Launch Member Service Portal URL
                stepName = "Launch Member Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);               
                MP_LoginPage.LaunchMemberPortal(login.MemberPortal_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login to Member Portal 
                stepName = "Login to Member Portal";
                testStep = TestStepHelper.StartTestStep(testStep);                
                loginPage.LoginMemberPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
                
                #region Step3:Navigate to My Wallet page
                stepName = "Verify successful navigation to My Wallet page";
                testStep = TestStepHelper.StartTestStep(testStep);
                myAccountPage.NavigateToMPDashBoardMenu(MemberPortal_MyAccountPage.MPDashboard.MyWallet, out Step_Output); 
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify My Loyalty Cards Loyalty ID
                stepName = "Verify My Loyalty Cards > Loyalty ID Number";
                testStep = TestStepHelper.StartTestStep(testStep);
                myWalletPage.VerifyMyLoyaltyCardsLoyaltyID(basePages.GetLoyaltyNumber(output), out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify My Loyalty Cards sections
                stepName = "Verify My Loyalty Cards sections";
                testStep = TestStepHelper.StartTestStep(testStep);
                myWalletPage.VerifyLoyaltyCardsSection(out Step_Output);
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify My Loyalty Cards buttons
                stepName = "Verify My Loyalty Cards buttons";
                testStep = TestStepHelper.StartTestStep(testStep);                
                myWalletPage.VerifyLoyaltyCardsButtonOptions(out Step_Output); 
                testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion               

                #region Step7:Logout from member portal 
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