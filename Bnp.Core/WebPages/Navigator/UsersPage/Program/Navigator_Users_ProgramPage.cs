using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.ComponentModel;


namespace Bnp.Core.WebPages.Navigator.UsersPage.Program
{
    /// <summary>
    /// This class handles Navigator > Users > Program Home Page elements
    /// </summary>
    public class Navigator_Users_ProgramPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_ProgramPage(DriverContext driverContext)
       : base(driverContext)
        {          
        }

        /// <summary>
        /// Enum for program tabs
        /// </summary>
        public enum ProgramTabs
        {
            Components,
            eCollateral,
            [DescriptionAttribute("Reward Catalog")]
            RewardCatalog,
            [DescriptionAttribute("Reference Data")]
            ReferenceData
        }

        /// <summary>
        /// Method for getting the element locator based on name
        /// </summary>
        /// <param name="TabName">Program tab name</param>
        /// <returns>element locator By xpath</returns>
        public ElementLocator Tab_Menu(string TabName)
        {
            ElementLocator Tab_Sample = new ElementLocator(Locator.XPath, "//span[contains(text(),'"+ TabName + "')]");
            return Tab_Sample;
        }        

        /// <summary>
        /// Navigates to program tabs
        /// </summary>
        /// <param name="programTabName"></param>
        /// <returns>
        /// returns true if successful, else false
        /// </returns>
        public bool NavigateToProgramTab(ProgramTabs programTabName)
        {
            try
            {
                switch (programTabName)
                {
                    case ProgramTabs.Components:
                        Driver.GetElement(Tab_Menu(ProgramTabs.Components.ToString())).ClickElement();
                        break;
                    case ProgramTabs.eCollateral:
                        Driver.GetElement(Tab_Menu(ProgramTabs.eCollateral.ToString())).ClickElement();
                        break;
                    case ProgramTabs.RewardCatalog:
                        Driver.GetElement(Tab_Menu(EnumUtils.GetDescription(ProgramTabs.RewardCatalog))).ClickElement();
                        break;
                    case ProgramTabs.ReferenceData:
                        Driver.GetElement(Tab_Menu(EnumUtils.GetDescription(ProgramTabs.ReferenceData))).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + programTabName + " tab");
                }
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + programTabName + " Page");
            }
        }
    }
}