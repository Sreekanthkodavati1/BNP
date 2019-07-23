using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Web;
using Bnp.Core.WebPages.Navigator.UsersPage;
using System.Configuration;
using System;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Website;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web.Helpers;
using System.Reflection;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story  BTA-85 Navigator _ Create Attribute set and add attributes ,Config Attributes and verify in CS portal
    /// </summary>
    [TestClass]
    public class BTA85_Navigator_Users_CreateAttributSet : ProjectTestBase
    {
        Login login = new Login();
        AttributeSet attribute = new AttributeSet(); 
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        public string OrderTest_Status = "";

        public BTA85_Navigator_Users_CreateAttributSet()
        {
        }
        public BTA85_Navigator_Users_CreateAttributSet(string executionFromOrderTest)
        {
            OrderTest_Status = executionFromOrderTest;
            this.DriverContext = ProjectTestBase.dr;
        }
        /// <summary>
        /// Test Case BTA-85 Navigator _ Create Attribute set and add attributes ,Config Attributes and verify in CS portal
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA85_Navigator_Users_CreateAttributSets()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            var attributeSetData = new NonAdminUserData(driverContext);
            MethodBase method = MethodBase.GetCurrentMethod();
            string methodName = method.Name;
            testCase = new TestCase(methodName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                NonAdminUserData Wesitedata = new NonAdminUserData(driverContext);
                navigator_LoginPage.Login(login,Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Navigating Model and Navigate to Attribute Set page
                stepName = "Navigating Model and Navigate to Attribute Set page";
                testStep = TestStepHelper.StartTestStep(testStep);
                var models_HomePage = new Navigator_ModelHomePage(DriverContext);
                models_HomePage.NavigateToModels_Page(out string Pageoutput);testStep.SetOutput(Pageoutput);
                models_HomePage.NavigatetoToAttributeSet_Page(out Pageoutput);testStep.SetOutput(Pageoutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create Attributes set , Add  Attribute sets and Generate Table
                var attributeSetPage = new Navigator_AttributeSetPage(DriverContext);
                attribute.MainAttributeSets = "Member";
                string randomStr = RandomDataHelper.RandomString(4);
                attribute.AttributeSets = attributeSetData.AttributeSets;
                attribute.Attributes = attributeSetData.Attribute;
                stepName = "Create Attributes set" + attribute.MainAttributeSets + "  Add  Attributes to the attribute set:" + attribute.Attributes + " is Successful and Generate Table is successful";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeSetPage.CreateAttributeSet(attribute.MainAttributeSets, attribute.AttributeSets);
                if (attributeSetPage.CreateAttributes(attribute.MainAttributeSets, attribute.AttributeSets, attribute.Attributes, attribute.Attributes, attribute.Attributes, "String", "1", "20").Contains("Attribute Element is Created Successfully"))
                { attributeSetPage.GenerateTable(attribute.MainAttributeSets, attribute.AttributeSets); }              
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Navigate to Websites and Select Website  as BTA_Dev_CS and Module Type as Member Profile 
                stepName = "Navigate to Websites and Select Website  as BTA_Dev_CS and Module Type as Member Profile";
                testStep = TestStepHelper.StartTestStep(testStep);
                var Website = new Application_Nav_Util_Page(DriverContext);
                Website.OpenApplication(NavigatorEnums.ApplicationName.website);
                var websitePage = new Navigator_Users_WebsitePage(DriverContext);
                websitePage.NavigateToWebsiteTab(Navigator_Users_WebsitePage.WebsiteTabs.Modules, out string msg);
                var Website_Modules = new Navigator_Users_Website_ModulesPage(DriverContext);                
                var webSiteName = Wesitedata.CSPortal_WebSiteName;
                var moduleType = EnumUtils.GetDescription(Navigator_Users_Website_ModulesPage.ModuleTypeList.MemberProfile);
                Website_Modules.Website_Select_WebsiteAndModuleType(webSiteName, moduleType);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Config Attribute set to CS portal
                stepName = "Select CSMemberRegConfig file and Drag Drap above created Attribute";
                testStep = TestStepHelper.StartTestStep(testStep);
                Website_Modules.CSMemberRegConfigurebutton();
                Website_Modules.DragandDropAttibuteSet(attribute.Attributes, "Country");
                Website_Modules.SaveConfigSetting();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Bounce the CS portal App pool
                stepName = "Bounce the CS Portal App pool";
                testStep = TestStepHelper.StartTestStep(testStep);
                var WebsiteManagement = new Navigator_Users_WebsiteManagementPage(DriverContext);
                WebsiteManagement.Navigator_Website_Select_WebsiteManagementTab();
                WebsiteManagement.BounceAppPool("CSPortal");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Launch CS Portal and Login to CS Portal as csadmin
                stepName = "Launch CS Portal and Login to CS Portal as csadmin";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPortal_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSPortal_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                string username = CsPortalData.csadmin;
                string password= CsPortalData.csadmin_password;
                CSPortal_LoginPage.LoginCSPortal(username, password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Navigate to Member Registration page and Verify Attribute
                stepName = "Navigate to Member Registration page and Verify Attribute:" + attribute.Attributes;
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPortal_HomePage = new CSPortal_HomePage(DriverContext);
                CSPortal_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration,out string message);
                var CSPortal_MemberRegistration = new CSPortal_MemberRegistrationPage(DriverContext);
                CSPortal_MemberRegistration.VerifyAttributeSetonRegisterPage(attribute.Attributes);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
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