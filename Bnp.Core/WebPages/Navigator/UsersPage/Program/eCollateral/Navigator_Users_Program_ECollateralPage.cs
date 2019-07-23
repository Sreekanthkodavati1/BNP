using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral
{
    /// <summary>
    /// This class handles Navigator > Users > Program > eCollateral Page elements
    /// </summary>
    public class Navigator_Users_Program_eCollateralPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Program_eCollateralPage(DriverContext driverContext) : base(driverContext)
        {           
        }

        /// <summary>
        /// Enum for eCollateral tabs
        /// </summary>
        public enum eCollateralTabs
        {
            Messages,
            Notifications,
            Coupons,
            Bonuses,
            AppleWallet,
            GooglePay,
        }

        #region ProgramECollateralTabs locators
        private ElementLocator Tab_ECollateral_Messages = new ElementLocator(Locator.XPath, "//span[contains(text(),'Messages')]");
        private ElementLocator Tab_ECollateral_Notifications = new ElementLocator(Locator.XPath, "//span[contains(text(),'Notifications')]");
        private ElementLocator Tab_ECollateral_Coupons = new ElementLocator(Locator.XPath, "//span[contains(text(),'Coupons')]");
        private ElementLocator Tab_ECollateral_Bonuses = new ElementLocator(Locator.XPath, "//span[contains(text(),'Bonuses')]");
        private ElementLocator Tab_ECollateral_AppleWallet = new ElementLocator(Locator.XPath, "//span[contains(text(),'Apple Wallet')]");
        private ElementLocator Tab_ECollateral_GooglePay = new ElementLocator(Locator.XPath, "//span[contains(text(),'Google Pay')]");
        #endregion

        /// <summary>
        /// Navigate to eCollateral tabs
        /// </summary>
        /// <param name="eCollateralTabsName">eCollateral Tab Name</param>
        /// <returns></returns>
        public bool NavigateToProgramECollateralTab(eCollateralTabs eCollateralTabsName)
        {
            try
            {
                switch (eCollateralTabsName)
                {
                    case eCollateralTabs.Messages:
                        Driver.GetElement(Tab_ECollateral_Messages).ClickElement();
                        break;
                    case eCollateralTabs.Notifications:
                        Driver.GetElement(Tab_ECollateral_Notifications).ClickElement();
                        break;
                    case eCollateralTabs.Coupons:

                        Driver.GetElement(Tab_ECollateral_Coupons).ClickElement();
                        break;
                    case eCollateralTabs.Bonuses:
                        Driver.GetElement(Tab_ECollateral_Bonuses).ClickElement();
                        break;
                    case eCollateralTabs.AppleWallet:
                        Driver.GetElement(Tab_ECollateral_AppleWallet).ClickElement();
                        break;
                    case eCollateralTabs.GooglePay:
                        Driver.GetElement(Tab_ECollateral_GooglePay).ClickElement();
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
