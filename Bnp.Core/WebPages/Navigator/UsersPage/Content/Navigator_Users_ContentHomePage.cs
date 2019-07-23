using System;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Content
{
    public class Navigator_Users_ContentHomePage : ProjectBasePage
    {
        public Navigator_Users_ContentHomePage(DriverContext driverContext) : base(driverContext)
        {

        }

        public enum ContentMenuTabs
        {
            Batches,
            TextBlocks,
            Images,
            ContentTypes
        }

        #region Content Page Locators
        private readonly ElementLocator Models_OnMainMenu = new ElementLocator(Locator.Id, "lblContent");
        private readonly ElementLocator Tab_Batches = new ElementLocator(Locator.XPath, "//span[contains(text(),'Batches')]");
        private readonly ElementLocator Tab_TextBlocks = new ElementLocator(Locator.XPath, "//span[contains(text(),'Text Blocks')]");
        private readonly ElementLocator Tab_Images = new ElementLocator(Locator.XPath, "//span[contains(text(),'Images')");
        private readonly ElementLocator Tab_ContentTypes = new ElementLocator(Locator.XPath, "//span[contains(text(),'Content Types')");
        #endregion

        /// <summary>
        /// This method is used to locate the Content tab Menu's
        /// </summary>
        /// <param name="MenuName"></param>
        /// <returns></returns>
        public ElementLocator Menu(string MenuName)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//ul[@class='section_menu']//span[text()='" + MenuName + "']");
            return _Menu;
        }

        public bool NavigateToContentTab(ContentMenuTabs ContentTabName)
        {
            try
            {
                switch (ContentTabName)
                {
                    case ContentMenuTabs.Batches:
                        Driver.GetElement(Menu("Batches")).ClickElement();
                        break;
                    case ContentMenuTabs.TextBlocks:
                        Driver.GetElement(Menu("Text Blocks")).ClickElement();
                        break;
                    case ContentMenuTabs.Images:
                        Driver.GetElement(Menu("Images")).ClickElement();
                        break;
                    case ContentMenuTabs.ContentTypes:
                        Driver.GetElement(Menu("Content Types")).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + ContentTabName + " tab");
                }
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + ContentTabName + " Page");
            }
        }

        /// <summary>
        /// This method is used to navigate to Content Page
        /// </summary>
        /// <param name="Output"></param>
        public void NavigateToContentPage(out string Output)
        {
            Driver.GetElement(Models_OnMainMenu).ClickElement();
            if (Driver.IsElementPresent(Tab_Batches, 1))
            {
                Output = "Navigated to Content Home Page Successfully";
            }
            else
            {
                throw new Exception("Navigating from Content Home Page is failed refer screenshot for more information");
            }
        }
    }
}
