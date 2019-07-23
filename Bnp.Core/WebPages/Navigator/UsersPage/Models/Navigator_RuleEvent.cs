using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using System;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Models
{
    class Navigator_RuleEvent : ProjectBasePage
    {
        public Navigator_RuleEvent(DriverContext driverContext) : base(driverContext) { }

        #region Attribute Set Page Locators
        private readonly ElementLocator Tree_RuleEvents = new ElementLocator(Locator.XPath, "//div[contains(@class,'RadTreeView')]//span[text()='Rule Events']");
        private readonly ElementLocator Left_Menu = new ElementLocator(Locator.XPath, "//span[text()='New Event']");
        private readonly ElementLocator TextBox_EventName = new ElementLocator(Locator.XPath, "//span[contains(text(),'Event Name:')]//..//..//td//input");
        private readonly ElementLocator TextBox_DisplayText = new ElementLocator(Locator.XPath, "//span[contains(text(),'Display Text:')]//..//..//td//input");
        private readonly ElementLocator Button_EventSave = new ElementLocator(Locator.XPath, "//a[contains(@id,'SaveEvent') and text()='Save']");
        private readonly ElementLocator Button_DeleteEvent = new ElementLocator(Locator.XPath, "//span[contains(text(),'Delete Event')]");


        //span[contains(text(),'Event Name:')]//..//..//td//input
        #endregion
        public enum TxnHeaderTabs
        {
            Attributes,
            Rules
        }

        public ElementLocator Menu(string MenuName)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//div[contains(@class,'RadTreeView')]//span[text()='" + MenuName + "']");
            return _Menu;

        }
        public bool CreateNewEvent(string RuleEvent, out string Message)
        {
            if (Driver.IsElementPresent(Menu(RuleEvent), 1))
            {
                Message = "Rule Event Already Available:" + RuleEvent;
                return true;
            }
            else
            {
                Driver.Actions().ContextClick(Driver.GetElement(Tree_RuleEvents)).Build().Perform();
                Driver.GetElement(Left_Menu).ClickElement();
                Driver.GetElement(TextBox_EventName).SendText(RuleEvent);
                Driver.GetElement(TextBox_DisplayText).SendText(RuleEvent);
                Driver.GetElement(Button_EventSave).ClickElement();
                if (Driver.IsElementPresent(Menu(RuleEvent), 1))
                {
                    Message = "Rule Event Created Successfully ;Rule Event Details" + RuleEvent;

                    return true;

                }

            }
            throw new Exception("Failed to Create Rule event:" + RuleEvent);
        }

        /// <summary>
        /// Delete newly created Rule Event
        /// </summary>
        /// <param name="RuleEvent"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public string DeleteEvent(string RuleEvent, out string Message)
        {
            try
            {
                if (VerifyRuleInRuleEventPage(RuleEvent))
                {
                    
                    Driver.Actions().ContextClick(Driver.FindElement(By.XPath("//span[text()='" + RuleEvent + "']"))).Build().Perform();
                    Driver.GetElement(Button_DeleteEvent).ClickElement();
                    if (!VerifyRuleInRuleEventPage(RuleEvent))
                    {
                        Message = "Rule Event " + RuleEvent + "deleted Sucessfully";
                        return Message;
                    }
                    else
                    {
                        Message = "Rule Event " + RuleEvent + " is not deleted ";
                        return Message;
                    }
                }
                else
                {
                    Message = RuleEvent + " does not exists";
                    return Message;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Delete Rule event:" + RuleEvent, e);
            }

        }

        /// <summary>
        /// Verify Rule in Rules  Event PAge
        /// </summary>
        /// <param name="RuleEvent"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool VerifyRuleInRuleEventPage(string RuleEvent)
        {
            string Name = "//span[text()='" + RuleEvent + "']";
            if (Driver.IsElementPresent(By.XPath(Name)))
            {
                return true;
            }
            return false;
        }

    }
}
