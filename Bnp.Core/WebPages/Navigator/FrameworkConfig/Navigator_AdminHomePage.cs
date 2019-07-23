using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.FrameworkConfig
{
  public  class Navigator_AdminHomePage : ProjectBasePage
    {
       
        public Navigator_AdminHomePage(DriverContext driverContext)
                : base(driverContext)
        {              }
        private readonly ElementLocator orgnizationsIconHomepage = new ElementLocator(Locator.XPath, "//*[@id='lblManageOrganizations']");
        private readonly ElementLocator usersIconHomepage = new ElementLocator(Locator.XPath, "//*[@id='lblManageUsers']");

        public void NavigatetorOrgnization_Page()
        {
            Driver.GetElement(orgnizationsIconHomepage).JavaScriptClick();
        }
    }
}
