using System;
using System.Collections.Generic;
using System.Configuration;
using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.CSPortal;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Helpers;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnp.Core.Tests.Web.CSPortal
{
    [TestClass]
    public class BTA1227_CSP_Regression_Login_Member_Registration : ProjectTestBase
    {
        Login login = new Login();
        public TestCase testCase;
        public List<TestStep> listOfTestSteps = new List<TestStep>();
        public TestStep testStep;

        [TestMethod]
        [TestCategory("CSPortal-Regression")]
        public void BTA01_CSPLLoginAsAgent()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            bool stepstatus;
            string stepName = "";
            #region Object Declaration
            var CSP_HomePage = new CSPortal_HomePage(DriverContext);
            var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
            var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
            AgentRegistration agent = new AgentRegistration();
            #endregion

            try
            {
                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(this.DriverContext);
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

                #region Step3:Navigate to User Administration                
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out stepName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Create AdminAgent if it does not exists
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Create AdminAgent if it does not exists";
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                agent.FistName = "AdminFirstName";
                agent.LastName = "AdminLastName";
                agent.Role = RoleValue.Admin.ToString();
                agent.UserName = AgentValues.AdminAgent;
                agent.Password = AgentValues.Agentpassword;
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify Login As Admin Agent Verify Dashboard Links and Logout
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Login As AdminAgent Verify Dashboard Links and Logout";
                CSP_HomePage.LogoutCSPortal();
                CSP_LoginPage.LoginCSPortal(login.UserName, login.Password, out Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.VerifyDashboard_Links(agent.UserName));
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

        [TestMethod]
        [TestCategory("CSPortal-Regression")]
        public void BTA02_LoginPageValidatations()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            #region Object Declaration
            var CSP_HomePage = new CSPortal_HomePage(DriverContext);
            var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
            var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
            AgentRegistration agent = new AgentRegistration();
            #endregion

            try
            {
                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(this.DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Verify Validation ->When User tried to Login With Invalid UserName
                stepName = "Verify Validation ->When User tried to Login With Invalid UserName";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = RandomDataHelper.RandomString(6);
                login.Password = RandomDataHelper.RandomAlphanumericString(6);

                CSP_LoginPage.VerfiyLoginValidation_withInvalidUserName(login.UserName, login.Password, login.UserName, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify Validation -> When User tried to Login With Invalid Password
                stepName = "Verify Validation -> When User tried to Login With Invalid Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.ChangePasswordTestAgent;
                login.Password = RandomDataHelper.RandomAlphanumericString(6);
                CSP_LoginPage.VerfiyLoginValidation_withInvalidInputs(login.UserName, login.Password, "Provided password is incorrect.", out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify Validation -> When User tried to Login With No UserName
                stepName = "Verify Validation -> When User tried to Login With No UserName";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = "";
                login.Password = RandomDataHelper.RandomAlphanumericString(6);
                CSP_LoginPage.VerfiyLoginValidation_withNoInputs(login.UserName, login.Password, "Please enter your User Name.", out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Verify Validation -> When User tried to Login With No Password
                stepName = "Verify Validation -> When User tried to Login With No Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                login.UserName = AgentValues.ForgotPasswordTestAgent;
                login.Password = "";
                CSP_LoginPage.VerfiyLoginValidation_withNoInputs(login.UserName, login.Password, "Please enter your Password.", out Step_Output); testStep.SetOutput(Step_Output);
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


        [TestMethod]
        [TestCategory("CSPortal-Regression")]
        public void BTA03_CSP_Member_Search()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput;
            try
            {
                Common common = new Common(DriverContext);
                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                #endregion
                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Launch Customer Service Portal URL is Successful");
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

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Select All Opt In/Out Check boxes
                stepName = "Select All Opt In/Out Check boxes";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Save Registration and Verify Loyalty ID 
                stepName = "Save Registration and Verify Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verifying Created Member in LW_VirtualCard Table
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying Created Member in LW_VirtualCard Table";

                string LoyaltyNumber = DatabaseUtility.GetLoyaltyCardStatusfromDbSOAP(vc.LoyaltyIdNumber);
                testStep.SetOutput("Generated MemeberLoyaltyID:" + vc.LoyaltyIdNumber + "is Avalable in DB");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";

                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(member.FirstName);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(LoyaltyNumber, out Step_Output); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Search Based on Email ID
                stepName = "Search Based on Email ID";
                CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepName);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_EmailID(member.PrimaryEmailAddress, out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step16:Search Based on FirstName
                stepName = "Search Based on FirstName";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_FirstName(member.FirstName, out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step17:Search Based on Last Name
                stepName = "Search Based on Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_LastName(member.LastName, out Step_Output); testStep.SetOutput(Step_Output);

                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step18:Logout As csadmin 
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


        [TestMethod]
        [TestCategory("CSPortal-Regression")]
        public void BTA04_CSP_Change_Password()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string StepOutput;
            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSPortal_UserAdministration = new CSPortal_UserAdministration(DriverContext);
                var CSPortal_UserAdministrationAgentPage = new CSPortal_UserAdministrationAgentPage(DriverContext);
                AgentRegistration agent = new AgentRegistration();

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

                #region Step3:Navigate to User Administration         
                stepName = "Navigate to UserAdministration Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.UserAdministration, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Navigate to Agent Page      
                stepName = "Navigate to Agent Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPortal_UserAdministration.NavigateToSectionMenu(CSPortal_UserAdministration.Menu.Agents);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("Navigate to Agent Page is Successful");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Create new TestAgent user as per config file,if not existed
                testStep = TestStepHelper.StartTestStep(testStep);
                agent.FistName = AgentValues.ChangePasswordTestAgent;
                agent.LastName = AgentValues.ChangePasswordTestAgent;
                agent.Role = RoleValue.Admin.ToString();
                agent.Status = AgentRegistration.AgentStatus.Active.ToString();
                agent.UserName = AgentValues.ChangePasswordTestAgent;
                stepName = "Create New Agent if user is not existed";
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);

                testStep.SetOutput(CSPortal_UserAdministrationAgentPage.CreateAgent(agent.FistName, agent.LastName, agent.Role, agent.UserName, agent.Password, agent.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Change Password for Test Agent
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password for Test Agent";
                stepstatus = CSPortal_UserAdministrationAgentPage.ChangeAgentPassword(agent.UserName, agent.Password, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Logout As Test User 
                stepName = "Logout from CS Portal";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as csadmin");
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Login As Test User 
                stepName = "Login As User, User Name:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LoginCSPortal(agent.UserName, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Verify First Name and Last Name
                stepName = "Verify First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(CSP_HomePage.VerifyFirstNameAndLastName(agent.FistName, agent.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Navigate to Change Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Navigate to Change Password";
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.ChangePassword, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept11: Verifying Error Message when No inputs provided in Password fields
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Verifying Error Message when No inputs provided in Password fields";
                var cSP_ChangePassword = new CSPortal_ChangePassword(DriverContext);
                string agent_OldPassword = "";

                stepstatus = cSP_ChangePassword.Enter_EmptyValuesInPasswordFields("Please enter an Old Password", "Please enter a New Password", "Please confirm New Password", out string Message); testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept12: Validating Changing Password with Old Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = " Validating Changing Password with Old Password";
                agent_OldPassword = agent.Password;

                stepstatus = cSP_ChangePassword.EnterOldandNew_AsSamePassword(agent_OldPassword, "New password cannot equal old password.", out Message); testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept13:Validating Error Message while tying to enter Confirm Password with New Password with Mismatching Values
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Validating Error Message while tying to enter Confirm Password with New Password with Mismatching Values";
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(7);
                agent.ConfirmPassword = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);

                stepstatus = cSP_ChangePassword.EnterMismatchingNewConfirmPassword(agent_OldPassword, agent.Password, agent.ConfirmPassword, "New Password and Confirm Password must match.", out Message); testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept14: Change Password with Maximum Values
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = " Validating Error Message when  password exceeds 50 charcters";
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(75);
                stepstatus = cSP_ChangePassword.EnterMaximumValidationsOnPassword(agent_OldPassword, agent.Password, agent.Password, "", out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Stept15: Change Password
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Change Password for Test Agent";
                agent.Password = RandomDataHelper.RandomAlphanumericStringWithSpecialChars(8);
                cSP_ChangePassword.EnterPasswordDetails(agent_OldPassword, agent.Password, agent.Password, out string ValidationMessage);
                cSP_ChangePassword.SavePassword();
                stepstatus = cSP_ChangePassword.VerifySuccessMessage(agent_OldPassword, agent.Password, out StepOutput); testStep.SetOutput(StepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Logout As Test User 
                stepName = "Logout from CS Portal User:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as User:" + agent.UserName);
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Login As Test User 
                stepName = "Login As User, User Name:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_LoginPage.LoginCSPortal(agent.UserName, agent.Password, out StepOutput); testStep.SetOutput(Step_Output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB")); testStep.SetOutput(stepName);
                #endregion

                #region Step14:Verify First Name and Last Name
                stepName = "Verify First Name and Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                testStep.SetOutput(CSP_HomePage.VerifyFirstNameAndLastName(agent.FistName, agent.LastName));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Logout As Test User 
                stepName = "Logout from CS Portal User:" + agent.UserName;
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_HomePage.LogoutCSPortal();
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput("LogOut is Successful as User:" + agent.UserName);
                listOfTestSteps.Add(testStep);
                #endregion
                testCase.SetStatus(true);
            }
            catch (Exception e)
            {
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                testCase.SetStatus(false);
                testStep.SetOutput(e.Message);
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
        /// BTA : Create Member With Mandatory and only Opt In Option fields
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA05_CSP_CreateMemberWith_OptIn_Options()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);

                vc.LoyaltyIdNumber = "";
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

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Select All Opt In/Out Check boxes
                stepName = "Select All Opt In/Out Check boxes";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId().Trim();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Logout As csadmin 
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

        /// <summary>
        /// BTA : Create Member Without Loyalty ID and Basic info and Mandatory Fields
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA06_CSP_CreateMemberWith_BasicInfo()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);

                vc.LoyaltyIdNumber = "";
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

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step5:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(member.FirstName);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
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

                #region Step13:Logout As csadmin 
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

        /// <summary>
        /// BTA : Create Member Without Loyalty ID and Basic info and Mandatory Fields
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA07_CSP_CreateMemberWith_Different_Password_Combination()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member_one = MP_Model.GenerateMemberBasicInfo();
                Member member_two = MP_Model.GenerateMemberBasicInfo();
                Member member_three = MP_Model.GenerateMemberBasicInfo();
                Member member_four = MP_Model.GenerateMemberBasicInfo();

                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();

                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                var CSPSearchPage = new CSPortal_SearchPage(DriverContext);

                vc.LoyaltyIdNumber = "";
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


                #region 1.Verifying Creating user with Password:LowerChars ,Special Chars and numeric values only
                #region Step1:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member_one.FirstName, member_one.LastName, member_one.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                vc.LoyaltyIdNumber = RandomDataHelper.RandomNumber(5);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member_one.PrimaryEmailAddress, member_one.PrimaryPhoneNumber, member_one.PrimaryPhoneNumber, member_one.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page and Entering Password Contains:LowerChars ,Special Chars and numeric values only";
                testStep = TestStepHelper.StartTestStep(testStep);
                member_one.Password = RandomDataHelper.RandomString(4).ToLower() + RandomDataHelper.RandomNumber(3).ToLower() + RandomDataHelper.RandomSpecialCharactersString(2);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member_one.Username, member_one.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId().Trim();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber + ";User Details: " + member_one.Username + "Password Details:" + member_one.Password;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select(member_one.FirstName);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #endregion  Verifying Creating user with Password:LowerChars ,Special Chars and numeric values only

                #region Pre-req:Navigate to Member Registration 
                stepName = "Navigate to Member Search";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region 2. Verifying Creating user with Password:UpperChars ,Special Chars and numeric values only
                #region Step1:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member_two.FirstName, member_two.LastName, member_two.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                vc.LoyaltyIdNumber = RandomDataHelper.RandomNumber(5);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member_two.PrimaryEmailAddress, member_two.PrimaryPhoneNumber, member_two.PrimaryPhoneNumber, member_two.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page and Entering Password Contains:UpperChars ,Special Chars and numeric values only";
                testStep = TestStepHelper.StartTestStep(testStep);
                member_two.Password = RandomDataHelper.RandomString(4).ToUpper() + RandomDataHelper.RandomNumber(3) + RandomDataHelper.RandomSpecialCharactersString(2);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member_two.Username, member_two.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId().Trim();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber + ";User Details: " + member_two.Username + "Password Details:" + member_two.Password;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #endregion  Verifying Creating user with Password:LowerChars ,Special Chars and numeric values only

                #region Pre-req:Navigate to Member Registration 
                stepName = "Navigate to Member Search";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region 3.Verifying Creating user with Password:UpperChars ,Lower Chars and numeric values only
                #region Step1:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member_three.FirstName, member_three.LastName, member_three.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                vc.LoyaltyIdNumber = RandomDataHelper.RandomNumber(5);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member_three.PrimaryEmailAddress, member_three.PrimaryPhoneNumber, member_three.PrimaryPhoneNumber, member_three.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page and Entering Password Contains:UpperChars ,Lower Chars and numeric values only";
                testStep = TestStepHelper.StartTestStep(testStep);
                member_three.Password = RandomDataHelper.RandomString(4).ToLower() + RandomDataHelper.RandomString(3).ToUpper() + RandomDataHelper.RandomNumber(2);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member_three.Username, member_three.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId().Trim();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber + ";User Details: " + member_three.Username + "Password Details:" + member_three.Password;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #endregion  Verifying Creating user with Password:LowerChars ,Special Chars and numeric values only

                #region Pre-req:Navigate to Member Registration 
                stepName = "Navigate to Member Search";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region 4.Verifying Creating user with Password:UpperChars ,Lower Chars and Special values only
                #region Step1:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step2:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member_four.FirstName, member_four.LastName, member_four.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                vc.LoyaltyIdNumber = RandomDataHelper.RandomNumber(5);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member_four.PrimaryEmailAddress, member_four.PrimaryPhoneNumber, member_four.PrimaryPhoneNumber, member_four.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page and Entering Password Contains:UpperChars ,Lower Chars and Special Char values only";
                testStep = TestStepHelper.StartTestStep(testStep);
                member_four.Password = RandomDataHelper.RandomString(4).ToLower() + RandomDataHelper.RandomString(3).ToUpper() + RandomDataHelper.RandomSpecialCharactersString(2);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member_four.Username, member_four.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Save Registration and Capture Loyalty ID 
                stepName = "Save Registration and Capture Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId().Trim();
                stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber + ";User Details: " + member_four.Username + "Password Details:" + member_four.Password;
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Navigate to Member Search
                stepName = "Navigate to Member Search ";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Search Based on Loyalty ID
                stepName = "Search Based on Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Select and Verify Loyalty ID
                stepName = "Select and Verify Loyalty ID";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSPSearchPage.Select();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #endregion  Verifying Creating user with Password:LowerChars ,Special Chars and numeric values only

                #region Step16:Logout As csadmin 
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


        /// <summary>
        /// BTA : Create Member Without Loyalty ID and Basic info and Mandatory Fields
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA08_CSP_UpdateMemberInfo()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);

                Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                MemberDetails details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                #endregion

                #region Precondtion:Create Members
                testStep = TestStepHelper.StartTestStep(testStep);
                stepName = "Adding member with CDIS service";
                Member output = basePages.CreateMemberThroughCDIS();
                vc.LoyaltyIdNumber = basePages.GetLoyaltyNumber(output);
                testStep.SetOutput("Loyalty Number:" + vc.LoyaltyIdNumber + ",Name:" + output.FirstName);             
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

                #region Step10:Save Registration and Verify Loyalty ID 
                stepName = "Save Registration and Verify Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_UpdateProfilePage.SaveRegistration();
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step11:Verify FirstName on Account Summary Page
                stepName = "Verify FirstName on Account Summary Page ;Appeared Name is Having Both First and Last Name , Hence Verifying First Part as First Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_FirstName = member.FirstName.Substring(0, 50);

                stepstatus = CSPAccountSummaryPage.VerifyFirstName(Member_FirstName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify Last tName on Account Summary Page
                stepName = "Verify Last Name on Account Summary Page;Appeared Name is Having Both First and Last Name , Hence Verifying Second Part as Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_LastName = member.LastName.Substring(0, 50);

                stepstatus = CSPAccountSummaryPage.VerifyLastName(Member_LastName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify Primary Email Address on Account Summary Page
                stepName = "Verify Primary Email Address on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_PrimaryEmailAddress = member.PrimaryEmailAddress.Substring(0, 254);

                stepstatus = CSPAccountSummaryPage.VerifyPrimaryEmail(Member_PrimaryEmailAddress, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Verify Address Line 1 on Account Summary Page
                stepName = "Verify Address Line 1 on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_details_AddressLineOne = details.AddressLineOne.Substring(0, 100);
                stepstatus = CSPAccountSummaryPage.VerifyAddressLine1(Member_details_AddressLineOne, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion


                #region Step14:Logout As csadmin 
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


        /// <summary>
        /// BTA : Create Member Without Loyalty ID and Basic info and Mandatory Fields
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA09_CSP_ValidateErrorMessages()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);

                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfo();
                MemberDetails details = MP_Model.GenerateMemberDetails();
                VirtualCard vc = MP_Model.GenerateVirtualCard();
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);

                vc.LoyaltyIdNumber = "";
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

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step3:Verify that the user cannot complete registration without completing all the required fields.
                stepName = "Verify the Error Prompt Message complee  the required fields UserName";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.Save_AndVerifyMessage("Please enter a username.", out stepOutput); testStep.SetOutput(stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Verify that the user cannot complete registration without completing all the required fields.
                stepName = "Verify the Error Prompt Message to complete the required fields Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.Save_AndVerifyMessage("Please enter a password", out stepOutput); testStep.SetOutput(stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion
              
                #region Step6:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page and Entering Password Contains:LowerChars ,Special Chars and numeric values only";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.Username = RandomDataHelper.RandomString(4).ToLower() + RandomDataHelper.RandomNumber(3).ToLower() + RandomDataHelper.RandomSpecialCharactersString(2);
                member.Password = member.Username;
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Verify the Error Prompt Message to Not Enter Username as Password
                stepName = "Verify the Error Prompt Message to Not Enter Username as Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.Save_AndVerifyMessage("Password may not contain the username.", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Enter login Credentials
                stepName = "Enter  login Credentials  User Name and Password Same";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.Username = RandomDataHelper.RandomNumber(3)+RandomDataHelper.RandomString(4).ToLower() + RandomDataHelper.RandomSpecialCharactersString(2);
                member.Password = member.Username;
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Verify the Error Prompt Message to Not Enter Username as Password
                stepName = "Verify the Error Prompt Message to Not Enter Username as Password";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.Save_AndVerifyMessage("Password must start with a letter.", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Enter login Credentials
                stepName = "Enter  login Credentials  User Name and Password Same";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.Password = RandomDataHelper.RandomNumber(1) + RandomDataHelper.RandomString(2).ToLower() + RandomDataHelper.RandomSpecialCharactersString(1);
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify the Error Prompt Message if Password is lessthan 7 Charcters
                stepName = "Verify the Error Prompt Message if Password is lessthan 7 Characters";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.Save_AndVerifyMessage("Password must be at least 7 characters.", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion



                //#region Step5:Enter login Credentials
                //stepName = "Enter  login Credentials Details on Member Registration page";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step6:Save Registration and Capture Loyalty ID 
                //stepName = "Save Registration and Capture Loyalty ID ";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //CSP_RegistrationPage.SaveRegistration();
                //var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                //vc.LoyaltyIdNumber = CSPAccountSummaryPage.CaptureLoyaltyId();
                //stepOutput = "Registration Completed Successfully and Loyalty ID Generated:" + vc.LoyaltyIdNumber;
                //testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step7:Navigate to Member Search
                //stepName = "Navigate to Member Search ";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberSearch, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step11:Search Based on Loyalty ID
                //stepName = "Search Based on Loyalty ID";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //var CSPSearchPage = new CSPortal_SearchPage(DriverContext);
                //stepstatus = CSPSearchPage.Search_BasedOnLoyaltyID(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step12:Select and Verify Loyalty ID
                //stepName = "Select and Verify Loyalty ID";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //CSPSearchPage.Select(member.FirstName);
                //stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step11:Verify FirstName on Account Summary Page
                //stepName = "Verify FirstName on Account Summary Page";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //stepstatus = CSPAccountSummaryPage.VerifyFirstName(member.FirstName, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                //#region Step12:Verify Last tName on Account Summary Page
                //stepName = "Verify Last Name on Account Summary Page";
                //testStep = TestStepHelper.StartTestStep(testStep);
                //stepstatus = CSPAccountSummaryPage.VerifyLastName(member.LastName, out stepOutput); testStep.SetOutput(stepOutput);
                //testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                //listOfTestSteps.Add(testStep);
                //#endregion

                #region Step13:Logout As csadmin 
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


        /// <summary>
        /// BTA-138 : Create Member With Loyalty ID and Verify the same on Member search Page
        /// </summary>
        [TestMethod]
        [TestCategory("CSPortal")]
        public void BTA10_CSP_CreateMemberWithMaximumValues()
        {
            ProjectBasePage basePages = new ProjectBasePage(driverContext);
            testCase = new TestCase(TestContext.TestName);
            listOfTestSteps = new List<TestStep>();
            testStep = new TestStep();
            string stepName = "";
            bool stepstatus;
            string stepOutput = "";
            try
            {
                Common common = new Common(DriverContext);
                #region Generating Test Data to Create new user with loyalty card
                MemberProfile MP_Model = new MemberProfile(DriverContext);
                Member member = MP_Model.GenerateMemberBasicInfoWithAboveMaxValues();
                MemberDetails details = MP_Model.GenerateMemberDetailsWithAboveMaxValues();
                VirtualCard vc = MP_Model.GenerateVirtualCardAboveMaxValues();
                #endregion

                #region Step1:Launch CSPortal Portal
                stepName = "Launch Customer Service Portal URL";
                testStep = TestStepHelper.StartTestStep(testStep);
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSP_LoginPage.LaunchCSPortal(login.Csp_url, out string Step_Output); testStep.SetOutput(Step_Output);
                testStep.SetOutput("Launch Customer Service Portal URL is Successful");
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

                #region Step3:Navigate to Member Registration 
                stepName = "Navigate to Member Registration";
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_HomePage.NavigateToDashBoardMenu(CSPortal_HomePage.DashBoard.MemberRegistration, out stepOutput);
                testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step4:Enter Basic Info
                stepName = "Enter Basic Info Details on Member Registration page";
                var CSP_RegistrationPage = new CSPortal_MemberRegistrationPage(DriverContext);
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterBasicInfo_Details(member.FirstName, member.LastName, member.MiddleName, details.Gender, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step5:Enter Loyalty Card Info
                stepName = "Enter  Loyalty Card Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step6:Enter Address
                stepName = "Enter  Address Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterAddress_Details(details.AddressLineOne, details.AddressLineTwo, details.Country, details.StateOrProvince, details.City, details.ZipOrPostalCode, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step7:Enter ContactInfo
                stepName = "Enter  Contact Info Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterContactInfo_Details(member.PrimaryEmailAddress, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, member.PrimaryPhoneNumber, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step8:Enter login Credentials
                stepName = "Enter  login Credentials Details on Member Registration page";
                testStep = TestStepHelper.StartTestStep(testStep);
                member.Password = RandomDataHelper.RandomString(10).ToLower() + RandomDataHelper.RandomNumber(10) + RandomDataHelper.RandomSpecialCharactersString(3)+ RandomDataHelper.RandomString(10).ToUpper();
                stepstatus = CSP_RegistrationPage.EnterLoginCredentials_Details(member.Username, member.Password, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step9:Select All Opt In/Out Check boxes
                stepName = "Select All Opt In/Out Check boxes";
                testStep = TestStepHelper.StartTestStep(testStep);
                stepstatus = CSP_RegistrationPage.EnterOptIn_Out_Details("DirectMailOptIn", "EmailOptIn", "SmsOptIn", out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step10:Save Registration and Verify Loyalty ID 
                stepName = "Save Registration and Verify Loyalty ID ";
                testStep = TestStepHelper.StartTestStep(testStep);
                CSP_RegistrationPage.SaveRegistration();
                string ExpectedVirtualCard = vc.LoyaltyIdNumber.Substring(0,255);
                var CSPAccountSummaryPage = new CSPortal_MemberAccountSummaryPage(DriverContext);
                stepstatus = CSPAccountSummaryPage.VerifyLoyaltyId(ExpectedVirtualCard, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step11:Verify FirstName on Account Summary Page
                stepName = "Verify FirstName on Account Summary Page ;Appeared Name is Having Both First and Last Name , Hence Verifying First Part as First Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_FirstName = member.FirstName.Substring(0, 50);

                stepstatus = CSPAccountSummaryPage.VerifyFirstName(Member_FirstName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step12:Verify Last tName on Account Summary Page
                stepName = "Verify Last Name on Account Summary Page;Appeared Name is Having Both First and Last Name , Hence Verifying Second Part as Last Name";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_LastName = member.LastName.Substring(0, 50);

                stepstatus = CSPAccountSummaryPage.VerifyLastName(Member_LastName, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step13:Verify Primary Email Address on Account Summary Page
                stepName = "Verify Primary Email Address on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_PrimaryEmailAddress = member.PrimaryEmailAddress.Substring(0, 254);

                stepstatus = CSPAccountSummaryPage.VerifyPrimaryEmail(Member_PrimaryEmailAddress, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step14:Verify Address Line 1 on Account Summary Page
                stepName = "Verify Address Line 1 on Account Summary Page";
                testStep = TestStepHelper.StartTestStep(testStep);
                string Member_details_AddressLineOne = details.AddressLineOne.Substring(0, 100);
                stepstatus = CSPAccountSummaryPage.VerifyAddressLine1(Member_details_AddressLineOne, out stepOutput); testStep.SetOutput(stepOutput);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                #endregion

                #region Step15:Logout As csadmin 
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
