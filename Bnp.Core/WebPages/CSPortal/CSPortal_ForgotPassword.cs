﻿using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.Helpers;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using System.Collections.Generic;
using BnPBaseFramework.Reporting.Utils;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Forgot Password
    /// </summary
    public class CSPortal_ForgotPassword : ProjectBasePage
    {
        private string[] logArray;
        int initialWordCount;
        int finalWordCount;
        readonly string singleUseCodeFromLog = "singleUseCode:";
        readonly string BTA_DEV_CS_LogPath = CsPortalData.BTA_DEV_CS_LogPath;
        public TestCase testCase;
        public TestStep testStep;

        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_ForgotPassword(DriverContext driverContext)
       : base(driverContext)
        { }
        #region ElementLoactors
        private readonly ElementLocator Input_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'tbFPIdentity')]");
        private readonly ElementLocator Button_Submit = new ElementLocator(Locator.LinkText, "Submit");
        private readonly ElementLocator Button_SendResetCode = new ElementLocator(Locator.LinkText, "Send my reset code");
        private readonly ElementLocator RadioButton_EmailOption = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='email']");
        private readonly ElementLocator RadioButton_AlreadyHaveResetCode = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='existing']");
        private readonly ElementLocator Field_EnterResetCode = PasswordPage_TextBox_Custom_ElementLocatorXpath("Enter your reset code:");
        private readonly ElementLocator Text_EmailOption = new ElementLocator(Locator.XPath, "//label[starts-with(text(),'Email my reset code to ')]");
        private readonly ElementLocator Text_AlreadyHaveResetCode = new ElementLocator(Locator.XPath, "//label[starts-with(text(),'I already have a reset code.')]");
        private readonly ElementLocator TextBox_NewPassword = new ElementLocator(Locator.XPath, "//label[contains(text(),'New Password:')]//following::input");
        private readonly ElementLocator TextBox_ConfirmPassword = new ElementLocator(Locator.XPath, "//label[contains(text(),'Confirm Password:')]//following::input");
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
        /// Verify all the Reset Options are available
        /// </summary>
        /// <returns></returns>
        public bool ValidateResetOptions(out string Message)
        {
            try
            {
                if (Driver.IsElementPresent(Text_EmailOption, 1) && Driver.IsElementPresent(Text_AlreadyHaveResetCode, 1) && Driver.IsElementPresent(Button_SendResetCode, 5))
                {
                    Message = "Reset Options are available in the Forgot Password Screen";
                    return true;
                }
                else
                {
                    Message = "Reset Options are not available in the Forgot Password Screen";
                    throw new Exception(Message);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to validate Reset Options");
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
                    logArray = Regex.Split(LogData, "GenerateCSAgentResetCode:");
                    string resetCodeText = logArray[logArray.Length - 1];
                    int index = resetCodeText.IndexOf(singleUseCodeFromLog);
                    string resetCode = resetCodeText.Substring(15, 7).Trim();
                    Driver.GetElement(Field_EnterResetCode).SendText(resetCode);
                    Message = "Reset Code is entered successfully and the reset code is " + resetCode;
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
        /// To enter the reset code
        /// </summary>
        /// <returns></returns>
        public void EnterResetCode(string Password, out string Message)
        {
            try
            {
                    Driver.GetElement(Field_EnterResetCode).SendText(Password);
                    Message = "Reset Code Entered successfully and the reset code is " + Password;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to enter reset code from Log file due to" + e.Message);
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
        public bool CreateNewPassword(string agentName, string NewPassword, string ConfirmPassword, out string output)
        {
            try
            {
                Driver.GetElement(TextBox_NewPassword).SendText(NewPassword);
                Driver.GetElement(TextBox_ConfirmPassword).SendText(ConfirmPassword);
                Click_OnButton(Button_Submit);
                if (Driver.IsElementPresent(Message_PasswordChanged, 1))
                {
                    output = "Your Password has been changed; Agent Name:" + agentName + " ;New Password:" + NewPassword;
                    return true;
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed To change the Password refer screenshot for more info");
            }
            output = "Failed to Change Password , Details are; Agent Name:" + agentName + " ;New Password:" + NewPassword;
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
        public void GetInitialWordCountFromLogFile()
        {
            initialWordCount = FilesHelper.GetWordCount(FilesHelper.ReadLogFile(BTA_DEV_CS_LogPath), singleUseCodeFromLog);
        }

        /// <summary>
        /// Verify Invalid Reset Code Error
        /// </summary>
        public bool VerifyInvalidResetCodeError(string ErrorMessage,out string Message)
        {
            return ValidateErrorMessage(ErrorFieldLevel(ErrorMessage), ErrorMessage, out Message);
        }

        /// <summary>
        /// To Select Email Option 
        /// </summary>
        /// <returns></returns>
        public TestStep SelectEmailOption(List<TestStep> listOfTestSteps)
        {
            string stepName = "Select Email option to receive reset code";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                bool stepstatus=SelectEmailOption();
                testStep.SetOutput("Selected Email Option to receive reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// To click Send reset code button
        /// </summary>
        /// <returns></returns>
        public TestStep ClickSendResetCodeButton(List<TestStep> listOfTestSteps)
        {
            string stepName = "Click on Send my reset code";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                bool stepstatus = ClickSendResetCodeButton();
                testStep.SetOutput("Clicked on Send my reset code");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// To Select already have reset code option
        /// </summary>
        /// <returns></returns>
        public TestStep SelectAlreadyHaveResetCode(List<TestStep> listOfTestSteps)
        {
            string stepName = "Select I already have a reset code option";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                bool stepstatus = SelectAlreadyHaveResetCode();
                testStep.SetOutput("Selected I already have a rest code option");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, stepstatus, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// To Select already have reset code option
        /// </summary>
        /// <returns></returns>
        public TestStep EnterResetCodeAnd_ClickOnSubmit(string ResetCode,List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter the reset code:"+ ResetCode+ "And Click on Submit";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterResetCode(ResetCode,out string StatusMessage);
                ClickSubmitButton();
                testStep.SetOutput(StatusMessage + ";Clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// To Select already have reset code option
        /// </summary>
        /// <returns></returns>
        public TestStep EnterUserNameandClickOnSubmit(string UserName, List<TestStep> listOfTestSteps)
        {
            string stepName = "Enter a valid User Name:"+ UserName + " and click Submit button";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                EnterUserName(UserName);
                ClickSubmitButton();
                testStep.SetOutput("Entered User Name: " + UserName + " and Clicked on Submit button");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Verify Invalid Reset Code Error
        /// </summary>
        public TestStep VerifyInvalidResetCodeError(string ErrorMessage, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify Valid Error Message Appeared When Invalid Reset Code Enterted";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyInvalidResetCodeError(ErrorMessage, out string StatusMessage);
                testStep.SetOutput(StatusMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message + ":Refer Screenshot  For More Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }


    }
}