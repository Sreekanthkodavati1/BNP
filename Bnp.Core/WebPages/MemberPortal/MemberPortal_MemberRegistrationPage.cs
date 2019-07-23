using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using Bnp.Core.WebPages.Models;
using System;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal > Member Registration Page elements
    /// </summary>
    public class MemberPortal_MemberRegistrationPage : ProjectBasePage
    {
        public MemberPortal_MemberRegistrationPage(DriverContext driverContext)
     : base(driverContext)
        {
        }

        #region Element Locators
        private readonly ElementLocator TextBox_FirstName = MemberRegistration_Textbox_Custom_ElementLocatorXpath("First Name");
        private readonly ElementLocator TextBox_LastName = MemberRegistration_Textbox_Custom_ElementLocatorXpath("Last Name");
        private readonly ElementLocator TextBox_EmailAddress = MemberRegistration_Textbox_Custom_ElementLocatorXpath("Email Address");
        private readonly ElementLocator TextBox_UserName = MemberRegistration_Textbox_Custom_ElementLocatorXpath("Username");
        private readonly ElementLocator TextBox_Password = MemberRegistration_Textbox_Custom_ElementLocatorXpath("Password");
        private readonly ElementLocator TextBox_ConfirmPassword = MemberRegistration_Textbox_Custom_ElementLocatorXpath("Confirm Password");
        private readonly ElementLocator Select_TermsAndConditions = new ElementLocator(Locator.XPath, "//label[@class='checkbox']//preceding-sibling::input");        
        private readonly ElementLocator Button_SAVE = new ElementLocator(Locator.XPath, "//a[contains(@id,'lnkSubmitOne')]");
        private readonly ElementLocator Button_CANCEL = new ElementLocator(Locator.XPath, "//a[contains(@id,'lnkCancel')]");
        private readonly ElementLocator Button_Skip = new ElementLocator(Locator.XPath, "//a[contains(text(),'Skip')]");        
        #endregion

        /// <summary>
        /// Custom element locator for Member Registration textbox 
        /// </summary>
        /// <param name="LabelName">Label Name</param>
        /// <returns>
        /// Returns element locator
        /// </returns>
        public static ElementLocator MemberRegistration_Textbox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator LabelName_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_MemberRegistration_Input(LabelName));
            return LabelName_Custom_ElementLocatorXpath;
        }

        /// <summary>
        /// Enter member registration field values
        /// </summary>
        /// <param name="mpData">MP_Registration data</param>
        public void EnterDetails(MP_Registration mpData)
        {
            try { 
                Driver.GetElement(TextBox_FirstName).SendText(mpData.FirstName);
                Driver.GetElement(TextBox_LastName).SendText(mpData.LastName);
                Driver.GetElement(TextBox_EmailAddress).SendText(mpData.EmailAddress);
                Driver.GetElement(TextBox_UserName).SendText(mpData.UserName);
                Driver.GetElement(TextBox_Password).SendText(mpData.Password);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(mpData.ConfirmPassword);
                Driver.GetElement(Select_TermsAndConditions).ClickElement();
            }
            catch(Exception)
            {
                throw new Exception("Failed to enter Member Registration elements refer screenshot for more info");
            }
        }

        /// <summary>
        /// Method to create new member for member portal
        /// </summary>
        /// <param name="mpData">Member creation data</param>
        /// <param name="status">Returns output status of member creation</param>
        /// <returns>
        /// Returns true if member creation is successful, else throws exception
        /// </returns>
        public bool CreateNewMember(MP_Registration mpData, out string status)
        {
            status = "";
            try { 
                EnterDetails(mpData);
                Click_OnButton(Button_SAVE);
                Click_OnButton(Button_Skip);
                status = "Member created successfully with " +
                    ";First Name : "+ mpData.FirstName + "\n" +
                    ";Last Name : "+ mpData.LastName + "\n" +
                    ";Email Address : " + mpData.EmailAddress + "\n" +
                    ";Username : " + mpData.UserName;
            }
            catch (Exception)
            {
                throw new Exception("Failed to enter Member Registration elements refer screenshot for more info");
            }
            return true;
        }
    }
}
