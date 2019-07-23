using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Program.RewardCatalog
{
    /// <summary>
    /// This class handles Navigator > Users > Program > Reward Catalog Page elements
    /// </summary>
    public class Navigator_Users_Program_RewardCatalogPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Program_RewardCatalogPage(DriverContext driverContext) : base(driverContext)
        {           
        }

        /// <summary>
        /// Enum for Reward Catalog Tabs
        /// </summary>
        public enum RewardCatalogTabs
        {
            Products,
            ProductVariants,
            ProductImages,
            DefaultRewards,
            Rewards,
            ExchangeRates
        }
        #region RewardCatalog locators
        private ElementLocator Tab_RewardCatalog_Products = new ElementLocator(Locator.XPath, "//span[contains(text(),'Products')]");
        private ElementLocator Tab_RewardCatalog_ProductVariants = new ElementLocator(Locator.XPath, "//span[contains(text(),'Product Variants')]");
        private ElementLocator Tab_RewardCatalog_ProductImages = new ElementLocator(Locator.XPath, "//span[contains(text(),'Product Images')]");
        private ElementLocator Tab_RewardCatalog_DefaultRewards = new ElementLocator(Locator.XPath, "//span[contains(text(),'Default Rewards')]");
        private ElementLocator Tab_RewardCatalog_Rewards = new ElementLocator(Locator.XPath, "//span[text()='Rewards']");
        private ElementLocator Tab_RewardCatalog_ExchangeRates = new ElementLocator(Locator.XPath, "//span[contains(text(),'Exchange Rates')]");
        #endregion

        /// <summary>
        /// Method for switching to Reward Catalog tabs
        /// </summary>
        /// <param name="rewardCatalogTabsName"></param>
        /// <returns>
        /// Returns true if click successful, else false
        /// </returns>
        public bool NavigateToProgramRewardCatalogTab(RewardCatalogTabs rewardCatalogTabsName)
        {
            try
            {
                switch (rewardCatalogTabsName)
                {
                    case RewardCatalogTabs.Products:
                        Driver.GetElement(Tab_RewardCatalog_Products).ClickElement();
                        break;
                    case RewardCatalogTabs.ProductVariants:
                        Driver.GetElement(Tab_RewardCatalog_ProductVariants).ClickElement();
                        break;
                    case RewardCatalogTabs.ProductImages:

                        Driver.GetElement(Tab_RewardCatalog_ProductImages).ClickElement();
                        break;
                    case RewardCatalogTabs.DefaultRewards:
                        Driver.GetElement(Tab_RewardCatalog_DefaultRewards).ClickElement();
                        break;
                    case RewardCatalogTabs.Rewards:
                        Driver.GetElement(Tab_RewardCatalog_Rewards).ClickElement();
                        break;
                    case RewardCatalogTabs.ExchangeRates:
                        Driver.GetElement(Tab_RewardCatalog_ExchangeRates).ClickElement();
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
