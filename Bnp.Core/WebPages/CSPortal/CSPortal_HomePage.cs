using BnPBaseFramework.Extensions;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Bnp.Core.WebPages.CSPortal
{
    public class CSPortal_HomePage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_HomePage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Element Locators
        private readonly ElementLocator Title_Home = new ElementLocator(Locator.XPath, "//title[contains(text(),'Member Search')]");
        private readonly ElementLocator MemberRegistration = new ElementLocator(Locator.XPath, "//a[text()='Member Registration']");
        public ElementLocator Button_Logout = new ElementLocator(Locator.XPath, "//a[text()='Logout']");

        #endregion

        public enum DashBoard
        {
            [DescriptionAttribute("Member Search")]
            MemberSearch,
            [DescriptionAttribute("Member Registration")]
            MemberRegistration,
            [DescriptionAttribute("User Administration")]
            UserAdministration,
            [DescriptionAttribute("Change Password")]
            ChangePassword,
            [DescriptionAttribute("Account Summary")]
            AccountSummary,
            [DescriptionAttribute("Update Profile")]
            UpdateProfile,
            [DescriptionAttribute("Account Activity")]
            AccountActivity,
            [DescriptionAttribute("Customer Appeasements")]
            CustomerAppeasements,
            [DescriptionAttribute("Merge Accounts")]
            MergeAccounts,
            [DescriptionAttribute("Request Credit")]
            RequestCredit,
            [DescriptionAttribute("Contact History")]
            ContactHistory
        }

        /// <summary>
        /// Returning Menu Board
        /// </summary>
        /// <param name="Menu"></param>
        public string DashBoardMenuLink(string Menu)
        {
            string CustomMenuLink = "//ul[@class='CSDashboardContainer']//a[text()='" + Menu + "']";
            return CustomMenuLink;
        }

        /// <summary>
        /// Returning Menu Board Locator
        /// </summary>
        /// <param name="Menu"></param>
        public ElementLocator DashBoardMenu(string Menu)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, DashBoardMenuLink(Menu));
            return Button_Custom_ElementLocatorXpath;
        }
        public ElementLocator HeaderOnPage(string Profile, string title)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, "//div[@id='" + Profile + "']//h2[contains(text(),'" + title + "')]");
            return Button_Custom_ElementLocatorXpath;
        }


        public bool VerifyHeaderOnPage(string Profile_Container, string _Header, out string Message)
        {
            if (Driver.IsElementPresent(HeaderOnPage(Profile_Container, _Header), 1))
            {
                Message = "Navigate to " + _Header + " Page is Successful ; Title Appeared as:" + _Header;
                return true;
            }
            throw new Exception("Failed to Navigate to:" + _Header);
        }

        public bool VerifyExpectedDashBoardlinksOnMainPage(string[] Dashboardlinks, out string Message)
        {
            List<IWebElement> AllDashBoradLinks = new List<IWebElement>(Driver.FindElements(By.XPath("//ul[@class='CSDashboardContainer']//li//a")));
            if (Dashboardlinks.Count() == AllDashBoradLinks.Count)
            {
                string[] ApplicationDashboardlinks = new string[Dashboardlinks.Count()];
                string[] ExpectedDashboarlinks = Dashboardlinks.ToArray();
                for (int i = 0; i < AllDashBoradLinks.Count; i++)
                {
                    ApplicationDashboardlinks[i] = AllDashBoradLinks[i].Text;
                }
                if (ApplicationDashboardlinks.SequenceEqual(Dashboardlinks))
                {
                    Message = "Available Dash Board Links:;" + String.Join(";", ApplicationDashboardlinks) + "; Are Matching With Expected Dashboard Links:;" + String.Join(";", ExpectedDashboarlinks);
                    return true;
                }
                throw new Exception("Available Dash Board Links:;" + String.Join("\n", ApplicationDashboardlinks) + " ;Are Mis Matching With Expected Dashboard Links:;" + String.Join("\n", ExpectedDashboarlinks));
            }

            throw new Exception("Available Dash Board Links Count:" + AllDashBoradLinks.Count + " is Mis Matching With Expected Dashboard Links Count:" + Dashboardlinks.Count());

        }

        public bool VerifyDashBoardSingleLinkPresent(string Dashboardlinks, bool LinksPresent, out string Message)
        {
            List<IWebElement> AllDashBoradLinks = new List<IWebElement>(Driver.FindElements(By.XPath("//ul[@class='CSDashboardContainer']//li//a")));
            string[] ApplicationDashboardlinks = new string[Dashboardlinks.Count()];
            for (int i = 0; i < AllDashBoradLinks.Count; i++)
                {
                    ApplicationDashboardlinks[i] = AllDashBoradLinks[i].Text;
                }
            for (int i = 0; i < AllDashBoradLinks.Count; i++)
            {
                if (Dashboardlinks.Contains(AllDashBoradLinks[i].Text))
                {
                    if (LinksPresent)
                    {
                        Message = "Available Dash Board Links are :;" + String.Join(";", ApplicationDashboardlinks) + "; and Expected Dashboard Link As Expected:;" + String.Join(";", Dashboardlinks);
                        return true;
                    }
                    else
                    {
                        throw new Exception("Available Dash Board Links:;" + String.Join("\n", ApplicationDashboardlinks) + " ;and Dashboard Link:;" + String.Join("\n", Dashboardlinks)+" is not Present As Expected");
                    }
                }
            }
            if (!LinksPresent)
            {
                Message = "Available Dash Board Links:;" + String.Join(";", ApplicationDashboardlinks) + "; Are Not Present on Dash  Dashboard Links as Expected:;" + String.Join(";", Dashboardlinks);
                return true;
            }
            throw new Exception("Available Dash Board Links Count:" + AllDashBoradLinks.Count + " is Mis Matching With Expected Dashboard Links Count:" + Dashboardlinks.Count());
        }

        /// <summary>
        /// Navigating To DashBoard Menu 
        /// </summary>
        /// <param name="Menu"></param>
        /// <param name="Message"></param>
        public bool NavigateToDashBoardMenu(DashBoard Menu, out string Message)
        {
            try
            {
                Message = "";
                switch (Menu)
                {
                    case DashBoard.MemberSearch:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberSearch))).ClickElement();
                        VerifyHeaderOnPage("MemberSearchContainer", EnumUtils.GetDescription(DashBoard.MemberSearch), out Message);
                        break;
                    case DashBoard.MemberRegistration:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.MemberRegistration))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.MemberRegistration), out Message);
                        break;
                    case DashBoard.UserAdministration:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.UserAdministration))).ClickElement();
                        VerifyHeaderOnPage("CSUserAdministrationContainer", "Agent Administration", out Message);
                        break;
                    case DashBoard.ChangePassword:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.ChangePassword))).ClickElement();
                        VerifyHeaderOnPage("ChangePasswordContainer", EnumUtils.GetDescription(DashBoard.ChangePassword), out Message);
                        break;
                    case DashBoard.AccountSummary:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.AccountSummary))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.AccountSummary), out Message);
                        break;
                    case DashBoard.UpdateProfile:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.UpdateProfile))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.UpdateProfile), out Message);
                        break;
                    case DashBoard.AccountActivity:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.AccountActivity))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.AccountActivity), out Message);
                        break;
                    case DashBoard.CustomerAppeasements:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.CustomerAppeasements))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", "Appeasements", out Message);
                        break;
                    case DashBoard.MergeAccounts:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.MergeAccounts))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", "Merge Members", out Message);
                        break;
                    case DashBoard.RequestCredit:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.RequestCredit))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.RequestCredit), out Message);
                        break;
                    case DashBoard.ContactHistory:
                        Driver.GetElement(DashBoardMenu(EnumUtils.GetDescription(DashBoard.ContactHistory))).ClickElement();
                        VerifyHeaderOnPage("MemberProfileContainer", EnumUtils.GetDescription(DashBoard.ContactHistory), out Message);
                        break;
                }

                return true;
            }
            catch
            {
                throw new Exception("Failed to Navigate to " + Menu + " Page");
            }
        }

        /// <summary>
        ///Verify First Name and last Name after login
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        public string VerifyFirstNameAndLastName(string FirstName, string LastName)
        {
            try
            {
                By VerifyFirstName_Last = By.XPath("//div[@class='user']//span[contains(text(),'" + FirstName + " " + LastName + "')]");
                if (Driver.IsElementPresent(VerifyFirstName_Last))
                {
                    return "First name: " + FirstName + ";Last Name: " + LastName + " ;Appeared on the Screen";
                }
                else
                {
                    throw new Exception("First name: " + FirstName + ";Last Name: " + LastName + ";Not Appeared on the Screen");
                }
            }
            catch (Exception)
            {
                throw new Exception("Login failed refer screenshot for more info");
            }
        }
        /// <summary>
        ///Logout CS portal
        /// </summary>
        public void LogoutCSPortal()
        {
            try
            {
                Click_OnButton(Button_Logout);
            }
            catch (Exception)
            {
                throw new Exception("Logout failed refer screenshot for more info");
            }
        }
        public TestStep LogoutCSPortal(List<TestStep> listOfTestSteps)
        {
            string stepName = "Logout Customer Service Portal URL";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                LogoutCSPortal();
                testStep.SetOutput("Logout Customer Service Portal is Successful");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput("Failed to Logout Customer Service Portal Due to:" + e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep NavigateToDashBoardMenu(DashBoard Menu, List<TestStep> listOfTestSteps)
        {
            string stepName = "Navigate to Menu:" + Menu;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                NavigateToDashBoardMenu(Menu, out string MenuMessage);
                testStep.SetOutput(MenuMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput("Failed to Click on or Navigate to Menu  Due to:" + e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep VerifyExpectedDashBoardlinksOnMainPage(string[] Dashboardlinks, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify DashBoard Menu Links";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyExpectedDashBoardlinksOnMainPage(Dashboardlinks, out string MenuMessage);
                testStep.SetOutput(MenuMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput("Failed to Verify DashBoard Menu Links  Due to:" + e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

        public TestStep VerifyDashBoardSingleLinkPresent(string Dashboardlinks,bool link_status, List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify DashBoard Menu Link";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerifyDashBoardSingleLinkPresent(Dashboardlinks, link_status,out string MenuMessage);
                testStep.SetOutput(MenuMessage);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput("Failed to Verify DashBoard Menu Links Due to:" + e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}
