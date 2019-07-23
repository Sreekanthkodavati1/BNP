
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Bnp.Core.WebPages.Navigator.UsersPage.Website;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-86 : Create Customer Service portal and Member portal if it doesnot exists
    /// </summary>    
    [TestClass]
    public class BTA86_Navigator_Create_CSPortal_MPPortals : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        public string OrderTest_Status = "";

        public BTA86_Navigator_Create_CSPortal_MPPortals()
        {
        }
        public BTA86_Navigator_Create_CSPortal_MPPortals(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        /// <summary>
        /// Test BTA-86: Create Customer Service portal and Member portal if it doesnot exists
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
         public void BTA86_Navigator_Create_CSPortal_MemberPortal()
        {

            #region:OrderExection Requirement
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string Prereq_testCase1 = "BTA84_Navigator_CreateUserWithAllPermissions";
            ProjectBasePage.VerifyOrderTest(OrderTest_Status, Prereq_testCase1, methodName, testStep);
            #endregion

            var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
            NonAdminUserData WebsiteData = new NonAdminUserData(driverContext);
            testStep = new TestStep();
           
            string stepName="";
            try
            {
                #region Step 1 : Open Navigator URL
                stepName = "Open Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));               
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 2 : Login to navigator using USER_WithAllRoles               
                stepName = "Login to navigator using USER_WithAllRoles";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login,Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 3 : Select organization and environment on USER Home page"
               stepName = "Select organization and environment on USER page"; 
                testStep = TestStepHelper.StartTestStep(testStep);               
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 4 : Create CS portal website               
                stepName = "Navigate to Website application and Create org_env_CS website";
                var application_Nav_Util_Page = new Application_Nav_Util_Page(driverContext);
                var websitePage = new Navigator_Users_WebsitePage(driverContext);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.website);
                 Portal portal = new Portal
                {
                    WebSiteName = WebsiteData.CSPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.CS.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.CSDefaultSkin.ToString()
                };
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(websitePage.Create_CS(portal));                
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 5 :  Create Member portal website                
                stepName = "Navigate to Website application and Create org_env_MP website"; 
                Portal portalMP = new Portal
                {
                    WebSiteName = WebsiteData.MemberPortal_WebSiteName,
                    WebSiteType = Navigator_Users_WebsitePage.PortalType.MP.ToString(),
                    DefaultSkin = Navigator_Users_WebsitePage.DefaultPortalSkinTypes.MemberFacing.ToString()
                };
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(websitePage.Create_MP(portalMP));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step 6 : Logout             
                stepName = "Logout from USER page";
                testStep = TestStepHelper.StartTestStep(testStep);               
                navigator_LoginPage.Logout();
                testStep = TestStepHelper.EndTestStep(testCase,testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));                
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
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
                    Assert.Fail();
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
