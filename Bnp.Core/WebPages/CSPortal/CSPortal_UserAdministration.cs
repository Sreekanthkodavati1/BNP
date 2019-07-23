using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > User Administration page elements
    /// </summary>
    public class CSPortal_UserAdministration : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_UserAdministration(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Element Locators
        private readonly ElementLocator DropDown_SearchCriteria = new ElementLocator(Locator.XPath, "//span[text()='Search:']//following::Select[1]");
        private readonly ElementLocator Input_Search = new ElementLocator(Locator.XPath, "//span[text()='Search:']//following::input[1]");
        private readonly ElementLocator Button_Search = new ElementLocator(Locator.XPath, "/span[text()='Search:']//following::input[@value='Search']");
        #endregion

        public enum Menu
        {
          Agents,
          Roles
        }

        /// <summary>
        /// Returning Menu Board
        /// </summary>
        /// <param name="Menu"></param>
        /// <returns>string</returns>
        public string UserAdministrationMenuLink(string Menu)
        {
            string CustomMenuLink = "//ul[@class='section_menu']//span[text()='"+Menu+"']";
            return CustomMenuLink;
        }

        /// <summary>
        /// Method to get element locator for User Administration Menu
        /// </summary>
        /// <param name="Menu">User Administration Menu Name</param>
        /// <returns>Element Locator for Menu</returns>
        public ElementLocator UserAdministrationMenu(string Menu)
        {
            ElementLocator Menu_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, UserAdministrationMenuLink(Menu));
            return Menu_Custom_ElementLocatorXpath;
        }

        /// <summary>
        /// Method to navigate to agents and roles pages
        /// </summary>
        /// <param name="Menu"></param>
        /// <returns></returns>
        public bool NavigateToSectionMenu(Menu Menu)
        {
            try
            {
                switch (Menu)
                {
                    case Menu.Agents:
                        Driver.GetElement(UserAdministrationMenu("Agents")).ClickElement();
                        break;
                    case Menu.Roles:
                        Driver.GetElement(UserAdministrationMenu("Roles")).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + Menu + " Page");
                }
                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Failed to Navigate to " + Menu + " Page Due to:"+ e.Message);
            }
        }

        public TestStep NavigateToSectionMenu(Menu Menu, List<TestStep> listOfTestSteps)
        {
            string stepName = "Navigate to Page: "+Menu;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                NavigateToSectionMenu(Menu);
                testStep.SetOutput("Navigate to Page: " + Menu +" is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}
