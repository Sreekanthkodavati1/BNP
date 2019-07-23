using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.Promotion;
using Bnp.Core.WebPages.Navigator.UsersPage;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// Test to test BTA-120 :  Create new Targeted and nontargeted Promotion and verify on Rules page.
    /// </summary>

    [TestClass]
    public class BTA120_Navigator_Create_Promotion : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;
        public string OrderTest_Status = "";

        public BTA120_Navigator_Create_Promotion()
        {
        }
        public BTA120_Navigator_Create_Promotion(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        /// <summary>
        /// BTA-120 :  Create new Targeted and nontargeted Promotion and verify on Rules page.
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA120_Navigator_Create_Targeted_Promotion()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            Promotions promotions = new Promotions();
            NonAdminUserData promotion = new NonAdminUserData(DriverContext);
            promotions.StartDate = DateHelper.GetDate("Current");
            promotions.EndDate = DateHelper.GetDate("Future");
            string stepName = "";
            try
            {
                #region stepName 1: "Open Navigator URL";
                stepName = "Open Navigator URL";
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 2: "Login to Navigator using User With AllRoles";
                stepName = "Login to Navigator using User With AllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 3: "Select organization and environment on USER page";
                stepName = "Select organization and environment on USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 4: "Create Targeted Promotion";
                stepName = "Create Targeted Promotion";
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = promotion.PromotionName + RandomDataHelper.RandomString(5);
                promotions.Description = "Value for " + promotions.Name;
                var navigator_PromotionPage = new Navigator_PromotionsPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                string enrollmentType = Navigator_PromotionsPage.EnrollmentTypes.Supported.ToString();
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.Targeted.ToString(), promotions, enrollmentType));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 5: "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 6:"Create Non-Targeted Promotion";
                stepName = "Create Non-Targeted Promotion";
                testStep = TestStepHelper.StartTestStep(testStep);
                promotions.Code = RandomDataHelper.RandomString(3);
                promotions.Name = "AutoPromo_" + RandomDataHelper.RandomString(5);
                promotions.Description = "Value for " + promotions.Name;
                testStep.SetOutput(navigator_PromotionPage.Create_Promotions(Navigator_PromotionsPage.PromotionTypes.NonTargeted.ToString(), promotions, enrollmentType));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 7: "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                stepName = "Verify Non-Targeted Promotion on Model -> Attribute Sets -> TxnHeader -> Rules Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                result = navigator_PromotionPage.VerifyPromotionisCreatedOrNot(promotions.Code);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, result, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region stepName 8 :"Logout from USER page";
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(true);
                #endregion
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Passed");

            }
            catch (Exception e)
            {
                ProjectBasePage.UpdateTestcaseStatus(method.Name.ToString(), "Failed");

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testCase.SetErrorMessage(e.Message);
                testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                if (!OrderTest_Status.Contains("true"))
                {
                    Assert.Fail(); testCase.SetImageContent(DriverContext.TakeScreenshot().ToString());
                }
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
