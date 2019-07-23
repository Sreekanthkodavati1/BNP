using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal >login Page
    /// </summary
    public class CSPortal_LoginPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_LoginPage(DriverContext driverContext)
        : base(driverContext)
        { }
    

        #region Element Locators
        private readonly ElementLocator Title_LoginPage = new ElementLocator(Locator.XPath, "//title[contains(text(),'Login ')]");
        private readonly ElementLocator TextBox_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_tbUsername')]");
        private readonly ElementLocator TextBox_Password = new ElementLocator(Locator.XPath, "//input[contains(@id,'_tbPassword')]");
        private readonly ElementLocator Button_Login = new ElementLocator(Locator.XPath, "//a[contains(@id,'_btnLogin')]");
        private readonly ElementLocator Button_ForgotPassword = new ElementLocator(Locator.LinkText, "Forgot Password");
        #endregion
      
        /// <summary>
        /// Launching CS Portal OverLoading Method
        /// </summary>
        /// <param name="CSPortal"></param>
        /// <returns>Launching CS Portal</returns>
        /// 
        public void LaunchCSPortal(string CSPortal, out string Message)
        {
            try
            {
                var browser = Browser;

                Uri CS_Portal = new Uri(CSPortal);
                Driver.Manage().Window.Maximize();
                Driver.NavigateTo(CS_Portal);
                if (Driver.IsElementPresent(TextBox_UserName, 1))
                {
                    Message = "Customer Portal Application Launched Successfully in to the browser: " + browser + ";URL Details: " + CS_Portal;
                }
                else
                {
                    throw new Exception("Failed to Launch CS Portal");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Launch CS Portal");
            }
        }
      
        /// <summary>
        ///Login  CS Portal
        /// </summary>
        /// <param name="Username"></param>
        ///  <param name="Password"></param>
        /// <returns>Login Succesful  CS Portal</returns>
        public void LoginCSPortal(string Username, string Password, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                if (!Driver.IsElementPresent(Button_Login, 1))
                {
                    Message = "Login CS Portal is Successful ;Username:" + Username
                      + ";Password:" + Password;
                }
                else
                {
                    throw new Exception("Login failed refer screenshot for more info");
                }
            }
            catch (Exception)
            {
                throw new Exception("Login failed refer screenshot for more info");
            }
        }

        /// <summary>
        ///Login  CS Portal
        /// </summary>
        /// <param name="Username"></param>
        ///  <param name="Password"></param>
        /// <returns>Login Succesful  CS Portal</returns>
        public void LoginCSPortal_ForActiveandInactive(string Username, string Password, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                Message = "Login CS Portal is Successful ;Username:" + Username
                      + ";Password:" + Password;

            }
            catch (Exception)
            {
                throw new Exception("Login failed refer screenshot for more info");
            }
        }

        public bool VerfiyLoginValidation_withNoInputs(string Username, string Password, string expectedMessage, out string Message)
        {

            try
            {
                Message = "";
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                if (ValidateErrorMessage(ErrorPageLevel(expectedMessage), expectedMessage, out string Message1) && ValidateErrorMessage(ErrorFieldLevel(expectedMessage), expectedMessage, out Message))
                {
                    Message = Message1;
                    return true;
                }
                throw new Exception("Error Message appeared Different from expected Message; Expected Error Message Details are:" + expectedMessage);

            }
            catch (Exception)
            {
                throw new Exception("Error Message appeared Different from expected Message; Expected Error Message Details are:" + expectedMessage);
            }
        }

        public bool VerfiyLoginValidation_withInvalidInputs(string Username, string Password, string expectedMessage, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                return (ValidateErrorMessage(ErrorPageLevel(expectedMessage), expectedMessage, out Message));

            }
            catch (Exception)
            {
                throw new Exception("Error Message appeared Different from expected Message; Expected Error Message Details are:" + expectedMessage);
            }
        }

        public bool VerfiyLoginValidation_withInvalidUserName(string Username, string Password, string expectedMessage, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                expectedMessage = "Unable to find '" + Username + "'";
                return (ValidateErrorMessage(ErrorPageForUserNameLevel(Username), expectedMessage, out Message));

            }
            catch (Exception)
            {
                throw new Exception("Error Message appeared Different from expected Message; Expected Error Message Details are:" + expectedMessage);
            }
        }

        /// <summary>
        /// Click on Forgot Password button
        /// </summary>
        public bool ClickForgotPassword()
        {
            try
            {
                Driver.GetElement(Button_ForgotPassword).ClickElement(); return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to click on Forgot Password");
            }
        }


        public TestStep LaunchCSPortal(string CSPortal, List<TestStep> listOfTestSteps)
        {
            string stepName = "Launch Customer Service Portal URL";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                LaunchCSPortal(CSPortal, out string LaunchoutMessage);
                testStep.SetOutput(LaunchoutMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception)
            {
                testStep.SetOutput("Failed to Launch CS Portal , Please refer Screenshot for more details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception("Failed to Launch CS Portal");
            }
        }

        public TestStep LoginCSPortal(Login login, List<TestStep> listOfTestSteps)
        {
            string stepName = "Login Customer Service Portal URL";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                LoginCSPortal(login.UserName, login.Password, out string _LoginCSPortal);
                testStep.SetOutput(_LoginCSPortal);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception)
            {
                testStep.SetOutput("Failed to Login CS Portal , Please refer Screenshot for more details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception("Failed to Login CS Portal");
            }
        }

        public TestStep ClickForgotPassword(List<TestStep> listOfTestSteps)
        {
            string stepName = "Click on Forgot Password";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                ClickForgotPassword();
                testStep.SetOutput("Click on Forgot Password");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message+": Refer Screenshot for more Details");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}

