using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Models
{
   public  class Navigator_ModelHomePage : ProjectBasePage
    {

        public Navigator_ModelHomePage(DriverContext driverContext) : base(driverContext) { }


        public enum ModelMenuTabs
        {
            Status,
            AttributeSets,
            RuleEvents,
            RuleTriggers,
            RemoteAssemblies,
            Validators
        }
        #region Home Page Locators
        private readonly ElementLocator Models_OnMainMenu = new ElementLocator(Locator.Id, "lblDataModeling");

        

        private readonly ElementLocator AttributeSetTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]");
        private readonly ElementLocator Status = new ElementLocator(Locator.XPath, "//span[contains(text(),'Status')]");
        private readonly ElementLocator DataModel_Assembly = new ElementLocator(Locator.XPath, " //span[contains(text(),'Data Model Assembly:')and contains(@id,'_pnlDataModeling_Label')]");
        private readonly ElementLocator tree = new ElementLocator(Locator.XPath, " //*[@id='ctl00_ContentPlaceHolder1_pnlDataModeling_rtvAttributeSets']");
        private readonly ElementLocator tree_Virtualcard = new ElementLocator(Locator.XPath, "//div[@class='rtMid']//span[@class='rtPlus']");
        private readonly ElementLocator txnHeaderNode = new ElementLocator(Locator.XPath, "//span[@class='rtIn'][contains(text(),'TxnHeader')]");
        private readonly ElementLocator txnheader_AttributeTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attributes')]");
        #endregion


        public ElementLocator Menu(string MenuName)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//ul[@class='section_menu']//span[text()='"+MenuName+"']");
            return _Menu;

         }

        public bool NavigateToModelTab(ModelMenuTabs ModelTabsName)
        {
            try
            {
                switch (ModelTabsName)
                {
                    case ModelMenuTabs.Status:
                        Driver.GetElement(Menu("Status")).ClickElement();
                        break;
                    case ModelMenuTabs.AttributeSets:
                        Driver.GetElement(Menu("Attribute Sets")).ClickElement();
                        break;
                    case ModelMenuTabs.RuleEvents:
                        Driver.GetElement(Menu("Rule Events")).ClickElement();
                        break;
                    case ModelMenuTabs.RuleTriggers:
                        Driver.GetElement(Menu("Rule Triggers")).ClickElement();
                        break;
                    case ModelMenuTabs.RemoteAssemblies:
                        Driver.GetElement(Menu("Remote Assemblies")).ClickElement();
                        break;
                    case ModelMenuTabs.Validators:
                        Driver.GetElement(Menu("Validators")).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + ModelTabsName + " tab");
                }
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + ModelTabsName + " Page");
            }
        }

        public void NavigateToModels_Page(out string Output)
        {
            Driver.GetElement(Models_OnMainMenu).ClickElement();
            if (Driver.IsElementPresent(Status, 1))
            {
                Output = "Navigating from  Home Page is successful and navigated to Models Home page ";
            }else
            {
                throw new Exception("Navigating from Home Page is failed refer screenshot for more information") ;
            }
        }

        public void NavigatetoToAttributeSet_Page(out string Output)
        {
            Driver.GetElement(AttributeSetTab).ClickElement();
            if (Driver.IsElementPresent(tree, 1))
            {
                Output= "Navigating to Attribute Set Tab is Successful";
            }
            else
            {
                throw new Exception("Navigating to Attribute Set Tab is Failed");
            }
        }
       
    }
}
