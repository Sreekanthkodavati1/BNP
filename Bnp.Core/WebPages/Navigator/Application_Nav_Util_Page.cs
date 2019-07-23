using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;

namespace Bnp.Core.WebPages.Navigator
{
    /// <summary>
    ///  This class handles Navigator > Application Page elements
    /// </summary>
    public class Application_Nav_Util_Page :ProjectBasePage
    {
        public Application_Nav_Util_Page(DriverContext driverContext)
        : base(driverContext)
        {
          
        }        

        /// <summary>
        /// Navigates to different application pages based on application name
        /// </summary>
        /// <param name="appliciationName"></param>
        /// <returns></returns>
        public bool OpenApplication(NavigatorEnums.ApplicationName appliciationName)
        {
            try
            {
                switch (appliciationName)
                {
                    case NavigatorEnums.ApplicationName.organization:
                        Driver.GetElement(Button_Organizations).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.model:
                        Driver.GetElement(Button_Model).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.program:
                        Driver.GetElement(Button_Programs).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.users:

                        Driver.GetElement(Button_Users).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.keys:
                        Driver.GetElement(Button_Keys).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.databases:
                        Driver.GetElement(Button_DataBases).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.website:
                        Driver.GetElement(Button_WebSites).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.promotion:
                        Driver.GetElement(Button_Promotion).JavaScriptClick();
                        break;
                    case NavigatorEnums.ApplicationName.survey:
                        Driver.GetElement(Button_Survey).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.sms:
                        Driver.GetElement(Button_SMS).ClickElement();
                        break; 
                    case NavigatorEnums.ApplicationName.email_aws:
                        Driver.GetElement(Button_EmailAWS).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.email:
                        Driver.GetElement(Button_Email).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.migration:
                        Driver.GetElement(Button_Migration).ClickElement();
                        break;
                    case NavigatorEnums.ApplicationName.content:
                        Driver.GetElement(Button_Content).ClickElement();
                        break;

                    default:
                        throw new Exception("Failed to match " + appliciationName);
                }
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + appliciationName + " section");
            }
        }
         
        private readonly ElementLocator Button_Model=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkDataModeling']");
        private readonly ElementLocator Button_Organizations=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkManageOrganizations']");
        private readonly ElementLocator Button_Programs=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkProgramConfig']");           
        private readonly ElementLocator Button_Promotion= new ElementLocator( Locator.XPath, "//*[@id='MainDashboard_lnkPromotion']");           
        private readonly ElementLocator Button_Survey= new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkSurveyManagement']");           
        private readonly ElementLocator Content_Programs_Landing= new ElementLocator(Locator.XPath, "//*[@id='wrapper']//h2[contains(.,'My Loyalty Program')]");           
        private readonly ElementLocator Content_WebSites_Landing=new ElementLocator(Locator.XPath, "//*[@id='wrapper']//h2[contains(.,'Website Configuration')]");
        private readonly ElementLocator Content_Survey_Landing=new ElementLocator(Locator.XPath, "//*[@id='wrapper']//h2[contains(.,'Survey Management')]");  
        private readonly ElementLocator Content_Promotions_Landing=new ElementLocator(Locator.XPath, "//*[@id='wrapper']//h2[contains(.,'Promotions')]");
        private readonly ElementLocator Button_Users=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkManageUsers']");           
        private readonly ElementLocator Button_Keys=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkKeys']");
        private readonly ElementLocator Button_DataBases= new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkDatabase']");           
        private readonly ElementLocator Button_WebSites=new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkWebsites']");
        private readonly ElementLocator Button_SMS= new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkSms']");
        private readonly ElementLocator Button_Email = new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkDmc']");
        private readonly ElementLocator Button_EmailAWS = new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkAws']");
        private readonly ElementLocator Button_Migration = new ElementLocator(Locator.XPath, "//*[@id='MainDashboard_lnkMigration']");
        private readonly ElementLocator Button_Content = new ElementLocator(Locator.XPath, "//*[@id='lblContent']");

    }
}
