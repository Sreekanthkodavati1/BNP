using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Helpers;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using static Bnp.Core.WebPages.CSPortal.CSPortal_HomePage;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// CS Portal Login > User Administration > Agent Page
    /// </summary>
    public class CSPortal_UserAdministrationAgentPage : ProjectBasePage
    {
        readonly string BTA_DEV_CS_LogPath = CsPortalData.BTA_DEV_CS_LogPath;
        int initialWordCount;

        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_UserAdministrationAgentPage(DriverContext driverContext) : base(driverContext) { }

        #region Agent Page Element Locators
        private readonly ElementLocator TextBox_AgentSearch = AgentPage_TextBox_Custom_ElementLocatorXpath("Search:");
        private readonly ElementLocator Select_Search = AgentPage_Select_Custom_ElementLocatorXpath("Search:");
        private readonly ElementLocator Button_Search = new ElementLocator(Locator.XPath, "//input[@value='Search']");
        private readonly ElementLocator TextBox_FirstName = AgentPage_TextBox_Name_Custom_ElementLocatorXpath("First Name");
        private readonly ElementLocator TextBox_LastName = AgentPage_TextBox_Name_Custom_ElementLocatorXpath("Last Name");
        private readonly ElementLocator Select_Role = AgentPage_Select_Custom_ElementLocatorXpath("Role");
        private readonly ElementLocator TextBox_UserName = AgentPage_TextBox_Custom_ElementLocatorXpath("Username");
        private readonly ElementLocator CheckBox_PasswordChangeRequired = AgentPage_TextBox_Custom_ElementLocatorXpath("Password Change Required");
        private readonly ElementLocator TextBox_Password = AgentPage_TextBox_Custom_ElementLocatorXpath("Password");
        private readonly ElementLocator Select_Status = AgentPage_Select_Custom_ElementLocatorXpath("Status");
        private readonly ElementLocator Link_CreateNewAgent = AgentPage_Button_Custom_ElementLocatorXpath("Create New Agent");
        private readonly ElementLocator Button_Save = AgentPage_Button_Custom_ElementLocatorXpath("Save");
        private readonly ElementLocator Label_Success = new ElementLocator(Locator.XPath, "//span[text()='CSR agent saved successfully.']");
        private readonly ElementLocator Select_AgentStatus = AgentPage_Select_Custom_ElementLocatorXpath("Status");
        private readonly ElementLocator Error_Message = new ElementLocator(Locator.XPath, "//span[contains(text(),'Account InActive.')]");
        private readonly ElementLocator TextBox_NewPassword = new ElementLocator(Locator.XPath, "//td//span[text()='New Password:']//parent::td//following-sibling::td//input");
        private readonly ElementLocator TextBox_ConfirmPassword = new ElementLocator(Locator.XPath, "//td//span[text()='Confirm Password:']//parent::td//following-sibling::td//input");
        private readonly ElementLocator Message_PasswordSuccess = new ElementLocator(Locator.XPath, "//span[text()='Password changed successfully.']");
        private readonly ElementLocator Message_AccountInactive = new ElementLocator(Locator.XPath, "//span[contains(text(),'Account InActive.')]");
        private readonly ElementLocator PasswordChangePrompt = new ElementLocator(Locator.XPath, "//h3[text()='You must change your password before proceeding']");

        
        #endregion

        /// <summary>
        /// Create Agent with different role 
        /// </summary>
        /// <param name="FirstName">First Name of the Agent</param>
        /// <param name="LastName">Last Name of the Agent</param>
        /// <param name="Role">Role of the Agent</param>
        /// <param name="AgentUserName">User Name of the Agent</param>
        /// <param name="Password">Password of the Agent</param>
        /// <param name="Status">Status of the Agent</param>
        /// <returns>
        /// returns string is agent create or not
        /// </returns> 
        public string CreateAgent(string FirstName, string LastName, string Role, string AgentUserName, string Password, string Status)
        {
            string Message = "";
            try
            {
                if (!VerifyAgentExists(FirstName, LastName, Role,AgentUserName, Status))
                {
                    var intialcount = GetInitialWordCountFromLogFile();
                    Click_OnButton(Link_CreateNewAgent);
                    Driver.GetElement(TextBox_FirstName).SendText(FirstName);
                    Driver.GetElement(TextBox_LastName).SendText(LastName);
                    SelectElement_AndSelectByText(Select_Role, Role);
                    Driver.GetElement(TextBox_UserName).SendText(AgentUserName);
                    Driver.GetElement(TextBox_Password).SendText(Password);
                    SelectElement_AndSelectByText(Select_Status, Status);
                    Click_OnButton(Button_Save);
                    if (Driver.IsElementPresent(Label_Success, 2))
                    {
                        VerifyAgentExists(FirstName, LastName, Role, AgentUserName, Status);
                        if (VerifyAdminCreatedFromLog(intialcount, out string output))
                        {
                            Message = "Agent Created Successfully and Please Details Below:" +
                               ";AgentName:"+ AgentUserName+
                               ";Password:" + Password+
                               ";FirstName:"+FirstName+
                               ";LastName:"+LastName+
                               ";Role:"+Role+
                               ";Status:"+Status;
                            return Message;
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to Verify Existing Agent " + AgentUserName);
                    }
                }
                else
                {
                    Message = "Agent Available Already and Please Details Below:" +
                               ";AgentName:" + AgentUserName +
                               ";FirstName:" + FirstName +
                               ";LastName:" + LastName +
                               ";Role:" + Role +
                               ";Status:" + Status;
                    return Message;
                }
                return Message;
            }
            catch (Exception e) { throw new Exception("Failed to Create Agent" + AgentUserName + " Due to " + e); }
        }

        /// <summary>
        /// Create Agent with different role 
        /// </summary>
        /// <param name="FirstName">First Name of the Agent</param>
        /// <param name="LastName">Last Name of the Agent</param>
        /// <param name="Role">Role of the Agent</param>
        /// <param name="AgentUserName">User Name of the Agent</param>
        /// <param name="Password">Password of the Agent</param>
        /// <param name="Status">Status of the Agent</param>
        /// <returns>
        /// returns string is agent create or not
        /// </returns> 
        public string CreateAgentWithIsPasswordRequiredOption(AgentRegistration agentRegistration)
        {
            string Message = "";
            try
            {
                if (!VerifyAgentExists(agentRegistration.FistName, agentRegistration.LastName, agentRegistration.Role, agentRegistration.UserName, agentRegistration.Status))
                {
                    var intialcount = GetInitialWordCountFromLogFile();
                    Click_OnButton(Link_CreateNewAgent);
                    Driver.GetElement(TextBox_FirstName).SendText(agentRegistration.FistName);
                    Driver.GetElement(TextBox_LastName).SendText(agentRegistration.LastName);
                    SelectElement_AndSelectByText(Select_Role, agentRegistration.Role);
                    Driver.GetElement(TextBox_UserName).SendText(agentRegistration.UserName);
                    Driver.GetElement(TextBox_Password).SendText(agentRegistration.Password);
                    SelectElement_AndSelectByText(Select_Status, agentRegistration.Status);
                    CheckBoxElmandCheck(CheckBox_PasswordChangeRequired,agentRegistration.PasswordChangeRequired);
                    Click_OnButton(Button_Save);
                    if (Driver.IsElementPresent(Label_Success, 2))
                    {
                        VerifyAgentExists(agentRegistration.FistName, agentRegistration.LastName, agentRegistration.Role, agentRegistration.UserName, agentRegistration.Status);
                        if (VerifyAdminCreatedFromLog(intialcount, out string output))
                        {
                            Message = "Agent Created Successfully and Please Details Below:" +
                               ";AgentName:" + agentRegistration.UserName +
                               ";Password:" + agentRegistration.Password +
                               ";FirstName:" + agentRegistration.FistName +
                               ";LastName:" + agentRegistration.LastName +
                               ";Role:" + agentRegistration.Role +
                                ";Password Change Reqiured:" + agentRegistration. PasswordChangeRequired.ToString()+
                               ";Status:" + agentRegistration.Status;
                            return Message;
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to Verify Existing Agent " + agentRegistration.UserName);
                    }
                }
                else
                {
                    Message = "Agent Available Already and Please Details Below:" +
                               ";AgentName:" + agentRegistration.UserName +
                               ";FirstName:" + agentRegistration.FistName +
                               ";LastName:" + agentRegistration.LastName +
                               ";Role:" + agentRegistration.Role +
                               ";Status:" + agentRegistration.Status;
                    return Message;
                }
                return Message;
            }
            catch (Exception e) { throw new Exception("Failed to Create Agent" + agentRegistration.UserName + " Due to " + e); }
        }

        /// <summary>
        /// Verify already Agent exists
        /// </summary>
        /// <param name="AgentUserName">Name of the Agent</param>
        /// <returns>
        /// returns true if Agent name exists, else false
        /// </returns>
        public bool VerifyExistingAgent(string FirstName,string LastName,string Role,string AgentUserName,string Status)
        {

            string AgentName = ".//td//span[text()='"+ FirstName + "' and contains(@id,'FirstName')]//parent::td//parent::" +
                                "tr//td//span[text()='"+ LastName + "' and contains(@id,'LastName')]//parent::td//parent::" +
                                "tr//td//span[text()='"+ Role + "' and contains(@id,'Role')]//parent::td//parent::" +
                                "tr//td//span[text()='"+ AgentUserName + "' and contains(@id,'Username')]//parent::td//parent::" +
                                "tr//td//span[text()='"+Status+"' and contains(@id,'Status')]";
            if (Driver.IsElementPresent(By.XPath(AgentName)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verify already Agent exists
        /// </summary>
        /// <param name="AgentUserName">Name of the Agent</param>
        /// <returns>
        /// returns true if Agent name exists, else false
        /// </returns>
        public bool VerifyExistingAgent_OnlyUserName(string AgentUserName)
        {

            string AgentName = ".//td//span[text()='" + AgentUserName + "' and contains(@id,'Username')]";
            if (Driver.IsElementPresent(By.XPath(AgentName)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verify existing Agent 
        /// </summary>
        /// <param name="AgentUserName">Name of the Agent</param>
        /// <returns>
        /// returns true if Agent name exists, else false
        /// </returns>   
        public bool VerifyAgentExists(string FirstName,string LastName,String Role,string AgentUserName,string Status)
        {
            SearchAgent(AgentUserName, "Username");
            if (VerifyExistingAgent(FirstName, LastName, Role,AgentUserName, Status))
                return true;
            else
                return false;
        }
        public bool VerifyAgentExists_BaseOnUserName(string AgentUserName)
        {
            SearchAgent(AgentUserName, "Username");
            if (VerifyExistingAgent_OnlyUserName(AgentUserName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Search Agent
        /// </summary>
        /// <param name="AgentUserName">Name of the Agent</param>
        /// <param name="SearchCriteria">Search Criteria </param>      
        public void SearchAgent(string AgentName, string SearchCriteria)
        {
            try
            {
                SelectElement_AndSelectByText(Select_Search, SearchCriteria);
                Driver.GetElement(TextBox_AgentSearch).SendText(AgentName);
                Driver.GetElement(Button_Search).ClickElement();
            }
            catch (Exception) { throw new Exception("Failed to Search Agent " + AgentName); }
        }


        /// <summary>
        /// Edit Agent Status from Active to InActive and InActive to Active
        /// </summary>
        /// <param name="agentName">Name of the Agent and status of the agent</param>
        /// <returns>
        /// returns string that able to change the status of agent or not
        /// </returns> 
        public bool EditAgentStatus(AgentRegistration agent1,string AgentExpectedStatus, out string output)
        {
            
            string outMsg = "";
            try
            {
                if (!VerifyAgentExists_BaseOnUserName(agent1.UserName))
                {
                    agent1.Status = AgentRegistration.AgentStatus.Active.ToString();
                    outMsg = CreateAgent(agent1.FistName, agent1.LastName, agent1.Role, agent1.UserName, agent1.Password, agent1.Status);
                }
                
                Driver.FindElement(By.XPath("//span[text()='" + agent1.UserName + "']//parent::td//parent::tr//a[text()='Edit']")).ClickElement();
                SelectElement_AndSelectByText(Select_Status, AgentExpectedStatus);
                Click_OnButton(Button_Save);
                if (Driver.IsElementPresent(Label_Success, 1))
                {
                    agent1.Status = AgentExpectedStatus;
                    output = outMsg + " " + agent1.UserName + "  Status:" + agent1.Status + " changed successfully";
                    return true;
                }
                else
                {
                    throw new Exception("Failed To change the agent status refer screenshot for more info");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed To change the agent status refer screenshot for more info");
            }
        }

        public bool EditAgentPasswordOption(AgentRegistration agent1, out string output)
        {

            string outMsg = "";
            try
            {
                if (!VerifyAgentExists_BaseOnUserName(agent1.UserName))
                {
                    agent1.Status = AgentRegistration.AgentStatus.Active.ToString();
                    outMsg = CreateAgent(agent1.FistName, agent1.LastName, agent1.Role, agent1.UserName, agent1.Password, agent1.Status);
                }

                Driver.FindElement(By.XPath("//span[text()='" + agent1.UserName + "']//parent::td//parent::tr//a[text()='Edit']")).ClickElement();
                CheckBoxElmandCheck(CheckBox_PasswordChangeRequired, agent1.PasswordChangeRequired);
                Click_OnButton(Button_Save);
                if (Driver.IsElementPresent(Label_Success, 1))
                {
                    output = outMsg + " " + agent1.UserName + "  Password RequiredOption:" + agent1.PasswordChangeRequired.ToString() + " changed successfully";
                    return true;
                }
                else
                {
                    throw new Exception("Failed To change the agent Password RequiredOption refer screenshot for more info");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed To change the agent status refer screenshot for more info");
            }
        }

        public bool EditAgentRoleOption(AgentRegistration agent1, out string output)
        {

            string outMsg = "";
            try
            {
                if (!VerifyAgentExists_BaseOnUserName(agent1.UserName))
                {
                    agent1.Status = AgentRegistration.AgentStatus.Active.ToString();
                    outMsg = CreateAgent(agent1.FistName, agent1.LastName, agent1.Role, agent1.UserName, agent1.Password, agent1.Status);
                }

                Driver.FindElement(By.XPath("//span[text()='" + agent1.UserName + "']//parent::td//parent::tr//a[text()='Edit']")).ClickElement();
                SelectElement_AndSelectByText(Select_Role, agent1.Role);
                Click_OnButton(Button_Save);
                if (Driver.IsElementPresent(Label_Success, 1))
                {
                    output = outMsg + "Agent: " + agent1.UserName + " ; Role Option: " + agent1.Role.ToString() + " changed successfully";
                    return true;
                }
                else
                {
                    throw new Exception("Failed To change the agent Role refer screenshot for more info");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed To change the agent Role refer screenshot for more info");
            }
        }

        public bool VerifyAgentPasswordSelectionOnEditMode(AgentRegistration agent1, out string output)
        {
            try {
                if (VerifyAgentExists_BaseOnUserName(agent1.UserName))
                {
                    Driver.FindElement(By.XPath("//span[text()='" + agent1.UserName + "']//parent::td//parent::tr//a[text()='Edit']")).ClickElement();
                    if (IsChecked(CheckBox_PasswordChangeRequired))
                    {
                        output = "Password Change Required Options Checked as Expected";
                        return true;
                    }
                    throw new Exception("Password Change Required Options is not Checked refer screenshot for more Inforation");

                }
                throw new Exception("Failed to Find Agent:"+agent1.UserName);
            }
            catch (Exception e)
            {
                throw new Exception("Password Change Required Options due to:"+e.Message);
            }
        }
        /// <summary>
        /// Edit Agent and Change Password
        /// </summary>
        /// <param name="agentName">Name of the Agent and status of the agent</param>
        /// <param name="NewPassword">Random Generated password</param>
        /// <param name="ConfirmPassword">Random Generated password</param>
        /// <returns>
        /// returns bool and Message with Password change status
        /// </returns> 
        public bool ChangeAgentPassword(string agentName, string NewPassword, string ConfirmPassword, out string output)
        {
            try
            {
                ElementLocator ChangePassword = new ElementLocator(Locator.XPath, "//span[text()='" + agentName + "']//parent::td//parent::tr//a[text()='Change Password']");
                Click_OnButton(ChangePassword);
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Click_OnButton(Button_Save);
                if (Driver.IsElementPresent(Message_PasswordSuccess, 1))
                {
                    output = "Password Changed Successfully; Agent Name:" + agentName + " ;New Password:" + NewPassword;
                    return true;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed To change the agent status refer screenshot for more info");
            }
            output = "Failed to Change Password , Details are; Agent Name:" + agentName + " ;New Password:" + NewPassword;
            throw new Exception(output);
        }

        public bool ChangeAgent_InvalidInputs(string agentName, string NewPassword, string ConfirmPassword, out string output)
        {
            try
            {
                ElementLocator ChangePassword = new ElementLocator(Locator.XPath, "//span[text()='" + agentName + "']//parent::td//parent::tr//a[text()='Change Password']");
                Click_OnButton(ChangePassword);
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Click_OnButton(Button_Save);
                output = "Agent Name:" + agentName + ";Password:" + NewPassword + ";Confirm Password:" + ConfirmPassword;
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed To Enter Details status refer screenshot for more info");
            }

        }

        /// <summary>
        ///VerifyLogin CS Portal using Different agent credentials
        /// </summary>
        /// <param name="login"></param>
        ///  <param name="Status"></param>
        /// <returns>Login Succesful Or Account InActive</returns>
        public bool VerifyLogin(Login login, string Status, out string status)
        {
            try
            {
                var CSP_LoginPage = new CSPortal_LoginPage(DriverContext);
                CSPortal_HomePage cSPortal_Home = new CSPortal_HomePage(DriverContext);

                CSP_LoginPage.LoginCSPortal_ForActiveandInactive(login.UserName, login.Password, out status);
                switch (Status)
                {
                    case "InActive":
                        return ValidateErrorMessage(ErrorPageLevel("Account InActive."), "Account InActive.", out status);

                    case "Active":
                        if (Driver.IsElementPresent(cSPortal_Home.Button_Logout, 1))
                        {
                            status = login.UserName + "Login CS Portal is Successful";
                            return true;
                        }
                        else { throw new Exception("Account Is Not Active"); }
                    case "Locked":
                        return ValidateErrorMessage(ErrorPageLevel("Account Locked."), "Account Locked.", out status);
                    case "PasswordChange":
                        if (Driver.IsElementPresent(PasswordChangePrompt,1))
                        { status = "You must change your password before proceeding Message appeared"; return true; }
                        throw new Exception("Failed Get password Promopt");

                    default:
                        throw new Exception("Failed to match role function");
                }
                throw new Exception("Invalid Status Appeared:" + Status);
            }
            catch (Exception)
            {
                throw new Exception("Login failed refer screenshot for more info");
            }
        }

        /// <summary>
        ///Verify Dashboard_Links with respective agents
        /// </summary>
        /// <param name="AgentName"></param>
        /// <returns>return true if Links present /// </returns>
        public string VerifyDashboard_Links(string AgentName)
        {
            string message = "Dashboard links verified successfully";
            try
            {
                var CSP_HomePage = new CSPortal_HomePage(DriverContext);
                if (AgentName.Equals(AgentValues.AdminAgent))
                {
                    if (Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberSearch)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberRegistration)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.UserAdministration)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.ChangePassword)), 5))
                    {
                        return message;
                    }
                }
                else if (AgentName.Equals(AgentValues.SrAdminAgent))
                {
                    if (Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberSearch)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberRegistration)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.ChangePassword)), 5))
                    {
                        return message;
                    }
                }
                else if (AgentName.Equals(AgentValues.JrAdminAgent))
                {
                    if (Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberSearch)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberRegistration)), 5) && Driver.IsElementPresent(CSP_HomePage.DashBoardMenu(EnumUtils.GetDescription(DashBoard.ChangePassword)), 5))
                    {
                        return message;
                    }
                }
                else
                {
                    throw new Exception("Failed to Verify Dashboard Links");
                }
                return message;
            }
            catch
            {
                throw new Exception("Failed to Verify Dashboard Links");
            }
        }

        public bool VerifyAdminCreatedFromLog(int initialWordCount, out string output)
        {
            bool status = false;
            output = "";
            for (int i = 0; i < 5; i++)
            {
                int finalWordCount = FilesHelper.GetWordCount(FilesHelper.ReadLogFile(BTA_DEV_CS_LogPath), "Creating new CSAgent");
                if (initialWordCount < finalWordCount)
                {
                    status = true;
                    output = "Admin is created successfully and created information is availble in Log file:" + BTA_DEV_CS_LogPath;
                    return status;
                }
                else
                {
                    throw new Exception("Admin creation data is not availble in Log file");
                }

            }
            return status;
        }

        public int GetInitialWordCountFromLogFile()
        {
            return initialWordCount = BnPBaseFramework.Web.Helpers.FilesHelper.GetWordCount(BnPBaseFramework.Web.Helpers.FilesHelper.ReadLogFile(BTA_DEV_CS_LogPath), "Creating new CSAgent");
        }

        #region Agent Page Locator methods
        public static ElementLocator AgentPage_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        public static ElementLocator AgentPage_Select_Custom_ElementLocatorXpath(string SelectName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Select(SelectName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static ElementLocator AgentPage_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Buttons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static ElementLocator AgentPage_TextBox_Name_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input_(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        #endregion

        public TestStep CreateAgent(AgentRegistration agentRegistration, List<TestStep> listOfTestSteps)
        {
            string stepName = "Create New Agent" + agentRegistration.UserName + " if user is not existed";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                testStep.SetOutput(CreateAgent(agentRegistration.FistName, agentRegistration.LastName, agentRegistration.Role, agentRegistration.UserName, agentRegistration.Password, agentRegistration.Status));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep CreateAgentWithIsPasswordRequiredOption(AgentRegistration agentRegistration, List<TestStep> listOfTestSteps)
        {
            string stepName = "Create New Agent" + agentRegistration.UserName + " if user is not existed";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                testStep.SetOutput(CreateAgentWithIsPasswordRequiredOption(agentRegistration));
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }
        public TestStep VerifyAgentPasswordSelectionOnEditMode(AgentRegistration agentRegistration, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify  Agent" + agentRegistration.UserName + " Password Selection on Edit Mode";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                 VerifyAgentPasswordSelectionOnEditMode(agentRegistration, out string output);testStep.SetOutput(output);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep ChangeAgentPassword(AgentRegistration agentRegistration, List<TestStep> listOfTestSteps)
        {
            string stepName = "Change Password for Test Agent";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                ChangeAgentPassword(agentRegistration.UserName, agentRegistration.Password, agentRegistration.Password, out string changepassword);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput(changepassword);
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep VerifyInvalidAgentSearch_WithDifferntTextOptions(string AgentName, string SearchCriteria, string ExpectedErrorMessage, List<TestStep> listOfTestSteps)
        {
            string stepName = "Search for AgentName:" + AgentName + " and Search Criteria:" + SearchCriteria;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                SearchAgent(AgentName, SearchCriteria);
                ValidateErrorMessage(ErrorPageLevel(ExpectedErrorMessage), ExpectedErrorMessage, out string Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                testStep.SetOutput(Message);
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep VerifyAgentsCount(int count, string AgentName, string SearchCriteria, List<TestStep> listOfTestSteps)
        {
            string stepName = "Search for " + AgentName + ": " + SearchCriteria + " in DB under Lw_CSAgent Table";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                string BuildQuery = "select COUNT(*) from Lw_Csagent where " + AgentName + "  = '" + SearchCriteria + "'";
                VerifyDBCount(count, BuildQuery, out string Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("API"));
                testStep.SetOutput(Message);
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("API"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }
        public TestStep EditAgentStatus(AgentRegistration agentRegistration, string NewStatus,List<TestStep> listOfTestSteps)
        {
            string stepName = "Edit Agent :" + agentRegistration.UserName + " Status, if user is not existed create Agent with Active Status";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EditAgentStatus(agentRegistration, NewStatus,out string Messge); testStep.SetOutput(Messge);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep EditAgentRole(AgentRegistration agentRegistration, List<TestStep> listOfTestSteps)
        {
            string stepName = "Edit Agent :" + agentRegistration.UserName + " Role, if user is not existed create Agent with Active Status";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EditAgentRoleOption(agentRegistration, out string Messge); testStep.SetOutput(Messge);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep VerifyLogin(Login login, string NewStatus, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Agent :" + login.UserName + " ;Password:"+login.Password+"; And Status:"+NewStatus;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyLogin(login, NewStatus, out string Messge); testStep.SetOutput(Messge);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}
