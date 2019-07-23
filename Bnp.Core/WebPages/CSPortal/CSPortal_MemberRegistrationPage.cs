using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Client;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Member Registration
    /// </summary
    public class CSPortal_MemberRegistrationPage : ProjectBasePage
    {
        public BnPBaseFramework.Reporting.Base.TestCase testCase;
        public TestStep testStep;

        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_MemberRegistrationPage(DriverContext driverContext)
        : base(driverContext)
        { }

        #region ElementLoactors
        private readonly ElementLocator HomeTitle = new ElementLocator(Locator.XPath, "//title[contains(text(),'Member Registration')]");

        private readonly ElementLocator TextBox_FirstName = MemberProfile_Textbox_Custom_ElementLocatorXpath("First Name");
        private readonly ElementLocator TextBox_MiddleName = MemberProfile_Textbox_Custom_ElementLocatorXpath("Middle Name");
        private readonly ElementLocator TextBox_LastName = MemberProfile_Textbox_Custom_ElementLocatorXpath("Last Name");
        private readonly ElementLocator Select_Gender = MemberProfile_Select_Custom_ElementLocatorXpath("Gender");

        private readonly ElementLocator TextBox_LoyaltyIDNumber = MemberProfile_Textbox_Custom_ElementLocatorXpath("Loyalty ID Number");
        private readonly ElementLocator Select_CardType = MemberProfile_Select_Custom_ElementLocatorXpath("Card Type");

        private readonly ElementLocator TextBox_AddressLine1 = MemberProfile_Textbox_Custom_ElementLocatorXpath("Address Line 1");
        private readonly ElementLocator TextBox_AddressLine2 = MemberProfile_Textbox_Custom_ElementLocatorXpath("Address Line 2");
        private readonly ElementLocator Select_Country = MemberProfile_Select_Custom_ElementLocatorXpath("Country");
        private readonly ElementLocator Select_StateOrProvince = MemberProfile_Select_Custom_ElementLocatorXpath("State Or Province");
        private readonly ElementLocator TextBox_City = MemberProfile_Textbox_Custom_ElementLocatorXpath("City");
        private readonly ElementLocator TextBox_ZipOrPostalCode = MemberProfile_Textbox_Custom_ElementLocatorXpath("Zip Or Postal Code");

        private readonly ElementLocator TextBox_EmailAddress = MemberProfile_Textbox_Custom_ElementLocatorXpath("Email Address");
        private readonly ElementLocator TextBox_HomePhone = MemberProfile_Textbox_Custom_ElementLocatorXpath("Home Phone");
        private readonly ElementLocator TextBox_MobilePhone = MemberProfile_Textbox_Custom_ElementLocatorXpath("Mobile Phone");
        private readonly ElementLocator TextBox_WorkPhone = MemberProfile_Textbox_Custom_ElementLocatorXpath("Work Phone");

        private readonly ElementLocator TextBox_Username = MemberProfile_Textbox_Custom_ElementLocatorXpath("Username");
        private readonly ElementLocator TextBox_Password = MemberProfile_Textbox_Custom_ElementLocatorXpath("Password");

        private readonly ElementLocator CheckBox_DirectMailOptIn = MemberProfile_CheckBox_Custom_ElementLocatorXpath("DirectMailOptIn");
        private readonly ElementLocator CheckBox_EmailOptIn = MemberProfile_CheckBox_Custom_ElementLocatorXpath("EmailOptIn");
        private readonly ElementLocator CheckBox_SmsOptIn = MemberProfile_CheckBox_Custom_ElementLocatorXpath("SmsOptIn");

        private readonly ElementLocator Button_Save = MemberProfile_Button_Custom_ElementLocatorXpath("Save");
        private readonly ElementLocator Button_Cancel = MemberProfile_Button_Custom_ElementLocatorXpath("Cancel");

        #endregion

        /// <summary>
        /// Verify Attribute Set is Existed from Navigator Portal
        /// </summary>
        /// <param name="AttributeSet"></param>
        public void VerifyAttributeSetonRegisterPage(string AttributeSet)
        {
            ElementLocator AttributeSet_Elm = new ElementLocator(Locator.XPath, "//input//preceding::label[contains(text(),'" + AttributeSet + "')]");
            if (Driver.IsElementPresent(AttributeSet_Elm, .5))
            {
                Driver.GetElement(AttributeSet_Elm).ScrollToElement();
            }
            else
            {
                throw new Exception("Attributeset is not available: " + AttributeSet);
            }
        }

        /// <summary>
        /// Enter BasicInfo Details on Member Registration page
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="MiddleName"></param>
        /// <param name="Select_Gender"></param>
        /// returns bool Status with Message
        public bool EnterBasicInfo_Details(string FirstName, string LastName, string MiddleName, string Select_Gender, out string Message)
        {
            Driver.GetElement(TextBox_FirstName).SendText(FirstName);
            Driver.GetElement(TextBox_LastName).SendText(LastName);
            Driver.GetElement(TextBox_MiddleName).SendText(MiddleName);
            SelectElement_AndSelectByText(this.Select_Gender, Select_Gender);
            Message = "Basic Info Details Entered for Registration </br> FirstName:" + FirstName
                                                                        + "</br>Last Name: " + LastName
                                                                        + "</br>Middle Name:" + MiddleName
                                                                        + "</br>Gender:" + Select_Gender;
            return true;
        }

        /// <summary>
        /// Enter Default LoyaltyCard Details on Member Registration page
        /// </summary>
        /// <param name="LoyaltyIDNumber"></param>
        /// returns bool Status with Message
        public bool EnterDefaultLoyaltyCard_Details(string LoyaltyIDNumber, out string Message)
        {
            Driver.GetElement(TextBox_LoyaltyIDNumber).SendText(LoyaltyIDNumber);
            SelectElement_AndSelectByText(Select_CardType, "Loyalty Card");
            Message = "Loyalty ID Number for Registration </br> Loyalty ID Number:" + LoyaltyIDNumber;
            return true;
        }

        /// <summary>
        /// Enter Default Address Details on Member Registration page
        /// </summary>
        /// <param name="AddressLine1"></param>
        /// <param name="AddressLine2"></param>
        /// <param name="Country"></param>
        /// <param name="StateOrProvince"></param>
        /// <param name="ZipOrPostalCode"></param>
        /// returns bool Status with Message
        public bool EnterAddress_Details(string AddressLine1, string AddressLine2, string Country, string StateOrProvince, string City, string ZipOrPostalCode, out string Message)
        {
            Driver.GetElement(TextBox_AddressLine1).SendText(AddressLine1);
            Driver.GetElement(TextBox_AddressLine2).SendText(AddressLine2);
            SelectElement_AndSelectByText(Select_Country, Country);
            SelectElement_AndSelectByText(Select_StateOrProvince, StateOrProvince);
            Driver.GetElement(TextBox_City).SendText(City);
            Driver.GetElement(TextBox_ZipOrPostalCode).SendText(ZipOrPostalCode);
            Message = " Address Details Entered for Registration </br> AddressLine1:" + AddressLine1
                                                                      + "</br>AddressLine2: " + AddressLine2
                                                                      + "</br>Country:" + Country
                                                                      + "</br>State Or Province:" + StateOrProvince
                                                                      + "</br>City:" + City
                                                                      + "</br>Zip Or PostalCode:" + ZipOrPostalCode;
            return true;
        }

        /// <summary>
        /// Enter Contact Info Details for Registration
        /// </summary>
        /// <param name="EmailAddress"></param>
        /// <param name="HomePhone"></param>
        /// <param name="MobilePhone"></param>
        /// <param name="WorkPhone"></param>
        /// returns bool Status with Message
        public bool EnterContactInfo_Details(string EmailAddress, string HomePhone, string MobilePhone, string WorkPhone, out string Message)
        {
            Driver.GetElement(TextBox_EmailAddress).SendText(EmailAddress);
            Driver.GetElement(TextBox_HomePhone).SendText(HomePhone);
            Driver.GetElement(TextBox_MobilePhone).SendText(MobilePhone);
            Driver.GetElement(TextBox_WorkPhone).SendText(WorkPhone);
            Message = "Contact Info Details Entered for Registration </br> EmailAddress:" + EmailAddress
                                                                     + "</br>HomePhone: " + HomePhone
                                                                     + "</br>MobilePhone:" + MobilePhone
                                                                     + "</br>WorkPhone:" + WorkPhone;
            return true;
        }

        /// <summary>
        /// Enter Default Login Details on Member Registration page
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// returns bool Status with Message
        public bool EnterLoginCredentials_Details(string Username, string Password, out string Message)
        {
            Driver.GetElement(TextBox_Username).SendText(Username);
            Driver.GetElement(TextBox_Password).SendText(Password);
            Message = "Login Credential Details Entered for Registration </br> Username:" + Username
                                                                    + "</br>Password: " + Password;
            return true;
        }

        /// <summary>
        /// Select OptIn Out Details on Member Registration page
        /// </summary>
        /// <param name="DirectMailOptIn"></param>
        /// <param name="EmailOptIn"></param>
        /// <param name="SmsOptIn"></param>
        /// returns bool Status with Message
        public bool EnterOptIn_Out_Details(string DirectMailOptIn, string EmailOptIn, string SmsOptIn, out string Message)
        {
            CheckBoxElmandCheck(CheckBox_DirectMailOptIn);
            CheckBoxElmandCheck(CheckBox_EmailOptIn);
            CheckBoxElmandCheck(CheckBox_SmsOptIn);
            Message = "Check box Selected for Registration </br> DirectMailOptIn:" + DirectMailOptIn
                                                                 + "</br>EmailOptIn: " + EmailOptIn
                                                                 + "</br>SmsOptIn:" + SmsOptIn;
            return true;
        }

        /// <summary>
        /// Save Registration on Member Registration page
        /// </summary>
        public void SaveRegistration()
        {
            Click_OnButton(Button_Save);
        }

        public bool Save_AndVerifyMessage(string ErrorMessage ,out string Message)
        {
            Click_OnButton(Button_Save);
            if(ValidateErrorMessage(ErrorPageLevel(ErrorMessage),ErrorMessage,out string Message1)&& ValidateErrorMessage(ErrorPageLevel(ErrorMessage), ErrorMessage, out string Message2))
            {
                Message = Message1 +";"+ Message2;
                return true;
            }
            throw new Exception("Failed to Verify Error Message on Page:" + ErrorMessage);
         }

        /// <summary>
        /// Enter BasicInfo Details on Member Registration page
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="MiddleName"></param>
        /// <param name="Select_Gender"></param>
        /// returns bool Status with Message
        public TestStep EnterBasicInfo_Details(Member member, MemberDetails memberDetails, List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter Basic Info Details on Member Registration page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterBasicInfo_Details(member.FirstName,member.LastName,member.MiddleName, memberDetails.Gender,out string BasicInfo);
                testStep.SetOutput(BasicInfo);
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

        /// <summary>
        /// Enter Default LoyaltyCard Details on Member Registration page
        /// </summary>
        /// <param name="LoyaltyIDNumber"></param>
        /// returns bool Status with Message
        public TestStep EnterDefaultLoyaltyCard_Details(VirtualCard vc, List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter  Loyalty Card Info Details on Member Registration page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterDefaultLoyaltyCard_Details(vc.LoyaltyIdNumber, out string LoyaltyCardInfo);
                testStep.SetOutput(LoyaltyCardInfo);
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

        /// <summary>
        /// Enter Default Address Details on Member Registration page
        /// </summary>
        /// <param name="AddressLine1"></param>
        /// <param name="AddressLine2"></param>
        /// <param name="Country"></param>
        /// <param name="StateOrProvince"></param>
        /// <param name="ZipOrPostalCode"></param>
        /// returns bool Status with Message
        public TestStep EnterAddress_Details(MemberDetails memberDetails, List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter  Loyalty Card Info Details on Member Registration page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterAddress_Details(memberDetails.AddressLineOne, memberDetails.AddressLineTwo,memberDetails.Country,memberDetails.StateOrProvince,memberDetails.City,memberDetails.ZipOrPostalCode ,out string EnterAddressDetail);
                testStep.SetOutput(EnterAddressDetail);
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

        /// <summary>
        /// Enter Default Login Details on Member Registration page
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// returns bool Status with Message
        public TestStep EnterContactInfo_Details(MemberDetails memberDetails,Member member, List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter  Loyalty Card Info Details on Member Registration page";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterContactInfo_Details(member.PrimaryEmailAddress, memberDetails.HomePhone, memberDetails.MobilePhone, memberDetails.WorkPhone, out string EnterAddressDetail);
                testStep.SetOutput(EnterAddressDetail);
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
