using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Content
{
    public class Navigator_Users_Content_TextBlocks : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_Users_Content_TextBlocks(DriverContext driverContext) : base(driverContext)
        {
        }

        #region Content TextBlock Locators         
        private readonly ElementLocator Button_CreateTextBlock = new ElementLocator(Locator.XPath, "//a[text()='Create New Text Block']");
        private readonly ElementLocator TextBox_Name = new ElementLocator(Locator.XPath, "//input[contains(@name,'Name')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[text()='Save']");
        private readonly ElementLocator Select_MultiLanguage = new ElementLocator(Locator.XPath, "//select[@class='MultiLanguageSelector']");
        private readonly ElementLocator TextBox_Text = new ElementLocator(Locator.XPath, "/html/body");
        #endregion

        /// <summary>
        /// This method is used to enter values for Textblock
        /// </summary>
        /// <param name="textblocks"></param>
        public void EnterValuesForTextBlock(TextBlocks textblock)
        {
            Click_OnButton(Button_CreateTextBlock);
            Driver.GetElement(TextBox_Name).SendText(textblock.Name);
            Select MultiLanguageList = new Select(Driver.GetElement(Select_MultiLanguage));
            MultiLanguageList.SelectByText(textblock.Language);
            Driver.SwitchTo().Frame(0);
            Driver.GetElement(TextBox_Text).SendText(textblock.Text);
            Driver.SwitchTo().DefaultContent();
            Click_OnButton(Button_Save);
        }

        /// <summary>
        /// This method is used to create Textblock
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public string CreateTextBlockAndVerify(TextBlocks textblock, out string status)
        {
            status = "Text Block : " + textblock.Name + " created successfully";
            try
            {
                if (!VerifyTextBlockExists(textblock.Name))
                {
                    EnterValuesForTextBlock(textblock);
                    if (VerifyTextBlockExists(textblock.Name))
                    {
                        return status;
                    }
                    else
                    {
                        status = "Failed to create new TextBlock";
                        return status;
                    }
                }
                else
                {
                    return "Text Block : " + textblock.Name + "  already exist";
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create TextBlock refer screenshot for more info", e);
            }
        }

        /// <summary>
        /// Verify the newly created TextBlock exists in TextBlock Page
        /// </summary>
        /// <param name="TextBlockName"></param>
        /// <returns></returns>
        public bool VerifyTextBlockExists(string TextBlockName)
        {
            try
            {
                if (Driver.IsElementPresent(By.XPath("//td[contains(@colspan,'2')]")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[contains(@colspan,'2')]//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var i = 1; i <= pageCount; i++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + i + "')]")))
                        {
                            Driver.FindElement(By.XPath("//a[contains(text(),'" + i + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(By.XPath("//td//span[text()='" + TextBlockName + "']")))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//td//span[text()='" + TextBlockName + "']")))
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
        /// Click on TextBlock Edit/Delete
        /// </summary>
        /// <param name="username"></param>
        /// <param name="action_button"></param>
        /// <returns></returns>
        public ElementLocator ActionButton_OfTextBlock(string textblockName, string action_button)
        {
            ElementLocator action = new ElementLocator(Locator.XPath, ("//span[text()='" + textblockName + "']//parent::td//parent::tr//a[text()='" + action_button + "']"));
            return action;
        }

        /// <summary>
        /// This method is used to delete TextBlock
        /// </summary>
        /// <param name="textblock"></param>
        /// <returns></returns>
        public string DeleteTextBlockAndVerify(TextBlocks textblock)
        {
            string status = "TextBlock " + textblock.Name + " Deleted successfully";
            try
            {
                if (VerifyTextBlockExists(textblock.Name))
                {
                    Driver.GetElement(ActionButton_OfTextBlock(textblock.Name, "Delete")).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    return status;
                }
                else
                {
                    throw new Exception("Failed to Delete TextBlock " + textblock.Name + " refer screenshot for more info");
                }
            }
            catch
            {
                throw new Exception("Failed to Delete TextBlock refer screenshot for more info");
            }
        }
    }

}
