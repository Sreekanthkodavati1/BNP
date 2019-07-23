using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;

namespace Bnp.Core.WebPages.MemberPortal
{
    /// <summary>
    /// This class handles Member Portal > Request Credit Page elements
    /// </summary>
    public class MemberPortal_RequestCredit : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public MemberPortal_RequestCredit(DriverContext driverContext)
       : base(driverContext)
        { }
        public enum TransactionType
        {
            Store,
            Online
        }
        #region
        private readonly ElementLocator TextBox_Store_TxnDate = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Store", "Txn Date");
        private readonly ElementLocator TextBox_Online_TxnDate = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "Txn Date");
        private readonly ElementLocator TextBox_Store_TxnNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Store", "Transaction Number");
        private readonly ElementLocator TextBox_Online_TxnNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "Transaction Number");
        private readonly ElementLocator TextBox_Store_StoreNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Store", "Store Number");
        private readonly ElementLocator TextBox_Online_StoreNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "Store Number");
        private readonly ElementLocator TextBox_Online_TxnAmount = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "Txn Amount");
        private readonly ElementLocator TextBox_Store_TxnAmount = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Store", "Txn Amount");
        private readonly ElementLocator TextBox_Online_TxnWebOrderNmbr = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "TxnWebOrderNmbr");
        private readonly ElementLocator Button_Store_Clear = Mp_MembeRequestCredit_Button_Custom_ElementLocatorXpath("SearchForm Store", "Clear");
        private readonly ElementLocator TextBox_Store_RegisterNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Store", "Register Number");
        private readonly ElementLocator TextBox_Online_RegisterNumber = MP_MembeRequestCredit_TextBox_ElementLocatorXpath("SearchForm Online", "Register Number");
        private readonly ElementLocator Button_Online_Clear = Mp_MembeRequestCredit_Button_Custom_ElementLocatorXpath("SearchForm Online", "Clear");
        private readonly ElementLocator Button_Store_Search = Mp_MembeRequestCredit_Button_Custom_ElementLocatorXpath("SearchForm Store", "Search");
        private readonly ElementLocator Button_Online_Search = Mp_MembeRequestCredit_Button_Custom_ElementLocatorXpath("SearchForm Online", "Search");
        private readonly ElementLocator Message_RequestProcessed = new ElementLocator(Locator.XPath, "//div[@id='Success']//span[contains(text(),'Request successfully processed')]");
        private readonly ElementLocator Message_NoResult = new ElementLocator(Locator.XPath, "//div[@id='NoResults']//span[contains(text(),'No transactions found')]");
        private readonly ElementLocator RadioButton_Online = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='Online']");
        private readonly ElementLocator RadioButton_Store = new ElementLocator(Locator.XPath, "//input[@type='radio' and @value='Store']");
        #endregion
  
        /// <summary>
        ///Search Based On TransactionDate for Store and Online
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransDate"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_BasedOnTransactionDate(string TransactionNumber, string TransDate, string transactionType, out string Message)
        {
            try
            {
                if (TransactionType.Online.ToString().Equals(transactionType))
                {
                    SelectRadioButton(RadioButton_Online);
                    EnterSearchCriteria_Online(TextBox_Online_TxnDate, TransDate);
                }
                else
                {
                    EnterSearchCriteria_Store(TextBox_Store_TxnDate, TransDate);
                }
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        /// <summary>
        ///Search Based On Transaction Number for Store or Online
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransDate"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_BasedOnTransactionNumber(string TransactionNumber, string transactionType, out string Message)
        {
            try
            {
                if (TransactionType.Online.ToString().Equals(transactionType))
                {
                    SelectRadioButton(RadioButton_Online);
                    EnterSearchCriteria_Online(TextBox_Online_TxnNumber, TransactionNumber);
                }
                else
                {
                    EnterSearchCriteria_Store(TextBox_Store_TxnNumber, TransactionNumber);
                }
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        /// <summary>
        ///Search Based On Transaction Number for Store or Online
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="transactionType"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_BasedOnStoreNumber(string TransactionNumber, string StoreNumber, string transactionType, out string Message)
        {
            try
            {
                if (TransactionType.Online.ToString().Equals(transactionType))
                {
                    SelectRadioButton(RadioButton_Online);
                    EnterSearchCriteria_Online(TextBox_Online_StoreNumber, TransactionNumber);
                }
                else
                {
                    EnterSearchCriteria_Store(TextBox_Store_StoreNumber, StoreNumber);
                }
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        /// <summary>
        ///Search Based On Txn Amount for Store or Online
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="transactionType"></param>
        /// <param name="RegisterNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool Search_BasedOnRegisterNumber(string TransactionNumber, string RegisterNumber, string transactionType, out string Message)
        {
            try
            {
                if (TransactionType.Online.ToString().Equals(transactionType))
                {
                    SelectRadioButton(RadioButton_Online);
                    EnterSearchCriteria_Online(TextBox_Online_RegisterNumber, TransactionNumber);
                }
                else
                {
                    EnterSearchCriteria_Store(TextBox_Store_RegisterNumber, RegisterNumber);
                }
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }


        /// <summary>
        /// Enter Search criteria (Store)
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
        /// Enter Search criteria (Online)
        /// </summary>
        /// <param name="SearchElement"></param>
        /// <param name="Search_Criteria"></param>
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

                ElementLocator TransactionWithSelectOption = new ElementLocator(Locator.XPath, TransactionSelect(TransactionNumber));
                if (Driver.IsElementPresent(TransactionWithSelectOption, 5))
                {
                    Driver.GetElement(TransactionWithSelectOption).ClickElement();
                    return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
                }
                else
                {
                    return VerifyTransactionSuccessMessage(TransactionNumber, out Message);
                }
                throw new Exception("Failed to SearchTransaction Number " + TransactionNumber);

            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number" + TransactionNumber);
            }
        }

        /// <summary>
        ///Customizing Select based on Transaction
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Customized Transaction String</returns>
        public string TransactionSelect(string Transaction)
        {
            string TransactionsString = "//span[text()='" + Transaction + "']//parent::span//following-sibling::a[contains(text(),'Request Credit')]";
            return TransactionsString;
        }

        /// <summary>
        /// Verify Request Processed Successfully message
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyTransactionSuccessMessage(string TransactionNumber, out string Message)
        {
            try
            {
                if (Driver.IsElementPresent(Message_RequestProcessed, 3))
                {
                    Driver.ScrollIntoMiddle(Message_RequestProcessed);
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
                throw new Exception("Failed to verify Transaction success mesage for Transaction number" + TransactionNumber);
            }
        }

        /// <summary>
        /// Search Based On Transaction Amount for Store and Online
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TransactionAmount"></param>
        /// <param name="transactionType"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool Search_BasedOnTransactionAmount(string TransactionNumber, string TransactionAmount, string transactionType, out string Message)
        {
            try
            {
                if (TransactionType.Online.ToString().Equals(transactionType))
                {
                    SelectRadioButton(RadioButton_Online);
                    EnterSearchCriteria_Online(TextBox_Online_TxnAmount, TransactionAmount);
                }
                else
                {
                    EnterSearchCriteria_Store(TextBox_Store_TxnAmount, TransactionAmount);
                }
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search based on Transaction Amount: " + TransactionAmount);
            }
        }
        /// <summary>
        /// Search based on Transaction web Order number
        /// </summary>
        /// <param name="TransactionNumber"></param>
        /// <param name="TxnWebOrderNmbr"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool Search_BasedOnTxnWebOrderNmbr(string TransactionNumber, string TxnWebOrderNmbr, out string Message)
        {
            try
            {
                SelectRadioButton(RadioButton_Online);
                EnterSearchCriteria_Online(TextBox_Online_TxnWebOrderNmbr, TxnWebOrderNmbr);
                return Select_TransactionFromTable(TransactionNumber, out Message);
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search based on TxnWebOrderNmbr: " + TxnWebOrderNmbr);
            }
        }
    }
}
