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
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;

namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA139_CSP_MemberSearch : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;


        [TestMethod]
        [TestCategory("CSPortal-Smoke")]
        public void BTA139_CSP_Member_Search()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            try
            {
                Common common = new Common(DriverContext);

                #region Precondtion:Create Members
                CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = cdis_Service_Method.AddCDISMemberWithAllFields();

                string LoyaltyNumber = DatabaseUtility.GetLoyaltyID(output.IpCode.ToString());
                testStep.SetOutput("Generated :" + output.IpCode + ",Name:" + output.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
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
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(LoyaltyNumber, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";

                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(output.FirstName);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyNumber, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Search Based on Email ID
                stepName = "Search Based on Email ID";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch,out stepName);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_EmailID(output.PrimaryEmailAddress, out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Search Based on FirstName
                stepName = "Search Based on FirstName";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_FirstName(output.FirstName, out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Search Based on Last Name
                stepName = "Search Based on Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_LastName(output.LastName, out  Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Search With no Inputs
                stepName = "Search With no Inputs";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_WithBlankInputs(out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Logout As csadmin 
                stepName = "Logout As csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);


                CSP_HomePage.LogoutCSPortal();
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