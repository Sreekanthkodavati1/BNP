using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Website
{
    /// <summary>
    /// This class handles Navigator > Users > Website > Modules Page elements
    /// </summary>
    public class Navigator_Users_Website_ModulesPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Website_ModulesPage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Modules Page Locators
        private readonly ElementLocator ModulesTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Modules')]");
        private readonly ElementLocator Website = new ElementLocator(Locator.XPath, "//select[contains(@name,'WebsiteFilter')]");
        private readonly ElementLocator ModuleType = new ElementLocator(Locator.XPath, "//select[contains(@name,'ModuleTypeFilter')]");
        private readonly ElementLocator ModuleTypeValue = new ElementLocator(Locator.XPath, "//select//option[contains(text(),'Member Profile')]");
        private readonly ElementLocator CSMemberReg_Configurebutton = new ElementLocator(Locator.XPath, "//td[text()='CSMemberRegistration_Config']//parent::tr//td//a[contains(text(),'Configure')]");
        private readonly ElementLocator CSCouponAppeasement_Configurebutton = new ElementLocator(Locator.XPath, "//td[text()='CSCouponAppeasement_Config']//parent::tr//td//a[contains(text(),'Configure')]");
        private readonly ElementLocator Button_CFUpdateProfile_Config = new ElementLocator(Locator.XPath, "//td[text()='CFUpdateProfile_Config']//parent::tr//td//a[contains(text(),'Configure')]");
        private readonly ElementLocator OfficeFax1 = new ElementLocator(Locator.XPath, "//span[contains(text(),'OfficeFax')]");
        private readonly ElementLocator WorkPhone1 = new ElementLocator(Locator.XPath, "//td//a[text()='Country']//parent::td");
        private readonly ElementLocator Button_SaveConfig = new ElementLocator(Locator.XPath, "//div[contains(@id,'_EditPanel')]//a[text()='Save']");
        private readonly ElementLocator Select_ModuleType = new ElementLocator(Locator.XPath, "//select[contains(@id,'_ddlModuleTypeFilter')]");
        private readonly ElementLocator Select_Website = new ElementLocator(Locator.XPath, "//select[contains(@id,'_ddlWebsiteFilter')]");
        private readonly ElementLocator CheckBox_Facebook = Administration_CheckBox_Custom_ElementLocatorXpath(SocialMedia.Facebook.ToString());
        private readonly ElementLocator CheckBox_Twitter = Administration_CheckBox_Custom_ElementLocatorXpath(SocialMedia.Twitter.ToString());
        private readonly ElementLocator CheckBox_GooglePlus = Administration_CheckBox_Custom_ElementLocatorXpath(EnumUtils.GetDescription(SocialMedia.GooglePlus));
        private readonly ElementLocator Label_ShareList = new ElementLocator(Locator.XPath, "//legend[contains(text(),'\"Share\" List Command Settings')]");
        private readonly ElementLocator Label_DropArea = new ElementLocator(Locator.XPath, "//div[contains(@id,'DataPicker_DropArea')]//div[2]");

        private readonly ElementLocator Label_AddNewConfig = new ElementLocator(Locator.XPath, "//h2[text()='Add New Module Configuration']");
        private readonly ElementLocator Button_AddNew = new ElementLocator(Locator.XPath, "//a[text()='Add New']");
        private readonly ElementLocator Select_Module = new ElementLocator(Locator.XPath, "//select[contains(@id,'ddlAddConfigModule')]");
        private readonly ElementLocator Input_ConfigName = new ElementLocator(Locator.XPath, "//input[contains(@id, 'txtAddConfigName')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[contains(@id, 'SaveConfiguration')]");
        private readonly ElementLocator Input_StartDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtStartDate_dateInput')]");
        private readonly ElementLocator Input_EndDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtEndDate_dateInput')]");
        private readonly ElementLocator Label_Name = new ElementLocator(Locator.XPath, "//span[text()='Tier']/../following-sibling::ul//span[text()='Name']");
        private readonly ElementLocator Label_Add = new ElementLocator(Locator.XPath, "//span[text()='Add']");
        private readonly ElementLocator Label_EditConfig = new ElementLocator(Locator.XPath, "//h2[contains(text(),'Edit Configuration')]");
        private readonly ElementLocator Input_ProviderClass = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtProviderClass')]");
        private readonly ElementLocator Input_ProviderAssembly = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtProviderAssembly')]");
        private readonly ElementLocator Button_SaveConfiguration = new ElementLocator(Locator.XPath, "//a[contains(@id,'cmdSave')]");
        private readonly ElementLocator Input_SpecificMTouch = new ElementLocator(Locator.XPath, "//input[contains(@name,'SpecificMTouch')]");
        private readonly ElementLocator Button_SaveInEditConfig = new ElementLocator(Locator.XPath, "//a[contains(@id,'lnkSave')]");
        private readonly ElementLocator Button_Update = new ElementLocator(Locator.XPath, "//a[text()='Update']");
        private readonly ElementLocator Input_FogetPasswordConfigName = new ElementLocator(Locator.XPath, "//td[contains(text(),'Configuration Name:')]//parent::tr//following-sibling::input");

        //Activation Module Elements
        private readonly ElementLocator CheckBox_Username = new ElementLocator(Locator.XPath, "//div[contains(@id,'RequiredFieldList')]//input[contains(@id,'Username')]");
        private readonly ElementLocator Input_SuccessPage = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtRouteUrl')]");
        private readonly ElementLocator Input_MaxAccountAge = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtMaxAge')]");
        private readonly ElementLocator Input_PhoneNumberMask = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtMask')]");
        #endregion

        public enum ModuleTypeList
        {
            [DescriptionAttribute("Coupons - List View")]
            CouponsListView,
            [DescriptionAttribute("Reward History - List View")]
            RewardHistoryListView,
            [DescriptionAttribute("Member Profile")]
            MemberProfile,
            [DescriptionAttribute("Customer Service - Award Coupons")]
            CustomerServiceAwardCoupons,
            [DescriptionAttribute("Tier History")]
            TierHistory
        }
        public enum ConfigNames
        {
            CFCoupons_Config,
            CFRewardsHistory_Config,
            CSMemberRegistration_Config,
            CSCouponAppeasement_Config
        }
        public enum SocialMedia
        {
            Facebook,
            Twitter,
            [DescriptionAttribute("Google Plus")]
            GooglePlus
        }

        /// <summary>
        /// To select website and moduletype to filter website configuration
        /// </summary>
        /// <param name="Website">Filter based on website</param>
        /// <param name="ModuleType">Filter based on ModuleType</param>
        public void Website_Select_WebsiteAndModuleType(string Website, string ModuleType)
        {
            try
            {
                Driver.GetElement(this.Website).ClickElement();
                SelectElement_AndSelectByText(this.Website, Website);
                Thread.Sleep(1000);
                SelectElement_AndSelectByText(this.ModuleType, ModuleType);
                Thread.Sleep(1000);
            }
            catch (Exception) { throw new Exception("Failed to Select Website:" + Website + "And Module Type:" + ModuleType); }
        }

        public ElementLocator ConfigurationName(string moduleType, string configName)
        {
            ElementLocator _ConfigurationName = new ElementLocator(Locator.XPath, "//td[text()='" + moduleType + "']/following-sibling::td[text()='" + configName + "']");
            return _ConfigurationName;
        }

        public ElementLocator ActionOnConfigName(string moduleType, string ConfigName, string Action)
        {
            ElementLocator _ActionConfigName = new ElementLocator(Locator.XPath, "//td[text()='" + moduleType + "']/following-sibling::td[text()='" + ConfigName + "']/../td//a[text()='" + Action + "']");
            return _ActionConfigName;
        }

        /// <summary>
        /// Search for website configuration using website and module type, cancel if exists
        /// </summary>
        /// <param name="Website">Filter based on website</param>
        /// <param name="ModuleType">Filter based on ModuleType</param>
        /// <param name="ConfigurationName">Configuration Name which is to be cancelled</param>
        public bool CancelConfiguration_IfExists(string Website, string ModuleType, string configName, out string Message)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(Website, ModuleType);
                if (VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                {
                    Driver.GetElement(ActionOnConfigName(ModuleType, configName, "Cancel")).ClickElement();
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to "))
                    {
                        Driver.SwitchTo().Alert().Accept();
                    }
                    if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                    {
                        Message = "Configuration Deleted Successfully and config Details are:" + configName;
                        return true;
                    }
                }
                Message = "No configuration available with name: " + configName;
                return true;
            }
            catch (Exception) { throw new Exception("Failed to Select Website:" + Website + "And Module Type:" + ModuleType); }
        }

        /// <summary>
        /// Search for website configuration using website and module type, create if not exists
        /// </summary>
        /// <param name="Website">Filter based on website</param>
        /// <param name="ModuleType">Filter based on ModuleType</param>
        /// <param name="ConfigurationName">Configuration Name which is to be cancelled</param>
        public bool CreateConfigurationIfNotExists(string Website, string ModuleType, string configName, string startDate, string endDate, string providerClass, string providerAssembly, out string Message)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(Website, ModuleType);
                if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                {
                    Driver.GetElement(Button_AddNew).ClickElement();
                    EnterConfigDetails(ModuleType, configName, startDate, endDate, providerClass, providerAssembly);
                    Driver.GetElement(Button_SaveConfiguration).ClickElement();
                    if (!Driver.IsElementPresent(Label_AddNewConfig, .5))
                    {
                        Message = "Configuration Created Successfully and config Details are:" + configName;
                        return true;
                    }
                }
                Message = "Configuration is already available with name: " + configName;
                return true;
            }
            catch (Exception) { throw new Exception("Failed to create configuration, refer the screenshot for more details"); }
        }

        /// <summary>
        /// Search for website configuration using website and module type, create if not exists
        /// </summary>
        /// <param name="Website">Filter based on website</param>
        /// <param name="ModuleType">Filter based on ModuleType</param>
        /// <param name="ConfigurationName">Configuration Name which is to be cancelled</param>
        public bool CreateActivationConfigurationIfNotExists(string Website, string ModuleType, string configName, string requiredfield, string successPage, string maxAccountAge, string phoneNumberMask, out string Message)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(Website, ModuleType);
                if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                {
                    Driver.GetElement(Button_AddNew).ClickElement();
                    EnterConfigDetails_ActivationConfiguration(ModuleType, configName, requiredfield, successPage, maxAccountAge, phoneNumberMask);
                    Driver.GetElement(Button_Update).ClickElement();
                    if (!Driver.IsElementPresent(Label_AddNewConfig, .5))
                    {
                        Message = "Configuration Created Successfully and config Details are:" + configName;
                        return true;
                    }
                }
                Message = "Configuration is already available with name: " + configName;
                return true;
            }
            catch (Exception) { throw new Exception("Failed to create configuration, refer the screenshot for more details"); }
        }

        public bool VerifyModulePresentInTheGrid(string website, string modulyeType, string moduleName)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(website, modulyeType);
                if (VerifyElementandScrollToElement(ConfigurationName(modulyeType, moduleName)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to verify the presence of module, refer the screenshot for more details");
            }
        }
        public void EnterConfigDetails(string module, string configName, string startDate, string endDate, string providerClass, string providerAssembly)
        {
            try
            {
                if (Driver.GetElement(Label_AddNewConfig).IsElementPresent())
                {
                    SelectElement_AndSelectByText(Select_Module, module);
                    Driver.GetElement(Input_ConfigName).SendText(configName);
                    Driver.GetElement(Button_Save).ClickElement();
                    if (Driver.GetElement(Label_EditConfig).Displayed)
                    {
                        Driver.GetElement(Input_StartDate).SendText(startDate);
                        Driver.GetElement(Input_EndDate).SendText(endDate);
                        Actions action = new Actions(Driver);
                        action.ContextClick(Driver.GetElement(Label_Name)).Build().Perform();
                        Driver.GetElement(Label_Add).ClickElement();
                        Driver.GetElement(Input_ProviderClass).SendText(providerClass);
                        Driver.GetElement(Input_ProviderAssembly).SendText(providerAssembly);
                    }
                    else
                    {
                        throw new Exception("Edit Configuration panel didn't open, refer the sreenshot for more details");
                    }
                }
                else
                {
                    throw new Exception("Add new Configuration panel didn't open, refer the screenshot for more details.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to enter configuration details");
            }
        }
        public void EnterConfigDetails_ActivationConfiguration(string module, string configName, string requiredFields, string successPage, string accountAge, string phoneNumberMask)
        {
            try
            {
                if (Driver.GetElement(Label_AddNewConfig).IsElementPresent())
                {
                    SelectElement_AndSelectByText(Select_Module, module);
                    Driver.GetElement(Input_ConfigName).SendText(configName);
                    Driver.GetElement(Button_Save).ClickElement();
                    if (Driver.GetElement(Label_EditConfig).Displayed)
                    {
                        Driver.GetElement(CheckBox_Username).ClickElement();
                        Driver.GetElement(Input_SuccessPage).SendText(successPage);
                        Driver.GetElement(Input_MaxAccountAge).SendText(accountAge);
                        Driver.GetElement(Input_PhoneNumberMask).SendText(phoneNumberMask);
                    }
                    else
                    {
                        throw new Exception("Edit Configuration panel didn't open, refer the sreenshot for more details");
                    }
                }
                else
                {
                    throw new Exception("Add new Configuration panel didn't open, refer the screenshot for more details.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to enter configuration details");
            }
        }

        /// <summary>
        /// To click CSMemberReg Configure button
        /// </summary>
        public void CSMemberRegConfigurebutton()
        {
            try
            {
                Driver.GetElement(CSMemberReg_Configurebutton).ClickElement();
            }
            catch (Exception) { throw new Exception("Failed to Select CSMemberReg_Config File "); }
        }

        /// <summary>
        /// To click on CSCouponAppeasement Configure button
        /// </summary>
        public void CSCouponAppeasementConfigurebutton()
        {
            try
            {
                Driver.GetElement(CSCouponAppeasement_Configurebutton).ClickElement();
            }
            catch (Exception) { throw new Exception("Failed to Select CSCouponAppeasement Config File "); }
        }

        public void DragandDropAttibuteSet(string _SourceElement, string _TargetElement)
        {
            try
            {
                By ElementAvailable_AtTargetLocation = By.XPath("//td//a[text()='" + _SourceElement + "']//parent::td");
                if (Driver.IsElementPresent(ElementAvailable_AtTargetLocation))
                {//Element Already Available at Target Location
                }
                else
                {
                    By SourceElement = By.XPath("//span[contains(text(),'" + _SourceElement + "')]");
                    By TargetElement = By.XPath("//td//a[text()='" + _TargetElement + "']//parent::td");
                    Driver.FindElement(SourceElement).ScrollToElement();
                    IWebElement Sourceelement = Driver.FindElement(SourceElement);
                    IWebElement Targetelement = Driver.FindElement(TargetElement);
                    Actions Sourcebuilder = new Actions(Driver);
                    Sourcebuilder.DragAndDrop(Sourceelement, Targetelement);
                    IAction dragAndDrop = Sourcebuilder.ClickAndHold(Sourceelement)
                   .MoveToElement(Targetelement)
                   .Release(Targetelement)
                   .Build();
                    dragAndDrop.Perform();
                }
            }
            catch (Exception e) { throw new Exception("Failed to Config the Attributes and Attribute Sets"); }
        }

        /// <summary>
        /// To verify Created Coupon in CSCouponAppeasementConfig file
        /// </summary>
        /// <param name="CouponName">Coupon name to be verified</param>
        public bool VerifyCouponInCSCouponAppeasementConfig(string CouponName)
        {

            if (Driver.IsElementPresent(By.XPath("//label[contains(text(),'" + CouponName + "')]")))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To add Coupon in CSCouponAppeasementConfig file
        /// </summary>
        /// <param name="CouponName">Coupon name to be added</param>
        public void AddCouponInCSCouponAppeasementConfig(string CouponName)
        {
            try
            {
                if (VerifyCouponInCSCouponAppeasementConfig(CouponName))
                {
                    ElementLocator Coupon = new ElementLocator(Locator.XPath, "//label[text()='" + CouponName + "']//preceding-sibling::input");
                    Driver.GetElement(Coupon).ClickElement();
                }
            }
            catch (Exception e) { throw new Exception("Failed to add Coupon in CSCouponAppeasementConfig"); }
        }

        public void SaveConfigSetting()
        {
            try
            {
                Driver.GetElement(Button_SaveConfig).ClickElement();
            }
            catch (Exception) { throw new Exception("Failed to Save config Settings"); }
        }

        /// <summary>
        /// To add Account status Attribute Name in CFUpdateProfile_Config file
        /// </summary>
        /// <param name="CouponName">Coupon name to be added</param>
        public string AddAttributeInCFUpdateProfile_Config(string Source, string Target)
        {
            string status = Target + " Added Successfully to CFUpdateProfile_Config file ";
            try
            {
                Click_OnButton(Button_CFUpdateProfile_Config);
                if (VerifyAttributeNameInDropArea(Source))
                {
                    if (DeleteAttributeFromFile(Source))
                    {
                        DragandDropAttibuteSet(Source, Target);
                        if (VerifyAttributeNameInDropArea(Source))
                        {
                            return status;
                        }
                    }
                }
                else
                {
                    DragandDropAttibuteSet(Source, Target);
                    if (VerifyAttributeNameInDropArea(Source))
                    {
                        return status;
                    }
                }
                throw new Exception("Failed to add Attribute Name in CFUpdateProfile_Config file");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to add Attribute Name in CFUpdateProfile_Config file");

            }
        }

        public string VerifyAttributeInQAEnv(string Source)
        {
            string status = Source + " is Added Successfully to CFUpdateProfile_Config file ";
            try
            {
                Click_OnButton(Button_CFUpdateProfile_Config);
                if (VerifyAttributeNameInDropArea(Source))
                {
                    status = Source + " is exists in CFUpdateProfile_Config file";
                }
                else
                {
                    status = Source + " is Not exists in CFUpdateProfile_Config file";
                }
                return status;
            }
            catch (Exception e) { throw new Exception("Failed to Verify added " + Source + " Attribute Name in CFUpdateProfile_Config file"); }
        }

        public bool DeleteAttributeFromFile(string source)
        {
            {
                Click_OnButton(Button_CFUpdateProfile_Config);
                IList<IWebElement> elements = Driver.FindElements(By.XPath("//a[text()='" + source + "']//parent::td//following-sibling::td[1]"));
                for (int index = 0; index < elements.Count; index++)
                {
                    IWebElement element = Driver.FindElement(By.XPath("//a[text()='" + source + "']//parent::td//following-sibling::td[1]"));
                    element.ClickElement();
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Delete this attribute?"))
                    {
                        Driver.SwitchTo().Alert().Accept();
                        if (!VerifyAttributeNameInDropArea(source))
                        {
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to Delete  Attribute Name from CFUpdateProfile_Config file");
            }
        }

        public bool VerifyAttributeNameInDropArea(string AccountStatus)
        {

            string DropArea = Driver.GetElement(Label_DropArea).GetTextContent();
            if (DropArea.Contains(AccountStatus))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to filter the search by module type, website and config name
        /// </summary>
        /// <param name="moduleType">module type name</param>
        /// <param name="website">CS or MP website name</param>
        /// <param name="configName">config name</param>
        /// <param name="message">outputs message if finds the config name after filtering by module type and website else throws exception</param>
        /// <returns>true if finds the config name after filtering by module type and website else throws exception</returns>
        public bool FilterByModuleTypeAndWebsite(string moduleType, string website, string configName, out string message)
        {
            try
            {
                Select selWebsite = new Select(Driver.GetElement(Select_Website));
                selWebsite.SelectByText(website);
                Thread.Sleep(1000);
                Select selModuleType = new Select(Driver.GetElement(Select_ModuleType));
                selModuleType.SelectByText(moduleType);
                Thread.Sleep(1000);
                if (Driver.IsElementPresent(By.XPath("//td[contains(text(),'" + configName + "')]")))
                {
                    message = "Filtered by Website: " + website + " and Module Type: " + moduleType + " successfully";
                    return true;
                }
                throw new Exception("Failed to filter by Website: " + website + " and Module Type: " + moduleType);
            }
            catch (Exception)
            {
                throw new Exception("Failed to filter by Website: " + website + " and Module Type: " + moduleType);
            }
        }

        /// <summary>
        /// Method to click on configure button for the selected config name
        /// </summary>
        /// <param name="configName"></param>
        /// <returns>true if click successful else throws exception</returns>
        public bool OpenConfigurePage(string configName)
        {
            try
            {
                ElementLocator Button_Config = new ElementLocator(Locator.XPath, "//td[text()='" + configName + "']//parent::tr//td//a[contains(text(),'Configure')]");
                Click_OnButton(Button_Config);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to open configure page: " + configName);
            }
            throw new Exception("Failed to open configure page: " + configName);
        }

        /// <summary>
        /// Method to select the social media icons for rewards and coupons
        /// </summary>
        /// <returns>true if select successful else throws exception</returns>
        public bool SelectAllSocialMedia()
        {
            try
            {
                if (Driver.IsElementPresent(Label_ShareList, .5))
                {
                    Driver.ScrollIntoMiddle(Label_ShareList);
                    CheckBoxElmandCheck(CheckBox_Facebook);
                    CheckBoxElmandCheck(CheckBox_Twitter);
                    CheckBoxElmandCheck(CheckBox_GooglePlus);
                    Click_OnButton(Button_SaveConfig);
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to enable social media");
            }
            throw new Exception("Failed to enable social media");
        }

        /// <summary>
        /// Method to navigate and enable the social media checkboxes for rewards and coupons
        /// </summary>
        /// <param name="moduleType">module type name</param>
        /// <param name="website">CS or MP website name</param>
        /// <param name="configName">config name</param>
        /// <param name="message">outputs message if finds the config name after filtering by module type and website else throws exception</param>
        /// <returns>true if enable successful else throws exception</returns>
        public bool EnableSocialMedia(string moduleType, string website, string configName, out string message)
        {
            try
            {
                FilterByModuleTypeAndWebsite(moduleType, website, configName, out string stroutput);
                OpenConfigurePage(configName);
                SelectAllSocialMedia();
                message = "Successfully selected social media for Website: " + website + " ModuleType: " + moduleType + "Configuration Name:" + configName;
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to enable social media");
            }
            throw new Exception("Failed to enable social media");
        }

        /// <summary>
        /// Enter values to fill the details of MTouchConfiguration details
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="specificMTouch"></param>
        public void EnterMTouchModuleConfigDetails(string configName,string specificMTouch)
        {
            try
            {
                if (Driver.GetElement(Label_AddNewConfig).IsElementPresent())
                {

                    Driver.GetElement(Input_ConfigName).SendText(configName);
                    Driver.GetElement(Button_Save).ClickElement();
                    if (Driver.GetElement(Label_EditConfig).Displayed)
                    {
                        Driver.GetElement(Input_SpecificMTouch).SendText(specificMTouch);
                    }
                    else
                    {
                        throw new Exception("Edit Configuration panel didn't open, refer the sreenshot for more details");
                    }
                }
                else
                {
                    throw new Exception("Add new Configuration panel didn't open, refer the screenshot for more details.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to enter configuration details");
            }
        }

        /// <summary>
        /// Create New Webiste Configuration For MtouchModule if does not exist
        /// </summary>
        /// <param name="Website"></param>
        /// <param name="ModuleType"></param>
        /// <param name="configName"></param>
        /// <param name="specificMTouch"></param>
        /// <param name="module"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool CreateNewWebsiteConfigurationForMTouchModuleIfNotExists(string Website, string ModuleType, string configName, string specificMTouch, string module, out string Message)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(Website, ModuleType);
                if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                {
                    Driver.GetElement(Button_AddNew).ClickElement();
                    EnterMTouchModuleConfigDetails(configName, specificMTouch);
                    Driver.GetElement(Button_SaveInEditConfig).ClickElement();
                    if (!Driver.IsElementPresent(Label_AddNewConfig, .5))
                    {
                        Message = "Configuration Created Successfully and config Details are:" + configName;
                        return true;
                    }
                }
                Message = "Configuration is already available with name: " + configName;
                return true;
            }
            catch (Exception) { throw new Exception("Failed to create configuration, refer the screenshot for more details"); }
        }
    
    /// <summary>
    /// Create Website Configuration for Visit Map Module if deos not exist 
    /// </summary>
    /// <param name="Website"></param>
    /// <param name="ModuleType"></param>
    /// <param name="configName"></param>
    /// <param name="module"></param>
    /// <param name="Message"></param>
    /// <returns></returns>
    public bool CreateNewWebsiteConfigurationForVisitMapModuleIfNotExists(string Website, string ModuleType, string configName, string module, out string Message)
    {
        try
        {
            Website_Select_WebsiteAndModuleType(Website, ModuleType);
            if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
            {
                Driver.GetElement(Button_AddNew).ClickElement();
                EnterVisitMapConfigDetails(configName);
                Driver.GetElement(Button_Update).ClickElement();
                if (!Driver.IsElementPresent(Label_AddNewConfig, .5))
                {
                    Message = "Configuration Created Successfully and config Details are:" + configName;
                    return true;
                }
            }
            Message = "Configuration is already available with name: " + configName;
            return true;
        }
        catch (Exception) { throw new Exception("Failed to create configuration, refer the screenshot for more details"); }
    }

        /// <summary>
        /// Enter details to fill Configuration details for VisitMap 
        /// </summary>
        /// <param name="configName"></param>
        public void EnterVisitMapConfigDetails(string configName)
        {
            try
            {
                if (Driver.GetElement(Label_AddNewConfig).IsElementPresent())
                {

                    Driver.GetElement(Input_ConfigName).SendText(configName);
                    Driver.GetElement(Button_Save).ClickElement();
                }
                else
                {
                    throw new Exception("Add new Configuration panel didn't open, refer the screenshot for more details.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to enter configuration details",e);
            }
        }

        public bool AddForgotPasswordToPageIfNotExistsAndConfigurareEmail(string Website, string ModuleType, string configName, string Email, out string Message)
        {
            try
            {
                Website_Select_WebsiteAndModuleType(Website, ModuleType);
                if (!VerifyElementandScrollToElement(ConfigurationName(ModuleType, configName)))
                {
                    Driver.GetElement(Button_AddNew).ClickElement();
                    Driver.GetElement(Input_FogetPasswordConfigName).SendText(configName);
                    Driver.GetElement(Button_SaveInEditConfig).ClickElement();
                    if (!Driver.IsElementPresent(Button_AddNew, .5))
                    {
                        Message = "Configuration Created Successfully and config Details are:" + configName;
                        return true;
                    }
                }
                Message = "Configuration is already available with name: " + configName;
                return true;
            }
            catch (Exception) { throw new Exception("Failed to create configuration, refer the screenshot for more details"); }
        }
    }
}
