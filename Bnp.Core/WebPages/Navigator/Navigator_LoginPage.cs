using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;

namespace Bnp.Core.WebPages.Navigator
{
    /// <summary>
    ///  This class handles Navigator Portal > Login Page elements
    /// </summary>
    public class Navigator_LoginPage : ProjectBasePage
    {
        public Navigator_LoginPage(DriverContext driverContext)
          : base(driverContext)
        {
        }

        #region Login page element locators
        private readonly ElementLocator TextBox_UserName = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtUserName')]");
        private readonly ElementLocator TextBox_Password = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtPassword')]");
        private readonly ElementLocator Button_Login = new ElementLocator(Locator.XPath, "//input[contains(@id,'lnkSubmit')]");
        private readonly ElementLocator Button_Logout = new ElementLocator(Locator.XPath, "//a[text()='logout']");
        private readonly ElementLocator Button_Keys = new ElementLocator(Locator.XPath, "//span[text()='keys']");
        private readonly ElementLocator Button_Databases = new ElementLocator(Locator.XPath, "//span[text()='databases']");
        private readonly ElementLocator Button_Organizations = new ElementLocator(Locator.XPath, "//span[text()='organizations']");
        private readonly ElementLocator Button_Users = new ElementLocator(Locator.XPath, "//span[text()='users']");
        private readonly ElementLocator Button_Jobs = new ElementLocator(Locator.XPath, "//span[text()='jobs']");
        private readonly ElementLocator Button_Sites = new ElementLocator(Locator.XPath, "//span[text()='sites']");
        private readonly ElementLocator Button_Home = new ElementLocator(Locator.XPath, "//a[text()='Home >']");
        private readonly ElementLocator Button_Client = new ElementLocator(Locator.XPath, "//span[text()='client']");
        private readonly ElementLocator Error_Message = new ElementLocator(Locator.XPath, "//div[@class='error_inner']");

        #endregion

        /// <summary>
        /// Navigate to Navigator Login Page
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Output"></param>
        /// <returns>
        /// returns true and Output on successful launching of login page
        /// </returns>
        public bool LaunchNavigatorPortal(string url, out string Output)
        {
            var browser = Browser;
            Uri navigatorurl = new Uri(url);
            Driver.Manage().Window.Maximize();
            Driver.NavigateTo(navigatorurl);
            if (Driver.IsElementPresent(TextBox_UserName, 10))
            {
                Output = "Navigator Application Launched Successfully in to the browser: " + browser + ";URL Details: " + url;
                return true;
            }
            else
            {
                throw new Exception("Navigator Application Launched Failed in to the browser;URL Details: " + url);
            }
        }

        /// <summary>
        /// Login to Navigator
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Output"></param>
        /// <returns>
        /// returns true and Output on successful login
        /// </returns>
        public bool Login(Login user, string role, out string Output)
        {
            try
            {
                Output = "";
                Driver.GetElement(TextBox_UserName).SendText(user.UserName);
                Driver.GetElement(TextBox_Password).SendText(user.Password);
                Driver.GetElement(Button_Login).ClickElement();
                if (Driver.IsElementPresent(Button_Logout, .5) || Driver.IsElementPresent(Button_Home, .5))
                {
                    if (VerifyLoginStatus(role))
                    {
                        Output = "Login is Successful  as ;User Name: " + user.UserName + ";Password : " + user.Password;
                        return true;
                    }
                    else
                    {
                        throw new Exception("Login is Fail  as ;User Name: " + user.UserName + ";Password : " + user.Password);
                    }
                }
                else if (Driver.IsElementPresent(Error_Message, .5))
                {
                    Output = "Access Denied Invalid userid/password.";
                    var resultString = Driver.GetElement(Error_Message).Text;
                    if (Output.Equals(resultString))
                    {
                        throw new Exception("Login is Fail  as ;User Name: " + user.UserName + ";Password : " + user.Password + "Due to " + Output);
                    }
                    else
                    {
                        throw new Exception("Login is Fail  as ;User Name: " + user.UserName + ";Password : " + user.Password + "Due to " + resultString);
                    }
                }
                throw new Exception("Login is Fail  as ;User Name: " + user.UserName + ";Password : " + user.Password + "For more information Please refer Screenshot");
            }
            catch (Exception e)
            {
                throw new Exception("Login is Fail  as ;User Name: " + user.UserName + ";Password : " + user.Password + "Due to:" + e.Message);
            }
        }
        /// <summary>
        /// Logout from Navigator
        /// </summary>
        public void Logout()
        {
            if (Driver.IsElementPresent(Button_Logout, 10))
            {
                Driver.GetElement(Button_Logout).ClickElement();
            }
            if (Driver.IsElementPresent(Button_Logout, 2))
            {
                LaunchNavigatorPortal(GetApplicationUrls(0), out string Output);
            }
        }

        /// <summary>
        /// Verify login status for different pages
        /// </summary>
        /// <param name="navigatorUser">user name</param>
        /// <returns></returns>
        public bool VerifyLoginStatus(string navigatorUser)
        {
            if (navigatorUser.Equals(Users.AdminRole.LWADM.ToString()))
            {
                if (Driver.IsElementPresent(Button_Organizations, 10) && (Driver.IsElementPresent(Button_Users, 10)))
                    return true;
            }
            else if (navigatorUser.Equals(Users.AdminRole.KEY.ToString()))
            {
                if (Driver.IsElementPresent(Button_Keys, 10) && (Driver.IsElementPresent(Button_Logout, 10)))
                    return true;
            }
            else if (navigatorUser.Equals(Users.AdminRole.DBA.ToString()))
            {
                if (Driver.IsElementPresent(Button_Databases, 10) && (Driver.IsElementPresent(Button_Logout, 10)))
                    return true;
            }
            else if (navigatorUser.Equals(Users.AdminRole.WEB.ToString()))
            {
                if (Driver.IsElementPresent(Button_Jobs, 10) && (Driver.IsElementPresent(Button_Sites, 10)))
                    return true;
            }
            else if (navigatorUser.Equals(Users.AdminRole.USER.ToString()))
            {
                if (Driver.IsElementPresent(Button_Home, 10) || (Driver.IsElementPresent(Button_Logout, 10)))
                    return true;
            }
            else
            {
                throw new Exception("Failed to match navigator user " + navigatorUser);
            }
            throw new Exception("Failed to match navigator user " + navigatorUser);
        }
    }
}
