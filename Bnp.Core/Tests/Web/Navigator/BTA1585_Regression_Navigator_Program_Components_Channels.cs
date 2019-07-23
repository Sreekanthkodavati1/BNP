using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Bnp.Core.Tests.Web.Navigator
{
    /// <summary>
    /// BTA1585 To Create new Channels and Validations
    /// </summary>
    [TestClass]
    public class BTA1585_Regression_Program_Navigator_Components_Channels : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        List<TestStep> listOfTestSteps = new List<TestStep>();
        TestStep testStep;

        /// <summary>
        /// BTA_ To Create new Channels sort and Validations
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA227_Regression_Navigator_Program_Components_Channels_Create_Sort_Validations()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            var ChannelPage = new Navigator_Users_Program_Components_ChannelPage(DriverContext);
            string randomStr = RandomDataHelper.RandomString(3);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields Channel = new CategoryFields
            {
                Name = NonAdminUserData.ChannelName + randomStr,
                Description = "Write Data In Description Area",
                ChannelType = Navigator_Users_Program_Components_ChannelPage.ChannelTypes.HTML.ToString()
            };
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

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
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify displayed Columns for the Channels Grid             
                stepName = "Verify displayed Columns for the Channels Grid";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                bool Out_Status = ChannelPage.CreateAndVerifyChannelExists(Channel, out string Out_Msg);
                string Col_Msg = "";
                if (Out_Status) { ChannelPage.VerifyAllChannelColumnsInGrid(out Col_Msg); }
                testStep.SetOutput(Col_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify that this grid is sortable by Channel Name            
                stepName = "Verify that this grid is sortable by Channel Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool Sort_Status = ChannelPage.VerifyGridIsSortableWithColName("Channel Name", out string Sort_Msg);
                testStep.SetOutput(Sort_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Sort_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify that this grid is sortable by Channel Description          
                stepName = "Verify that this grid is sortable by Channel Description";
                testStep = TestStepHelper.StartTestStep(testStep);
                Sort_Status = ChannelPage.VerifyGridIsSortableWithColName("Channel Description", out Sort_Msg);
                testStep.SetOutput(Sort_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Sort_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Verify that this grid is sortable by Channel Type            
                stepName = "Verify that this grid is sortable by Channel Type";
                testStep = TestStepHelper.StartTestStep(testStep);
                Sort_Status = ChannelPage.VerifyGridIsSortableWithColName("Channel Type", out Sort_Msg);
                testStep.SetOutput(Sort_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Sort_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify Max Rows before Pagination           
                stepName = "Verify Max Rows before Pagination";
                testStep = TestStepHelper.StartTestStep(testStep);
                bool _Status = ChannelPage.VerifyMaxRows(Channel, out string _Msg);
                testStep.SetOutput(_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, _Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion               

                #region Step8:Logout
                stepName = "Logout from USER page";
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
        /// BTA_ To Create new Channel from Channels Tab
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA228_Regression_Navigator_Program_Components_Channels_CreateChannels()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            var ChannelPage = new Navigator_Users_Program_Components_ChannelPage(DriverContext);
            string randomStr = RandomDataHelper.RandomString(3);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields Channel = new CategoryFields
            {
                Name = NonAdminUserData.ChannelName + randomStr,
                Description = "Write Data In Description Area",
                ChannelType = Navigator_Users_Program_Components_ChannelPage.ChannelTypes.Text.ToString()
            };
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

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
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify that a user is able to add a new Text Channel              
                stepName = " Verify that a user is able to add a new Text Channel";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                bool Out_Status = ChannelPage.CreateAndVerifyChannelExists(Channel, out string Out_Msg);
                testStep.SetOutput(Out_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Delete and Verify Text Channel              
                stepName = "Delete and Verify Text Channel ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                bool Del_Status = ChannelPage.DeleteChannelAndVerify(Channel, out string Del_Msg);
                testStep.SetOutput(Del_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Del_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify that a user is able to add a new HTML channel
                stepName = "Verify that a user is able to add a new HTML channel";
                testStep = TestStepHelper.StartTestStep(testStep);
                randomStr = RandomDataHelper.RandomString(3);
                CategoryFields ChannelNew = new CategoryFields
                {
                    Name = NonAdminUserData.ChannelName + randomStr,
                    Description = "Write Data In Description Area",
                    ChannelType = Navigator_Users_Program_Components_ChannelPage.ChannelTypes.HTML.ToString()
                };
                bool Out_html_Status = ChannelPage.CreateAndVerifyChannelExists(ChannelNew, out string Out_Html);
                testStep.SetOutput(Out_Html);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_html_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Delete and Verify Text Channel              
                stepName = "Delete and Verify Text Channel ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                Del_Status = ChannelPage.DeleteChannelAndVerify(Channel, out Del_Msg);
                testStep.SetOutput(Del_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Del_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7: Logout
                stepName = "Logout from USER page";
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
        /// BTA_230 To Editand verify chanel details 
        /// </summary>
        [TestMethod]
        [TestCategory("Navigator")]
        public void BTA_230_Regression_Navigator_Components_Channels_EditChanel()
        {
            #region Object Initialization
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var LocationGroup = new Navigator_Users_Program_Components_LocationGroupsPage(driverContext);
            var ChannelPage = new Navigator_Users_Program_Components_ChannelPage(DriverContext);
            string randomStr = RandomDataHelper.RandomString(3);
            testCase = new TestCase(TestContext.TestName);
            CategoryFields Channel = new CategoryFields
            {
                Name = NonAdminUserData.ChannelName + randomStr,
                Description = "Write Data In Description Area",
                ChannelType = Navigator_Users_Program_Components_ChannelPage.ChannelTypes.HTML.ToString()
            };
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #endregion

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
                navigator_LoginPage.Login(login, Users.AdminRole.USER.ToString(), out string stroutput); testStep.SetOutput(stroutput);
                var navigator_UsersHomePage = new Navigator_UsersHomePage(DriverContext);
                navigator_UsersHomePage.Navigator_Users_SelectOrganizationEnvironment();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify that a user is able to add a new HTML Channel              
                stepName = " Verify that a user is able to add a new HTML Channel";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                bool Out_Status = ChannelPage.CreateAndVerifyChannelExists(Channel, out string Out_Msg);
                testStep.SetOutput(Out_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Edit and verify the chanel details gets updated              
                stepName = " Edit and verify the chanel details gets updated ";
                testStep = TestStepHelper.StartTestStep(testStep);
                Out_Status = ChannelPage.EditAndVerifyChannelDetailsUpdated(Channel, out Out_Msg);
                testStep.SetOutput(Out_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Out_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Delete and Verify Existance of the Channel              
                stepName = "Delete and Verify Existance of the Channel ";
                testStep = TestStepHelper.StartTestStep(testStep);
                application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Channels);
                bool Del_Status = ChannelPage.DeleteChannelAndVerify(Channel, out string Del_Msg);
                testStep.SetOutput(Del_Msg);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, Del_Status, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6: Logout
                stepName = "Logout from USER page";
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
