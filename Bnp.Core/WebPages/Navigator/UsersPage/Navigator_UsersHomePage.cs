using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.UsersPage
{
    /// <summary>
    /// This class handles Navigator > Users > Home Page elements
    /// </summary>
    public class Navigator_UsersHomePage : ProjectBasePage
    {

        public Navigator_UsersHomePage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Users page locators
        private readonly ElementLocator HomeIconOnUsersHomepage = new ElementLocator(Locator.XPath, "//a[@id='ContentPlaceHolder1_lnkSkip']");
        private readonly ElementLocator OrganizationOnUsersHomepage = new ElementLocator(Locator.XPath, "//*[contains(., '"+ Orgnization_value + "')]");

        private readonly ElementLocator OrgEnvOnUsersHomepage = new ElementLocator(Locator.XPath, "//h3[contains(text(),'" + Orgnization_value + "')]//following-sibling::span[position()=" + Env_position + "]//a[contains(text(),'"+Env_value+"')]");
        private readonly ElementLocator btnLogout = new ElementLocator(Locator.XPath, "//a[text()='logout']");
        private readonly ElementLocator Button_SwitchEnv = new ElementLocator(Locator.XPath, "  //div[@id='menus_wrapper']//a[contains(text(),'"+ Orgnization_value + "')]");

        #endregion

        /// <summary>
        /// Click home button on users page
        /// </summary>
        public void Navigator_Users_ClickHome()
        {
            if(Driver.IsElementPresent(HomeIconOnUsersHomepage,.5))
                Driver.GetElement(HomeIconOnUsersHomepage).ClickElement();
        }

        /// <summary>
        /// select organization and environment
        /// </summary>
        public void Navigator_Users_SelectOrganizationEnvironment()
        {
            if (Driver.IsElementPresent(HomeIconOnUsersHomepage, .4)|| (Driver.IsElementPresent(Button_SwitchEnv, 1)))

            { Driver.GetElement(OrgEnvOnUsersHomepage).ClickElement(); }
        }

        public void Navigator_Users_SwitchEnvironment()
        {
            if (Driver.IsElementPresent(Button_SwitchEnv, 1))

            { Driver.GetElement(Button_SwitchEnv).ClickElement(); }
        }
      
        public void Navigator_Users_SelectOrganizationEnvironment(string envName,string orderid,out string output)
        {
            if (Driver.IsElementPresent(HomeIconOnUsersHomepage, .4) || (Driver.IsElementPresent(Button_SwitchEnv, 1)))
            {
                string orgName = Orgnization_value;
                ElementLocator SelectOrgEnvOnUsersHomepage = new ElementLocator(Locator.XPath, "//h3[contains(text(),'" + orgName + "')]//following-sibling::span[position()=" + orderid + "]//a[contains(text(),'" + envName + "')]");
                { Driver.GetElement(SelectOrgEnvOnUsersHomepage).ClickElement(); }
            }
            output = "Selected Environment :" + envName + " Successfully";
        }
    }
}
