using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Website
{
    /// <summary>
    /// This class handles Navigator > Users > Website > Websites Page elements and actions
    /// </summary>
    class Navigator_Users_Website_WebsitesPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Website_WebsitesPage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region
        private readonly ElementLocator ModulesTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Modules')]");
        private readonly ElementLocator Button_AddNewModule = new ElementLocator(Locator.XPath, "//a[contains(@id,'AddNewModule')]");
        private readonly ElementLocator Label_AddNewModule = new ElementLocator(Locator.XPath, "//h2[text()='Add New Module']");
        private readonly ElementLocator Select_Module = new ElementLocator(Locator.XPath, "//select[contains(@id,'ddlModule')]");
        private readonly ElementLocator Input_ConfigName = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtConfigurationName')]");
        private readonly ElementLocator Input_ModuleArea = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtModuleArea')]");
        private readonly ElementLocator Input_Order = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtModuleOrder')]");
        private readonly ElementLocator Button_saveModule = new ElementLocator(Locator.XPath, "//a[contains(@id,'SaveModule')]");
        #endregion

        public ElementLocator ActionOnWebsiteName(string websiteName, string action)
        {
            ElementLocator _ActionWebsiteName = new ElementLocator(Locator.XPath, "//td[text()='"+websiteName+"']/..//td//div//a[text()='"+action+"']");
            return _ActionWebsiteName;
        }

        public bool ClickOnPagesAction_VerifyAllPagesDisplayedForTheWebsite(string websiteName)
        {
            try
            {
                Driver.GetElement(ActionOnWebsiteName(websiteName, "Pages")).ClickElement();
                string pagesLabel = "//a[text()='Pages - "+websiteName+"']";
                if (Driver.FindElement(By.XPath(pagesLabel)).Displayed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new Exception("Pages table didn't appear, refer the screenshot for more details.");
            }
        }

        public ElementLocator PageName(string pageName)
        {
            ElementLocator _PageName = new ElementLocator(Locator.XPath, "//td[text()='"+pageName+"']");
            return _PageName;
        }

        public ElementLocator ActionOnPage(string pageName, string action)
        {
            ElementLocator _ActionOnPage = new ElementLocator(Locator.XPath, "//td[text()='" + pageName + "']/..//td//div//a[text()='" + action + "']");
            return _ActionOnPage;
        }

        public bool ClickOnModulesAction_VerifyAllModulesDisplayedForThePage(string pageName)
        {
            try
            {
                if (VerifyElementandScrollToElement(PageName(pageName)))
                {
                    Driver.GetElement(ActionOnPage(pageName, "Modules")).ClickElement();
                    string modulesLabel = "//a[text()='Modules - " + pageName + "']";
                    if (Driver.FindElement(By.XPath(modulesLabel)).Displayed)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new Exception("Given page is not present, refer the screenshot for more details.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Modules table didn't appear, refer the screenshot for more details.");
            }
        }

        public ElementLocator ModuleName(string moduleName)
        {
            ElementLocator _ModuleName = new ElementLocator(Locator.XPath, "//table[contains(@id,'grdPageModules')]//td[text()='" + moduleName + "']");
            return _ModuleName;
        }

        public ElementLocator ActionOnModule(string moduleName, string action)
        {
            ElementLocator _ActionOnPage = new ElementLocator(Locator.XPath, "//td[text()='" + moduleName + "']/..//td//div//a[text()='" + action + "']");
            return _ActionOnPage;
        }

        public bool DeleteModuleIfExists(string websiteName, string pageName, string moduleName, out string message)
        {
            try
            {
                ClickOnPagesAction_VerifyAllPagesDisplayedForTheWebsite(websiteName);
                ClickOnModulesAction_VerifyAllModulesDisplayedForThePage(pageName);
                if (VerifyElementandScrollToElement(ModuleName(moduleName)))
                {
                    Driver.GetElement(ActionOnModule(moduleName, "Delete")).ClickElement();
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to "))
                    {
                        Driver.SwitchTo().Alert().Accept();
                    }
                    if (!VerifyElementandScrollToElement(ModuleName(moduleName)))
                    {
                        message = "Configuration Deleted Successfully and config Details are:" + moduleName;
                        return true;
                    }
                }
                message = "No module available with the name : " + moduleName;
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to delete the module, refer the screenshot for more details.");
            }
        }

        public bool AddModuleToPageIfNotExists(string websiteName, string pageName, string moduleName, string configName, string moduleArea, string moduleOrder, out string message)
        {
            try
            {
                ClickOnPagesAction_VerifyAllPagesDisplayedForTheWebsite(websiteName);
                ClickOnModulesAction_VerifyAllModulesDisplayedForThePage(pageName);
                if (!VerifyModuleandScrollToModule(ModuleName(moduleName)))
                {
                    Driver.GetElement(Button_AddNewModule).ClickElement();
                    EnterModuleDetails(moduleName, configName, moduleArea, moduleOrder);
                    Driver.GetElement(Button_saveModule).ClickElement();
                    if (!Driver.IsElementPresent(Button_saveModule, .5))
                    {
                        if (VerifyElementandScrollToElement(ModuleName(moduleName)))
                        {
                            message = "Module added successfully.";
                            return true;
                        }
                        else
                        {
                            throw new Exception("Something went wrong, refer the screenshot for more details");
                        }
                    }
                }
                    message = "Module exists.";
                    return true;
            }
            catch(Exception e)
            {
                throw new Exception("Failed to add module to the page, refer the screenshot for more details.");
            }
        }

        public bool VerifyModuleandScrollToModule(ElementLocator ElementName)
        {
            try
            {
                if (Driver.IsElementPresent(By.XPath("//table[contains(@id,'grdPageModules')]//td[@colspan]//table")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[@colspan]//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var pagenum = 1; pagenum <= pageCount; pagenum++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + pagenum + "')]")))
                        {
                            Driver.FindElement(By.XPath("//a[contains(text(),'" + pagenum + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(ElementName, 1))
                        {
                            Driver.FindElement(By.XPath(ElementName.Value)).ScrollToElement();
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(ElementName, 1))
                    {
                        Driver.FindElement(By.XPath(ElementName.Value)).ScrollToElement();
                        //Driver.GetElement(ElementName).ScrollToElement();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Search the Module:" + ElementName);
            }
            return false;
        }

        public void EnterModuleDetails(string moduleName, string configName, string moduleArea, string moduleOrder)
        {
            try
            {
                if (Driver.GetElement(Label_AddNewModule).IsElementPresent())
                {
                    SelectElement_AndSelectByText(Select_Module, moduleName);
                    Driver.GetElement(Input_ConfigName).SendText(configName);
                    Driver.FindElement(By.XPath("//a[text()='" + configName + "']")).ClickElement();
                    Driver.GetElement(Input_ModuleArea).SendText(moduleArea);
                    Driver.FindElement(By.XPath("//a[text()='" + moduleArea + "']")).ClickElement();
                    Driver.GetElement(Input_Order).SendText(moduleOrder);
                }
                else
                {
                    throw new Exception("Add new module panel didn't appear, refer the screenshot for more details.");
                }
            }
            catch(Exception e)
            {
                throw new Exception("Failed to enter module details, refer the screenshot for more details.");
            }
        }
    }
}
