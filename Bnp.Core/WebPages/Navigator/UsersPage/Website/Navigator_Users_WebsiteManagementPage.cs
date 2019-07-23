using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Configuration;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Website
{
    public class Navigator_Users_WebsiteManagementPage : ProjectBasePage
    {
        public Navigator_Users_WebsiteManagementPage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Management Page Locators
        private readonly ElementLocator WebsiteManagementTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Website Management')]");
        private readonly ElementLocator BouncePoolSuccess = new ElementLocator(Locator.XPath, "//strong[text()='Bounce app pool successful.']");
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Navigator_Website_Select_WebsiteManagementTab()
        {
            Driver.GetElement(WebsiteManagementTab).ClickElement();
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Website"></param>
        /// <returns></returns>
        public bool BounceAppPool(string Website)
        {
            NonAdminUserData WebsiteData = new NonAdminUserData(DriverContext);
            if (Website.Contains("CSPortal"))
            {
                string WebSiteName = WebsiteData.CSPortal_WebSiteName;
                string CS_Endpoint = GetWebsiteInfo("Website", "CSPortal_EndPoint"); 

                ElementLocator WebSite = new ElementLocator(Locator.XPath, "//td//span[contains(text(),'"+ WebSiteName + "')]//parent::td//parent::tr//td//span[contains(text(),'"+CS_Endpoint+"')]//parent::td//following-sibling::td//a[text()='Bounce App Pool']");
                if(!Driver.IsElementPresent(WebSite,1))
                {
                    throw new Exception("CS Portal is not Available");

                }
                Driver.GetElement(WebSite).ScrollToElement();
                Driver.GetElement(WebSite).ClickElement();
                if (Driver.IsElementPresent(BouncePoolSuccess, 1))
                { return true; }
            }else if(Website.Contains("MemberPortal"))
            {
                string WebSiteName = WebsiteData.MemberPortal_WebSiteName;
                string MP_Endpoint = GetWebsiteInfo("Website", "MPPortal_EndPoint");
                ElementLocator WebSite = new ElementLocator(Locator.XPath, "//td//span[contains(text(),'" + WebSiteName + "')]//parent::td//parent::tr//td//span[contains(text(),'" + MP_Endpoint + "')]//parent::td//following-sibling::td//a[text()='Bounce App Pool']");
                if (!Driver.IsElementPresent(WebSite, 1))
                {
                    throw new Exception("MP Portal is not Available");

                }
                Driver.GetElement(WebSite).ScrollToElement();
                Driver.GetElement(WebSite).ClickElement();
                if (Driver.IsElementPresent(BouncePoolSuccess, 1))
                { return true; }
            }
            throw new Exception("Failed to Bounce App Pool");
        }
    }
}
