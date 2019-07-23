using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Types;
using System.Collections.Generic;
using BnPBaseFramework.Web.Extensions;
using OpenQA.Selenium;
using System;
using OpenQA.Selenium.Interactions;
using Bnp.Core.WebPages.Models;
using System.Configuration;

namespace Bnp.Core.WebPages.Navigator.Admin
{
    /// <summary>
    ///  This class handles Navigator Portal > Admin > Users Page elements
    /// </summary>
    public class Navigator_Admin_UsersPage : ProjectBasePage
    {
        ///<summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Admin_UsersPage(DriverContext driverContext)
          : base(driverContext)
        {
        }

        #region Admin page element locators
        private readonly ElementLocator Button_AddNewUser = new ElementLocator(Locator.XPath, "//a[text()='Add New User']");
        private readonly ElementLocator TextBox_FirstName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtFirstName')]");
        private readonly ElementLocator TextBox_LastName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtLastName')]");
        private readonly ElementLocator TextBox_Email = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtEmail')]");
        private readonly ElementLocator TextBox_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtUserName')]");
        private readonly ElementLocator TextBox_Password = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtPassword')]");
        private readonly ElementLocator TextBox_ConfirmPassword = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlUserDetail_txtCPassword')]");
        private readonly ElementLocator CheckBox_Enabled = new ElementLocator(Locator.XPath, "//span[text()='Enabled']//preceding-sibling::input[@type='checkbox']");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[text()='Save']");
        private readonly ElementLocator Button_Cancel = new ElementLocator(Locator.XPath, "//a[text()='Cancel']");
        private readonly ElementLocator Checkbox_SelectAdmin = new ElementLocator(Locator.XPath, "//span[contains(text(),'Administrative Role')]//preceding-sibling::input");
        private readonly ElementLocator Checkbox_SelectAllRoles = new ElementLocator(Locator.XPath, "//span[contains(text(),'All Roles')]//preceding-sibling::input");
        private readonly ElementLocator Checkbox_SelectDba = new ElementLocator(Locator.XPath, "//label[contains(text(),'DB Credential Administrator')]//preceding-sibling::input");
        private readonly ElementLocator Checkbox_SelectKey = new ElementLocator(Locator.XPath, "//label[contains(text(),'Key Administrator')]//preceding-sibling::input");
        private readonly ElementLocator Checkbox_SelectLwa = new ElementLocator(Locator.XPath, "//label[contains(text(),'LoyaltyWare Administrator')]//preceding-sibling::input");
        private readonly ElementLocator Checkbox_SelectWeb = new ElementLocator(Locator.XPath, "//label[contains(text(),'Web Management')]//preceding-sibling::input");
        private readonly ElementLocator Menu_ReadWriteExecute = new ElementLocator(Locator.XPath, "//div[contains(@id,'_rtvPermissions_NonSubRoleNodeContextMenu_detached')]/ul//span[contains(text(),'Read/Write/Execute')]");

        #endregion

        /// <summary>
        /// Enum for User Details tab
        /// </summary>
        public enum UserDetailsTabs
        {
            Profile,
            Roles,
            Permissions,
            Reporting
        }

        /// <summary>
        /// Forms xpath for tab names
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns>
        /// returns string of the XPath
        /// </returns>
        public string UserTabsElement(string TabName)
        {
            string TabElement = "//span[contains(text(),'" + TabName + "')]";
            return TabElement;
        }


        public ElementLocator Tree_OrgnizationFromPermissions(string orgName)
        {
            ElementLocator organization = new ElementLocator(Locator.XPath, "//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']/../input");
            return organization;
        }

        public ElementLocator ActionButton_OfUser(string username,string action_button)
        {
            ElementLocator Edit = new ElementLocator(Locator.XPath, ("//span[text()='" + username + "']//parent::td//parent::tr//a[text()='"+ action_button + "']"));
            return Edit;
        }


        /// <summary>
        /// Gets web element locator for User details Tabs based on tab name
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns>
        /// returns web element by xpath
        /// </returns>
        public ElementLocator UserDetailsTab(string TabName)
        {
            ElementLocator Tab_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, UserTabsElement(TabName));
            return Tab_Custom_ElementLocatorXpath;
        }

        /// <summary>
        /// Navigate to User Details tabs
        /// </summary>
        /// <param name="TabName"></param>
        /// <param name="Message"></param>
        /// <returns>
        /// Returns true if tab navigation successful, else false
        /// </returns>
        public bool NavigateToUserDetailsTabs(UserDetailsTabs TabName, out string Message)
        {
            try
            {
                switch (TabName)
                {
                    case UserDetailsTabs.Profile:
                        Driver.GetElement(UserDetailsTab(UserDetailsTabs.Profile.ToString())).ClickElement();
                        break;
                    case UserDetailsTabs.Roles:
                        Driver.GetElement(UserDetailsTab(UserDetailsTabs.Roles.ToString())).ClickElement();
                        break;
                    case UserDetailsTabs.Permissions:
                        Driver.GetElement(UserDetailsTab(UserDetailsTabs.Permissions.ToString())).ClickElement();
                        break;
                    case UserDetailsTabs.Reporting:
                        Driver.GetElement(UserDetailsTab(UserDetailsTabs.Reporting.ToString())).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to find Navigate to " + TabName + " tab");
                }
                Message = " Navigate to " + TabName + " is Successful";
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + TabName + " tab");
            }
        }

        /// <summary>
        /// Enter the user details
        /// </summary>
        /// <param name="user"> user details</param>        
        public void CreateUser(Users user)
        {
            Click_OnButton(Button_AddNewUser);
            Driver.GetElement(TextBox_FirstName).SendText(user.FirstName);
            Driver.GetElement(TextBox_LastName).SendText(user.LastName);
            Driver.GetElement(TextBox_Email).SendText(user.Email);
            Driver.GetElement(TextBox_UserName).SendText(user.UserName);
            Driver.GetElement(TextBox_Password).SendText(user.Password);
            Driver.GetElement(TextBox_ConfirmPassword).SendText(user.ConfirmPassword);
            Click_OnButton(Button_Save);
        }

        /// <summary>
        /// Edit the user details
        /// </summary>
        /// <param name="user"> user details</param>        
        public void EditUser(Users user)
        {
            user.FirstName = user.FirstName + "Edited";
            user.LastName = user.LastName + "Edited";
            user.Email = "Updated" + user.Email;
            user.Password = "Password1*";
            user.ConfirmPassword = "Password1*";
            Driver.GetElement(TextBox_FirstName).SendText(user.FirstName);
            Driver.GetElement(TextBox_LastName).SendText(user.LastName);
            Driver.GetElement(TextBox_Email).SendText(user.Email);
            Driver.GetElement(TextBox_UserName).SendText(user.UserName);
            Driver.GetElement(TextBox_Password).SendText(user.Password);
            Driver.GetElement(TextBox_ConfirmPassword).SendText(user.ConfirmPassword);
            //if (!Driver.GetElement(CheckBox_Enabled).Selected)
            //    Driver.GetElement(CheckBox_Enabled).ClickElement();
            CheckBoxElmandCheck(CheckBox_Enabled);
            Click_OnButton(Button_Save);
        }

        /// <summary>
        /// assigning the user details to model properties
        /// </summary>
        /// <param name="username, password"> username and password for the user </param>        
        /// <returns>
        ///  returns user details object
        /// </returns>
        public Users UserDetails(string userName, string password)
        {
            Users user = new Users
            {
                FirstName = userName + "FIRST",
                LastName = userName + "LAST"
            };
            user.Email = user.FirstName + "." + user.LastName + "@this.com";
            user.UserName = userName;
            user.Password = password;
            user.ConfirmPassword = password;
            return user;
        }

        /// <summary>
        /// Click on EditUser to assign roles for particular user
        /// </summary>
        /// <param name="username, role"> username and Role for the user </param>        
        /// <returns>
        ///  returns true if  click on edituser and assigning role is successfull else false
        /// </returns>
        public bool ClickEditUser_And_AssignRoles(string username, string role)
        {
            var message = "Failed to assign roles";
            if (VerifyUserExists(username))
            {
                Driver.GetElement(ActionButton_OfUser(username,"Edit User")).ClickElement();
                if (NavigateToUserDetailsTabs(UserDetailsTabs.Roles, out message))
                {
                    AssignRoles(role);
                    Click_OnButton(Button_Save);
                    Driver.SwitchTo().Alert().Accept();

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Click on EditUser to verify user loaded in edit mode
        /// </summary>
        /// <param name="username"> username</param>        
        /// <returns>
        ///  returns success message if click on edituser is loads the user in edit mode
        /// </returns>
        public string ClickEditUser_And_VerifyTheEditModeAndRolesTab(Users user)
        {
            var message = "User doesn't exist.";
            if (VerifyUserExists(user.UserName))
            {
                Driver.GetElement(ActionButton_OfUser(user.UserName,"Edit User")).ClickElement();
                IWebElement userDetails = Driver.FindElement(By.XPath("//h2[contains(text(),'User Details')]"));
                IWebElement rolesTab = Driver.FindElement(By.XPath("//span[text()='Roles']"));
                if (userDetails.Displayed && rolesTab.Displayed)
                {
                    return "User loaded in edit mode";
                }
                else
                {
                    return "Faild to load in edit mode";
                }
            }
            else
            {
                return message;
            }
        }

        /// <summary>
        /// AssigningRoles To Users Based On Role
        /// </summary>
        /// <param name="role"> role of the user </param>        
        public void AssignRoles(string role)
        {
            try
            {
                switch (role)

                {
                    case "KEY":
                        VerifyAndSelectAdministrativeRoleCheckBox();
                        Driver.GetElement(Checkbox_SelectKey).ClickElement();
                        break;
                    case "WEB":
                        VerifyAndSelectAdministrativeRoleCheckBox();
                        Driver.GetElement(Checkbox_SelectWeb).ClickElement();
                        break;
                    case "LWADM":
                        VerifyAndSelectAdministrativeRoleCheckBox();
                        Driver.GetElement(Checkbox_SelectLwa).ClickElement();
                        break;
                    case "DBA":
                        VerifyAndSelectAdministrativeRoleCheckBox();
                        Driver.GetElement(Checkbox_SelectDba).ClickElement();
                        break;
                    case "USER":
                        Driver.GetElement(Checkbox_SelectAllRoles).ScrollToElement();
                        CheckBoxElmandCheck(Checkbox_SelectAllRoles);
                        //Driver.GetElement(Checkbox_SelectAllRoles).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to find user type " + role + " role");
                }
            }
            catch
            {
                throw new Exception("Failed to assign " + role + " role");
            }
        }
        /// <summary>
        /// Check whether Administrative Role Check-box is selected if not select the AdministrativeRole Check-box
        /// </summary>
        /// <returns>
        /// retuns true if Administrative Role Check-box is selected
        /// </returns>
        private bool VerifyAndSelectAdministrativeRoleCheckBox()
        {
            if (Driver.IsElementPresent(Checkbox_SelectAdmin, 2))
            {
                CheckBoxElmandCheck(Checkbox_SelectAdmin);
                return true;
            }
            throw new Exception("Failed to Select Administrative Role Checkbox Refer Screenshot for more inputs");
        }

        /// <summary>
        /// Verify User Exists Or Not
        /// </summary>
        /// <param name="username"> username </param>        
        /// <returns>
        ///  returns true if user is already exists else return false
        /// </returns>
        public bool VerifyUserExists(string username)
        {
            try
            {
                if (Driver.IsElementPresent(By.XPath("//td[@colspan]//table")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[@colspan]//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var i = 1; i <= pageCount; i++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + i + "')]")))
                        {
                            Driver.FindElement(By.XPath("//a[contains(text(),'" + i + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(By.XPath("//tr/td/span[text()='" + username + "']")))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//tr/td/span[text()='" + username + "']")))
                    {
                        return true;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        ///    Assigning organization To User
        /// </summary>
        /// <param name="orgName">The organization name </param>        
        public void User_with_All_Permission_And_Assigning_organization(string orgName)
        {
            NavigateToUserDetailsTabs(UserDetailsTabs.Permissions, out string msg);
            ElementLocator organization = new ElementLocator(Locator.XPath, "//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']/../input");
            CheckBoxElmandCheck(organization);
            var org = Driver.FindElement(By.XPath("//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']"));
            Actions action = new Actions(Driver);
            action.ContextClick(org).Build().Perform();
            Driver.ScrollIntoMiddle(organization);
            Driver.FindElement(By.XPath("//div[@id='ctl00_ContentPlaceHolder1_pnlUserDetail_rtvPermissions_NonSubRoleNodeContextMenu_detached']/ul/li[2]")).ClickElement();
            Click_OnButton(Button_Save);
        }

        /// <summary>
        /// Create LWAdmin   
        /// </summary>
        /// <param name="user">The LWAdmin user details </param>        
        /// <returns>
        ///  LWAdmin is created Or Not
        /// </returns>
        public string Create_AdminUsers(Users user, string role)
        {
            var output = "";
            if (!VerifyUserExists(user.UserName))
            {

                CreateUser(user);
                ClickEditUser_And_AssignRoles(user.UserName, role);
                output = " : User is created and required role  :" + role + " Admin is assigned";
            }
            else
            {
                Verify_And_Assign_Role(user.UserName, role);
                output = " : User already exists and verified the required role :" + role + " Admin is assigned";
            }
            return user.UserName + output;
        }


        /// <summary>
        /// Verify and Assign Role to User  
        /// </summary>
        /// <param name="user">user name </param>        
        /// <returns>
        /// Appropriate Role is assigned to the user
        /// </returns>
        public string Verify_And_Assign_Role(string user, string role)
        {
            var message = "Failed to navigate to roles";
            Driver.GetElement(ActionButton_OfUser(user,"Edit User")).ClickElement();

            if (NavigateToUserDetailsTabs(UserDetailsTabs.Roles, out message))
                Driver.GetElement(Checkbox_SelectAdmin).ScrollToElement();
            if (Driver.GetElement(Checkbox_SelectAdmin).Selected || Driver.GetElement(Checkbox_SelectAllRoles).Selected)
            {
                if (VerifyAssignedRole(role))
                {
                    message = "Ëxpected role:  :" + role + " is selected";
                }
                else
                {
                    AssignRoles(role);
                    Click_OnButton(Button_Save);
                    Driver.SwitchTo().Alert().Accept();
                    message = "Given role  :" + role + " is selected";
                }
            }
            else
            {
                AssignRoles(role);
                Click_OnButton(Button_Save);
                Driver.SwitchTo().Alert().Accept();
                message = "Given role  :" + role + " is selected";
            }


            return message;
        }

        /// <summary>
        /// Verifies the asssigned role is correct or not
        /// </summary>
        /// <param name="role">user role </param>        
        /// <returns>
        /// Appropriate Role is checked or not
        /// </returns>
        public bool VerifyAssignedRole(string role)
        {
            try
            {
                var projectBasePage = new ProjectBasePage(DriverContext);

                bool role_checked = false;
                switch (role)
                {
                    case "KEY":
                        Driver.GetElement(Checkbox_SelectAdmin).ScrollToElement();
                        role_checked = Driver.GetElement(Checkbox_SelectKey).Selected;
                        //return role_checked;
                        break;
                    case "WEB":
                        Driver.GetElement(Checkbox_SelectAdmin).ScrollToElement();
                        role_checked = Driver.GetElement(Checkbox_SelectWeb).Selected;
                        //return role_checked;
                        break;
                    case "LWADM":
                        Driver.GetElement(Checkbox_SelectAdmin).ScrollToElement();
                        role_checked = Driver.GetElement(Checkbox_SelectLwa).Selected;
                        // return role_checked;
                        break;
                    case "DBA":
                        Driver.GetElement(Checkbox_SelectAdmin).ScrollToElement();
                        role_checked = Driver.GetElement(Checkbox_SelectDba).Selected;
                        //return role_checked;
                        break;
                    case "USER":
                        Driver.GetElement(Checkbox_SelectAllRoles).ScrollToElement();
                        role_checked = Driver.GetElement(Checkbox_SelectAllRoles).Selected;
                        break;
                    default:
                        return false;
                }
                return role_checked;
            }catch (Exception) { return false; }
        }

        /// <summary>
        /// Get User Name when user name is empty   
        /// </summary>
        /// <param name="userName">The user details </param> 
        /// <param name="Role">The Role details </param>        
        /// <returns>
        ///  Return User name
        /// </returns>
        public string GetUserName(string userName, string role)
        {
            if (userName.Equals(""))
            {
                string orgName = ProjectBasePage.Orgnization_value;
                string environment = ProjectBasePage.Env_value;
                return orgName + environment + role;
            }
            else
            {
                return userName;
            }
        }

        /// <summary>
        /// Create User With AllRoles   
        /// </summary>
        /// <param name="user">The user details </param>        
        /// <returns>
        ///  User is created Or Not
        /// </returns>
        public string CreateUserWithAllRoles(Users user, string orgName)
        {
            var output = "";
            if (!VerifyUserExists(user.UserName))
            {
                CreateUser(user);
                ClickEditUser_And_AssignRoles(user.UserName, Users.AdminRole.USER.ToString());
                Verify_And_Assign_Permission(orgName);

                output = " : User is created";
            }
            else
            {
                Verify_And_Assign_Role(user.UserName, Users.AdminRole.USER.ToString());
                Verify_And_Assign_Permission(orgName);
                output = " : User already exists and verified the required role and environment is assigned";
            }
            return user.UserName + output;
        }

        /// <summary>
        /// Verify Assigend Permission and Assign Correct Permission
        /// </summary>
        /// <param name="orgName">The Organization name</param>        
        /// <returns>
        ///  Required permission is assigned or not
        /// </returns>
        public void Verify_And_Assign_Permission(string orgName)
        {
            NavigateToUserDetailsTabs(UserDetailsTabs.Permissions, out string msg);
            //bool permission_status = Driver.FindElement(By.XPath("//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']/../input")).Selected;
            //if (permission_status == false)
            //{
            //    //Driver.FindElement(By.XPath("//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']/../input")).ClickElement();
            CheckBoxElmandCheck(Tree_OrgnizationFromPermissions(orgName));
            Driver.FindElement(By.XPath("//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']")).ClickElement();
            var org = Driver.FindElement(By.XPath("//*[contains(.,'User Details')]/following::span[text()='" + orgName + "']"));
            Actions action = new Actions(Driver);
            action.ContextClick(org).Build().Perform();
            Driver.GetElement(Menu_ReadWriteExecute).ClickElement();
            Click_OnButton(Button_Save);
        }


        /// <summary>
        /// Delete Adminuser  
        /// </summary>
        /// <param name="user">The Admin user details </param>        
        /// <returns>
        ///  User Deleted or Not
        /// </returns>
        public string Delete_AdminUsers(Users user)
        {
            var output = "";
            if (VerifyUserExists(user.UserName))
            {
                if (DeleteUser(user.UserName))
                {
                    output = " : User is deleted successfully";
                }
                else
                {
                    output = " : Not able to delete the user";
                }
            }
            else
            {
                output = "No User exist with this name";
            }
            return user.UserName + output;
        }

        /// <summary>
        /// Click on DeleteUser to delete  particular user
        /// </summary>
        /// <param name="username, role"> username  </param>        
        /// <returns>
        ///  returns true if  user deleted successfully else false
        /// </returns>
        public bool DeleteUser(string username)
        {
            if (VerifyUserExists(username))
            {
                Driver.GetElement(ActionButton_OfUser(username, "Delete User")).ClickElement();
                Driver.SwitchTo().Alert().Accept();
                return true;
            }
            return false;
        }
                     
        /// <summary>
        /// Deselect the Organization
        /// </summary>
        /// <param name="username, role"> orgName  </param>        
        public void Deselect_Organization(string orgName)
        {
            NavigateToUserDetailsTabs(UserDetailsTabs.Permissions, out string msg);
            CheckBoxElmandCheck(Tree_OrgnizationFromPermissions(orgName),false);
            Click_OnButton(Button_Save);
        }
    }
}
   