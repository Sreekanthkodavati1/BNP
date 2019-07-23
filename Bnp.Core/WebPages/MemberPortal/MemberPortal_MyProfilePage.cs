using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using OpenQA.Selenium;
using System;
using System.ComponentModel;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal >login Page elements
    /// </summary>
    public class MemberPortal_MyProfilePage : ProjectBasePage
    {
        public MemberPortal_MyProfilePage(DriverContext driverContext)
      : base(driverContext)
        {
        }

        public enum Sections
        {
            [DescriptionAttribute("My Profile")]
            MyProfile,
            [DescriptionAttribute("Change Password")]
            ChangePassword
        }
        public enum Headers
        {
            [DescriptionAttribute("Basic Info")]
            BasicInfo,
            [DescriptionAttribute("Address")]
            Address,
            [DescriptionAttribute("Contact Info")]
            ContactInfo,
            [DescriptionAttribute("Communication Preferences")]
            CommunicationPreferences,

        }
        public enum Labels
        {
            [DescriptionAttribute("First Name")]
            FirstName,
            [DescriptionAttribute("Last Name")]
            LastName,
            [DescriptionAttribute("Date of Birth")]
            DateofBirth,
            [DescriptionAttribute("Gender")]
            Gender,
            [DescriptionAttribute("Address Line 1")]
            AddressLine1,
            [DescriptionAttribute("Address Line 2")]
            AddressLine2,
            [DescriptionAttribute("Country")]
            Country,
            [DescriptionAttribute("StateOrProvince")]
            StateOrProvince,
            [DescriptionAttribute("City")]
            City,
            [DescriptionAttribute("Zip or Postal Code")]
            ZiporPostalCode,
            [DescriptionAttribute("Email Address")]
            EmailAddress,
            [DescriptionAttribute("Home Phone")]
            HomePhone,
            [DescriptionAttribute("Mobile Phone")]
            MobilePhone,
            [DescriptionAttribute("Work Phone")]
            WorkPhone,
            [DescriptionAttribute("Direct Mail Opt In")]
            DirectMailOptIn,
            [DescriptionAttribute("Email Opt In")]
            EmailOptIn,
            [DescriptionAttribute("SMS Opt In")]
            SMSOptIn
        }

        #region MyRegion
        private readonly ElementLocator Button_Update = new ElementLocator(Locator.XPath, "//a[text()='Update']");
        private readonly ElementLocator Link_MyProfile = new ElementLocator(Locator.XPath, "//a[@id='edit-profile']");
        private readonly ElementLocator Link_ChangePassword = new ElementLocator(Locator.XPath, "//a[@id='edit-password']");
        private readonly ElementLocator Header_BasicInfo = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo));
        private readonly ElementLocator Header_Address = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address));
        private readonly ElementLocator Header_ContactInfo = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo));
        private readonly ElementLocator Header_CommunicationPreferences = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences));
        private readonly ElementLocator Label_FirstName = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.FirstName));
        private readonly ElementLocator Label_LastName = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.LastName));
        private readonly ElementLocator Label_DateofBirth = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.DateofBirth));
        private readonly ElementLocator Label_Gender = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.Gender));
        private readonly ElementLocator Label_AddressLine1 = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.AddressLine1));
        private readonly ElementLocator Label_AddressLine2 = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.AddressLine2));
        private readonly ElementLocator Label_Country = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.Country));
        private readonly ElementLocator Label_StateOrProvince = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.StateOrProvince));
        private readonly ElementLocator Label_City = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.City));
        private readonly ElementLocator Label_ZiporPostalCode = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.ZiporPostalCode));
        private readonly ElementLocator Label_EmailAddress = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.EmailAddress));
        private readonly ElementLocator Label_HomePhone = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.HomePhone));
        private readonly ElementLocator Label_MobilePhone = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.MobilePhone));
        private readonly ElementLocator Label_WorkPhone = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.WorkPhone));
        private readonly ElementLocator Label_DirectMailOptIn = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.DirectMailOptIn));
        private readonly ElementLocator Label_EmailOptIn = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.EmailOptIn));
        private readonly ElementLocator Label_SMSOptIn = MyProfile_HeaderAndLabel_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.SMSOptIn));

        private readonly ElementLocator TextBox_FirstName = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.FirstName));
        private readonly ElementLocator TextBox_LastName = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.LastName));
        private readonly ElementLocator TextBox_DateofBirth = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.BasicInfo), EnumUtils.GetDescription(Labels.DateofBirth));
        private readonly ElementLocator Select_Gender = new ElementLocator(Locator.XPath, "//h3[text()='Basic Info']//following::label[text()='Gender']//following-sibling::select");
        private readonly ElementLocator TextBox_AddressLine1 = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.AddressLine1));
        private readonly ElementLocator TextBox_AddressLine2 = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.AddressLine2));
        private readonly ElementLocator Select_Country = new ElementLocator(Locator.XPath, "//h3[text()='Address']//following::label[text()='Country']//following-sibling::select");
        private readonly ElementLocator Select_StateOrProvince = new ElementLocator(Locator.XPath, "//h3[text()='Address']//following::label[text()='StateOrProvince']//following-sibling::select");
        private readonly ElementLocator TextBox_City = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.City));
        private readonly ElementLocator TextBox_ZipOrPostalCode = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.Address), EnumUtils.GetDescription(Labels.ZiporPostalCode));
        private readonly ElementLocator TextBox_EmailAddress = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.EmailAddress));
        private readonly ElementLocator TextBox_HomePhone = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.HomePhone));
        private readonly ElementLocator TextBox_MobilePhone = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.MobilePhone));
        private readonly ElementLocator TextBox_WorkPhone = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.ContactInfo), EnumUtils.GetDescription(Labels.WorkPhone));
        private readonly ElementLocator CheckBox_DirectMailOptIn = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.DirectMailOptIn));
        private readonly ElementLocator CheckBox_EmailOptIn = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.EmailOptIn));
        private readonly ElementLocator CheckBox_SmsOptIn = MyProfile_Input_Custom_ElementLocator(EnumUtils.GetDescription(Headers.CommunicationPreferences), EnumUtils.GetDescription(Labels.SMSOptIn));
        private readonly ElementLocator TextBox_NewPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("New Password:");
        private readonly ElementLocator TextBox_OldPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("Old Password:");
        private readonly ElementLocator TextBox_ConfirmPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("Confirm Password:");
        private readonly ElementLocator Message_PasswordSuccess = new ElementLocator(Locator.XPath, "//div[@id='ChangePasswordContainer']//div[contains(text(),'Your password was successfully changed')]");
        #endregion

        #region  Locator methods
        public static ElementLocator PasswordPage_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input_Field(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        #endregion
        /// <summary>
        /// This method used for Clicking the My Profile page sections
        /// </summary>
        /// <param name="sectionName"></param>
        public void ClickSections(string sectionName)
        {
            try
            {
                if (sectionName.Equals(Sections.MyProfile.ToString()))
                    Click_OnButton(Link_MyProfile);
                else if (sectionName.Equals(Sections.ChangePassword))
                    Click_OnButton(Link_ChangePassword);
                else
                    throw new Exception("Failed to click " + sectionName + " section");
            }
            catch (Exception)
            {
                throw new Exception("Failed to click " + sectionName + " section");
            }
        }

        /// <summary>
        /// This method to verify My Profile page headers and labels based on header name
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns>true if verify successful, else false</returns>
        public bool VerifyMyProfile_HeadersAndLabels(string headerName)
        {
            try
            {
                if (headerName.Equals(Headers.BasicInfo.ToString()))
                {
                    if (Driver.IsElementPresent(Header_BasicInfo, .5))
                    {
                        if (!(Driver.IsElementPresent(Label_FirstName, .5)
                            && Driver.IsElementPresent(Label_LastName, .5)
                            && Driver.IsElementPresent(Label_DateofBirth, .5)
                            && Driver.IsElementPresent(Label_Gender, .5)))
                        {
                            throw new Exception("Failed to match " + headerName + " header details");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to match " + headerName + " header details");
                    }
                }
                else if (headerName.Equals(Headers.Address.ToString()))
                {
                    if (Driver.IsElementPresent(Header_Address, .5))
                    {
                        if (!(Driver.IsElementPresent(Label_AddressLine1, .5)
                            && Driver.IsElementPresent(Label_AddressLine2, .5)
                            && Driver.IsElementPresent(Label_Country, .5)
                            && Driver.IsElementPresent(Label_StateOrProvince, .5)
                            && Driver.IsElementPresent(Label_City, .5)
                            && Driver.IsElementPresent(Label_ZiporPostalCode, .5)))
                        {
                            throw new Exception("Failed to match " + headerName + " header details");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to match " + headerName + " header details");
                    }
                }
                else if (headerName.Equals(Headers.ContactInfo.ToString()))
                {
                    if (Driver.IsElementPresent(Header_ContactInfo, .5))
                    {
                        if (!(Driver.IsElementPresent(Label_EmailAddress, .5)
                            && Driver.IsElementPresent(Label_HomePhone, .5)
                            && Driver.IsElementPresent(Label_MobilePhone, .5)
                            && Driver.IsElementPresent(Label_WorkPhone, .5)))
                        {
                            throw new Exception("Failed to match " + headerName + " header details");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to match " + headerName + " header details");
                    }
                }
                else if (headerName.Equals(Headers.CommunicationPreferences.ToString()))
                {
                    if (Driver.IsElementPresent(Header_CommunicationPreferences, .5))
                    {
                        if (!(Driver.IsElementPresent(Label_DirectMailOptIn, .5)
                            && Driver.IsElementPresent(Label_EmailOptIn, .5)
                            && Driver.IsElementPresent(Label_SMSOptIn, .5)))
                        {
                            throw new Exception("Failed to match " + headerName + " header details");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to match " + headerName + " header details");
                    }
                }
                else
                {
                    throw new Exception("Failed to match " + headerName + " header");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify " + headerName + " header");
            }
            return true;
        }

        /// <summary>
        /// This method to verify My Profile page
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="message">Return status of message</param>
        /// <returns>true if verify successful, else false</returns>
        public bool VerifyMyProfilePage(string profileName, out string message)
        {
            try
            {
                ClickSections(profileName);
                if (!(VerifyMyProfile_HeadersAndLabels(Headers.BasicInfo.ToString()) && VerifyMyProfile_HeadersAndLabels(Headers.Address.ToString()) && VerifyMyProfile_HeadersAndLabels(Headers.ContactInfo.ToString()) && VerifyMyProfile_HeadersAndLabels(Headers.CommunicationPreferences.ToString())))
                {
                    throw new Exception("Failed to verify " + profileName + " profile details");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify " + profileName + " profile details");
            }
            message = "Successfully verified My Profile Page";
            return true;
        }

        /// <summary>
        /// Update BasicInfo Details on Member Update page
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="DateOfBirth"></param>
        /// <param name="Select_Gender"></param>
        /// returns bool Status with Message
        public bool UpdateBasicInfo_Details(string FirstName, string LastName, string Select_Gender, out string Message)
        {
            Driver.GetElement(TextBox_FirstName).SendText(FirstName);
            Driver.GetElement(TextBox_LastName).SendText(LastName);
            SelectElement_AndSelectByText(this.Select_Gender, Select_Gender);
            Driver.ScrollIntoMiddle(TextBox_FirstName);
            Message = "Basic Info Details Entered for Registration </br> FirstName:" + FirstName
                                                                        + "</br>Last Name: " + LastName
                                                                        + "</br>Gender:" + Select_Gender;
            return true;
        }

        /// <summary>
        /// Update Default Address Details on Member Update page
        /// </summary>
        /// <param name="AddressLine1"></param>
        /// <param name="AddressLine2"></param>
        /// <param name="Country"></param>
        /// <param name="StateOrProvince"></param>
        /// <param name="ZipOrPostalCode"></param>
        /// returns bool Status with Message
        public bool UpdateAddress_Details(string AddressLine1, string AddressLine2, string Country, string StateOrProvince, string City, string ZipOrPostalCode, out string Message)
        {
            Driver.GetElement(TextBox_AddressLine1).SendText(AddressLine1);
            Driver.GetElement(TextBox_AddressLine2).SendText(AddressLine2);
            SelectElement_AndSelectByText(Select_Country, Country);
            SelectElement_AndSelectByText(Select_StateOrProvince, StateOrProvince);
            Driver.GetElement(TextBox_City).SendText(City);
            Driver.GetElement(TextBox_ZipOrPostalCode).SendText(ZipOrPostalCode);
            Driver.ScrollIntoMiddle(TextBox_AddressLine1);
            Message = " Address Details Entered for Registration </br> AddressLine1:" + AddressLine1
                                                                      + "</br>AddressLine2: " + AddressLine2
                                                                      + "</br>Country:" + Country
                                                                      + "</br>State Or Province:" + StateOrProvince
                                                                      + "</br>City:" + City
                                                                      + "</br>Zip Or PostalCode:" + ZipOrPostalCode;
            return true;
        }

        /// <summary>
        /// Update Contact Info Details on Member Update page
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="HomePhone"></param>
        /// <param name="MobilePhone"></param>
        /// <param name="WorkPhone"></param>
        /// returns bool Status with Message
        public bool UpdateContactInfo_Details(string EmailAddress, string HomePhone, string MobilePhone, string WorkPhone, out string Message)
        {
            Driver.GetElement(TextBox_EmailAddress).SendText(EmailAddress);
            Driver.GetElement(TextBox_HomePhone).SendText(HomePhone);
            Driver.GetElement(TextBox_MobilePhone).SendText(MobilePhone);
            Driver.GetElement(TextBox_WorkPhone).SendText(WorkPhone);
            Driver.ScrollIntoMiddle(TextBox_EmailAddress);
            Message = "Contact Info Details Entered for Registration </br> EmailAddress:" + EmailAddress
                                                                     + "</br>HomePhone: " + HomePhone
                                                                     + "</br>MobilePhone:" + MobilePhone
                                                                     + "</br>WorkPhone:" + WorkPhone;
            return true;
        }

        /// <summary>
        /// Update Communication Preferences Details on Member Update page
        /// </summary>
        /// <param name="DirectMailOptIn"></param>
        /// <param name="EmailOptIn"></param>
        /// <param name="SmsOptIn"></param>
        /// returns bool Status with Message
        public bool CommunicationPreferences(string DirectMailOptIn, string EmailOptIn, string SmsOptIn, out string Message)
        {
            CheckBoxElmandCheck(CheckBox_DirectMailOptIn);
            CheckBoxElmandCheck(CheckBox_EmailOptIn);
            CheckBoxElmandCheck(CheckBox_SmsOptIn);
            Driver.ScrollIntoMiddle(CheckBox_DirectMailOptIn);
            Message = "Check box Selected for Registration </br> DirectMailOptIn:" + DirectMailOptIn
                                                                 + "</br>EmailOptIn: " + EmailOptIn
                                                                 + "</br>SmsOptIn:" + SmsOptIn;
            return true;
        }

        /// <summary>
        /// Save Details on Member Update page
        /// </summary>        
        /// <param name="message">Out message on status</param>
        /// <returns>true if save is successful</returns>
        public bool SaveRegistrationAndUpdateSuccess(out string message)
        {
            try
            {
                Click_OnButton(Button_Update);
                if (Driver.IsElementPresent(ProfileUpdateMessage(), .5))
                {
                    Driver.ScrollIntoMiddle(ProfileUpdateMessage());
                    message = "Member updates saved successfully";
                    return true;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to save member update changes");
            }
            throw new Exception("Failed to save member update changes");
        }

        private static ElementLocator ProfileUpdateMessage()
        {
            return new ElementLocator(Locator.XPath, "//span[contains(text(),'Your profile has been updated.')]");
        }

        /// <summary>
        ///Enter Password Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        public bool ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            try
            {
                Click_OnButton(Link_ChangePassword);
                Driver.GetElement(TextBox_OldPassword).SendText(OldPassword);
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Click_OnButton(Button_Update);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Change Password");
            }
        }

        /// <summary>
        /// Verify Success Message
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="Message"></param>
        /// <returns>Message with Status</returns>
        public bool VerifySuccessMessage(string OldPassword, string NewPassword, out string Message)
        {
            try
            {
                if (Driver.IsElementPresent(Message_PasswordSuccess, 1))
                {
                    Message = "Password Changed Successfully;Old Password:" + OldPassword + ";New Password:" + NewPassword;
                    return true;
                }
                else
                {
                    Message = "Failed to Change Password;Old Password:" + OldPassword + ";New Password:" + NewPassword;
                    throw new Exception(Message);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify Success message");
            }
        }
    }
}
