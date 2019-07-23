using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.Helpers;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal > Forgot Password
    /// </summary
    public class MemberPortal_ForgotPassword : ProjectBasePage
    {
        private string[] logArray;
        int initialWordCount;
        int finalWordCount;
        readonly string singleUseCodeFromLog = "singleUseCode:";

        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public MemberPortal_ForgotPassword(DriverContext driverContext)
       : base(driverContext)
        { }
        #region ElementLoactors
        private readonly ElementLocator Input_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'tbFPIdentity')]");
        private readonly ElementLocator Button_Submit = new ElementLocator(Locator.LinkText, "Submit");
        private readonly ElementLocator Button_SendResetCode = new ElementLocator(Locator.XPath, "//div[@id='ForgotPasswordContainer']//a[text()='Send my reset code']");
        private readonly ElementLocator RadioButton_EmailOption = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='email']");
        private readonly ElementLocator RadioButton_AlreadyHaveResetCode = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='existing']");
        private readonly ElementLocator Field_EnterResetCode = PasswordPage_TextBox_Custom_ElementLocatorXpath("Enter your reset code:");
        private readonly ElementLocator TextBox_NewPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("New Password:");
        private readonly ElementLocator TextBox_OldPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("Old Password:");
        private readonly ElementLocator TextBox_ConfirmPassword = PasswordPage_TextBox_Custom_ElementLocatorXpath("Confirm Password:");
        private readonly ElementLocator Message_PasswordChanged = new ElementLocator(Locator.XPath, "//span[text()='Your password has been changed!']");
        private readonly ElementLocator LinkText_ReturnToLoginPage = new ElementLocator(Locator.LinkText, "Click here to return to login page.");
        #endregion

        #region  Locator methods
        public static ElementLocator PasswordPage_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input_Field(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        #endregion

        /// <summary>
        /// To enter Username in Forgot Password Screen
        /// </summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public bool EnterUserName(string Username)
        {
            try
            {
                Driver.GetElement(Input_UserName).SendText(Username); return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to enter Username");
            }
        }

        /// <summary>
        /// To click on Submit button
        /// </summary>
        /// <returns></returns>
        public bool ClickSubmitButton()
        {
            try
            {
                Click_OnButton(Button_Submit); return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to click on Submit button");
            }
        }

        /// <summary>
        /// To Select Email Option 
        /// </summary>
        /// <returns></returns>
        public bool SelectEmailOption()
        {
            try
            {
                SelectRadioButton(RadioButton_EmailOption);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Select Email Option");
            }
        }

        /// <summary>
        /// To Select already have reset code option
        /// </summary>
        /// <returns></returns>
        public bool SelectAlreadyHaveResetCode()
        {
            try
            {
                SelectRadioButton(RadioButton_AlreadyHaveResetCode);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Select already have reset code Option");
            }
        }

        /// <summary>
        /// To click Send reset code button
        /// </summary>
        /// <returns></returns>
        public bool ClickSendResetCodeButton()
        {
            try
            {
                Click_OnButton(Button_SendResetCode); return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to click Send Reset Code button");
            }
        }

        /// <summary>
        /// To enter the reset code received from Log file
        /// </summary>
        /// <returns></returns>
        public void EnterResetCodeFromLogFile(string path, out string Message)
        {
            try
            {
                bool status = false;
                for (int i = 0; i < 5; i++)
                {
                    finalWordCount = FilesHelper.GetWordCount(FilesHelper.ReadLogFile(path), singleUseCodeFromLog);
                    if (initialWordCount < finalWordCount)
                    {
                        status = true;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
                if (status == true)
                {
                    string LogData = FilesHelper.ReadLogFile(path);
                    logArray = Regex.Split(LogData, "GenerateMemberResetCode:");
                    string resetCodeText = logArray[logArray.Length - 1];
                    int index = resetCodeText.IndexOf(singleUseCodeFromLog);
                    string resetCode = resetCodeText.Substring(15, 7).Trim();
                    Driver.GetElement(Field_EnterResetCode).SendText(resetCode);
                    Message = "Reset Code is entered successfully and the reset code is: " + resetCode;
                }
                else
                {
                    throw new Exception("Reset code is not generated in Log file");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to enter reset code from Log file");
            }
        }

        /// <summary>
        /// To create a new Password for a user
        /// </summary>
        /// <param name="agentName"></param>
        /// <param name="NewPassword"></param>
        /// <param name="ConfirmPassword"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool CreateNewPassword(string memberName, string NewPassword, string ConfirmPassword, out string output)
        {
            try
            {
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Click_OnButton(Button_Submit);
                if (Driver.IsElementPresent(Message_PasswordChanged, 1))
                {
                    output = "Your Password has been changed; Member Name:" + memberName + " ;New Password:" + NewPassword;
                    return true;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed To change the Password refer screenshot for more info");
            }
            output = "Failed to Change Password , Details are; Agent Name:" + memberName + " ;New Password:" + NewPassword;
            throw new Exception(output);
        }

        /// <summary>
        /// To  return to Login Page
        /// </summary>
        public bool ReturnToLoginPage(out string output)
        {
            try
            {
                Click_OnButton(LinkText_ReturnToLoginPage);
                output = "Return to Login page is successful";
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Faile to return to Login Page");
            }
        }
        /// <summary>
        /// /To get the initial count of a word from the Log file
        /// </summary>
        public void GetInitialWordCountFromLogFile(string path)
        {
            initialWordCount = FilesHelper.GetWordCount(FilesHelper.ReadLogFile(path), singleUseCodeFromLog);
        }
    }
}