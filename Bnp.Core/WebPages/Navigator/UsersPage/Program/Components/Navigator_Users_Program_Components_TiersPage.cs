using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Helpers;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Program.Components
{
    /// <summary>
    /// This class handles Navigator > Users > Program > Components > Tiers Page elements
    /// </summary>
    public class Navigator_Users_Program_Components_TiersPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Program_Components_TiersPage(DriverContext driverContext) : base(driverContext)
        {
        }

        #region Tiers Page Locators  
        private readonly ElementLocator Label_ExtendedAttributes = new ElementLocator(Locator.XPath, "//legend[contains(text(),'Extended Attributes')]");
        private readonly ElementLocator Button_CreateNewTier = new ElementLocator(Locator.XPath, "//a[text()='Create new tier']");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[contains(@id, 'lnkSave')]");
        private readonly ElementLocator Button_Cancel = new ElementLocator(Locator.XPath, "//a[contains(@id, 'lnkCancel')]");
        private readonly ElementLocator TextBox_TierName = new ElementLocator(Locator.XPath, " //input[contains(@id,'_Name')]");
        #endregion

        /// <summary>
        /// Verifies whether attributeName exists in ExtendedAttributes list on Tiers page    
        /// </summary>
        /// <param name="attributeName">The Attribute Name to search</param>        
        /// <returns>
        /// true if exists, else false
        /// </returns>
        public bool VerifyAttributeExistsInExtendedAttributes(String attributeName)
        {
            try
            {
                var attributeSetElement = Label_ExtendedAttributes.Value + "";
                if (Driver.IsElementPresent(Label_ExtendedAttributes, .1))
                {
                    Driver.GetElement(Label_ExtendedAttributes).ScrollToElement();
                    if (Driver.IsElementPresent(By.XPath("//tr/td/span[text()='" + attributeName + "']")))
                    {
                        return true;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Click button CreateNewTier
        /// </summary>
        public void Click_CreateNewTier()
        {
            Driver.GetElement(Button_CreateNewTier).ClickElement();
        }

        // <summary>
        /// Click button Save
        /// </summary>
        public void Click_Save()
        {
            Driver.GetElement(Button_Save).ClickElement();
        }

        /// <summary>
        /// Click button Cancel
        /// </summary>
        public void Click_Cancel()
        {
            Driver.GetElement(Button_Cancel).ClickElement();
        }

        /// <summary>
        /// Verifies attributes displayed under Extended attributes block on Tiers page
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns>
        /// returns true if attribute exists else false
        /// </returns>
        public bool VerifyTiersPage_ForExtendedAttributes(string attributeName)
        {
            var status = false;
            Click_CreateNewTier();
            if (VerifyAttributeExistsInExtendedAttributes(attributeName))
            {
                status = true;
            }
            return status;
        }

        public ElementLocator TierName(string tier_name)
        {
            ElementLocator _TierName = new ElementLocator(Locator.XPath, "//span[contains(@id,'TiersName')][contains(text(),'" + tier_name + "')]");
            return _TierName;
        }


        /// <summary>
        /// Create Tier and verify the newly created Tier
        /// </summary>
        /// <param name="tierName"> Tier name which is to be created</param>
        /// <returns></returns>
        public bool CreateTierAndVerify(string tierName, out string message)
        {
            message = "Failed to create new Tier refer screenshot for more info";
            try
            {
                if (!VerifyElementandScrollToElement(TierName(tierName)))
                {
                    Click_CreateNewTier();
                    Driver.GetElement(TextBox_TierName).SendText(tierName);
                    Click_Save();
                    //Verify the created Tier is displayed on Tier Page
                    if (VerifyElementandScrollToElement(TierName(tierName)))
                    {
                        message = "Tier " + tierName + " created successfully";
                        return true;
                    }
                    else
                    {
                        message = "Failed to create Tier " + tierName;
                        throw new Exception(message);
                    }
                }
            }
            catch
            {
                throw new Exception(message);
            }
            return false;
        }

        /// <summary>
        /// Create Tier with attribute and verify the newly created Tier
        /// </summary>
        /// <param name="tierName"> Tier name which is to be created</param>
        /// <param name="attribute"> attribute name</param>
        /// <returns></returns>
        public bool CreateTierWithAttributeAndVerify(string tierName, string attribute, string attValue, out string message)
        {
            message = "Failed to create new Tier refer screenshot for more info";
            try
            {
                if (!VerifyElementandScrollToElement(TierName(tierName)))
                {
                    Click_CreateNewTier();
                    Driver.GetElement(TextBox_TierName).SendText(tierName);
                    Driver.FindElement(By.XPath("//legend[text()='Extended Attributes']/following-sibling::table//input[contains(@id,'"+ attribute + "')]")).SendText(attValue);
                    Click_Save();
                    //Verify the created Tier is displayed on Tier Page
                    if (VerifyElementandScrollToElement(TierName(tierName)))
                    {
                        message = "Tier " + tierName + " with attribute "+attValue+" created successfully";
                        return true;
                    }
                    else
                    {
                        message = "Failed to create Tier " + tierName;
                        throw new Exception(message);
                    }
                }
            }
            catch
            {
                throw new Exception(message);
            }
            return false;
        }

        /// <summary>
        /// To verify the error message when more than 2000 characters provided in the content attribute field
        /// </summary>
        /// <param name="messageData"></param>
        /// <returns>
        /// returns status of message creation as true if created else false
        /// returns status of message creation as created or exists
        /// </returns>
        public bool VerifyTheErrorMessageForContentAttributeFieldExceedingTheMaxCharLimitInTierDefination(string tierName, string attributeName, string attributeValue, out string messageStatus)
        {
            try
            {
                if (!VerifyElementandScrollToElement(TierName(tierName)))
                {
                    Click_CreateNewTier();
                    if (VerifyAttributeExistsInExtendedAttributes(attributeName))
                    {
                        Driver.GetElement(TextBox_TierName).SendText(tierName);
                        IWebElement attributeField = Driver.FindElement(By.XPath("//legend[text()='Extended Attributes']/following-sibling::table//input[contains(@id,'" + attributeName + "')]"));
                        IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
                        js.ExecuteScript("arguments[0].scrollIntoView(true); ", attributeField);
                        js.ExecuteScript("arguments[0].setAttribute('value', '" + attributeValue + "')", attributeField);
                        //.SendText(attributeValue);
                        Click_Save();
                        ElementLocator errorMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'Attribute " + attributeName + " cannot have a value > 2000 characters.')]");
                        if (Driver.IsElementPresent(errorMessage, .5))
                        {
                            messageStatus = "Expected error message : \"Attribute " + attributeName + " cannot have a value > 2000 characters.\" " + "displayed.";
                            return true;
                        }
                        else
                        {
                            throw new Exception("Expected error message didn't get displayed, refer the screenshot for more details.");
                        }
                    }
                    else
                    {
                        throw new Exception("Expected attribute field doesn't exist.");
                    }
                }
                else
                {
                    throw new Exception("Message already exists.");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to verify the error message, refer the screenshot for more details.");
            }
        }

        /// <summary>
        /// verify Tier is present in the grid
        /// </summary>
        /// <param name="tierName"> Tier name which is to be created</param>
        /// <param name="attribute"> attribute name</param>
        /// <returns></returns>
        public bool VerifyTierInGrid(string tierName, out string message)
        {
            message = "Failed to the presence of Tier refer screenshot for more info";
            try
            {
                if (VerifyElementandScrollToElement(TierName(tierName)))
                {
                    message = "Tier: " + tierName + " is present in the grid";
                    return true;
                }
                else
                {
                    throw new Exception("Tier: " + tierName + " is not present in the grid");
                }
            }
            catch
            {
                throw new Exception(message);
            }
        }

        /// Verify no tier is created 
        /// </summary>
        /// <returns> returns true if there is no tier displayed else returns false</returns>
        public bool VerifyThereISNoTierDisplayed(out string message)
        {
            message = "Failed to verify, refer the screenshot for more details";
            try
            {
                if (Driver.FindElement(By.XPath("//div[@class='table_wrapper']//table[@cellspacing]//span[contains(@id,'TiersName')]")).IsElementPresent())
                {
                    message = "Tier(s) are present, refer the screensot for more details";
                    return false;
                }
                else
                {
                    message = "No tier is created";
                    return true;
                }
            }
            catch(Exception e)
            {
                message = "No tier is created";
                return true;
            }
        }

        /// Verify the warning message is default reward page
        /// </summary>
        /// <returns> returns true if there is no tier displayed else returns false</returns>
        public bool VerifyTheWarningMessageInDefaultRewardPage(out string message)
        {
            message = "Failed to verify the warning message, refer the screenshot for more details";
            try
            {
                if (Driver.FindElement(By.XPath("//ul[contains(@id,'ErrorPanel')]//strong")).IsElementPresent())
                {
                    if(Driver.FindElement(By.XPath("//ul[contains(@id,'ErrorPanel')]//strong")).Text.Contains("You must first create tiers in order to choose default rewards per tier."))
                    message = "Verified the expected warning message: "+ "\"You must first create tiers in order to choose default rewards per tier.\"" + " is present";
                    return true;
                }
                else
                {
                    message = "Expected warning message: " + "\"You must first create tiers in order to choose default rewards per tier.\"" + " is not present";
                    return true;
                }
            }
            catch
            {
                throw new Exception(message);
            }
        }
        
    }
}