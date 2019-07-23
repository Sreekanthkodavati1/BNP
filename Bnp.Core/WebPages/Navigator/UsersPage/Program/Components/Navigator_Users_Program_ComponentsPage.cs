using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Program.Components
{
    /// <summary>
    /// This class handles Navigator > Users > Program > Components Home Page elements
    /// </summary>
    public class Navigator_Users_Program_ComponentsPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Program_ComponentsPage(DriverContext driverContext) : base(driverContext)
        {            
        }

        /// <summary>
        /// Enum for Components Tab elements
        /// </summary>
        public enum ComponentsTabs
        {
            GlobalValues,
            LoyaltyEvents,
            LoyaltyCurrency,
            Tiers,
            Attributes,
            Categories,
            Stores,
            LocationGroups,
            bScriptLibrary,
            Channels
        }
        #region ProgramComponentsTabs locators
        private ElementLocator Tab_Components_GlobalValues = new ElementLocator(Locator.XPath, "//span[contains(text(),'Global Values')]");
        private ElementLocator Tab_Components_LoyaltyEvents = new ElementLocator(Locator.XPath, "//span[contains(text(),'Loyalty Events')]");
        private ElementLocator Tab_Components_LoyaltyCurrency = new ElementLocator(Locator.XPath, " //span[contains(text(),'Loyalty Currency')]");
        private ElementLocator Tab_Components_Tiers = new ElementLocator(Locator.XPath, " //span[contains(text(),'Tiers')]");
        private ElementLocator Tab_Components_Attributes = new ElementLocator(Locator.XPath, " //span[contains(text(),'Attributes')]");
        private ElementLocator Tab_Components_Categories = new ElementLocator(Locator.XPath, "//span[contains(text(),'Categories')]");
        private ElementLocator Tab_Components_Stores = new ElementLocator(Locator.XPath, " //span[contains(text(),'Stores')]");
        private ElementLocator Tab_Components_LocationGroups = new ElementLocator(Locator.XPath, " //span[contains(text(),'Location Groups')]");
        private ElementLocator Tab_Components_bScriptLibrary = new ElementLocator(Locator.XPath, "//span[contains(text(),'bScript Library')]");
        private ElementLocator Tab_Components_Channels = new ElementLocator(Locator.XPath, "//span[contains(text(),'Channels')]");
        #endregion

        /// <summary>
        /// Navigates to components tab based on enum ComponentsTabs
        /// </summary>
        /// <param name="componentsTabsName"></param>
        /// <returns></returns>
        public bool NavigateToProgramComponentsTab(ComponentsTabs componentsTabsName)
        {
            try
            {
                switch (componentsTabsName)
                {
                    case ComponentsTabs.GlobalValues:  
                        Driver.GetElement(Tab_Components_GlobalValues).ClickElement();
                        break;
                    case ComponentsTabs.LoyaltyEvents:                       
                        Driver.GetElement(Tab_Components_LoyaltyEvents).ClickElement();
                        break;
                    case ComponentsTabs.LoyaltyCurrency:
                       
                        Driver.GetElement(Tab_Components_LoyaltyCurrency).ClickElement();
                        break;
                    case ComponentsTabs.Tiers:
                        Driver.GetElement(Tab_Components_Tiers).ClickElement();
                        break;
                    case ComponentsTabs.Attributes:
                        Driver.GetElement(Tab_Components_Attributes).ClickElement();
                        break;
                    case ComponentsTabs.Categories:
                        Driver.GetElement(Tab_Components_Categories).ClickElement();
                        break;
                    case ComponentsTabs.Stores:
                        Driver.GetElement(Tab_Components_Stores).ClickElement();
                        break;
                    case ComponentsTabs.LocationGroups:
                        Driver.GetElement(Tab_Components_LocationGroups).ClickElement();
                        break;
                    case ComponentsTabs.bScriptLibrary:
                        Driver.GetElement(Tab_Components_bScriptLibrary).ClickElement();
                        break;
                    case ComponentsTabs.Channels:
                        Driver.GetElement(Tab_Components_Channels).ClickElement();
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
