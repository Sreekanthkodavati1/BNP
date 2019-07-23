using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Extensions;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > User Administration > Roles Page elements
    /// </summary>
    public class CSPortal_UserAdministration_RolePage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_UserAdministration_RolePage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        /// <summary>
        /// Enum for Role Functions
        /// </summary>
        public enum RoleFunctions
        {
            [DescriptionAttribute("User Administration")]
            UserAdministration,
            [DescriptionAttribute("Change Account Status")]
            ChangeAccountStatus,
            [DescriptionAttribute("Update Account")]
            UpdateAccount,
            [DescriptionAttribute("Allow Point Award")]
            AllowPointAward,
            [DescriptionAttribute("Search Member")]
            SearchMember,
            [DescriptionAttribute("View Member Profile")]
            ViewMemberProfile,
            [DescriptionAttribute("View Transaction History")]
            ViewTransactionHistory,
            [DescriptionAttribute("View Reward History")]
            ViewRewardHistory
        }

        #region Element Locators
        private readonly ElementLocator Button_AddRole = Administration_Button_Custom_ElementLocatorXpath("Add Role");
        private readonly ElementLocator Button_Save = Administration_Button_Custom_ElementLocatorXpath("Save");
        private readonly ElementLocator Button_Cancel = Administration_Button_Custom_ElementLocatorXpath("Cancel");
        private readonly ElementLocator TextBox_Name = Administration_TextBox_Custom_ElementLocatorXpath("Name");
        private readonly ElementLocator TextBox_PointAwardLimit = Administration_TextBox_Custom_ElementLocatorXpath("Point Award Limit");
        private readonly ElementLocator TextArea_RoleDescription = Administration_TextArea_Custom_ElementLocatorXpath("Description");

        #endregion

        /// <summary>
        /// Gets the function check box element locator based on the function name 
        /// </summary>
        /// <param name="functionName">Function name</param>
        /// <returns>
        /// String representation of function name element locator
        /// </returns>
        private static string Custom_CheckBox_ElementLocator(string functionName)
        {
            return "//td//label[text()='" + functionName + "']//preceding-sibling::input";
        }

        /// <summary>
        /// Gets the following locator element based on role name
        /// </summary>
        /// <param name="followingTextToVerify">following element locator</param>
        /// <param name="roleElement">role name</param>
        /// <returns></returns>
        private static string Custom_ElementLocator_FollowingRoleName(string roleElement, string followingTextToVerify)
        {
            return roleElement + "//parent::td//following-sibling::td//span[contains(text(),'" + followingTextToVerify + "')]";
        }

        /// <summary>
        /// Gets the role name element locator based on role name
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        private static string Custom_ElementLocator_RoleName(string role)
        {
            return "//span[text()='" + role + "']";
        }

        /// <summary>
        /// Verify role name exists on the Roles page
        /// </summary>
        /// <param name="roleElement">Xpath of the role element</param>
        /// <returns>
        /// returns true if Role Name match for the role else false
        /// </returns>
        private bool VerifyRoleName(string roleElement)
        {
            return Driver.IsElementPresent(By.XPath(roleElement));
        }

        /// <summary>
        /// Verify Point Award Limit exists on the Roles page
        /// </summary>
        /// <param name="followingElement">Xpath ofPoint Award Limit element</param>
        /// <returns>
        /// returns true if Role Name following element Point Award Limit match for the role, else false
        /// </returns>
        private bool VerifyRoleNameFollowoingElements(string followingElement)
        {
            return Driver.IsElementPresent(By.XPath(followingElement));
        }

        /// <summary>
        /// Verify function names exists based on role name
        /// </summary>
        /// <param name="role"></param>
        /// <returns>
        /// returns false if any function name fails to match else true
        /// </returns>
        public bool VerifyFunctionNames(string role, string roleElement)
        {
            string functionDescription;
            string functionLocator;
            if (role.Equals(RoleValue.Admin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    functionDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_ElementLocator_FollowingRoleName(roleElement, functionDescription);
                    if (!VerifyRoleNameFollowoingElements(functionLocator))
                        return false;
                }
            }
            else if (role.Equals(RoleValue.SrAdmin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    functionDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_ElementLocator_FollowingRoleName(roleElement, functionDescription);
                    if (!function.ToString().Equals(RoleFunctions.UserAdministration.ToString()))
                    {
                        if (!VerifyRoleNameFollowoingElements(functionLocator))
                            return false;
                    }
                }
            }
            else if (role.Equals(RoleValue.JrAdmin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    functionDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_ElementLocator_FollowingRoleName(roleElement, functionDescription);
                    if (!function.ToString().Equals(RoleFunctions.UserAdministration.ToString()) && !function.ToString().Equals(RoleFunctions.ChangeAccountStatus.ToString()) && !function.ToString().Equals(RoleFunctions.UpdateAccount.ToString()))
                    {
                        if (!VerifyRoleNameFollowoingElements(functionLocator))
                            return false;
                    }
                }
            }
            else
            {
                throw new Exception("Failed to match role function");
            }
            return true;
        }

        /// <summary>
        /// Verify User Administration role exists or not
        /// </summary>
        /// <param name="role">role to verify</param>
        /// <param name="pointAwardLimit">Point Award Limit value</param>
        /// <returns> returns true if Role Name,Point Award Limit and functions match for the role, else false
        /// </returns>
        public bool VerifyRoleExists(string role, string pointAwardLimit)
        {
            string roleElement = Custom_ElementLocator_RoleName(role);
            string pointAwardLimitElement = Custom_ElementLocator_FollowingRoleName(roleElement, pointAwardLimit);
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
                    if (VerifyRoleName(roleElement))
                    {
                        if (VerifyRoleNameFollowoingElements(pointAwardLimitElement) && VerifyFunctionNames(role, roleElement))
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                if (VerifyRoleName(roleElement) && VerifyRoleNameFollowoingElements(pointAwardLimitElement) && VerifyFunctionNames(role, roleElement))
                {
                    return true;
                }
            }
            return false;
        }

        ElementLocator RoleWithFunctions(string Role, string Function)
        {
            ElementLocator _RoleWithFunctions = new ElementLocator(Locator.XPath, "//span[contains(@id,'CSRolesName') and contains(text(),'" + Role + "')]//..//..//span[contains(text(),'" + Function + "') and contains(@id,'CSRolesAssociatedFunctions')]");
            return _RoleWithFunctions;
        }
        ElementLocator RoleWithActions(string Role, string Action)
        {
            ElementLocator _RoleWithFunctions = new ElementLocator(Locator.XPath, "//span[contains(@id,'CSRolesName') and contains(text(),'"+Role+"')]//..//..//div//a[text()='"+Action+"']");
            return _RoleWithFunctions;
        }
        public bool VerifyRoleExists(string role, string functions, out string Message)
        {
            if (VerifyElementandScrollToElement(RoleWithFunctions(role, functions)))
            {
                Message = "Role Existed And Details are;Role:" + role + ";Functions:" + functions;
                return true;
            }
            throw new Exception("Failed to Create Rule Refere Screenshot for more details");
        }
 
        /// <summary>
        /// To create new role if it doesnot exists
        /// </summary>
        /// <param name="role">role name</param>
        /// <param name="pointAwardLimit">integer value</param>
        /// <param name="roleStatus"></param>
        /// <returns>
        ///  returns status of role creation as true if created else false
        ///  returns status of role creation as created or exists
        /// </returns>
        public bool CreateNewRole(string role, string pointAwardLimit, out string roleStatus)
        {
            bool status = true;
            roleStatus = "Role " + role + " already exists";
            try
            {
                if (!VerifyRoleExists(role, pointAwardLimit))
                {
                    Click_OnButton(Button_AddRole);
                    EnterRoleValues(role, pointAwardLimit);
                    Click_OnButton(Button_Save);
                    if (VerifyRoleExists(role, pointAwardLimit))
                    {
                        roleStatus = "Role " + role + " created successfully";
                    }
                    else
                    {
                        throw new Exception("Failed to create new Role " + role);
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to create new Role " + role);
            }
            return status;
        }

        /// <summary>
        /// Enter Role field values
        /// </summary>
        /// <param name="role">Role Name</param>
        /// <param name="pointAwardLimit">Point Award Limit</param>
        private void EnterRoleValues(string role, string pointAwardLimit)
        {
            Driver.GetElement(TextBox_Name).SendText(role);
            Driver.GetElement(TextBox_PointAwardLimit).SendText(pointAwardLimit);
            SelectFunctions(role);
        }

        /// <summary>
        /// Enter Role field values
        /// </summary>
        /// <param name="role">Role Name</param>
        /// <param name="pointAwardLimit">Point Award Limit</param>
        private void EnterRoleValues(string role,string Description, string pointAwardLimit)
        {
            Driver.GetElement(TextBox_Name).SendText(role);
            Driver.GetElement(TextArea_RoleDescription).SendText(Description);
            Driver.GetElement(TextBox_PointAwardLimit).SendText(pointAwardLimit);
        }
        public void SelectRole(RoleFunctions Functions)
        {
            try
            {
                string enumDescription;
                enumDescription = EnumUtils.GetDescription(Functions);
                ElementLocator FuncionCheckbox=new ElementLocator(Locator.XPath, Custom_CheckBox_ElementLocator(enumDescription));
                CheckBoxElmandCheck(FuncionCheckbox);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to select Functions Due to:" + e.Message);

            }
        }

        public bool CreateRole(string RoleName, string RolePoints, RoleFunctions Functions, out string Message)
        {
            if (VerifyElementandScrollToElement(RoleWithFunctions(RoleName, EnumUtils.GetDescription(Functions).ToString())))
            {
                return VerifyRoleExists(RoleName, EnumUtils.GetDescription(Functions).ToString(), out Message);
            }
            Click_OnButton(Button_AddRole);
            EnterRoleValues(RoleName,RoleName+"_Desc", RolePoints);
            SelectRole(Functions);
            Click_OnButton(Button_Save);
            return VerifyRoleExists(RoleName, EnumUtils.GetDescription(Functions).ToString(), out Message);

        }

        public bool DeleteRole(string RoleName, RoleFunctions Functions, out string Message)
        {
            string CreateRoleMsg = "";
            if (!VerifyElementandScrollToElement(RoleWithActions(RoleName, "Delete")))
            {
                CreateRole(RoleName, "", Functions, out Message);
                CreateRoleMsg = Message+"And Used to Test Delete Functionality";
            }

            Click_OnButton(RoleWithActions(RoleName, "Delete"));
            Driver.SwitchTo().Alert().Accept();
            if (!VerifyElementandScrollToElement(RoleWithActions(RoleName, "Delete")))
            {
                Message = CreateRoleMsg+";Role Deleted Successfully ; Role Details:" + RoleName;
                return true;
            }

            throw new Exception("Failed to Delete User:");
        }

        /// <summary>
        /// Select role functions based on role type
        /// </summary>
        /// <param name="role">role name</param>
        public void SelectFunctions(string role)
        {
            string enumDescription;
            string functionLocator;

            if (role.Equals(RoleValue.Admin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    enumDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_CheckBox_ElementLocator(enumDescription);
                    Driver.FindElement(By.XPath(functionLocator)).ClickElement();
                }
            }
            else if (role.Equals(RoleValue.SrAdmin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    enumDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_CheckBox_ElementLocator(enumDescription);
                    if (!function.ToString().Equals(RoleFunctions.UserAdministration.ToString()))
                    {
                        Driver.FindElement(By.XPath(functionLocator)).ClickElement();
                    }
                }
            }
            else if (role.Equals(RoleValue.JrAdmin))
            {
                foreach (Enum function in Enum.GetValues(typeof(RoleFunctions)))
                {
                    enumDescription = EnumUtils.GetDescription(function);
                    functionLocator = Custom_CheckBox_ElementLocator(enumDescription);
                    if (!function.ToString().Equals(RoleFunctions.UserAdministration.ToString()) && !function.ToString().Equals(RoleFunctions.ChangeAccountStatus.ToString()) && !function.ToString().Equals(RoleFunctions.UpdateAccount.ToString()))
                    {
                        Driver.FindElement(By.XPath(functionLocator)).ClickElement();
                    }
                }
            }
            else
            {
                throw new Exception("Failed to match role");
            }
        }

        public TestStep CreateRole(string RoleName, string RolePoints, RoleFunctions Functions, List<TestStep> listOfTestSteps)
        {
            string stepName = "Create New Role Under User Management";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {

                CreateRole(RoleName, RolePoints,Functions,out string Message);
                testStep.SetOutput(Message);
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
        public TestStep DeleteRole(string RoleName, string RolePoints, RoleFunctions Functions, List<TestStep> listOfTestSteps)
        {
            string stepName = "Deleting Role Under User Management";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                DeleteRole(RoleName, Functions, out string Message);
                testStep.SetOutput(Message);
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