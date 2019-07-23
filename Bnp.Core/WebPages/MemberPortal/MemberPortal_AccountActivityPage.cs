using BnPBaseFramework.Extensions;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Bnp.Core.WebPages.MemberPortal
{
    public class MemberPortal_AccountActivityPage : ProjectBasePage
    {
        public MemberPortal_AccountActivityPage(DriverContext driverContext)
     : base(driverContext)
        {
        }

        public enum AccountActivitySections
        {
            [DescriptionAttribute("Sales Transactions")]
            SalesTransactions,
            [DescriptionAttribute("Appeasements and Bonuses")]
            AppeasementsandBonuses
        }
        #region My Wallet Page Locators
        private readonly ElementLocator Header_SalesTransactions = MemberProfile_Header_Custom_ElementLocatorXpath(EnumUtils.GetDescription(AccountActivitySections.SalesTransactions));
        private readonly ElementLocator Header_AppeasementsandBonuses = MemberProfile_Header_Custom_ElementLocatorXpath(EnumUtils.GetDescription(AccountActivitySections.AppeasementsandBonuses));
        private readonly ElementLocator Section_SaleTransaction_Total = MemberProfile_Section_Custom_ElementLocatorXpath("Sales Transactions", "Total");
        private readonly ElementLocator Textbox_TransactionFromDate = MemberPortal_Textbox_Custom_ElementLocatorXpath("From Date: ");
        private readonly ElementLocator Textbox_TransactionToDate = MemberPortal_Textbox_Custom_ElementLocatorXpath("To Date: ");
        private readonly ElementLocator Button_Transaction_Search = MemberProfile_Button_Custom_ElementLocatorXpath("Sales Transactions", "Search");
        #endregion

        /// <summary>
        /// Method to verify Account Activity page
        /// </summary>
        /// <param name="strStatus"> return string status</param>
        /// <returns>True if page verified successfully, else throws exception</returns>
        public bool VerifyAccountActivityPage(out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Header_SalesTransactions, 5) && Driver.IsElementPresent(Header_AppeasementsandBonuses, 5))
                {
                    strStatus = "Successfully verified Account Activity page";
                    return true;
                }
                else
                {
                    throw new Exception("Failed to verify Account Activity page");
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify Account Activity page");
            }
        }
        /// <summary>
        /// To verify Sales Transactions Section
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public bool VerifySalesTransactionsSection(string attributeName, string attributeValue, out string strStatus)
        {
            try
            {
                if (Driver.IsElementPresent(Section_SaleTransaction_Total, .5))
                {
                    string TransactionNumber = "//a[text()='Sales Transactions']//ancestor::div//following::span[text()='" + attributeName + "']//following-sibling::span[text()='" + attributeValue + "']";
                    if (Driver.IsElementPresent(By.XPath(TransactionNumber)))
                    {
                        ElementLocator Transaction = new ElementLocator(Locator.XPath, TransactionNumber);
                        Driver.ScrollIntoMiddle(Transaction);
                        strStatus = "Sales Transactions Successfully verified With Transaction HeaderId : " + attributeValue;
                        return true;
                    }
                    else
                        throw new Exception("Failed to verify Sales Transaction" + attributeValue);
                }
                throw new Exception("Failed to verify Sales Transaction Section");
            }
            catch (Exception)
            {
                throw new Exception("Failed to verify Sales Transaction Section");
            }
        }

        /// <summary>
        /// Select specific date to verify Sales Transactions
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        public void SelectDate_RC(DateTime FromDate, DateTime ToDate)
        {
            Driver.GetElement(Textbox_TransactionFromDate).SendText(FromDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            Driver.GetElement(Textbox_TransactionToDate).SendText(ToDate.ToString("MM/dd/yyyy", new CultureInfo("en-US")));
            Driver.GetElement(Textbox_TransactionToDate).SendKeys(Keys.Tab);
            Click_OnButton(Button_Transaction_Search);
        }
    }
}