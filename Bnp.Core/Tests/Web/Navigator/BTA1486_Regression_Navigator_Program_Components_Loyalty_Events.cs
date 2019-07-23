using Bnp.Core.WebPages.Navigator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;
using System;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using BnPBaseFramework.Web.Helpers;
using Bnp.Core.Tests.API.Validators;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// User Story BTA-1486 : Create Loyalty Events and Validations of attribute with different type of content type
    /// </summary>
    [TestClass]
    public class BTA1486_Regression_Navigator_Program_Components_Loyalty_Events : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;
        /// <summary>
        /// User Story BTA-213 : Loyalty Events Create Validations
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA213_Navigator_CreateLoyaltyEvents()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            NonAdminUserData data = new NonAdminUserData(driverContext);
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
            var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            var loyaltyEvents = new Navigator_Users_Program_Components_LoyaltyEventsPage(DriverContext);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool StepStatus;
            login.UserName = NavigatorUsers.NonAdminUser;
            login.Password = NavigatorUsers.NavigatorPassword;
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
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify Empty Name field is not allowed while creating Loyalty Event
                stepName = "Verify Empty Name field is not allowed while creating Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                CategoryFields LEvent = new CategoryFields
                {
                    Name = "",
                    Description = "LoyaltyEventDescription",
                    AttributeName = data.AttributeAllContentType,
                    ValueToSetInAttribute = "ValueGivenForAttribute"
                };
                loyaltyEvents.CreateAndVerifyLoyaltyEvent(LEvent, out string stepOutput);
                bool status = loyaltyEvents.VerifyErrorMessage(out string outMsg);
                testStep.SetOutput(outMsg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify Error Message Name cannot be more than 150 characters displays
                stepName = "Verify Error Message Name cannot be more than 150 characters displays";
                testStep = TestStepHelper.StartTestStep(testStep);
                CategoryFields LoyaltyEvent = new CategoryFields
                {
                    Name = "" + RandomDataHelper.RandomString(151),
                    Description = "LoyaltyEventDescription",
                    AttributeName = data.AttributeAllContentType,
                    ValueToSetInAttribute = "ValueGivenForAttribute"
                };
                loyaltyEvents.CreateAndVerifyLoyaltyEvent(LoyaltyEvent, out string StepMsg);
                bool Out_status = loyaltyEvents.VerifyErrorMessage(out string OutStatus);
                testStep.SetOutput(OutStatus);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify No Error Message Name cannot be more than 150 characters displays and Loyalty Event Created 
                stepName = "Verify No Error Message Name cannot be more than 150 characters displays and Loyalty Event Created";
                testStep = TestStepHelper.StartTestStep(testStep);
                CategoryFields LoyaltyEventValues = new CategoryFields
                {
                    Name = "" + RandomDataHelper.RandomString(150),
                    Description = "LoyaltyEventDescription",
                    AttributeName = data.AttributeAllContentType,
                    ValueToSetInAttribute = "ValueGivenForAttribute"
                };
                loyaltyEvents.CreateAndVerifyLoyaltyEvent(LoyaltyEventValues, out string msg);
                testStep.SetOutput(msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Delete Loyalty Event
                stepName = "Delete Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Delete_Status = loyaltyEvents.DeleteLoyaltyEventAndVerify(LoyaltyEventValues, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:LogOut
                stepName = "Logout as USER Admin With All roles";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-214 : Create Attribute for Loyalty Event content type with data type as date
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA214_Navigator_LoyaltyEvents_Create_Attribute_With_Date()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as Date and content type as Loyalty Event
                stepName = "Create attribute with Data Type as Date and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.Date.ToString(),
                    DefaultValues = ""
                };
                attribute.Name = attName + attribute.ContentType + RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step4:Update attribute for Loyalty Events content type
                stepName = "Update attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Update_status = attributesPage.UpdateAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Update_Output);
                testStep.SetOutput(Update_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Update_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-215 :Create Attribute for Loyalty Event content type with data type as DateTime
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA215_Navigator_LoyaltyEvents_Create_Attribute_With_DateTime()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as DateTime and content type as Loyalty Event
                stepName = "Create attribute with Data Type as DateTime and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.DateTime.ToString(),
                    DefaultValues = ""
                };
                attribute.Name = attName + attribute.ContentType + RandomDataHelper.RandomString(4); ;
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step4:Update attribute for Loyalty Events content type
                stepName = "Update attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Update_status = attributesPage.UpdateAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Update_Output);
                testStep.SetOutput(Update_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Update_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-216 : Create Attribute for Loyalty Event content type with data type as String
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA216_Navigator_LoyaltyEvents_Create_Attribute_With_DataType_As_String()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as String and content type as Loyalty Event
                stepName = "Create attribute with Data Type as String and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.DateTime.ToString(),
                    DefaultValues = ""
                };
                attribute.Name = attName + attribute.ContentType + RandomDataHelper.RandomString(4); ;
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step4:Update attribute for Loyalty Events content type
                stepName = "Update attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Update_status = attributesPage.UpdateAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Update_Output);
                testStep.SetOutput(Update_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Update_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-217 : Create Attribute for Loyalty Event and Update with different type of default values 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA217_Navigator_LoyaltyEvents_Create_Attribute_Update_With_Different_DefaultValues()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify a new content attribute with a Default Values for Loyalty Events can be created
                stepName = "Verify a new content attribute with a Default Values for Loyalty Events can be created";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString(),
                    DefaultValues = "Largo, Medium"
                };
                attribute.Name = attName + attribute.ContentType + RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify that content attribute with a Default Values for Loyalty Events can be Updated
                stepName = "Verify that content attribute with a Default Values for Loyalty Events can be Updated";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DefaultValues = "Large, Medium";
                bool Update_status = attributesPage.UpdateAttributeDefaultValues(attribute, out string Update_Output);
                testStep.SetOutput(Update_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Update_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step5:Verify that after provide 1 default value in content attribute the formulary of new Loyalty Events
                stepName = "Verify that after provide 1 default value in content attribute the formulary of new Loyalty Events";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                bool _status = attributesPage.VerifyAttributeDefaultValuesInLoyaltyEvent(attribute, out string _Output);
                testStep.SetOutput(_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, _status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step6:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify a new content attribute with a Default Values as Null for Loyalty Events can be created
                stepName = "Verify a new content attribute with a Default Values as Null for Loyalty Events can be created";
                testStep = TestStepHelper.StartTestStep(testStep);
                CategoryFields attributeWithDeafaultValues = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString(),
                    DefaultValues = ""
                };
                attributeWithDeafaultValues.Name = attName + attributeWithDeafaultValues.ContentType + RandomDataHelper.RandomString(4);
                bool Default_status = attributesPage.CreateAttributeWithContentType(attributeWithDeafaultValues, out string Default_Output);
                testStep.SetOutput(Default_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Default_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Verify that content attribute with a Default Values for Loyalty Events can be Updated
                stepName = "Verify that content attribute with a Default Values for Loyalty Events can be Updated";
                testStep = TestStepHelper.StartTestStep(testStep);
                attributeWithDeafaultValues.DefaultValues = "Large; Medium; Small";
                bool Edit_Status = attributesPage.UpdateAttributeDefaultValues(attributeWithDeafaultValues, out string Edit_Output);
                testStep.SetOutput(Edit_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Edit_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify that after provide more than 1 default value in content attribute the formulary of new Loyalty Events
                stepName = "Verify that after provide more than 1 default value in content attribute the formulary of new Loyalty Events";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                bool Second_status = attributesPage.VerifyAttributeMultipleDefaultValuesInLoyaltyEvent(attributeWithDeafaultValues, out string Second_Output);
                testStep.SetOutput(Second_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Second_status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step9:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Del_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attributeWithDeafaultValues, out string Del_Output);
                testStep.SetOutput(Del_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Del_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-218 : Create Attribute for Loyalty Event content type with Grid as true and verify in Grid Cloumn
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA218_Navigator_LoyaltyEvents_Create_Attribute_With_Grid_As_True()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as String and content type as Loyalty Event
                stepName = "Create attribute with Data Type as String and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString(),
                    DefaultValues = ""
                };
                attribute.Name = attName + attribute.ContentType+RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step4:Verify grid will be displayed checked box in the Visible in Grid column 
                stepName = "Verify grid will be displayed checked box in the Visible in Grid column";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Check_Status = attributesPage.VerifyGridIsCheckedInGridcolumn(attribute.Name, out string Check_Output);
                testStep.SetOutput(Check_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Check_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify Extended Atrribute in Loyalty Event Page 
                stepName = "Verify Extended Atrribute in Loyalty Event Page ";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                bool Att_Status = attributesPage.VerifyExtendedAttributeExistsInLoyaltyEvent(attribute.Name, out string Status);
                testStep.SetOutput(Status);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Att_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-219 : Create and Update_Attribute_Validations
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA219_Navigator_LoyaltyEvents_Update_Attribute_Validations()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            var loyaltyEvents = new Navigator_Users_Program_Components_LoyaltyEventsPage(DriverContext);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as String and content type as Loyalty Event
                stepName = "Create attribute with Data Type as String and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString(),
                    DefaultValues = ""
                };
                attribute.Name =  attName + attribute.ContentType+RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step4:Verify that the name of content attribute of a Loyalty Events cannot be updated 
                stepName = "Verify that the name of content attribute of a Loyalty Events cannot be updated";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Check_Status = attributesPage.VerifyNameFiledIsDisabled(attribute, out string Check_Output);
                testStep.SetOutput(Check_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Check_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify that content attribute cannot be deleted if there is a Loyalty Events with the content attr
                stepName = "Verify that content attribute cannot be deleted if there is a Loyalty Events with the content attr";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                CategoryFields LoyaltyEventValues = new CategoryFields
                {
                    Name = attName + attribute.ContentType,
                    Description = "LoyaltyEventDescription",
                    AttributeName = attribute.Name,
                    ValueToSetInAttribute = "ValueGivenForAttribute"
                };
                loyaltyEvents.CreateAndVerifyLoyaltyEvent(LoyaltyEventValues, out string msg);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                bool Delete_Status = attributesPage.DeleteAttributeAndVerify(attribute, out string Delete_Output);
                if (!Delete_Status) { Delete_Status = true; }
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion 

                #region Step6:Delete Loyalty Event
                stepName = "Delete Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.LoyaltyEvents);
                bool Del_Status = loyaltyEvents.DeleteLoyaltyEventAndVerify(LoyaltyEventValues, out string Del_Output);
                testStep.SetOutput(Del_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Del_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                bool Status_Delete = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Output_Delete);
                testStep.SetOutput(Output_Delete);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Status_Delete, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-220 : Create and update Attribute with default value as null
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA220_Navigator_LoyaltyEvents_Create_Update_Attribute_With_Default_Value_As_Null()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify a new content attribute with no Default Values for Loyalty Events can be created
                stepName = "Verify a new content attribute with no Default Values for Loyalty Events can be created";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.Date.ToString(),
                    DefaultValues = "",
                };
                attribute.Name =   attName + attribute.ContentType+RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify that content attribute with no Default Values for Loyalty Events can be Updated
                stepName = "Verify that content attribute with no Default Values for Loyalty Events can be Updated";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Update_Status = attributesPage.UpdateAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Update_Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-221 : Create and update Attribute with grid visible 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA221_Navigator_LoyaltyEvents_Create_Update_Attribute_With_Visible_In_Grid()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify that content attribute does not appears in grid after created for Loyalty Events if value is false
                stepName = "Verify that content attribute does not appears in grid after created for Loyalty Events if value is false";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString(),
                    DefaultValues = "",
                };
                attribute.Name =  attName + attribute.ContentType+RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithVisibleInGrid(attribute, false, out string Output);
                if (status) { if (!attributesPage.VerifyGridIsCheckedInGridcolumn(attribute.Name, out string msg)) { status = true; } }
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify that value for appears in grid can be updated for an attribute of a Loyalty Events
                stepName = "Verify that value for appears in grid can be updated for an attribute of a Loyalty Events";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Update_Status = attributesPage.UpdateAttributeVisibleInGridStatus(attribute, out string Update_Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion       

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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

        /// <summary>
        /// Test BTA-223 : Create Attribute for Loyalty Event content type ,
        /// verify the attribute in DB and delete it from LN and again verify it in DB
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        [TestCategory("Regression")]
        public void BTA223_Navigator_LoyaltyEvents_Create_Attribute_Delete_VerifyInDB()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            try
            {
                #region Object initialization
                var navigator_LoginPage = new Navigator_LoginPage(DriverContext);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
                var ProgramPage = new Navigator_Users_ProgramPage(driverContext);
                var componentsPage = new Navigator_Users_Program_ComponentsPage(driverContext);
                var attributesPage = new Navigator_Users_Program_Components_AttributesPage(driverContext);
                #endregion

                #region Step1:Launch Navigator Portal
                stepName = "Launch Navigator URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.LaunchNavigatorPortal(login.Url, out string LaunchMessage); testStep.SetOutput(LaunchMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Login As User Admin User 
                stepName = "Login As User Admin User and Navigate to Home page by selecting Organization and Environment";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = NavigatorUsers.NonAdminUser;
                login.Password = NavigatorUsers.NavigatorPassword;
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Create attribute with Data Type as String and content type as Loyalty Event
                stepName = "Create attribute with Data Type as String and content type as Loyalty Event";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                componentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Attributes);
                var data = new NonAdminUserData(driverContext);
                var attName = data.AttributeAllContentType;
                CategoryFields attribute = new CategoryFields
                {
                    ContentType = Navigator_Users_Program_Components_AttributesPage.ContentTypes.LoyaltyEvent.ToString(),
                    DataType = Navigator_Users_Program_Components_AttributesPage.DataType.DateTime.ToString(),
                    DefaultValues = ""
                };
                attribute.Name = attName + attribute.ContentType + RandomDataHelper.RandomString(4);
                bool status = attributesPage.CreateAttributeWithContentType(attribute, out string Output);
                testStep.SetOutput(Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion                

                #region Step4:Verify Created Attribute is present in DB
                stepName = "Verify Created Attribute is present in DB";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                List<string> output = DatabaseUtility.GetAttributeDetailsfromDB();
                bool DbStatusBeforDelete = output[0].Equals(attribute.Name);
                if (DbStatusBeforDelete) { testStep.SetOutput("Attribute for Loyalty Events is present in table lw_contentattributedef before it is deleted"); }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, DbStatusBeforDelete, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Delete attribute for Loyalty Events content type
                stepName = "Delete attribute for Loyalty Events content type";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                bool Delete_Status = attributesPage.DeleteAttributeWithContentTypeAsLoyaltyEvent(attribute, out string Delete_Output);
                testStep.SetOutput(Delete_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Delete_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify the attribute for Loyalty Events is erased in table lw_contentattributedef after is deleted
                stepName = "Verify the attribute for Loyalty Events is erased in table lw_contentattributedef after is deleted";
                testStep = TestStepHelper.StartTestStep(testStep);
                attribute.DataType = Navigator_Users_Program_Components_AttributesPage.DataType.String.ToString();
                output = DatabaseUtility.GetAttributeDetailsfromDB();
                bool DB_Status = !output[0].Equals(attribute.Name);
                if (DB_Status) { testStep.SetOutput("attribute for Loyalty Events is erased in table lw_contentattributedef after is deleted"); }
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, DB_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Logout from Navigator application
                stepName = "Logout from Navigator application";
                testStep = TestStepHelper.StartTestStep(testStep);
                navigator_LoginPage.Logout();
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
