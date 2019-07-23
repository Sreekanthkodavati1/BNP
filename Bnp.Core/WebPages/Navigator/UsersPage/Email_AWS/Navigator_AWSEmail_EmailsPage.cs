using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Email_AWS
{
   public class Navigator_AWSEmail_EmailsPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_AWSEmail_EmailsPage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region Element Locators Emails Page
        private readonly ElementLocator Header_Emails = new ElementLocator(Locator.XPath, "//h2//a[text()='Emails']");
        private readonly ElementLocator PanelHeader_CreateNewEmail = new ElementLocator(Locator.XPath, "//h2[text()='Create new email']");
        private readonly ElementLocator Button_CreateNewEmail = new ElementLocator(Locator.XPath, "//div//a[contains(@id,'AddNew_grdEmails')]");
        private readonly ElementLocator Input_MessageName = new ElementLocator(Locator.XPath, "//input[contains(@id,'GridAction_Name')]");
        private readonly ElementLocator Select_Template = new ElementLocator(Locator.XPath, "//select[contains(@id,'Template')]"); 
        private readonly ElementLocator Input_Subject = new ElementLocator(Locator.XPath, "//input[contains(@id,'GridAction_Subject')]");
        private readonly ElementLocator Input_FromEmail = new ElementLocator(Locator.XPath, "//input[contains(@id,'FromEmail')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//div[@class='buttons']//a[contains(@id,'_lnkSave')]");
        private readonly ElementLocator Button_Cancel = new ElementLocator(Locator.XPath, "//div[@class='buttons']//a[contains(@id,'_lnkCancel')]");
        #endregion

        #region Element Locators Templates Page
        private readonly ElementLocator Header_Templates = new ElementLocator(Locator.XPath, "//h2//a[text()='Templates']");
        private readonly ElementLocator PanelHeader_NewTemplate = new ElementLocator(Locator.XPath, "//h2[text()='New Template']");
        private readonly ElementLocator Button_CreateNewTemplate = new ElementLocator(Locator.XPath, "//a[contains(@id,'AddNew_grdTemplates')]");
        private readonly ElementLocator Input_TemmplateName = new ElementLocator(Locator.XPath, "//input[contains(@id,'GridAction_Name')]");
        private readonly ElementLocator Input_Description = new ElementLocator(Locator.XPath, "//input[contains(@id,'Description')]");
        #endregion

        /// <summary>
        /// Enum for Email (AWS) tabs
        /// </summary>
        public enum AWSEMailTabs
        {
            Emails,
            Templates
        }

        /// <summary>
        /// Method for getting the element locator based on name
        /// </summary>
        /// <param name="TabName">Program tab name</param>
        /// <returns>element locator By xpath</returns>
        public ElementLocator Tab_Menu(string TabName)
        {
            ElementLocator Tab_Sample = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + TabName + "')]");
            return Tab_Sample;
        }

        /// <summary>
        /// Navigates to program tabs
        /// </summary>
        /// <param name="programTabName"></param>
        /// <returns>
        /// returns true if successful, else false
        /// </returns>
        public bool NavigateToEmailAWSTab(AWSEMailTabs awsEmailTabs)
        {
            try
            {
                switch (awsEmailTabs)
                {
                    case AWSEMailTabs.Emails:
                        Driver.GetElement(Tab_Menu(AWSEMailTabs.Emails.ToString())).ClickElement();
                        break;
                    case AWSEMailTabs.Templates:
                        Driver.GetElement(Tab_Menu(AWSEMailTabs.Templates.ToString())).ClickElement();
                        break;
                    default:
                        throw new Exception("Failed to match " + awsEmailTabs + " tab");
                }
                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + awsEmailTabs + " Page");
            }
        }


        /// <summary>
        /// Create New Email Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="Description">Description</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool CreateNewAWSEmailMessage(string MessageName, string template, string subject, string from_Email, out string Message)
        {
            if (VerifyMailWithTemplate(MessageName, template))
            {
                Message = "MailingName  Already Existed!! MailingName :" + MessageName + " and DMC Code: " + template;
                return true;
            }
            if (Driver.IsElementPresent(Header_Emails, .1))
            {
                Driver.GetElement(Button_CreateNewEmail).ClickElement();
                Driver.GetElement(Input_MessageName).SendText(MessageName);
                SelectElement_AndSelectByText(Select_Template, template);
                Driver.GetElement(Input_Subject).SendText(subject);
                Driver.GetElement(Input_FromEmail).SendText(from_Email);
                Driver.GetElement(Button_Save).ClickElement();

                if (Driver.IsElementPresent(Button_Save, 1))
                {
                    throw new Exception("Failed to create a new email message refer screenshot for more details.");
                }
                if (!VerifyMailWithTemplate(MessageName, template))
                {
                    throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
                }
                Message = "New MailingName  Created MailingName:" + MessageName + " and DMC Code: " + template;
                return true;
            }
            throw new Exception("Failed to Create the Mailing Name :" + MessageName + ", Refer  screenshot for more details");
        }

        /// <summary>
        /// Verify Existed Email Message
        /// </summary>
        /// <param name="MessageName">MessageName field</param>
        /// <param name="DMC_EmailCode">DMC EMail Code </param>
        public bool VerifyMailWithTemplate(string MessageName, string TemplateName)
        {
            ElementLocator Row_EmailWithTemplate = new ElementLocator(Locator.XPath, "//span[text()='" + MessageName + "']");
               // "/../following-sibling::td//span[text()='"+TemplateName+"']");
          
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
                        if (Driver.IsElementPresent(Row_EmailWithTemplate, 1))
                        {
                            Driver.GetElement(Row_EmailWithTemplate).ScrollToElement();
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(Row_EmailWithTemplate, 1))
                    {
                        Driver.GetElement(Row_EmailWithTemplate).ScrollToElement();
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

        public ElementLocator TemplateName(string template)
        {
            ElementLocator _templateName = new ElementLocator(Locator.XPath, "//span[contains(@id,'TemplatesName')][contains(text(),'"+template+"')]");
            return _templateName;

        }
        /// <summary>
        /// Create New AWS Email Template
        /// </summary>
        /// <param name="TemplateName">Template Name field</param>
        /// 
        public bool CreateNewTemplate(string template, string description, out string Message)
        {
            if (VerifyElementandScrollToElement(TemplateName(template)))
            {
                Message = "Template: "+ template + "  Already Exists!! " ;
                return true;
            }
            if (Driver.IsElementPresent(Header_Templates, .1))
            {
                Driver.GetElement(Button_CreateNewTemplate).ClickElement();
                Driver.GetElement(Input_TemmplateName).SendText(template);
                Driver.GetElement(Input_Description).SendText(description);
                Driver.GetElement(Button_Save).ClickElement();

                if (Driver.IsElementPresent(Button_Save, 1))
                {
                    throw new Exception("Failed to create a new template refer screenshot for more details.");
                }
                if (!VerifyElementandScrollToElement(TemplateName(template)))
                {
                    throw new Exception("Failed to Create the template :" + template + ", Refer  screenshot for more details");
                }
                Message = "New template Created: " + template;
                return true;
            }
            throw new Exception("Failed to Create the template: " + template + ", Refer  screenshot for more details");
        }

        public ElementLocator ActionOnTemplate(string templateName, string Action)
        {
            ElementLocator _ActionOnTemplate = new ElementLocator(Locator.XPath, "//span[text()='"+templateName+"']/../following-sibling::td//a[text()='"+Action+"']");
            return _ActionOnTemplate;
        }

        public bool DeleteTemplateIfExists(string templateName, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(TemplateName(templateName)))
                {
                    Driver.GetElement(ActionOnTemplate(templateName, "Delete")).ClickElement();
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to "))
                    {
                        Driver.SwitchTo().Alert().Accept();
                    }
                    if (!VerifyElementandScrollToElement(TemplateName(templateName)))
                    {
                        Message = "Template Deleted Successfully and template details are:" + templateName;
                        return true;
                    }
                }
                Message = "No template available with the name : " + templateName;
                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Failed to delete the template, refer the screenshot for more details.");
            }
        }
    }
}
