using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.Database
{
  public class Navigator_DBHomePage : ProjectBasePage
    {

        public Navigator_DBHomePage(DriverContext driverContext)
        : base(driverContext)
        {       }


       
        private readonly ElementLocator DBIconOnHomepage = new ElementLocator(Locator.XPath, "//*[@id='lblDatabase']");


        public void NavigatetoDatabases_Page(out string Output)
        {
           
            Driver.GetElement(DBIconOnHomepage).JavaScriptClick();

            Output="Navigating from  Keys Databases is successful by _Clicking on Databases ";

        }


    }
}
