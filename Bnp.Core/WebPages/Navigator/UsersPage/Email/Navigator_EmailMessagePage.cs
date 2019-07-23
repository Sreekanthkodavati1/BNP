using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Email
{
    public class Navigator_EmailMessagePage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_EmailMessagePage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region Element Locators
        private readonly ElementLocator Header_EmailMessage = new ElementLocator(Locator.XPath, "//h2//a[text()='Email Messages']");
        private readonly ElementLocator Button_CreateNewEmail = new ElementLocator(Locator.XPath, "//div//a[contains(@id,'_lnkCreateEmail')]");
        private readonly ElementLocator Input_MessageName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtName')]");
        private readonly ElementLocator Input_Description = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtDescription')]");
        private readonly ElementLocator Input_DMCEmailId = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtExternalId')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//div[@class='buttons']//a[contains(@id,'_lnkSaveEmail')]");
        private readonly ElementLocator Message_Error = new ElementLocator(Locator.XPath, "//div[contains(text(),'The DMC Email Id you entered does not appear to exis')]");
        #endregion

        /// <summary>
        /// Create New Email Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="Description">Description</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool CreateNewEmailMessage(string MessageName, string Description, string DMC_EmailCode, out string Message)
        {
            if (VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
            {
                Message = "MailingName Already Existed!! MailingName :" + MessageName + " and DMC Code: " + DMC_EmailCode;
                return true;
            }
            if (Driver.IsElementPresent(Header_EmailMessage, .1))
            {
                Click_OnButton(Button_CreateNewEmail);
                Driver.GetElement(Input_MessageName).SendText(MessageName);
                Driver.GetElement(Input_Description).SendText(Description);
                Driver.GetElement(Input_DMCEmailId).SendText(DMC_EmailCode);
                Driver.GetElement(Input_DMCEmailId).SendKeys(OpenQA.Selenium.Keys.Tab);

                if (Driver.IsElementPresent(Message_Error, 1))
                {
                    throw new Exception("Failed to Add Mailing Name Details due to Invalid DMC Code refer screenshot for more details ,DMC Details are:" + DMC_EmailCode);
                }
                Click_OnButton(Button_Save);
                if (!VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
                {
                    throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
                }
                Message = "New MailingName Created MailingName:" + MessageName + " and DMC Code: " + DMC_EmailCode;
                return true;
            }
            throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");

        }

        public ElementLocator EmailMessage(string emailMessage)
        {
            ElementLocator _emailMessage = new ElementLocator(Locator.XPath, "//td[contains(text(),'" + emailMessage + "')]");
            return _emailMessage;

        }

        /// <summary>
        /// Update newly Created Email Message description
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="Description">Description</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool UpdateEmailMessage(string MessageName, string Description, string DMC_EmailCode, out string Message)
        {
            if (VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
            {
                if (Driver.IsElementPresent(Header_EmailMessage, .1))
                {
                    Click_OnButton(ActionOnEmailName(MessageName, "Edit"));
                    Driver.GetElement(Input_Description).SendText(Description);
                    Click_OnButton(Button_Save);
                    if (Driver.IsElementPresent(Message_Error, 1))
                    {
                        throw new Exception("Failed to Update Mailing Name Details due to Invalid DMC Code refer screenshot for more details ,DMC Details are:" + DMC_EmailCode);
                    }
                    if (!VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
                    {
                        throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
                    }
                    Message = "Mailing Description Updated Successfully:" + MessageName + " and Email Description: " + Description;
                    return true;
                }
                throw new Exception("Failed to Update the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
            }
            throw new Exception("Failed to Update the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
        }

        /// <summary>
        /// Delete Created Email Message 
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="Description">Description</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool DeleteEmailMessage(string MessageName, string Description, string DMC_EmailCode, out string Message)
        {
            if (VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
            {
                if (Driver.IsElementPresent(Header_EmailMessage, .1))
                {
                    Click_OnButton(ActionOnEmailName(MessageName, "Delete"));
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to delete this Email?"))
                    {
                        Driver.SwitchTo().Alert().Accept();
                        if (!Driver.IsElementPresent(Message_Error, 2))
                        {
                            if (VerifyMailingNameMessageDetails(MessageName, DMC_EmailCode))
                            {
                                throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
                            }
                            Message = "Mail Message Deleted Successfully:" + MessageName + " and Email Name: " + MessageName;
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to Delete the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
            }
            throw new Exception("Failed to Delete the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
        }

        public ElementLocator ActionOnEmailName(string EmailName, string Action)
        {
            ElementLocator _EmailName = new ElementLocator(Locator.XPath, "//table[contains(@id,'_grdEmails')]//following::td[text()='" + EmailName + "']//../../td//a[text()='" + Action + "']");
            return _EmailName;
        }

        /// <summary>
        /// Verify Existed Email Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool VerifyMailingNameMessageDetails(string MessageName, string DMC_EmailCode)
        {
            ElementLocator Row_WithEmailandDMC = new ElementLocator(Locator.XPath, "//td[text()='" + DMC_EmailCode + "']//following::td[text()='" + MessageName + "']");

            try
            {
                if (Driver.IsElementPresent(By.XPath("//td[@colspan]//table")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[@colspan]//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var pagenum = 1; pagenum <= pageCount; pagenum++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + pagenum + "')]")))
                        {
                            Driver.FindElement(By.XPath("//a[contains(text(),'" + pagenum + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(Row_WithEmailandDMC, 1))
                        {
                            Driver.GetElement(Row_WithEmailandDMC).ScrollToElement();
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(Row_WithEmailandDMC, 1))
                    {
                        Driver.GetElement(Row_WithEmailandDMC).ScrollToElement();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify the Mailing Name  details, Refere screenshot for more details.");
            }
            return false;
        }
    }
}
