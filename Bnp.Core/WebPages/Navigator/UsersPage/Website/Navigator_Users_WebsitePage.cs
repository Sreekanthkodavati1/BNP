using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Website
{
    /// <summary>
    /// This class handles Navigator > Users > Website Page elements
    /// </summary>
    public class Navigator_Users_WebsitePage : ProjectBasePage
    {
        public Navigator_Users_WebsitePage(DriverContext driverContext)
       : base(driverContext)
        {
        }
        public enum DefaultPortalSkinTypes
        {
            CSDefaultSkin,
            EngagementDefaultSkin,
            MemberFacing,
            SPDefaultSkin
        }
        public enum PortalType
        {
            CS,
            MP
        }
        public enum PortalWebsiteType
        {
            CustomerFacing,
            CustomerService
        }
        public enum WebsiteTabs
        {
            Websites,
            Modules,
            Skins,
            Pagelets,
            Templates,
            [DescriptionAttribute("User Agent Mapping")]
            UserAgentMapping,
            [DescriptionAttribute("Website Management")]
            WebsiteManagement
        }

        #region Website page locators        
        private readonly ElementLocator Button_AddNew = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkAddNew')]");
        private readonly ElementLocator TextBox_WebsiteName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtWebsiteName')]");
        private readonly ElementLocator RadioButton_CustomerFacingWebsiteMode = new ElementLocator(Locator.XPath, "//input[contains(@id,'_rdoWebsiteMode_0')]");
        private readonly ElementLocator RadioButton_CustomerServiceWebsiteMode = new ElementLocator(Locator.XPath, "//input[contains(@id,'_rdoWebsiteMode_1')]");
        private readonly ElementLocator Select_DefaultSiteSkin = new ElementLocator(Locator.XPath, "//select[contains(@id,'_ddlDefaultSiteSkin')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkSaveWebsite')]");
        private readonly ElementLocator Button_Cancel = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkCancelWebsite')]"); 
        #endregion

        /// <summary>
        /// Method for getting the element locator based on name
        /// </summary>
        /// <param name="TabName">Program tab name</param>
        /// <returns>element locator By xpath</returns>
        public ElementLocator Tab_Menu(string TabName)
        {
            ElementLocator Tab_Sample = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + TabName + "')]");
            return Tab_Sample;
        }

        /// Navigates to Website tabs
        /// </summary>
        /// <param name="websiteTabName"></param>
        /// <returns>
        /// returns true if successful, else throws exception
        /// </returns>
        public bool NavigateToWebsiteTab(WebsiteTabs websiteTabName, out string Message)
        {
            try
            {
                switch (websiteTabName)
                {
                    case WebsiteTabs.Websites:
                        Driver.GetElement(Tab_Menu(WebsiteTabs.Websites.ToString())).ClickElement();
                        break;
                    case WebsiteTabs.Modules:
                        Driver.GetElement(Tab_Menu(WebsiteTabs.Modules.ToString())).ClickElement();
                        break;
                    case WebsiteTabs.Skins:
                        Driver.GetElement(Tab_Menu(WebsiteTabs.Skins.ToString())).ClickElement();
                        break;
                    case WebsiteTabs.Pagelets:
                        Driver.GetElement(Tab_Menu(WebsiteTabs.Pagelets.ToString())).ClickElement();
                        break;
                    case WebsiteTabs.Templates:
                        Driver.GetElement(Tab_Menu(WebsiteTabs.Templates.ToString())).ClickElement();
                        break;
                    case WebsiteTabs.UserAgentMapping:
                        Driver.GetElement(Tab_Menu(EnumUtils.GetDescription(WebsiteTabs.UserAgentMapping))).ClickElement();
                        break;
                    case WebsiteTabs.WebsiteManagement:
                        Driver.GetElement(Tab_Menu(EnumUtils.GetDescription(WebsiteTabs.WebsiteManagement))).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + websiteTabName + " tab");
                }
                Message = " Navigate to " + websiteTabName + " Page is Successful";
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + websiteTabName + " Page");
            }
        }

        /// <summary>
        /// Enter website details and save
        /// </summary>
        /// <param name="portalData">Website field values in Portal object</param>
        public void EnterWebsiteDetailsAndSave(Portal portalData)
        {            
            Driver.GetElement(TextBox_WebsiteName).SendText(portalData.WebSiteName);
            if (portalData.WebSiteType.Equals(PortalType.CS.ToString()))
            {
                Driver.GetElement(RadioButton_CustomerServiceWebsiteMode).ClickElement();
            }
            else
            {
                Driver.GetElement(RadioButton_CustomerFacingWebsiteMode).ClickElement();
            }
            Select sel = new Select(Driver.GetElement(Select_DefaultSiteSkin));
            sel.SelectByText(portalData.DefaultSkin);
            Click_OnButton(Button_Save);            
        }

        /// <summary>
        /// Verify if website already exists
        /// </summary>
        /// <param name="websiteName">Name of the website</param>
        /// <returns>
        /// returns true if website name exists, else false
        /// </returns>
        public bool VerifyWebsiteExists(String websiteName)
        {
            try
            {
                if (Driver.IsElementPresent(By.XPath("//td[@colspan='3']")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[@colspan='3']//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var i = 1; i <= pageCount; i++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//td[@colspan='3']//a[contains(text(),'" + i + "')]")))
                        {
                            Driver.FindElement(By.XPath("//td[@colspan='3']//a[contains(text(),'" + i + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(By.XPath("//td[text()='" + websiteName + "']")))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//td[text()='" + websiteName + "']")))
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
        /// Method to create Customer Service portal website
        /// </summary>
        /// <returns>
        /// Status of website creation as created or already exists
        /// </returns>
        public string Create_CS(Portal portal)
        {
            var output = "";  
            if (!VerifyWebsiteExists(portal.WebSiteName))
            {                
                Click_OnButton(Button_AddNew);
                EnterWebsiteDetailsAndSave(portal);
                output = portal.WebSiteName + " : Website is created";
            }
            else
            {
                output = portal.WebSiteName + " : Website already exists";
            }
            return output;
        }

        /// <summary>
        /// Method to create Member portal website
        /// </summary>
        /// <returns>
        /// Status of website creation as created or already exists
        /// </returns>
        public string Create_MP(Portal portal)
        {
            var output = "";           
            
            if (!VerifyWebsiteExists(portal.WebSiteName))
            {                
                Click_OnButton(Button_AddNew);
                EnterWebsiteDetailsAndSave(portal);
                output = portal.WebSiteName + " : Website is created";
            }
            else
            {
                output = portal.WebSiteName + " : Website already exists";
            }
            return output;
        }   
    }
}
