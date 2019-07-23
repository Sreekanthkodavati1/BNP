using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.Keys
{
    /// <summary>
    /// This class handles Login Navigator as Key Admin user
    /// </summary
    public class Navigator_KeysHomePage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_KeysHomePage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region ElementLoactors
        private readonly ElementLocator keysIconOnHomepage = new ElementLocator(Locator.XPath, "//*[@id='lblKeys']");
        #endregion

        /// <summary>
        /// Click on Keys Icon on Home page and navigate to Mange Keys
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful login of respective user
        /// </returns>
        public bool NavigatetoMangeKeys_Page(out string Output)
        {
            Driver.GetElement(keysIconOnHomepage).ClickElement();
            Output = "Navigating from  Keys Homepage is successful by clicking on Keys ";
            return true;
        }
    }
}
