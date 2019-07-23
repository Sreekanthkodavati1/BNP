using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bnp.Core.WebPages.CSPortal
{
    /// <summary>
    /// This class handles Customer Service Portal > Member Search> Request Credit Page
    /// </summary
    public class CSPortal_RequestCredit : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public CSPortal_RequestCredit(DriverContext driverContext)
       : base(driverContext)
        { }

        #region ElementLoactors
        public string ResultTable_Count = "//th[contains(text(),'Actions')]//ancestor::table[1]";
        public string ResultTable = "//th[contains(text(),'Actions')]//ancestor::table[1]//tr";
        private readonly ElementLocator Button_Store_Clear = MembeRequestCredit_Button_Custom_Store_ElementLocatorXpath("Clear");
        private readonly ElementLocator TextBox_Store_TransactionNumber = MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath("Transaction Number");
        private readonly ElementLocator TextBox_Store_StoreNumber = MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath("Store Number");
        private readonly ElementLocator TextBox_Store_TxnDate = MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath("Txn Date");
        private readonly ElementLocator TextBox_Store_TxnAmount = MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath("Txn Amount");
        private readonly ElementLocator TextBox_Store_RegisterNumber = MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath("Register Number");
        private readonly ElementLocator Button_Store_Search = MembeRequestCredit_Button_Custom_Store_ElementLocatorXpath("Search");

        private readonly ElementLocator Button_Online_Clear = MembeRequestCredit_Button_Custom_Online_ElementLocatorXpath("Clear");
        private readonly ElementLocator TextBox_Online_TransactionNumber = MembeRequestCredit_TextBox_Custom_Online_ElementLocatorXpath("Transaction Number");
        private readonly ElementLocator TextBox_Online_TxnDate = MembeRequestCredit_TextBox_Custom_Online_ElementLocatorXpath("Txn Date");
        private readonly ElementLocator TextBox_Online_TxnAmount = MembeRequestCredit_TextBox_Custom_Online_ElementLocatorXpath("Txn Amount");
        private readonly ElementLocator TextBox_Online_OrderNumber = MembeRequestCredit_TextBox_Custom_Online_ElementLocatorXpath("Order Number");

        private readonly ElementLocator Button_Online_Search = MembeRequestCredit_Button_Custom_Online_ElementLocatorXpath("Search");

        private readonly ElementLocator Message_RequestProcessed = new ElementLocator(Locator.XPath, "//div[@id='Success']//span[contains(text(),'Request successfully processed')]");
        private readonly ElementLocator Message_NoResult = new ElementLocator(Locator.XPath, "//div[@id='NoResults']//span[contains(text(),'No transactions found')]");
        private readonly ElementLocator Checkbox_Online = new ElementLocator(Locator.XPath, "//label[contains(text(),'Online')]//input");


        #endregion

        /// <summary>
        ///Customizing Select based on Transaction
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Customized Transaction String</returns>
        public string TransactionSelect(string Transaction)
        {
            string TransactionsString = "//td//span[text()='" + Transaction + "']//parent::td//parent::tr//td//a[text()='Select']";
            return TransactionsString;
        }
        /// <summary>
        ///Search Based On Select_Online
        /// </summary>
        public void Select_Online()
        {
            try
            {
                CheckBoxElmandCheck(Checkbox_Online);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Click on Online buttons");
            }
        }


        /// <summary>
        /// Enter Search criteria  (Store)
        /// </summary>
        /// <param name="SearchElement"></param>
        /// <param name="Search_Criteria"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public void EnterSearchCriteria_Store(ElementLocator SearchElement, string Search_Criteria)
        {
            try
            {
                Click_OnButton(Button_Store_Clear);
                Driver.GetElement(SearchElement).SendText(Search_Criteria);
                Click_OnButton(Button_Store_Search);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Values");
            }
        }

        /// <summary>
        /// Enter Search criteria  (Store)
        /// </summary>
        /// <param name="SearchElement"></param>
        /// <param name="Search_Online"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public void EnterSearchCriteria_Online(ElementLocator SearchElement, string Search_Criteria)
        {
            try
            {
                Click_OnButton(Button_Online_Clear);
                Driver.GetElement(SearchElement).SendText(Search_Criteria);
                Click_OnButton(Button_Online_Search);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Enter Values");
            }
        }

        /// <summary>
        ///Search Based On TransactionNumber
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Select_TransactionFromTable(string TransactionNumber, out string Message)
        {
            try
            {
                string Table = "//td[@colspan='6']//table//td//a";
                ElementLocator Web_ResultTable = new ElementLocator(Locator.XPath, Table);

                bool Avaiod_doublclick = false;
                if (Driver.IsElementPresent(Web_ResultTable,1))
                {
                    List<IWebElement> TableValues = new List<IWebElement>(Driver.FindElements(By.XPath(Table)));
                    int TableCount = TableValues.Count();
                    int firstimevalue = 2;

                    Driver.FindElement(By.XPath(Table)).ScrollToElement();
                    for (int RowValue = 0; RowValue <= TableCount; RowValue++)
                    {
                        Driver.FindElement(By.XPath(Table)).ScrollToElement();
                        TableValues = new List<IWebElement>(Driver.FindElements(By.XPath(Table)));
                        Avaiod_doublclick = false;
                          ElementLocator TransactionWithSelectOption = new ElementLocator(Locator.XPath, TransactionSelect(TransactionNumber));
                        if (Driver.IsElementPresent(TransactionWithSelectOption, 1))
                        {
                            Driver.GetElement(TransactionWithSelectOption).ClickElement();
                            return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
                        }
                        if (TableValues[RowValue].GetTextContent().Contains("..."))
                        {
                            int currentpagevalue = int.Parse(TableValues[RowValue - 1].GetTextContent());
                            Driver.FindElement(By.XPath("//td[@colspan]//table//td//a[text()='...']")).ClickElement();
                            List<IWebElement>  TableValues1 = new List<IWebElement>(Driver.FindElements(By.XPath(Table)));
                            RowValue = 3;
                            TableCount = TableValues1.Count;
                            Avaiod_doublclick = true;
                            firstimevalue = firstimevalue + 1;
                        }
                        if (Avaiod_doublclick == false)
                        {
                            Driver.FindElement(By.XPath("//td[@colspan]//table//td//a[text()='" + firstimevalue + "']")).ClickElement();
                            firstimevalue++;
                        }
                    }
                }
                else
                {
                    return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
                }
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        #region SearchBased on Transaction Number

        /// <summary>
        ///Search Based On TransactionNumber (Online)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Online_BasedTransactionNumber(string TransactionNumber, out string Message)
        {
            try
            {
                EnterSearchCriteria_Online(TextBox_Online_TransactionNumber, TransactionNumber);
                return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        /// <summary>
        ///Search Based On TransactionNumber (Store)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Store_BasedTransactionNumber(string TransactionNumber, out string Message)
        {
            try
            {
                EnterSearchCriteria_Store(TextBox_Store_TransactionNumber, TransactionNumber);
                return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        #endregion

        #region SearchBased on TransactionDate
        /// <summary>
        ///Search Based On TransactionDate (Store)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransDate"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Store_BasedOnTransactionDate(string TransactionNumber, string TransDate, out string Message)
        {
            try
            {
                EnterSearchCriteria_Store(TextBox_Store_TxnDate, TransDate);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        /// <summary>
        ///Search Based On TransactionDate (Online)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransDate"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Online_BasedOnTransactionDate(string TransactionNumber, string TransDate, out string Message)
        {
            try
            {
                EnterSearchCriteria_Online(TextBox_Online_TxnDate, TransDate);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        #endregion

        #region SearchBased on TransactionAmount

        /// <summary>
        ///Search Based On TransactionAmount (Store)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransAmount"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Store_BasedOnTransactionAmount(string TransactionNumber, string TransAmount, out string Message)
        {
            try
            {
                EnterSearchCriteria_Store(TextBox_Store_TxnAmount, TransAmount);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        /// <summary>
        ///Search Based On TransactionDate (Online)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransAmount"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Online_BasedOnTransactionAmount(string TransactionNumber, string TransAmount, out string Message)
        {
            try
            {
                EnterSearchCriteria_Online(TextBox_Online_TxnAmount, TransAmount);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        #endregion

        #region SearchBased on RegisterNumber

        /// <summary>
        ///Search Based On TransactionAmount (Store)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="RegisterNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Store_BasedOnRegisterNumber(string TransactionNumber, string RegisterNumber, out string Message)
        {
            try
            {
                EnterSearchCriteria_Store(TextBox_Store_RegisterNumber, RegisterNumber);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        #endregion

        /// <summary>
        ///Search Based On StoreNumber (Store)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="StoreNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Store_BasedOnStoreNumber(string TransactionNumber, string StoreNumber, out string Message)
        {
            try
            {
                EnterSearchCriteria_Store(TextBox_Store_StoreNumber, StoreNumber);
                return Select_TransactionFromTable(TransactionNumber, out Message);

            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Transaction Number" + TransactionNumber);
            }
        }
        /// <summary>
        ///Search Based On OrderNumber (Online)
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="OrderNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_Online_BasedOnOrderNumber(string TransactionNumber, string OrderNumber, out string Message)
        {
            try
            {
                EnterSearchCriteria_Online(TextBox_Online_OrderNumber, OrderNumber);
                return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
        /// <summary>
        /// Verify Request Processed Successfully Messge
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyTransactionSuccessMessage(string TransactionNumber, out string Message)
        {
            try
            {
                if (Driver.IsElementPresent(Message_RequestProcessed, 3))
                {
                    Driver.GetElement(Message_RequestProcessed).ScrollToElement();
                    Message = "Transaction Processed Successfully:" + TransactionNumber;
                    return true;
                }
                else if (Driver.IsElementPresent(Message_NoResult, 2))
                {
                    Message = "No Records Available Message appeared On Search of Transaction Number: " + TransactionNumber;
                    throw new Exception(Message);
                }
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }
    }
}
