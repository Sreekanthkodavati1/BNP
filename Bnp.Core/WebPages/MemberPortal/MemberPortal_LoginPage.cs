using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Configuration;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal >login Page
    /// </summary
    public class MemberPortal_LoginPage : ProjectBasePage
    {
        public MemberPortal_LoginPage(DriverContext driverContext)
       : base(driverContext)
        {
        }

        #region Member Portal Login Page Element Locators
        private readonly ElementLocator LoginPageTitle = new ElementLocator(Locator.XPath, "//title[contains(text(),'Login ')]");
        private readonly ElementLocator TextBox_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_tbUsername')]");
        private readonly ElementLocator TextBox_Password = new ElementLocator(Locator.XPath, "//input[contains(@id,'_tbPassword')]");
        private readonly ElementLocator Button_Login = new ElementLocator(Locator.XPath, "//a[contains(@id,'_btnLogin')]");
        private readonly ElementLocator Button_Logout = new ElementLocator(Locator.XPath, "//div[@id='user-information']//a[text()='Logout']");
        private readonly ElementLocator Button_Join = new ElementLocator(Locator.XPath, "//div[@id='sign-up']//a[@title='Sign up'][contains(text(),'Join')]");
        private readonly ElementLocator Logo_MyRewards = new ElementLocator(Locator.XPath, "//img[@src='skin/MemberFacing/images/MyRewardsLogo.png']");
        private readonly ElementLocator Button_ForgotPassword = new ElementLocator(Locator.LinkText, "Forgot Password?");
        #endregion

        /// <summary>
        /// Launching Member Portal OverLoading Method
        /// </summary>
        /// <param name="MemberPortal"></param>
        /// <returns>Launching Member Portal</returns>
        public void LaunchMemberPortal(string MemberPortal, out string Message)
        {
            try
            {
                var browser = Browser;
                Uri MemberPortalUrl = new Uri(MemberPortal);
                Driver.Manage().Window.Maximize();
                Driver.NavigateTo(MemberPortalUrl);
                if (Driver.IsElementPresent(TextBox_UserName, 1))
                {
                    Message = "Member Portal Application Launched Successfully in to the browser: " + browser + ";URL Details: " + MemberPortalUrl;
                }
                else
                {
                    throw new Exception("Failed to Launch Member Portal");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Launch Member Portal");
            }
        }

        /// <summary>
        ///Login Member Portal
        /// </summary>
        /// <param name="Username"></param>
        ///  <param name="Password"></param>
        /// <returns>Login Succesful Memeber Portal</returns>
        public void LoginMemberPortal(string Username, string Password, out string Message)
        {
            try
            {
                Driver.GetElement(TextBox_UserName).SendText(Username);
                Driver.GetElement(TextBox_Password).SendText(Password);
                Driver.GetElement(Button_Login).ClickElement();
                if (!Driver.IsElementPresent(Button_Login, 1))
                {
                    Message = "Login Member Portal is Successful ;Username:" + Username
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
        ///Logout MP portal
        /// </summary>
        public void LogoutMPPortal()
        {
            try
            {
                Driver.GetElement(Button_Logout).ClickElement();
            }
            catch (Exception)
            {
                throw new Exception("Logout failed refer screenshot for more info");
            }
        }
        /// <summary>
        /// Verify Member Portal Login successful for a User
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <returns></returns>
        public string VerifyMemberPortalLoginSuccessfulForUser(string FirstName, string LastName)
        {
            try
            {
                By successMessageForuser = By.XPath("//p[@id='welcome-message'and starts-with(text(),'" + "Welcome" + " " + FirstName + " " + LastName + "')]");
                if (Driver.IsElementPresent(successMessageForuser) && Driver.IsElementPresent(Logo_MyRewards, .1))
                {
                    Driver.ScrollIntoMiddle(Logo_MyRewards);
                    return "Login is Sucessful for the User with First name: " + FirstName + ";Last Name: " + LastName + " ";
                }
                else
                {
                    throw new Exception("Failed to Login the User with First name: " + FirstName + ";Last Name: " + LastName);
                }
            }
            catch (Exception)
            {
                throw new Exception("Login Failed to Member Portal");
            }
        }

        /// <summary>
        /// Navigate to Member Portal registration page
        /// </summary>
        /// <returns>
        /// Returns the navigation status
        /// </returns>
        public string NavigateToMemberRegistrationPage()
        {
            string status = "Successfully navigated Member Portal registration page";
            try
            {
                Click_OnButton(Button_Join);
            }
            catch (Exception)
            {
                throw new Exception("Failed to navigate Member Portal registration page");
            }
            return status;
        }
        /// <summary>
        /// Click on Forgot Password button
        /// </summary>
        public bool ClickForgotPassword()
        {
            try
            {
                Click_OnButton(Button_ForgotPassword); return true;
            }
            catch (Exception)
            {
                throw new Exception("Failed to click on Forgot Password");
            }
        }
    }
}

