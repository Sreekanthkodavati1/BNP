using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Change Password
    /// </summary
    public class CSPortal_ChangePassword : ProjectBasePage
    {

        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_ChangePassword(DriverContext driverContext)
       : base(driverContext)
        { }

        #region ElementLoactors
        private readonly ElementLocator TextBox_OldPassword = ChangePassword_TextBox_Custom_ElementLocatorXpath("Old Password:");
        private readonly ElementLocator TextBox_NewPassword = ChangePassword_TextBox_Custom_ElementLocatorXpath("New Password:");
        private readonly ElementLocator TextBox_ConfirmPassword = ChangePassword_TextBox_Custom_ElementLocatorXpath("Confirm Password:");
        private readonly ElementLocator TextBox_ConfirmNewPassword = ChangePassword_TextBox_Custom_ElementLocatorXpath("Confirm New Password:");

        private readonly ElementLocator Button_Save = ChangePassword_Button_Custom_ElementLocatorXpath("Save");
        private readonly ElementLocator Button_ChangePassword = new ElementLocator(Locator.XPath, "//a[text()='Change Password' and (@class='StandardButton')]");

        private readonly ElementLocator Button_Cancel = ChangePassword_Button_Custom_ElementLocatorXpath("Cancel");

        private readonly ElementLocator Message_PasswordSuccess = new ElementLocator(Locator.XPath, "//span[contains(text(),'Your password was successfully changed.')]");
        #endregion


        //public ElementLocator ErrorFieldLevel(string ErrorMessage)
        //{
        //    ElementLocator _ErrorFieldLevel = new ElementLocator(Locator.XPath, ".//span[@class='Validator' and contains(text(),'" + ErrorMessage + "'" + ")]");
        //    return _ErrorFieldLevel;
        //}
        //public ElementLocator ErrorPageLevel(string ErrorMessage)
        //{
        //    string singlequotestring = ".//li[text()='" + ErrorMessage + "']";
        //    ElementLocator _ErrorPageLevel = new ElementLocator(Locator.XPath, singlequotestring);
        //    return _ErrorPageLevel;
        //}


        //public bool ValidateErrorMessage(ElementLocator Elm, string Errormessage, out string Message)
        //{
        //    string AppearedErrorMesage = "";
        //    if (Driver.IsElementPresent(Elm, 2))
        //    {
        //        AppearedErrorMesage = Driver.GetElement(Elm).GetTextContent();
        //    }else
        //    {
        //        throw new Exception("Expected Error Message is not appeared:"+Errormessage);

        //    }
        //    if (AppearedErrorMesage.Contains(Errormessage))
        //    {
        //        Message = "Error Message appeared as expected;  Error Message Details are:" + Errormessage;
        //        return true;
        //    }
        //    Message = "Error Message appeared Different from expected Message; Expected Error Message Details are:" + Errormessage
        //                                                                          + "Appeared Error Message Details are:" + AppearedErrorMesage;
        //    throw new Exception(Message);
        //}

        /// <summary>
        ///Enter Password Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        public bool EnterPasswordDetails(string OldPassword, string NewPassword, string ConfirmPassword, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_OldPassword).SendText(OldPassword);
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Message = "Old Password ,New Password, Confirm Password Fields are available";
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }

        /// <summary>
        ///Enter Password Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        public bool EnterPasswordDetailsOnlogin(string OldPassword, string NewPassword, string ConfirmPassword, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_OldPassword).SendText(OldPassword);
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmNewPassword).SendText(ConfirmPassword);
                Message = "Old Password ,New Password, Confirm Password Fields are available";
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }

        /// <summary>
        /// Save Password
        /// </summary>
        /// <returns>Click on Save Successful</returns>
        public bool SavePassword()
        {
            try
            {
                Click_OnButton(Button_Save);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
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
                throw new Exception("Failed to Enter Password Details");
            }
        }

        /// <summary>
        ///Enter Verify Validations on  Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="OldPassword"></param>
        /// <param name="OldPassword"></param>
        public bool EnterOldandNew_AsSamePassword(string OldPassword, string ExpectedErrorMessage, out string Message)
        {
            try
            {
                Click_OnButton(Button_Cancel);
                EnterPasswordDetails(OldPassword, OldPassword, OldPassword,out string ValidationMessage);
                Click_OnButton(Button_Save);
                bool result= ValidateErrorMessage(ErrorPageLevel(ExpectedErrorMessage), ExpectedErrorMessage, out Message);
                Message = Message + ";Test Data Details; OldPassword:" + OldPassword;
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }

        /// <summary>
        ///Enter Verify Validations on  Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        public bool EnterMismatchingNewConfirmPassword(string OldPassword, string NewPassword,string ConfirmPassword, string ExpectedErrorMessage, out string Message)
        {
            try
            {
                Click_OnButton(Button_Cancel);
                EnterPasswordDetails(OldPassword, NewPassword, ConfirmPassword, out string ValidationMessage);
                Click_OnButton(Button_Save);
                bool result = ValidateErrorMessage(ErrorFieldLevel(ExpectedErrorMessage), ExpectedErrorMessage, out Message);
                Message = Message + ";Test Data Details; OldPassword:" + OldPassword+";New Password:"+NewPassword+";ConfirmPassword:"+ConfirmPassword;
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }

        /// <summary>
        ///Enter Verify Validations on  Details to change Password
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        public bool EnterMaximumValidationsOnPassword(string OldPassword, string NewPassword, string ConfirmPassword, string ExpectedErrorMessage, out string Message)
        {
            try
            {
                Click_OnButton(Button_Cancel);
                EnterPasswordDetails(OldPassword, NewPassword, ConfirmPassword,out string ValidationMessage);
                Click_OnButton(Button_Save);
                bool result = ValidateErrorMessage(ErrorFieldLevel(ExpectedErrorMessage), ExpectedErrorMessage, out Message);
                Message = Message + ";Test Data Details; OldPassword:" + OldPassword + ";New Password:" + NewPassword + ";ConfirmPassword:" + ConfirmPassword;
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Verify MaximumValidationPassword Criteria , Please Refer  screenshot for More details;Password Details"+ ";Test Data Details; OldPassword:" + OldPassword + ";New Password:" + NewPassword + ";ConfirmPassword:" + ConfirmPassword);
            }
        }
        /// <summary>
        ///Enter Verify Validations on  Details to change Password
        /// </summary>
        /// <param name="_OldPassword_Message1"></param>
        /// <param name="_NewPassword_Message1"></param>
        /// <param name="_ConfirmNewPassword_Message1"></param>

        public bool Enter_EmptyValuesInPasswordFields(string _OldPassword_Message1, string _NewPassword_Message1, string _ConfirmNewPassword_Message1, out string Message)
        {
            try
            {
                Click_OnButton(Button_Cancel);
                EnterPasswordDetails("", "", "", out string ValidationMessage);
                Click_OnButton(Button_Save);
                if (ValidateErrorMessage(ErrorPageLevel(_OldPassword_Message1), _OldPassword_Message1, out string OldPassword_Message1) && ValidateErrorMessage(ErrorPageLevel(_NewPassword_Message1), _NewPassword_Message1, out string NewPassword_Message1) && ValidateErrorMessage(ErrorPageLevel(_ConfirmNewPassword_Message1), _ConfirmNewPassword_Message1, out string ConfirmNewPassword_Message1) 
                && ValidateErrorMessage(ErrorFieldLevel(_OldPassword_Message1), _OldPassword_Message1, out string OldPassword_Message2) && ValidateErrorMessage(ErrorFieldLevel(_NewPassword_Message1), _NewPassword_Message1, out string NewPassword_Message2) && ValidateErrorMessage(ErrorFieldLevel(_ConfirmNewPassword_Message1), _ConfirmNewPassword_Message1, out string ConfirmNewPassword_Message2))
                {
                    Message = OldPassword_Message1 + ";" + NewPassword_Message1 + ";" + ConfirmNewPassword_Message1;
                    return true;
                }

                throw new Exception("Failed to Validate Below Details:;" + _OldPassword_Message1 + ";" + _NewPassword_Message1 + ";" + _OldPassword_Message1);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }

        public bool Enter_EmptyValuesInPasswordFieldsOnLoginPasswordChange(string _OldPassword_Message1, string _NewPassword_Message1, string _ConfirmNewPassword_Message1, out string Message)
        {
            try
            {
                EnterPasswordDetailsOnlogin("", "", "", out string ValidationMessage);
                Click_OnButton(Button_ChangePassword);
                if (ValidateErrorMessage(ErrorPageLevel(_OldPassword_Message1), _OldPassword_Message1, out string OldPassword_Message1) && ValidateErrorMessage(ErrorPageLevel(_NewPassword_Message1), _NewPassword_Message1, out string NewPassword_Message1) && ValidateErrorMessage(ErrorPageLevel(_ConfirmNewPassword_Message1), _ConfirmNewPassword_Message1, out string ConfirmNewPassword_Message1))
                  {
                    Message = OldPassword_Message1 + ";" + NewPassword_Message1 + ";" + ConfirmNewPassword_Message1;
                    return true;
                }

                throw new Exception("Failed to Validate Below Details:;" + _OldPassword_Message1 + ";" + _NewPassword_Message1 + ";" + _OldPassword_Message1);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Password Details");
            }
        }
        public bool ChangePasswordOnLoginPrompt(string _OldPassword_Message1, string _NewPassword_Message1, string _ConfirmNewPassword_Message1, out string Message)
        {
            try
            {
                EnterPasswordDetailsOnlogin(_OldPassword_Message1, _NewPassword_Message1, _ConfirmNewPassword_Message1, out string ValidationMessage);
                Click_OnButton(Button_ChangePassword);
                if(!Driver.IsElementPresent(Button_ChangePassword,1))
                {
                    Message = "Password Changed OnLogin Prompt Page ;OldPassword:" + _OldPassword_Message1 + ";New Password" +_NewPassword_Message1;
                    return true;
                }
                throw new Exception("Failed to Change Password OnLogin Prompt Page ;OldPassword:" + _OldPassword_Message1 + ";New Password" + _NewPassword_Message1);

            }
            catch (Exception)
            {
                throw new Exception("Failed to Change Password Details are: Oldpassword:"+_OldPassword_Message1+"; New Password:"+_NewPassword_Message1);
            }
        }

        public TestStep Enter_EmptyValuesInPasswordFieldsOnLoginPasswordChange(List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Valid Error Message Appeared When Password Values are Empty";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                Enter_EmptyValuesInPasswordFieldsOnLoginPasswordChange("Value required.", "Value required.", "Value required.", out string Message);
                testStep.SetOutput(Message);
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

        public TestStep ChangePasswordWithValidValues(string _OldPassword_Message1, string _NewPassword_Message1, string _ConfirmNewPassword_Message1, List<TestStep> listOfTestSteps)
        {
            string stepName = "Change Password with Valid Values";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                ChangePasswordOnLoginPrompt(_OldPassword_Message1, _NewPassword_Message1, _ConfirmNewPassword_Message1, out string Message);
                testStep.SetOutput(Message);
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
