using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Bnp.Core.WebPages.Navigator.Admin
{
    /// <summary>
    /// This class handles Navigator Portal > Admin > Organization Page elements
    /// </summary>
    public class Navigator_Admin_OrganizationsPage : ProjectBasePage
    {
        ///<summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Admin_OrganizationsPage(DriverContext driverContext)
          : base(driverContext)
        {
        }
        public enum OrganizationsTabs
        {
            [DescriptionAttribute("Organization Details")]
            OrganizationDetails,
            [DescriptionAttribute("Environment Details")]
            EnvironmentDetails,
            [DescriptionAttribute("Framework Configuration")]
            FrameworkConfiguration,
            [DescriptionAttribute("Version History")]
            VersionHistory
        }

        #region Admin > Organization page element locators        
        private readonly ElementLocator Button_Organaization = new ElementLocator(Locator.XPath, "//span[text()='organizations']");
        private readonly ElementLocator Button_NewOrg = new ElementLocator(Locator.XPath, "//a[text()='New Org']");
        private readonly ElementLocator TextBox_OrganizationName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlOrganizations_ConfigPanel_tbOrganization')]");
        private readonly ElementLocator Button_OrganizationSave = new ElementLocator(Locator.XPath, "//a[contains(@id,'btnSaveOrg')]");
        private readonly ElementLocator Button_Users = new ElementLocator(Locator.XPath, "//span[text()='users']");
        private readonly ElementLocator Label_EnvDetails = new ElementLocator(Locator.XPath, "//h3[contains(text(),'Environment Details')]");
        private readonly ElementLocator TextBox_EnvironmentName = new ElementLocator(Locator.XPath, "//input[contains(@id,'tbEnvName')]");
        private readonly ElementLocator TextBox_EnvironmentOrder = new ElementLocator(Locator.XPath, "//input[contains(@id,'tbEnvLevel')]");
        private readonly ElementLocator Button_EnvironmentSave = new ElementLocator(Locator.XPath, "//a[contains(@id,'btnSaveEnv')]");
        private readonly ElementLocator Link_CreateNewEnvironment = new ElementLocator(Locator.XPath, "//span[contains(text(),'Create New Environment')]");
        private readonly ElementLocator Header_ManageOrganizations = new ElementLocator(Locator.XPath, "//h2[contains(text(),'Manage Organizations')]");
        private readonly ElementLocator Header_OrganizationTree = new ElementLocator(Locator.XPath, "//h3[contains(text(),'Organization Tree')]");
        private readonly ElementLocator Label_NameErrorMessage = new ElementLocator(Locator.XPath, "//span[contains(@id,'_ConfigPanel_lblOrganization')]//parent::strong//parent::td//following-sibling::td[2]//span[contains(@style,'display: inline;') or contains(@style,'visibility: visible;')]");
        private readonly ElementLocator Label_ExceptionMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'ORA-12899: value too large for column \"OE_11\".\"LN_')] ");
        private readonly ElementLocator Label_EnvNameErrorMessage = new ElementLocator(Locator.XPath, "//span[contains(@id,'EnvName')]//parent::strong//parent::td//following-sibling::td[2]//span[contains(@style,'display: inline;') or contains(@style,'visibility: visible;')]");
        private readonly ElementLocator Label_EnvExceptionMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'ORA-12899: value too large for column')]");
        #endregion

        public string OrganizationsTabsElement(string TabName)
        {
            string TabElement = "//span[text()='" + TabName + "']";
            return TabElement;
        }

        /// <summary>
        /// Gets web element locator for User details Tabs based on tab name
        /// </summary>
        /// <param name="TabName"></param>
        /// <returns>
        /// returns web element by xpath
        /// </returns>
        public ElementLocator OrganizationTabLocator(string TabName)
        {
            ElementLocator Tab_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, OrganizationsTabsElement(TabName));
            return Tab_Custom_ElementLocatorXpath;
        }
        public bool NavigateToOrganizationsTabs(OrganizationsTabs TabName, out string Message)
        {
            try
            {
                switch (TabName)
                {
                    case OrganizationsTabs.OrganizationDetails:
                        Driver.GetElement(OrganizationTabLocator(EnumUtils.GetDescription(OrganizationsTabs.OrganizationDetails))).ClickElement();
                        break;
                    case OrganizationsTabs.EnvironmentDetails:
                        Driver.GetElement(OrganizationTabLocator(EnumUtils.GetDescription(OrganizationsTabs.EnvironmentDetails))).ClickElement();
                        break;
                    case OrganizationsTabs.FrameworkConfiguration:
                        Driver.GetElement(OrganizationTabLocator(EnumUtils.GetDescription(OrganizationsTabs.FrameworkConfiguration))).ClickElement();
                        break;
                    case OrganizationsTabs.VersionHistory:
                        Driver.GetElement(OrganizationTabLocator(EnumUtils.GetDescription(OrganizationsTabs.VersionHistory))).ClickElement();
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
        /// Custom element locator for organization
        /// </summary>
        /// <param name="organizationName"></param>
        /// <returns>
        /// Returns element locator by xpath
        /// </returns>
        private static By Custom_ElementLocator_Organization(string organizationName)
        {
            return By.XPath("//div[@class='RadTreeView RadTreeView_Default']//li//span[text()='" + organizationName + "']");
        }

        /// <summary>
        /// Custom element locator for environment based on organization name
        /// </summary>
        /// <param name="organizationName"></param>
        /// <param name="envionmentName"></param>
        /// <returns>
        /// Returns element locator by xpath
        /// </returns>
        private static By Custom_ElementLocator_Environment(string organizationName, object envionmentName)
        {
            return By.XPath("//span[contains(text(),'" + organizationName + "')]//parent::div//following-sibling::ul//span[text()='" + envionmentName + "']");
        }

        /// <summary>
        /// Custom element locator for environment creation status
        /// </summary>
        /// <returns>
        /// Returns element locator by xpath
        /// </returns>
        private static By Custom_ElementLocator_EnvironmentCreationStatus()
        {
            return By.XPath("//strong[contains(text(),'Environment QA has been created successfully.')]");
        }

        /// <summary>
        /// Verify if organization exists already
        /// </summary>
        /// <param name="organizationName">Organization Name</param>
        /// <returns>
        /// Returns true if organization exists already, else false
        /// </returns>
        public bool VerifyOrganizationExists(string organizationName)
        {
            By organizationElement = Custom_ElementLocator_Organization(organizationName);
            if (Driver.IsElementPresent(organizationElement))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verify whether envionment already exists
        /// </summary>
        /// <param name="organizationName">Organization name</param>
        /// <param name="envionmentName"></param>
        /// <returns>
        /// returns true if environment exists, else false
        /// </returns>
        private bool VerifyEnvironmentExist(string organizationName, object envionmentName)
        {
            By element = Custom_ElementLocator_Environment(organizationName, envionmentName);
            if (Driver.IsElementPresent(element))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Create New Organizations   
        /// </summary>
        /// <param name="organizationName">The Organizations Name </param>        
        /// <returns>
        /// true if Organization is created successfully, else false
        /// </returns>
        public bool CreateOrganization(string organizationName, out string organizationCreationStatus)
        {
            try
            {
                organizationCreationStatus = organizationName + " already exists";
                if (!VerifyOrganizationExists(organizationName))
                {
                    Click_OnButton(Button_NewOrg);
                    Driver.GetElement(TextBox_OrganizationName).SendText(organizationName);
                    Click_OnButton(Button_OrganizationSave);
                    if (VerifyEnvironmentExist(organizationName, "Development"))
                        organizationCreationStatus = organizationName + " created with default environment Development";
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to create organization refer screenshot for more info");
            }
            return true;
        }

        /// <summary>
        /// Method to create QA environment
        /// </summary>
        /// <param name="organizationName">Organization Name</param>
        /// <param name="status">Out string status whether QA environment created or already exists</param>
        /// <returns>
        /// Returns true if QA environment created, else false
        /// </returns>
        public bool CreateQAEnvironment(string organizationName, out string status)
        {
            try
            {
                status = "Failed to create QA Environment";
                DrillDownOrg(out string Message);
                var QAEnvironment = new ElementLocator(Locator.XPath, "//span[text()='" + organizationName + "']//following::li//span[text()='QA']");
                if (Driver.IsElementNotPresent(QAEnvironment, .5))
                {
                    status = "QA Environment already exists";
                }
                else
                {
                    var orgElement = new ElementLocator(Locator.XPath, "//*[text()='" + organizationName + "']");
                    Driver.RightClick(orgElement);
                    Driver.GetElement(Link_CreateNewEnvironment).ClickElement();
                    Driver.GetElement(TextBox_EnvironmentName).SendText("QA");
                    Driver.GetElement(TextBox_EnvironmentOrder).SendText("2");
                    Click_OnButton(Button_EnvironmentSave);
                    if (Driver.IsElementPresent(Custom_ElementLocator_EnvironmentCreationStatus()))
                    {
                        status = "QA Environment created";
                    }
                    else
                    {
                        throw new Exception("Failed to create QA environment Refer screenshot for more info");
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to create QA environment Refer screenshot for more info");
            }
            return true;
        }

        /// <summary>
        /// Method to create environment
        /// </summary>
        /// <param name="organizationName">Organization Name</param>
        /// <param name="envName">Environment Name</param>
        /// <param name="status">Out string status whether QA environment created or already exists</param>
        /// <returns>
        /// Returns true if QA environment created, else false
        /// </returns>
        public bool CreateEnvironment(string organizationName, string envName, out string status)
        {
            try
            {
                status = "Failed to create Environment";
                DrillDownOrg(out string Message);
                var Environment = new ElementLocator(Locator.XPath, "//span[text()='" + organizationName + "']//following::li//span[text()='" + envName + "']");
                /* if (Driver.IsElementNotPresent(Environment, .5))
                 {
                     status = "Environment already exists";
                 }*/
                var orgElement = new ElementLocator(Locator.XPath, "//*[text()='" + organizationName + "']");
                Driver.RightClick(orgElement);
                Driver.GetElement(Link_CreateNewEnvironment).ClickElement();
                Driver.GetElement(TextBox_EnvironmentName).SendText(envName);
                Driver.GetElement(TextBox_EnvironmentOrder).SendText("2");
                Click_OnButton(Button_EnvironmentSave);
                if (Driver.IsElementPresent(By.XPath("//strong[contains(text(),'Environment " + envName + " has been created successfully.')]")))
                {
                    status = "Environment created successfully";
                }
                else if (Driver.IsElementPresent(By.XPath("//strong[text()='An environment with the name \"" + envName + "\" already exists in " + organizationName + ".']")))
                {
                    status = "Environemtnt with the given name " + envName + "already exists";
                }
                else
                {
                    throw new Exception("Failed to create environment Refer screenshot for more info");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to create environment Refer screenshot for more info");
            }
            return true;
        }

        /// <summary>
        /// Method to Verify Environment Details displayed when create new environment clicked
        /// </summary>
        /// <param name="orgName">Organization name for which create environment is being clicked</param>
        /// <returns>returns success message if verification is success else throws exception</returns>
        public string verifyEnvironmentDetailsPage(string orgName)
        {
            string output = "Environment details page didn't get displayed";
            try
            {
                var orgElement = new ElementLocator(Locator.XPath, "//*[text()='" + orgName + "']");
                Driver.RightClick(orgElement);
                Driver.GetElement(Link_CreateNewEnvironment).ClickElement();
                if (Driver.GetElement(Label_EnvDetails).Displayed)
                    return "Environment details page displayed";
                else
                    return output;
            }
            catch (Exception)
            {
                return output;
            }
        }

        /// <summary>
        /// Method to Verify Organization Page Headers
        /// </summary>
        /// <param name="status">Status string on the verification</param>
        /// <returns>returns true if verification is success else throws exception</returns>
        public bool VerifyOrganizationPageHeaders(out string status)
        {
            status = "";
            try
            {
                if (Driver.IsElementPresent(Header_ManageOrganizations, .2) && Driver.IsElementPresent(Header_OrganizationTree, .2))
                {
                    status = "Verified successfully headers Manage Organizations and Organization Tree";
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify headers Manage Organizations and Organization Tree");
            }
            return true;
        }

        /// <summary>
        /// Method to validate organization name for different input parameters
        /// </summary>
        /// <param name="value">OrganizationName value</param>
        /// <param name="message"></param>
        /// <returns>true if validation message successful else false</returns>
        public bool ValidateOrganizationName(string value, out string errorMessage)
        {
            errorMessage = "";
            try
            {
                Click_OnButton(Button_NewOrg);
                if (!value.Equals(""))
                {
                    Driver.GetElement(TextBox_OrganizationName).SendText(value);
                }
                Click_OnButton(Button_OrganizationSave);
                if (Driver.IsElementPresent(Label_NameErrorMessage, .2))//Get name error message
                {
                    Driver.GetElement(Label_NameErrorMessage).ScrollToElement();
                    errorMessage = Driver.GetElement(Label_NameErrorMessage).GetTextContent();
                    return true;
                }
                else if (Driver.IsElementPresent(Label_ExceptionMessage, .2))//Get exception message
                {
                    Driver.GetElement(Label_ExceptionMessage).ScrollToElement();
                    errorMessage = Driver.GetElement(Label_ExceptionMessage).GetTextContent();
                    return true;
                }
                else //If no error message displays then throw exception
                {
                    throw new Exception("No error message found");
                }

                throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch (Exception)
            {
                throw new Exception("Failed to validate organization name parameter Refer screenshot for more info");
            }
        }

        public bool ValidateOrgNameByGivingEmptyOrSpace(string value, out string message)
        {
            try
            {
                message = "";
                if (ValidateOrganizationName(value, out string Outmessage))
                {
                    if (Regex.IsMatch(value, "^\\s+") || value.Contains("")) //matching name for empty and space values
                    {
                        if (Outmessage.Equals("Required field.") || Outmessage.Equals("No spaces or special characters allowed, and must start with non-numeric."))
                        {
                            message = "Successfully verified error message for empty/space characters; " +
                                "Expected error message:\"Required field.\"; " +
                                "Actual error message : \"" + Outmessage + "\"";
                            return true;
                        }
                    }
                }

                throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch
            {
                throw new Exception("Failed to validate organization name parameter Refer screenshot for more info");
            }
        }

        public bool ValidateOrgNameByGivingLargeLength(string value, out string message)
        {
            message = "";
            var errorMessage = "";
            try
            {
                if (ValidateOrganizationName(value, out string Outmessage))
                {
                    if (value.Length > 255)//matching name >255 characters
                    {
                        if (Outmessage.Contains("ORA-12899: value too large for column \"OE_11\".\"LN_ORGANIZATIONS\".\"NAME\""))
                        {
                            message = "Successfully verified error message for Name more than 255 characters: " + value + " " +
                                "Expected error message:\"ORA-12899: value too large for column \"OE_11\".\"LN_ORGANIZATIONS\".\"NAME\";" +
                                ";Actual error message: " + errorMessage;
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch
            {
                throw new Exception("Failed to validate organization name parameter Refer screenshot for more info");

            }
        }

        public bool ValidateOrgNameByGivingNumericValuesOrSpaceOrSpecialCharacters(string value, out string message)
        {
            message = "";
            var errorMessage = "";
            try
            {
                if (ValidateOrganizationName(value, out string Outmessage))
                {
                    if (Regex.IsMatch(value, "^\\d+") || Regex.IsMatch(value, "^[^a-zA-Z0-9]+$") || Regex.IsMatch(value, "^[a-zA-Z0-9 ]*$"))//matching numeric, name with space and special characters
                    {
                        if (Outmessage.Equals("No spaces or special characters allowed, and must start with non-numeric."))
                        {
                            message = "Successfully verified error message for " + value + "; " +
                                "Expected error message:\"No spaces or special characters allowed, and must start with non-numeric.\";" +
                                "Actual error message : \"" + errorMessage + "\"";
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch
            {
                throw new Exception("Failed to validate organization name parameter Refer screenshot for more info");
            }
        }

        /// <summary>
        /// Method to validate environment name for different input parameters
        /// </summary>
        /// <param name="value">EnvironmentName value</param>
        /// <param name="message"></param>
        /// <returns>true if validation message successful else false</returns>
        public bool ValidateEnvironmentName(string envName, out string errorMsg)
        {
             errorMsg = "";
            try
            {
                if (!envName.Equals(""))
                {
                    Driver.GetElement(TextBox_EnvironmentName).SendText(envName);
                }
                Driver.GetElement(TextBox_EnvironmentOrder).SendText("2");
                Click_OnButton(Button_EnvironmentSave);
                if (Driver.IsElementPresent(Label_EnvNameErrorMessage, .2))//Get name error message
                {
                    Driver.GetElement(Label_EnvNameErrorMessage).ScrollToElement();
                    errorMsg = Driver.GetElement(Label_EnvNameErrorMessage).GetTextContent();
                    return true;
                }
                else if (Driver.IsElementPresent(Label_EnvExceptionMessage, .2))//Get exception message
                {
                    Driver.GetElement(Label_EnvExceptionMessage).ScrollToElement();
                    errorMsg = Driver.GetElement(Label_EnvExceptionMessage).GetTextContent();
                    return true;
                }
                else //If no error message displays then throw exception
                {
                    throw new Exception("No error message found");
                }

                throw new Exception("Failed to match error message Refer screenshot for more info");

            }
            catch (Exception)
            {
                throw new Exception("Failed to validate organization name parameter Refer screenshot for more info");
            }
        }

        public bool ValidateEnvNameByGivingLargeLength(string envName, out string msg)
        {
            msg = "";
            try
            {
                if (ValidateEnvironmentName(envName, out string Outmessage))
                {
                    if (Outmessage.Length > 255)//matching name >255 characters
                    {
                        if (Outmessage.Contains("ORA-12899: value too large for column"))
                        {
                            msg = "Successfully verified error message for Name more than 255 characters: " + Outmessage + " " +
                                "Expected error message:\"ORA-12899: value too large for column \"OE_11\".\"LN_ORGANIZATIONS\".\"NAME\";" +
                                ";Actual error message: " + Outmessage;
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch
            {
                throw new Exception("Failed to validate environment name parameter Refer screenshot for more info");

            }
        }

        public bool ValidateEnvNameByGivingNumericValuesOrSpaceOrSpecialCharacters(string envName, out string message)
        {
            message = "";
            try
            {
                if (ValidateEnvironmentName(envName, out string Outmessage))
                {
                    if (Regex.IsMatch(envName, "^\\d+") || Regex.IsMatch(envName, "^[^a-zA-Z0-9]+$") || Regex.IsMatch(envName, "^[a-zA-Z0-9 ]*$"))//matching numeric, name with space and special characters
                    {
                        if (Outmessage.Equals("No spaces or special characters allowed, and must start with non-numeric."))
                        {
                            message = "Successfully verified error message for " + envName + "; " +
                                "Expected error message:\"No spaces or special characters allowed, and must start with non-numeric.\";" +
                                "Actual error message : \"" + message + "\"";
                            return true;
                        }
                    }
                }
                    throw new Exception("Failed to match error message Refer screenshot for more info");
            }
            catch
            {
                throw new Exception("Failed to validate environment name parameter Refer screenshot for more info");

            }
        }
    }
}
