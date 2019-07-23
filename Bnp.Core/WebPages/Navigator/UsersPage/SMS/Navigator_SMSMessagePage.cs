using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;

namespace Bnp.Core.WebPages.Navigator.SMS
{
    public class Navigator_SMSMessagePage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_SMSMessagePage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region Element Locators
        private readonly ElementLocator Header_SMSMessage = new ElementLocator(Locator.XPath, "//h2//a[text()='SMS Messages']");
        private readonly ElementLocator Button_CreateNewMessage = new ElementLocator(Locator.XPath, "//div//a[contains(@id,'_lnkCreateSms')]");
        private readonly ElementLocator Input_MessageName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtName')]");
        private readonly ElementLocator Input_Description = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtDescription')]");
        private readonly ElementLocator Input_DMCSMSMessageId = new ElementLocator(Locator.XPath, "//input[contains(@id,'_txtExternalId')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//div[@class='buttons']//a[contains(@id,'_lnkSaveSms')]");
        private readonly ElementLocator Message_Error = new ElementLocator(Locator.XPath, "//div[contains(text(),'The DMC SMS message Id you entered does not appear to exis')]");
        #endregion

        /// <summary>
        /// Create New SMS Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="Description">Description</param>
        /// <param name="DMC_EmailCode">DMC SMS Code </param>
        public bool CreateNewSMSMessage(string MessageName, string Description, string DMC_SMSCODE, out string Message)
        {
            if (VerifySMSMessageDetails(MessageName, DMC_SMSCODE))
            {
                Message = "SMS Message Already Existed!! SMS Message:" + MessageName + " and DMC Code: " + MessageName;
                return true;
            }
            if (Driver.IsElementPresent(Header_SMSMessage, .1))
            {
                Driver.GetElement(Button_CreateNewMessage).ClickElement();
                Driver.GetElement(Input_MessageName).SendText(MessageName);
                Driver.GetElement(Input_Description).SendText(Description);
                Driver.GetElement(Input_DMCSMSMessageId).SendText(DMC_SMSCODE);
                Driver.GetElement(Input_DMCSMSMessageId).SendKeys(OpenQA.Selenium.Keys.Tab);
                if (Driver.IsElementPresent(Message_Error, 1))
                {
                    throw new Exception("Failed to Add SMS Details due to Invalid DMC Code refer screenshot for more details ,DMC Details are:" + DMC_SMSCODE);
                }
                Driver.GetElement(Button_Save).ClickElement();
                if (!VerifySMSMessageDetails(MessageName, DMC_SMSCODE))
                { throw new Exception("Failed to Verify the SMS Message details, Refere screenshot for more details"); }
                Message = "New SMS Message Created SMS Message:" + MessageName + " and DMC Code: " + DMC_SMSCODE;
                return true;
            }
            throw new Exception("Failed to Add SMS Details due to Invalid DMC Code refer screenshot for more details ,DMC Details are:" + DMC_SMSCODE);
        }

        /// <summary>
        /// Verify Existed Email Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="DMC_EmailCode">DMC SMS Code </param>
        public bool VerifySMSMessageDetails(string MessageName, string DMC_SMSCODE)
        {
            ElementLocator Row_WithSMSandDMC = new ElementLocator(Locator.XPath, "//td[text()='" + DMC_SMSCODE + "']//following::td[text()='" + MessageName + "']");
            try
            {
                if (Driver.IsElementPresent(Row_WithSMSandDMC, 3))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify the SMS Message details, Refere screenshot for more details and Exception is:");
            }
        }

        /// <summary>
        /// Delete Existed SMS Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="DMC_EmailCode">DMC SMS Code </param>
        public bool DeleteSMS(string MessageName, string DMC_SMSCODE)
        {
            ElementLocator DeleteButton = new ElementLocator(Locator.XPath, "//td[text()='" + DMC_SMSCODE + "']//following::td[text()='" + MessageName + "']//following::a[text()='Delete']");
            try
            {
                if (Driver.IsElementPresent(DeleteButton, 3))
                {
                    Click_OnButton(DeleteButton);
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to delete this SMS message?"))
                    {
                        Driver.SwitchTo().Alert().Accept();
                            return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Delete SMS Message details, Refere screenshot for more details and Exception is:");
            }
        }

        /// <summary>
        /// Delete Existed SMS Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="DMC_EmailCode">DMC SMS Code </param>
        public bool DeleteSMSAndVerify(string MessageName, string DMC_SMSCODE, out string message)
        {
            try
            {
                if (DeleteSMS(MessageName, DMC_SMSCODE))
                {
                    if (!VerifySMSMessageDetails(MessageName, DMC_SMSCODE))
                    {
                        message = MessageName + " Deleted Succcessfully and it is not appearing in SMS List"; ;
                        return true;
                    }
                }
                throw new Exception("Failed to Delete SMS Message");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Delete SMS Message details, Refere screenshot for more details and Exception is:");
            }
        }

    }
}
