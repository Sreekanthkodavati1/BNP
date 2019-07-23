using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.ComponentModel;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal >My Account Page elements
    /// </summary>
    public class MemberPortal_MyAccountPage : ProjectBasePage
    {
        public MemberPortal_MyAccountPage(DriverContext driverContext)
       : base(driverContext)
        {
        }
        public enum MPDashboard
        {
            [DescriptionAttribute("My Account")]
            MyAccount,
            [DescriptionAttribute("My Wallet")]
            MyWallet,
            [DescriptionAttribute("Account Activity")]
            AccountActivity,
            [DescriptionAttribute("Request Credit")]
            RequestCredit,
            [DescriptionAttribute("My Profile")]
            MyProfile,
            [DescriptionAttribute("Favorite Stores")]
            FavoriteStores,
            [DescriptionAttribute("My Visits")]
            MyVisits,
            [DescriptionAttribute("Contact Us")]
            ContactUs,
        }

        #region Element Locators
        private readonly ElementLocator Button_Logout = new ElementLocator(Locator.LinkText, "Logout");
        private readonly ElementLocator Button_MyAccount = MemberPortal_DashBoardLinks( EnumUtils.GetDescription(MPDashboard.MyAccount));
        private readonly ElementLocator Button_MyWallet = MemberPortal_DashBoardLinks(EnumUtils.GetDescription(MPDashboard.MyWallet));       
        private readonly ElementLocator Button_AccountActivity = MemberPortal_DashBoardLinks(EnumUtils.GetDescription(MPDashboard.AccountActivity));
        private readonly ElementLocator Button_RequestCredit = MemberPortal_DashBoardLinks( EnumUtils.GetDescription(MPDashboard.RequestCredit));
        private readonly ElementLocator Button_MyProfile = MemberPortal_DashBoardLinks( EnumUtils.GetDescription(MPDashboard.MyProfile));
        private readonly ElementLocator Button_FavoriteStores = MemberPortal_DashBoardLinks( EnumUtils.GetDescription(MPDashboard.FavoriteStores));
        private readonly ElementLocator Button_MyVisits = MemberPortal_DashBoardLinks( EnumUtils.GetDescription(MPDashboard.MyVisits));
        private readonly ElementLocator Button_ContactUs = MemberPortal_DashBoardLinks(EnumUtils.GetDescription(MPDashboard.ContactUs));
        #endregion

        /// <summary>
        /// Method to navigate to dashboard menu
        /// </summary>
        /// <param name="Menu">Menu Name</param>
        /// <param name="Message">Out message for staus</param>
        /// <returns>Returns true if navigation to menu successful, else throws exception</returns>
        public bool NavigateToMPDashBoardMenu(MPDashboard Menu, out string Message)
        {
            try
            {
                switch (Menu)
                {
                    case MPDashboard.MyAccount:
                        Driver.GetElement(Button_MyAccount).ClickElement();
                        break;
                    case MPDashboard.MyWallet:
                        Driver.GetElement(Button_MyWallet).ClickElement();
                        break;
                    case MPDashboard.AccountActivity:
                        Driver.GetElement(Button_AccountActivity).ClickElement();
                        break;
                    case MPDashboard.RequestCredit:
                        Driver.GetElement(Button_RequestCredit).ClickElement();
                        break;
                    case MPDashboard.MyProfile:
                        Driver.GetElement(Button_MyProfile).ClickElement();
                        break;
                    case MPDashboard.FavoriteStores:
                        Driver.GetElement(Button_FavoriteStores).ClickElement();
                        break;
                    case MPDashboard.MyVisits:
                        Driver.GetElement(Button_MyVisits).ClickElement();
                        break;
                    case MPDashboard.ContactUs:
                        Driver.GetElement(Button_ContactUs).ClickElement();
                        break;                    
                    default:
                        throw new Exception("Failed to match to " + Menu + " Page");
                }
                Message = " Navigate to " + Menu + " Page is Successful";
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + Menu + " Page");
            }
        }
    }
}


