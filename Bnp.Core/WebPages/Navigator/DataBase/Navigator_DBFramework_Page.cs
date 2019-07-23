using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using System;
using System.Threading;


namespace Bnp.Core.WebPages.Navigator.Database
{
    public class Navigator_DBFramework_Page : ProjectBasePage
    {
        /// <summary>
        /// This class handles DB Page after login as DB Admin User
        /// </summary>
        public Navigator_DBFramework_Page(DriverContext driverContext)
        : base(driverContext)
        { }

        #region Element Locators
        private readonly ElementLocator ConnectionSuccessStatus = new ElementLocator(Locator.XPath, "//tr//td//span[contains(text(),'FrameworkDB')]//parent::td//following-sibling::td//span//img[@src='../image/approved.gif']");
        private readonly ElementLocator ExportDBCondfig_Dat = new ElementLocator(Locator.XPath, ".//a[contains(text(),'Export DBConfig.dat')]");

        private readonly ElementLocator CreateNewConnection_Button = new ElementLocator(Locator.XPath, "//div/a[contains(text(),'Create new connection')]");
        private readonly ElementLocator EditButton = new ElementLocator(Locator.XPath, "//tr//td//span[contains(text(),'FrameworkDB')]//parent::td//following-sibling::td//a[text()='Edit']");
        private readonly ElementLocator DBName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_Name')]");
        private readonly ElementLocator DBType = new ElementLocator(Locator.XPath, "//select[contains(@id,'_DBType')]");
        private readonly ElementLocator DBUserID = new ElementLocator(Locator.XPath, "//input[contains(@id,'_UserID')]");
        private readonly ElementLocator DBPassword = new ElementLocator(Locator.XPath, "//input[contains(@id,'Password')]");
        private readonly ElementLocator DB_DefaultSchema = new ElementLocator(Locator.XPath, "//input[contains(@id,'_DefaultSchema')]");
        private readonly ElementLocator DB_Server = new ElementLocator(Locator.XPath, "//input[contains(@id,'_Server')]");
        private readonly ElementLocator DB_Databases = new ElementLocator(Locator.XPath, "//input[contains(@id,'_Database')]");
        private readonly ElementLocator DB_ConnectionProps = new ElementLocator(Locator.XPath, "//input[contains(@id,'_ConnectionProps')]");
        private readonly ElementLocator Save_Button = new ElementLocator(Locator.XPath, "//a[text()='Save']");

        private readonly ElementLocator StandardAttributeSet = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlTotalDbManagement_cbCreateAttSets')]");
        private readonly ElementLocator StandardProgramSet = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlTotalDbManagement_cbCreateProgramData')]");

        private readonly ElementLocator InitializeFrameworkDB_Button = new ElementLocator(Locator.XPath, "//a[contains(text(),'Initialize FrameworkDB')]");
        private readonly ElementLocator VersionAfterInitialize = new ElementLocator(Locator.XPath, "//span[contains(@id,'_lblgrdSChemaHistoryVersionNumber')]");
        private readonly ElementLocator LoyaltyNavigator = new ElementLocator(Locator.XPath, "//span[contains(@id, '_lblgrdSChemaHistoryAppliedBy')]");
        #endregion

        /// <summary>
        /// This Method is used to Connect the DB
        /// </summary>
        /// <param name="DBName"></param>
        /// <param name="DBType"></param>
        /// <param name="DBUserID"></param>
        /// <param name="DBPassword"></param>
        /// <param name="DB_DefaultSchema"></param>
        /// <param name="DB_Server"></param>
        /// <param name="DB_Databases"></param>
        /// <param name="DB_ConnectionProps"></param>
        /// <returns>
        /// Output with boolean type as True on Successful connection of DB
        /// </returns>
        public bool CreatingDBConnection(string DBName, string DBType, string DBUserID, string DBPassword, string DB_DefaultSchema, string DB_Server, string DB_Databases, string DB_ConnectionProps, out string Output)
        {

            bool Connectionflag = false;
            if (Driver.IsElementPresent(ConnectionSuccessStatus, 1))
            {
                Output = "DB Connection already Avilable and Appeared as Succesful connection";
                Connectionflag = true;
            }
            else

            {
                Driver.GetElement(EditButton).ClickElement();
                Driver.ScrollIntoMiddle(this.DB_Server);
                Select _DBType = new Select(Driver.GetElement(this.DBType));
                _DBType.SelectByText(DBType);
                Driver.GetElement(this.DBUserID).SendText(DBUserID);
                Driver.GetElement(this.DBPassword).SendText(DBPassword);
                Driver.GetElement(this.DB_DefaultSchema).SendText(DB_DefaultSchema);
                Driver.GetElement(this.DB_Server).SendText(DB_Server);
                Driver.GetElement(this.DB_Databases).SendText(DB_Databases);
                Driver.GetElement(this.DB_ConnectionProps).SendText(DB_ConnectionProps);
                Driver.GetElement(Save_Button).ClickElement();
                Connectionflag = true;

            }

            if (Connectionflag == true && Driver.IsElementPresent(ConnectionSuccessStatus, 5))
            {
                Output = "DB Connection is Successful; Details are;DB UserID:" + DBUserID +
                                                                ";DBPassword:" + DBPassword +
                                                                ";DB DefaultSchema:" + DBPassword +
                                                                ";DB Server:" + DB_Server +
                                                                ";DB Databases:" + DB_Databases +
                                                                ";DB ConnectionProps" + DB_ConnectionProps;
                return true;
            }
            else
            {
                throw new Exception("New DB Connection is Failed Refer Screenshot for more information");
            }
        }

        /// <summary>
        /// This Method is used to Download DB File
        /// </summary>
        /// <param name="DBConfigfile"></param>
        /// <returns>
        /// Output with boolean type as True on Successful connection of DB
        /// </returns>
        public bool Download_DBConfigFile(string DBConfigfile, out string Message)
        {
            Driver.GetElement(ExportDBCondfig_Dat).ClickElement();
            Thread.Sleep(300);
            return VerifyExistedorDownloadedFile(DBConfigfile, "DBConfig.dat File Generated Successfully , and Downloaded path:" + DBConfigfile, out Message);
        }

        /// <summary>
        /// This Method is used to IntializeFramework DB
        /// </summary>
        /// <param name="StandardAttributeSet"></param>
        /// <param name="StandardProgramSet"></param>
        /// <returns>
        /// Output with boolean type as True on Successful connection of DB
        /// </returns>
        public bool InitializeFrameworkDB(bool StandardAttributeSet, bool StandardProgramSet, out string Output)
        {

            if (Driver.IsElementPresent(LoyaltyNavigator, 1))
            {
                string versionhistory = Driver.GetElement(VersionAfterInitialize).GetTextContent();
                Output = "DB Initialization is Completed before and available  version history:" + versionhistory;
               // Output = "New DB Initialization is Completed";
                return true;
            }
            else
            {
                Checkbox _StandardAttributeSet = new Checkbox(Driver.GetElement(this.StandardAttributeSet));
                if (StandardAttributeSet) { _StandardAttributeSet.TickCheckbox(); }
                Checkbox _StandardProgramSet = new Checkbox(Driver.GetElement(this.StandardProgramSet));
                if (StandardProgramSet) { _StandardProgramSet.TickCheckbox(); }
                Driver.GetElement(InitializeFrameworkDB_Button).ClickElement();
                try
                {
                    Driver.SwitchTo().Alert().Accept();
                }
                catch (NoAlertPresentException) { }
                Thread.Sleep(60000);
                Output = "New DB Initialization is Completed";
                return true;
                //if (Driver.IsElementPresent(LoyaltyNavigator, 14))
                //{
                //    string versionhistory = Driver.GetElement(VersionAfterInitialize).GetTextContent();
                //    Output = "DB Initialization is Completed Successfully and available  version history:" + versionhistory;
                //    return true;
                //}
                //else
                //{
                //    throw new Exception("DB Initialization Failed refer screenshot for more info");

                //}
            }

        }

    }
}
