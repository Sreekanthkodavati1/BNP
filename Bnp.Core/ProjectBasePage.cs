using Bnp.Core.Tests.API.CDIS_Services.CDIS_Methods;
using Bnp.Core.Tests.API.Enums;
using Bnp.Core.Tests.API.Validators;
using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator;
using Bnp.Core.WebPages.Navigator.UsersPage.Program;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.Components;
using Bnp.Core.WebPages.Navigator.UsersPage.Program.eCollateral;
using BnPBaseFramework.Reporting.Base;
using BnPBaseFramework.Reporting.Utils;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using Brierley.LoyaltyWare.ClientLib.DomainModel.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bnp.Core
{
    public class ProjectBasePage
    {
        public ProjectBasePage(DriverContext driverContext)
        {
            DriverContext = driverContext;
            Driver = driverContext.Driver;
        }
        protected IWebDriver Driver { get; set; }
        protected DriverContext DriverContext { get; set; }
        protected string TestTitle { get; set; }

        public static string Orgnization_value => GetOrgInfo("Organization");
        public static string testOrgnization_value => GetOrgInfo("TestOrganization");
        public static string Env_value => GetOrgInfo("Environment");
        public static string Env_position => GetOrgInfo("Order ID");
        public static string ConfigFileUploadPath => GetOrgInfo("ConfigFileUploadPath");
        public static string GenerateClientAssembly_value => GetOrgInfo("GenerateClientAssembly");

        public TestCase testCase;
        public TestStep testStep;

        //public static string Product
        //{
        //    get
        //    {
        //        //return ConfigurationManager.AppSettings["GenerateClientAssembly"];
        //    }
        //}

        #region Common element locators        
        private readonly ElementLocator orgnizationName_expand = new ElementLocator(Locator.XPath, ".//span[normalize-space() = '" + Orgnization_value + "']//preceding-sibling::span[@class='rtPlus']");
        private readonly ElementLocator organizationNode = new ElementLocator(Locator.XPath, ".//span[text()='" + Orgnization_value + "']");
        private readonly ElementLocator env_Name = new ElementLocator(Locator.XPath, "//span[text()='" + Orgnization_value + "']//parent::div[contains(@class,'rtSelected')]//following-sibling::ul//span[contains(text(),'" + Env_value + "')]");
        private readonly ElementLocator org_Selected = new ElementLocator(Locator.XPath, "//span[text()='" + Orgnization_value + "']//parent::div[contains(@class,'rtSelected')]");
        private readonly ElementLocator env_Selected = new ElementLocator(Locator.XPath, "//span[text()='" + Orgnization_value + "']//parent::div//following-sibling::ul//span[contains(text(),'" + Env_value + "')]//parent::div[contains(@class,'rtSelected')]");

        private readonly ElementLocator testOrgnizationName_expand = new ElementLocator(Locator.XPath, ".//span[normalize-space() = '" + testOrgnization_value + "']//preceding-sibling::span[@class='rtPlus']");
        private readonly ElementLocator testOrganizationNode = new ElementLocator(Locator.XPath, ".//span[text()='" + testOrgnization_value + "']");
        private readonly ElementLocator testEnv_Name = new ElementLocator(Locator.XPath, "//span[text()='" + testOrgnization_value + "']//parent::div[contains(@class,'rtSelected')]//following-sibling::ul//span[contains(text(),'" + Env_value + "')]");
        private readonly ElementLocator testOrg_Selected = new ElementLocator(Locator.XPath, "//span[text()='" + testOrgnization_value + "']//parent::div[contains(@class,'rtSelected')]");
        private readonly ElementLocator testEnv_Selected = new ElementLocator(Locator.XPath, "//span[text()='" + testOrgnization_value + "']//parent::div//following-sibling::ul//span[contains(text(),'" + Env_value + "')]//parent::div[contains(@class,'rtSelected')]");


        #endregion
        public bool DrillDownOrg(out string Message)
        {
            if (Driver.IsElementPresent(env_Selected, 5))
            {
                Message = "Organization is available  and selecting organization  is successful , Organization Details:" + Orgnization_value;
                return true;
            }
            else
            {
                if (Driver.IsElementPresent(orgnizationName_expand, 5))
                {
                    Message = "Organization is available , Organization Details:" + Orgnization_value;
                }
                else
                {
                    throw new Exception("Organization is not available  , Organization Details:" + Orgnization_value);
                }

                Driver.GetElement(orgnizationName_expand).ClickElement();
                Driver.GetElement(organizationNode).ClickElement();

                if (Driver.IsElementPresent(org_Selected, 5))
                {
                    Message = "Organization is available  and selecting organization  is successful , Organization Details:" + Orgnization_value;
                    return true;
                }
                else
                {
                    throw new Exception("Organization is not available  , Organization Details:" + Orgnization_value);

                }
            }
        }

        public bool DrillDownTestOrg(out string Message)
        {
            if (Driver.IsElementPresent(testEnv_Selected, 5))
            {
                Message = "Organization is available  and selecting organization  is successful , Organization Details:" + testOrgnization_value;
                return true;
            }
            else
            {
                if (Driver.IsElementPresent(testOrgnizationName_expand, 5))
                {
                    Message = "Organization is available , Organization Details:" + testOrgnization_value;
                }
                else
                {
                    throw new Exception("Organization is not available  , Organization Details:" + testOrg_Selected);
                }

                Driver.GetElement(testOrgnizationName_expand).ClickElement();
                Driver.GetElement(testOrganizationNode).ClickElement();

                if (Driver.IsElementPresent(testOrg_Selected, 5))
                {
                    Message = "Organization is available  and selecting organization  is successful , Organization Details:" + testOrgnization_value;
                    return true;
                }
                else
                {
                    throw new Exception("Organization is not available  , Organization Details:" + testOrgnization_value);

                }
            }
        }


        public bool SelectEnvironment(out string Message)
        {
            if (Driver.IsElementPresent(env_Selected, 5))
            {
                Message = "Environment is available  and selecting Environment is successful , Environment Details:" + Env_value; return true;
            }
            else
            {

                Driver.GetElement(env_Name).ClickElement();
                if (Driver.IsElementPresent(env_Selected, 2))
                {
                    Message = "Environment is available  and selecting Environment is successful , Environment Details:" + Env_value; return true;
                }
                else
                {
                    throw new Exception("Environment is not available  , Environment Details:" + Env_value);
                }
            }
        }

        public bool SelectTestEnvironment(out string Message)
        {
            if (Driver.IsElementPresent(testEnv_Selected, 5))
            {
                Message = "Environment is available  and selecting Environment is successful , Environment Details:" + Env_value; return true;
            }
            else
            {

                Driver.GetElement(testEnv_Name).ClickElement();
                if (Driver.IsElementPresent(testEnv_Selected, 2))
                {
                    Message = "Environment is available  and selecting Environment is successful , Environment Details:" + Env_value; return true;
                }
                else
                {
                    throw new Exception("Environment is not available  , Environment Details:" + Env_value);
                }
            }
        }


        public string ConfigDownloadPath
        {
            get
            {
                return BnPBaseFramework.Web.Helpers.FilesHelper.GetFolder(BaseConfiguration.DownloadFolder, Directory.GetCurrentDirectory());
            }
        }
        public string ConfigUploadPath
        {
            get
            {
                string _configUploadPath = ConfigFileUploadPath + @"\" + Orgnization_value + "_" + Env_value;
                return _configUploadPath;
            }
        }

        public void CreateOrVerfiyConfigFolder()
        {
            try
            {
                bool folderExists = Directory.Exists(ConfigUploadPath);
                if (!folderExists)
                {
                    Directory.CreateDirectory(ConfigUploadPath);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Create Folder ;Folder Details:" + ConfigUploadPath);
            }
        }


        public void CopyFile(string sourcefile, string destinationfile)
        {
            try
            {
                DeleteExistedFile(destinationfile);
                System.IO.File.Copy(sourcefile, destinationfile);
            }
            catch (Exception)
            {
                throw new Exception("Copying from" + sourcefile + " is failed" + destinationfile);
            }
        }

        public bool VerifyExistedorDownloadedFile(string FileNameAlongWithPath, string Description, out string stepDetails)
        {
            if (File.Exists(FileNameAlongWithPath))
            {
                stepDetails = Description;
                return true;
            }
            else
            {
                throw new Exception("Failed to Verify File" + FileNameAlongWithPath);
            }
        }

        /// <summary>
        /// This Method is used to Verify file with starting letter of file and extension of file
        /// </summary>
        /// <param name="FileNameAlongWithPath">Message Name</param>
        /// <param name="fileFormate">Message Name</param>
        /// <param name="Description">Message Name</param>
        /// <returns>
        /// returns true if file is present in download folder else throw an exception
        /// </returns>
        public bool VerifyExistedorDownloadedFileStartingWith(string FileNameAlongWithPath, string fileFormate, string Description, out string stepDetails)
        {
            DirectoryInfo di = new DirectoryInfo(FileNameAlongWithPath);
            FileInfo[] TXTFiles = di.GetFiles(fileFormate);
            if (TXTFiles.Length == 0)
            {
                throw new Exception("Failed to Verify File" + FileNameAlongWithPath);
            }

            //We can use both

            //foreach (var fi in TXTFiles)
            //    if (fi.Exists)
            //    {
            //        stepDetails = Description;
            //        return true;
            //    }
            bool exist = Directory.EnumerateFiles(FileNameAlongWithPath, fileFormate).Any();
            if (exist)
            {
                stepDetails = Description;
                return true;
            }
            else
            {
                throw new Exception("Failed to Verify File" + FileNameAlongWithPath);
            }
        }

        /// <summary>
        /// This Method is used to Delete Existed File
        /// </summary>
        /// <param name="FileNameAlongWithPath">Message Name</param>
        /// <returns>
        /// None
        /// </returns>
        public void DeleteExistedFile(string FileNameAlongWithPath)
        {
            try
            {
                if (File.Exists(FileNameAlongWithPath))
                {
                    File.Delete(FileNameAlongWithPath);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Delete Existed File:" + FileNameAlongWithPath);
            }
        }

        public bool VerifyTableRowsBasedOnHeader(string Table, string TableHeadersXpath, string header, string value, out string Message)
        {
            try
            {

                Driver.FindElement(By.XPath(Table)).ScrollToElement();
                //List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.TagName("th//a")));
                List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.XPath(TableHeadersXpath)));
                for (int ColumnValue = 0; ColumnValue < TableHeader.Count; ColumnValue++)
                {
                    string HeaderElem = TableHeader[ColumnValue].Text;
                    if (HeaderElem.Contains(header))
                    {
                        ColumnValue++;
                        List<IWebElement> TableRows = new List<IWebElement>(Driver.FindElements(By.XPath(Table + "//td[" + ColumnValue + "]//span")));
                        foreach (IWebElement elem in TableRows)
                        {

                            if (elem.Text.Contains(value))
                            {
                                //For loops returns the Pass condition
                            }
                            else
                            {
                                throw new Exception("Header Details:" + header + ":Provided Input and Expected Values are not matching, Actual Result:" + elem.Text + "Expected Result:" + value);
                            }

                            Message = header + ": Provided Input and Expected Values are  matching, Actual Result:" + elem.Text + "  Expected Result:" + value;
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to verify Header Details: " + header + " And Result as:" + value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed due to:" + e);
            }
        }

        /// <summary>
        /// This Method is used to Verify Multiple row of table
        /// </summary>
        /// <param name="Table">Message Name</param>
        /// <param name="TableHeadersXpath">Message Name</param>
        /// <param name="header">Message Name</param>
        /// <param name="value">Message Name</param>
        /// <returns>
        /// return true if value exists in row else throw exception
        /// </returns>
        public bool VerifyTableRowsBasedOnHeaderId(string Table, string TableHeadersXpath, string header, string value, out string Message)
        {
            try
            {
                Driver.FindElement(By.XPath(Table)).ScrollToElement();
                //List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.TagName("th//a")));
                List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.XPath(TableHeadersXpath)));
                for (int ColumnValue = 0; ColumnValue < TableHeader.Count; ColumnValue++)
                {
                    string HeaderElem = TableHeader[ColumnValue].Text;
                    if (HeaderElem.Contains(header))
                    {
                        ColumnValue++;
                        List<IWebElement> TableRows = new List<IWebElement>(Driver.FindElements(By.XPath(Table + "//td[" + ColumnValue + "]//span")));
                        foreach (IWebElement elem in TableRows)
                        {
                            if (elem.Text.Contains(value))
                            {
                                //For loops returns the Pass condition
                                Message = header + ": Provided Input and Expected Values are  matching, Actual Result:" + elem.Text + "  Expected Result:" + value;
                                return true;
                            }
                        }
                    }
                }
                throw new Exception("Failed to verify Header Details: " + header + " And Result as:" + value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed due to:"+e.Message);
            }
        }

        public bool VerifyTableRowsBasedOnHeader_ExlusiveForNoInputs(string Table, string header, string value, out string Message)
        {
            try
            {
                List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.TagName("th")));

                for (int ColumnValue = 0; ColumnValue < TableHeader.Count; ColumnValue++)
                {
                    string HeaderElem = TableHeader[ColumnValue].Text;
                    if (HeaderElem.Contains(header))
                    {
                        ColumnValue++;
                        List<IWebElement> TableRows = new List<IWebElement>(Driver.FindElements(By.XPath(Table + "//td[" + ColumnValue + "]//span")));
                        foreach (IWebElement elem in TableRows)
                        {
                            if (elem.Text != null)
                            {
                                //For loops returns the Pass condition
                            }
                            else
                            {
                                throw new Exception("Header Details:" + header + ": Values appeared is not Matching with Provided input, Actual Result:" + elem.Text + "Expected Result:" + value);
                            }
                            Message = "Values Appeared Under Header:" + header;
                            return true;
                        }
                    }
                }
                throw new Exception("Failed to verify Header Details: " + header + " Expected Result:" + value);
            }
            catch (Exception e)
            {
                throw new Exception("Failed due to:" + e);
            }
        }

        public bool VerifyNumberOfRowsIncludingHeader(string Table, int Rows)
        {
            try
            {
                List<IWebElement> TableHeader = new List<IWebElement>(Driver.FindElement(By.XPath(Table)).FindElements(By.TagName("tr")));
                int Table_Count = TableHeader.Count;
                if (Table_Count == Rows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Rows are mismatching:" + e);
            }
        }

        public bool VerifyElementandScrollToElement(ElementLocator ElementName)
        {
            try
            {
                if (Driver.IsElementPresent(ElementName, 1))
                {
                    Driver.FindElement(By.XPath(ElementName.Value)).ScrollToElement();
                    return true;

                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//td[@colspan]//table")))
                    {
                        List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[@colspan]//table//tbody//tr//td")));
                        var pageCount = pagesTd.Count;
                        for (var pagenum = 1; pagenum <= pageCount; pagenum++)
                        {
                            if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + pagenum + "')]")))
                            {
                                Driver.FindElement(By.XPath("//a[contains(text(),'" + pagenum + "')]")).ClickElement();
                            }
                            if (Driver.IsElementPresent(ElementName, 1))
                            {
                                Driver.FindElement(By.XPath(ElementName.Value)).ScrollToElement();
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Search the Object:" + ElementName+" Due to"+e.Message);
            }
            return false;
        }

        /// <summary>
        /// This Method is to Click on Button
        /// </summary>
        /// <param name="Element"></param>
        public void Click_OnButton(ElementLocator Locator)
        {
            Driver.GetElement(Locator).ClickElement();
        }

        /// <summary>
        /// This Method is to Delcare a Select type element locator and Select Text
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Select_Text"></param>
        public void SelectElement_AndSelectByText(ElementLocator Element, string Select_Text)
        {
            Select Select_Elem = new Select(Driver.GetElement(Element));
            Select_Elem.SelectByText(Select_Text);
        }

        /// <summary>
        /// This Method is Make sure Expected value selected in the element, Use for Validations 
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Select_Text"></param>
        public bool IsTextSelected(ElementLocator Element, string Select_Text)
        {
     
            Select Select_Elem = new Select(Driver.FindElement(By.XPath(Element.Value)));
            return Select_Elem.IsTextSelected(Select_Text, 1);
        }

        /// <summary>
        /// This Method is Make sure Expected value Checkbox is Selected, Use for Validations 
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Is CheckboxSelected"></param>
        public bool IsChecked(ElementLocator Element)
        {
            Checkbox Checkbox_Elem_ = new Checkbox(Driver.GetElement(Element));
            return Checkbox_Elem_.IsSelected();
        }
        /// <summary>
        /// This Method is to Delcare a Select type element locator and Select Text
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Select_Text"></param>
        public void SelectElement_AndSelectByValue(ElementLocator Element, string Select_Text)
        {
            Select Select_Elem = new Select(Driver.GetElement(Element));
            Select_Elem.SelectByValue(Select_Text,1);
        }

        /// <summary>
        /// This Method is to Delcare a Select type element locator and Select Text
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Select_Index"></param>
        public void SelectElement_AndSelectByIndex(ElementLocator Element, int index)
        {
            Select Select_Elem_ = new Select(Driver.GetElement(Element));
            Select_Elem_.SelectByIndex(index);

        }

        /// <summary>
        /// This Method is to Delcare a Check box  type and Select the Checkbox By Default
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="Checked"></param>
        public void CheckBoxElmandCheck(ElementLocator Element, bool Checked = true)
        {
            Checkbox Checkbox_Elem_ = new Checkbox(Driver.GetElement(Element));
            if (Checked == true)
            { Checkbox_Elem_.TickCheckbox(); }
            else
            { Checkbox_Elem_.UntickCheckbox(); }
        }

       
        // <summary>
        /// This Method is to Delcare a Radio button and Select the Radio button by Default
        /// </summary>
        /// <param name="Element"></param>
        /// <param name="selected"></param>
        public void SelectRadioButton(ElementLocator Element, bool selected = true)
        {
            RadioButton RadioButton_Elem = new RadioButton(Driver.GetElement(Element));
            if (selected == true)
            { RadioButton_Elem.SelectRadioButton(); }
        }

        public static string _ElementLocatorXpathfor_Input(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//span[contains(text(),'" + LabelName + "')]//following::input[1]";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_RC_Store_Input(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='SearchForm Store']//label[contains(text(),'" + LabelName + "')]//following-sibling::input[1]";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_RC_Store_Buttons(string ButtonName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='SearchForm Store']//a[contains(@class,'btn Button') and text()='" + ButtonName + "']";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_RC_Online_Input(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='SearchForm Online']//label[contains(text(),'" + LabelName + "')]//following-sibling::input[1]";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_RC_Online_Buttons(string ButtonName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='SearchForm Online']//a[contains(@class,'btn Button') and text()='" + ButtonName + "']";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_Input_(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//span[text()='" + LabelName + "']//following::input[1]";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_Input_Label(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//*[text()='" + LabelName + "']//following::input[1]";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_Buttons(string ButtonName)
        {
            string Custom_ElementLocatorXpath = "//a[contains(@class,'button') and text()='" + ButtonName + "']";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_Select(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//span[text()='" + LabelName + "']//following::Select[1]";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_TextArea(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//span[text()='" + LabelName + "']//following::textArea[1]";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_CheckBox(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//label[text()='" + LabelName + "']//preceding-sibling::input";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_ActionButtons(string ButtonName)
        {
            string Custom_ElementLocatorXpath = "//*[@class='ActionButtons']//a[text()='" + ButtonName + "']";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_MemberProfie_Input(string Label)
        {
            string Custom_ElementLocatorXpath = "//label[contains(text(),'" + Label + "')]//following::td[position()=count(//label[contains(text(),'" + Label + "')]//following::tr[position()=1]//td)]//input";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_MemberProfie_Select(string Label)
        {
            string Custom_ElementLocatorXpath = "//label[contains(text(),'" + Label + "')]//following::td[position()=count(//label[contains(text(),'" + Label + "')]//following::tr[position()=1]//td)]//Select";
            return Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_MemberProfie_CheckBox(string Label)
        {
            string Custom_ElementLocatorXpath = "//label[contains(text(),'" + Label + "')]//input";
            return Custom_ElementLocatorXpath;
        }

        public static string _ElementLocatorXpathfor_MemberRegistration_Input(string Label)
        {
            string Custom_ElementLocatorXpath = "//label[text()='" + Label + "']//following-sibling::input";
            return Custom_ElementLocatorXpath;
        }

        public static string _LabelLocator(string Label)
        {
            string Custom_ElementLocatorXpath = "//td//strong[text()='" + Label + "']//parent::td";
            return Custom_ElementLocatorXpath;
        }

        public static string _HeaderLocator_MyWallet(string Header)
        {
            string Custom_ElementLocatorXpath = "//a[text()='" + Header + "']";
            return Custom_ElementLocatorXpath;
        }
        public static string _HeaderAndLabelLocator_MyProfile(string Header, string Label = "")
        {
            string Custom_ElementLocatorXpath = "";
            if (Label.Equals(""))
            {
                Custom_ElementLocatorXpath = "//h3[text()='" + Header + "']";
            }
            else
            {
                Custom_ElementLocatorXpath = "//h3[text()='" + Header + "']//following::label[text()='" + Label + "']";
            }

            return Custom_ElementLocatorXpath;
        }
        public static string _InputLocator_MyProfile(string Header, string Label)
        {
            string Custom_ElementLocatorXpath = "";
            if (Header.Equals("Communication Preferences"))
            {
                Custom_ElementLocatorXpath = "//h3[text()='" + Header + "']//following::label[text()='" + Label + "']//preceding-sibling::input";
            }
            else
            {
                Custom_ElementLocatorXpath = "//h3[text()='" + Header + "']//following::label[text()='" + Label + "']//following-sibling::input";
            }
            return Custom_ElementLocatorXpath;
        }
        public static string _SectionLocator_MyWallet(string Header, string Section)
        {
            string Custom_ElementLocatorXpath = "//a[text()='" + Header + "']//following::span[contains(text(),'" + Section + "')]";
            return Custom_ElementLocatorXpath;
        }
        public static string _ButtonLocator_MyWallet(string Header, string Button)
        {
            string Custom_ElementLocatorXpath = "//a[text()='" + Header + "']//parent::h2//parent::div//following::a[text()='" + Button + "']";
            return Custom_ElementLocatorXpath;
        }
        public static string MemberProtalDashboardLinks(string MenuName)
        {
            string memberProtalDashboardLinks = "//div[@class='container main']//a[text()='" + MenuName + "']";
            return memberProtalDashboardLinks;
        }
        #region CS Portal MemberSearch

        public static ElementLocator MemberSearch_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }

        public static ElementLocator MemberSearch_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Buttons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        #endregion

        #region CS Portal AccountSummary
        public bool IsElementPresentBasedon_LabelAndText(string LabelName, string InputValue, out string Message)
        {
            Message = "";
            ElementLocator LabelWithText = new ElementLocator(Locator.XPath, _LabelLocator(LabelName));
            if (Driver.IsElementPresent(LabelWithText, 1))
            {
                if (Driver.GetElement(LabelWithText).GetTextContent().Contains(InputValue))
                {
                    Message = "For Label :" + LabelName + "</br>  Actual Value Appeared as :" + Driver.GetElement(LabelWithText).GetTextContent() + " </br>  Expected Value is:" + InputValue + " <br> And Both are Matching"; return true;
                }
                else
                {
                    Message = "For Label :" + LabelName + "  Actual Value Appeared as" + Driver.GetElement(LabelWithText).GetTextContent() + "and Expected Value is:" + InputValue + " are not Matching";
                    throw new Exception("For Label :" + LabelName + "  Actual Value Appeared as" + Driver.GetElement(LabelWithText).GetTextContent() + " and Expected Value is:" + InputValue + " are not matching");
                }
            }
            throw new Exception("Failed to Read Label: " + LabelName);
        }

        public bool IsElementPresentBasedon_LabelAndWithExactText(string LabelName, string InputValue, out string Message)
        {
            Message = "";
            ElementLocator LabelWithText = new ElementLocator(Locator.XPath, _LabelLocator(LabelName));
            Driver.ScrollIntoMiddle(LabelWithText);
            string ActualOutputTotal = Driver.GetElement(LabelWithText).GetTextContent().ToString().Trim();
            string[] _ActualOutput = ActualOutputTotal.Split(new string[] { ":" }, StringSplitOptions.None);
            string ActualOutput = _ActualOutput[1].Trim();
            if (Driver.IsElementPresent(LabelWithText, 1))
            {
                if (ActualOutput.Equals(InputValue.Trim()))
                {
                    Message = "For Label :" + LabelName + "</br>  Actual Value Appeared as :" + Driver.GetElement(LabelWithText).GetTextContent() + " </br>  Expected Value is:" + InputValue + " <br> And Both are Matching"; return true;
                }
                else
                {
                    Message = "For Label :" + LabelName + "  Actual Value Appeared as" + Driver.GetElement(LabelWithText).GetTextContent() + "and Expected Value is:" + InputValue + " are not Matching";
                    throw new Exception("For Label :" + LabelName + "  Actual Value Appeared as" + Driver.GetElement(LabelWithText).GetTextContent() + " and Expected Value is:" + InputValue + " are not matching");
                }
            }
            throw new Exception("Failed to Read Label: " + LabelName);
        }

        public string CaptureTextBasedon_Label(string LabelName)
        {
            ElementLocator LabelWithText = new ElementLocator(Locator.XPath, _LabelLocator(LabelName));
            if (Driver.IsElementPresent(LabelWithText, .5))
            {
                string CaptureText = Driver.GetElement(LabelWithText).GetTextContent();
                string SplitLabelAndText_CaptureLastValue = CaptureText.Split(':').Last();
                return SplitLabelAndText_CaptureLastValue;
            }
            else
            {
                throw new Exception("For Label : " + LabelName + " Failed to Capture the data");
            }

            throw new Exception("Failed to Read Label: " + LabelName);
        }
        #endregion

        #region CS Portal Administration
        public static ElementLocator Administration_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
       

        public static ElementLocator Administration_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Buttons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }

        public static ElementLocator Administration_CheckBox_Custom_ElementLocatorXpath(string CheckBoxName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_CheckBox(CheckBoxName));
            return Button_Custom_ElementLocatorXpath;
        }

        public static ElementLocator Administration_Select_Custom_ElementLocatorXpath(string SelectName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Select(SelectName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static ElementLocator Administration_TextArea_Custom_ElementLocatorXpath(string TextAreaName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_TextArea(TextAreaName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_Input_Field(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//label[contains(text(),'" + LabelName + "')]//following::input[1]";
            return Custom_ElementLocatorXpath;
        }
        #endregion

        #region CS Portal MergeAccounts
        public static ElementLocator MergeAccounts_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_ActionButtons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        #endregion

        #region CS Portal ChangePassword
        public static ElementLocator ChangePassword_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_ActionButtons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static ElementLocator ChangePassword_TextBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input_Label(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        #endregion

        #region CS Portal Membe Request Credit

        public static ElementLocator MembeRequestCredit_TextBox_Custom_Store_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_RC_Store_Input(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }

        public static ElementLocator MembeRequestCredit_Button_Custom_Store_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_RC_Store_Buttons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }

        public static ElementLocator MembeRequestCredit_TextBox_Custom_Online_ElementLocatorXpath(string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_RC_Online_Input(LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }

        public static ElementLocator MembeRequestCredit_Button_Custom_Online_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_RC_Online_Buttons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        #endregion

        #region Member Portal
        public static ElementLocator MemberProfile_Button_Custom_ElementLocatorXpath(string ButtonName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_ActionButtons(ButtonName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_Textbox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator LabelName_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_MemberProfie_Input(LabelName));
            return LabelName_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_Select_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator LabelName_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_MemberProfie_Select(LabelName));
            return LabelName_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_CheckBox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator Label_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_MemberProfie_CheckBox(LabelName));
            return Label_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_Header_Custom_ElementLocatorXpath(string HeaderName)
        {
            ElementLocator Header_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _HeaderLocator_MyWallet(HeaderName));
            return Header_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_Section_Custom_ElementLocatorXpath(string HeaderName, string SectionName)
        {
            ElementLocator Header_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _SectionLocator_MyWallet(HeaderName, SectionName));
            return Header_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberProfile_Button_Custom_ElementLocatorXpath(string HeaderName, string ButtonName)
        {
            ElementLocator Header_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ButtonLocator_MyWallet(HeaderName, ButtonName));
            return Header_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberPortal_DashBoardLinks(string Menu)
        {
            ElementLocator DashBoardLinks_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, MemberProtalDashboardLinks(Menu));
            return DashBoardLinks_Custom_ElementLocatorXpath;
        }

        public static ElementLocator MyProfile_HeaderAndLabel_Custom_ElementLocator(string Header, string Label = "")
        {
            ElementLocator Header_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _HeaderAndLabelLocator_MyProfile(Header, Label));
            return Header_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MyProfile_Input_Custom_ElementLocator(string Header, string Label = "")
        {
            ElementLocator Header_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _InputLocator_MyProfile(Header, Label));
            return Header_Custom_ElementLocatorXpath;
        }
        public static ElementLocator MemberPortal_Textbox_Custom_ElementLocatorXpath(string LabelName)
        {
            ElementLocator Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Input_(LabelName));
            return Custom_ElementLocatorXpath;
        }
        public static ElementLocator Reward_Select_Custom_ElementLocatorXpath(string SelectName)
        {
            ElementLocator Button_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_Select_Label(SelectName));
            return Button_Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_Select_Label(string LabelName)
        {
            string Custom_ElementLocatorXpath = "//label[contains(text(),'" + LabelName + "')]//following::Select[1]";
            return Custom_ElementLocatorXpath;
        }
        public static ElementLocator MP_MembeRequestCredit_TextBox_ElementLocatorXpath(string ClassName, string LabelName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpath_Input(ClassName, LabelName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpath_Input(string ClassName, string LabelName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='" + ClassName + "']//label[text()='" + LabelName + "']//following::input[1]";
            return Custom_ElementLocatorXpath;
        }
        public static ElementLocator Mp_MembeRequestCredit_Button_Custom_ElementLocatorXpath(string ClassName, string ButtonName)
        {
            ElementLocator TextBox_Custom_ElementLocatorXpath = new ElementLocator(Locator.XPath, _ElementLocatorXpathfor_RC_Buttons(ClassName, ButtonName));
            return TextBox_Custom_ElementLocatorXpath;
        }
        public static string _ElementLocatorXpathfor_RC_Buttons(string ClassName, string ButtonName)
        {
            string Custom_ElementLocatorXpath = "//div[@class='" + ClassName + "']//a[contains(@class,'btn Button') and text()='" + ButtonName + "']";
            return Custom_ElementLocatorXpath;
        }

        /// <summary>
        ///Get Transaction Details from DB Details Transactions hisotry Table
        /// </summary>
        /// <returns>List With Transaction Details</returns>
        public static List<string> GetTransactionDetailsFromTransationHistoryTableFromDB(out string Message)
        {
            try
            {
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                List<string> TransactionList = new List<string>();

                TransactionList = DatabaseUtility.GetTransactionDetailsFromTransationHistoryTablewithoutTransaction();
                RequestCredit_Search_Criteria.TransactionNumber = TransactionList[0].ToString();
                RequestCredit_Search_Criteria.RegisterNumber = TransactionList[1].ToString();
                RequestCredit_Search_Criteria.TxnAmount = TransactionList[2].ToString();
                RequestCredit_Search_Criteria.TxnDate = TransactionList[3].ToString();
                RequestCredit_Search_Criteria.StoreNumber = TransactionList[4].ToString();
                RequestCredit_Search_Criteria.OrderNumber = TransactionList[5].ToString();

                Message = "Orphan Transaction Details; Transaction Number: " + RequestCredit_Search_Criteria.TransactionNumber
                                                      + ";Register Number: " + RequestCredit_Search_Criteria.RegisterNumber
                                                      + ";Transaction Amount: " + RequestCredit_Search_Criteria.TransactionNumber
                                                      + ";Transaction Date: " + RequestCredit_Search_Criteria.TxnDate
                                                      + ";Transaction Amount: " + RequestCredit_Search_Criteria.TxnAmount
                                                      + ";Store Number: " + RequestCredit_Search_Criteria.StoreNumber
                                                      + ";Order Number: " + RequestCredit_Search_Criteria.OrderNumber;
                return TransactionList;
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number");
            }
        }

        /// <summary>
        ///Get Transaction Details from DB Transactions header table
        /// </summary>
        /// <returns>List With Transaction Details</returns>
        public static List<string> GetTransactionDetailsFromTransactionHeaderTableFromDB(string Transacation, out string Message)
        {
            try
            {
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                List<string> TransactionList = new List<string>();
                TransactionList = DatabaseUtility.GetTransactionDetailsFromTransactionHeaderTable(Transacation);
                RequestCredit_Search_Criteria.TransactionNumber = TransactionList[0].ToString();
                RequestCredit_Search_Criteria.RegisterNumber = TransactionList[1].ToString();
                RequestCredit_Search_Criteria.TxnAmount = TransactionList[2].ToString();
                RequestCredit_Search_Criteria.TxnDate = TransactionList[3].ToString();
                RequestCredit_Search_Criteria.StoreNumber = TransactionList[4].ToString();
                Message = "Orphan Transaction Details; Transaction Number: " + RequestCredit_Search_Criteria.TransactionNumber
                                                                     + ";Register Number: " + RequestCredit_Search_Criteria.RegisterNumber
                                                                     + ";Transaction Amount: " + RequestCredit_Search_Criteria.TransactionNumber
                                                                     + ";Transaction Date: " + RequestCredit_Search_Criteria.TxnDate
                                                                     + ";Transaction Amount: " + RequestCredit_Search_Criteria.TxnAmount
                                                                     + ";Store Number: " + RequestCredit_Search_Criteria.StoreNumber;
                return TransactionList;
            }
            catch (Exception)
            {
                throw new Exception("Failed to SearchTransaction Number");
            }
        }

        public bool GetCategoryDetails(string Environment,string Category)
        {
            bool status = DatabaseUtility.VerifyCategoryAvilableDetails(Environment,Category);
            return status;
        }

        public bool GetNotificationDetails(string Environment, string Category)
        {
            bool status = DatabaseUtility.VerifyNotificationAvilableDetails(Environment, Category);
            return status;
        }

        public bool VerifyCategory_IfNotExistedCreateNew(string Environment, CategoryFields Category,string CategoryType,  out string Message,bool Migration = false)
        {
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
             var navigator_CreateCoupon_Category = new Navigator_Users_Program_Components_CategoriesPage(DriverContext);

            switch (CategoryType)
            {
                case "Product":break;
               
                case "Bonus": break;
                case "Coupon":
                    if (GetCategoryDetails(Environment, Category.CategoryName))
                    {
                        Message = "Below Mentioned  Category Already Existed In,;Environment: " + Environment + ";Category: " + Category.CategoryName;
                        return true;
                    }
                    else
                    {

                        application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                        navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.Components);
                        navigator_Users_Program_ComponentsPage.NavigateToProgramComponentsTab(Navigator_Users_Program_ComponentsPage.ComponentsTabs.Categories);
                        Message = navigator_CreateCoupon_Category.CreateCategory(Category);

                        if (Migration)
                        {
                            Message = "Category is not Available in;Environment: " + Environment + ";Category: " + Category.CategoryName;
                            return false;
                        }

                        return true;
                    }
                 
                default: throw new Exception("Invalid CategoryType");
            }
            throw new Exception("Failed to Create or Verify Category Name:" + Category.Name + " ;Category Type:" + Category.CategoryTypeValue);
        }



      public bool VerifyNotification_IfNotExistsCreateNew(string Environment, CategoryFields pushNotification, out string Message)
        {
            var application_Nav_Util_Page = new Application_Nav_Util_Page(DriverContext);
            var navigator_Users_ProgramPage = new Navigator_Users_ProgramPage(DriverContext);
            var navigator_Users_Program_ComponentsPage = new Navigator_Users_Program_ComponentsPage(DriverContext);
            var navigator_Users_Program_eCollateralPage = new Navigator_Users_Program_eCollateralPage(DriverContext);
            var navigator_Cusers_program_ecollateral_notificationPage = new Navigator_Users_Program_eCollateral_NotificationPage(DriverContext);
            try
            {
                if (GetNotificationDetails(Environment, pushNotification.Name))
                {

                    Message = "Below Mentioned  Notification Already Existed In,;Environment: " + Environment + ";Notification Name: " + pushNotification.Name;
                    return true;
                }
                else
                {
                    application_Nav_Util_Page.OpenApplication(NavigatorEnums.ApplicationName.program);
                    navigator_Users_ProgramPage.NavigateToProgramTab(Navigator_Users_ProgramPage.ProgramTabs.eCollateral);
                    navigator_Users_Program_eCollateralPage.NavigateToProgramECollateralTab(Navigator_Users_Program_eCollateralPage.eCollateralTabs.Notifications);
                    navigator_Cusers_program_ecollateral_notificationPage.CreatePushNotication(pushNotification, out string msg);
                    Message = msg;
                    return true;
                }
            }
               catch(Exception e)
            {
                throw new Exception("Failed to Create or Verify Notification Name:" + pushNotification.Name+"Due to:"+e.Message);
            } 
        }


        /// <summary>
        /// Compare Data fetched from DB with expected Result
        /// </summary>
        /// <param name="DBTableHeader"></param>
        /// <param name="Input"></param>
        /// <param name="Output"></param>
        /// <returns>Message with Status (Boolean)</returns>
        public bool VerifyInputandOutputFromDB(string DBTableHeader, string Input, string Output, out string Message)
        {
            try
            {
                if (Input.Equals(Output))
                {
                    Message = "Provided Values are available  in DB;" + DBTableHeader + ":" + Input;
                    return true;

                }
                throw new Exception("Provided Values are not available  in DB;" + DBTableHeader + ":" + Input);
            }
            catch (Exception)
            {
                throw new Exception("Provided Values are not available  in DB;" + DBTableHeader + ":" + Input);
            }
        }

        /// <summary>
        ///Get Coupon Details from DB CouponDef table
        /// </summary>
        /// <returns>return couponName</returns>
        public static CategoryFields GetCouponDetailsFromCouponDefTableFromDB(string CouponName, out string Message)
        {
            try
            {
                CategoryFields coupon = new CategoryFields();
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                coupon = DatabaseUtility.GetCouponDetailsFromDB(CouponName);
                Message = "Coupon Saved successfully :" + coupon.Name;
                return coupon;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Coupon Name");
            }
        }

        /// <summary>
        ///Get Coupon Details from DB CouponDef table where global value is set to 1
        /// </summary>
        /// <returns>return couponName</returns>
        public static CategoryFields GetCouponDetailsFromCouponDefTableFromDB(string CouponName, string isGlobalStatus, out string Message)
        {
            try
            {
                CategoryFields coupon = new CategoryFields();
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                coupon = DatabaseUtility.GetCouponDetailsFromDBCDIS(CouponName, isGlobalStatus);
                Message = "Coupon with Global value set to 1 Found successfully :" + coupon.Name;
                return coupon;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Coupon Name with Global Value");
            }
        }

        public static Promotions GetPromotionDetailsFromlw_promotionTableFromDB(string Name, int EnrollmenntType, int Targeted,out string Message)
        {
            try
            {
                Promotions promotion = new Promotions();
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                promotion = DatabaseUtility.GetpromotionsDetailsfromDb(Name,EnrollmenntType,Targeted);
                Message = "Promotion details retrived successfully from database:" + promotion.Name;
                return promotion;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Promotion ;Promotion Details:"+ Name);
            }
        }

        #endregion

        private static IList<JToken> GetJSONData()
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));


            using (var reader = new StreamReader(TestDatPath + "//TestData//TestData.json"))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var parsedData = new JsonSerializer().Deserialize(jsonReader);
                    JArray data = (JArray)parsedData;
                    return data.ToList();
                }
            }
        }

        public static IList<JToken> JsonData
        {
            get
            {
                return GetJSONData();
            }
        }

        public string Browser
        {
            get
            {
                return (string)JsonData.FirstOrDefault()["Browser"];
            }
        }
   //     public static string IsDBReportingRequired => (string)JsonData.FirstOrDefault()["IsDBReportingRequired"];


        #region Navigator
        public static string GetUserInfo(string UserRole, string Value)
        {

            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()["AllAdminusers"].Where(o => (string)o["User Role"] == UserRole).FirstOrDefault()[Value];

        }
        public static string GetCategoryInfo(string CategoryName, string Value)
        {

            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()["Category"].Where(o => (string)o["Category Type"] == CategoryName).FirstOrDefault()[Value];

        }
        public static string GetMigrationCategoryInfo(string CategoryName, string Value)
        {

            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()["MigrationCategory"].Where(o => (string)o["Category Type"] == CategoryName).FirstOrDefault()[Value];

        }
        public static string GetWebsiteInfo(string CategoryName, string Value)
        {

            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()[CategoryName].FirstOrDefault()[Value];

        }
        public static string GetOrgInfo(string Value)
        {
            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()[Value];
        }

        public static string GetApplicationUrls(int index)
        {
            var result = JsonData.FirstOrDefault()["Application_URLS"].ToArray();
            return ((JProperty)((JContainer)result[index]).First).Value.ToString();
        }        public static string GetMigrationEnvironment(string CategoryName, string Value)
        {

            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()[CategoryName].FirstOrDefault()[Value];

        }

        public static IList<JToken> GetJSONData(string fileName)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            using (var reader = new StreamReader(TestDatPath + "//TestData//" + fileName + ".json"))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var parsedData = new JsonSerializer().Deserialize(jsonReader);
                    JArray data = (JArray)parsedData;
                    return data.ToList();
                }
            }
        }

        public static string GetWesiteName(int index)
        {
            var result = JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()["Website"].ToArray();
            return ((JProperty)((JContainer)result[index]).First).Value.ToString();
        }

        public static string GetNavigatorNodesInfo(string Name, string Value)
        {
            return (string)JsonData.FirstOrDefault()["Navigator"].FirstOrDefault()[Name].FirstOrDefault()[Value];
        }

        #endregion

        #region Csportal

        public static string GetCsportalTestDataInfo(string Name, string Value)
        {
            return (string)JsonData.FirstOrDefault()["CsPortal"].FirstOrDefault()[Name].FirstOrDefault()[Value];
        }

        #endregion

        #region MemberPortal

        public static string GetMemberPortalTestDataInfo(string Value)
        {
            return (string)JsonData.FirstOrDefault()["MemberPortal"].FirstOrDefault()[Value];
        }

        #endregion

        public Member CreateMemberThroughCDIS()
        {
            Common common = new Common(DriverContext);
            CDIS_Service_Methods cdis_Service_Method = new CDIS_Service_Methods(common);
            Member output = cdis_Service_Method.AddCDISMemberWithAllFields();
            return output;
        }

        public string GetLoyaltyNumber(Member output)
        {
            return DatabaseUtility.GetLoyaltyID(output.IpCode.ToString());
        }

        public string GetLoyaltyNumberWithTier()
        {
            return DatabaseUtility.GetFirstLoyaltyIDFromDBUSingSOAP(Tiers_EntryPoints.Standard);
        }
        

     

        public static bool UpdateTestcaseStatus(string testcase, string status)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            var filePath = Path.Combine(TestDatPath, "TestData//TestCases_RunStat.json");
            var result = string.Empty;
            using (var reader = new StreamReader(filePath))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var parsedData = new JsonSerializer().Deserialize(jsonReader);
                    JArray data = (JArray)parsedData;
                    if (data.ToList().Any())
                    {
                        var filtereddata = data.FirstOrDefault(o => (string)o["TestCase"] == testcase);
                        if (filtereddata != null)
                        {
                            filtereddata["Status"] = status;
                        }
                        var jsonResult = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data));
                        result = jsonResult.ToString();

                    }
                }
            }

            return String.IsNullOrEmpty(result) ? false : WriteJSON(filePath, result);
        }

        public static string ReadTestcaseStatus(string testcase)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            var filePath = Path.Combine(TestDatPath, "TestData//TestCases_RunStat.json");
            var result = string.Empty;
            using (var reader = new StreamReader(filePath))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var parsedData = new JsonSerializer().Deserialize(jsonReader);
                    JArray data = (JArray)parsedData;
                    if (data.ToList().Any())
                    {
                        var filtereddata = data.FirstOrDefault(o => (string)o["TestCase"] == testcase);
                        if (filtereddata != null)
                        {
                            return filtereddata["Status"].ToString();
                        }
                        var jsonResult = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data));
                        result = jsonResult.ToString();

                    }
                }
            }

            return "No Test Found";
        }

        public static bool UpsetTestCaseStatus(string testcase, string status)
        {
            string TestDatPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            var filePath = Path.Combine(TestDatPath, "TestData//TestCases_RunStat1.json");
            var result = string.Empty;
            using (var reader = new StreamReader(filePath))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var parsedData = new JsonSerializer().Deserialize(jsonReader);
                    JArray data = (JArray)parsedData;
                    if (data.ToList().Any())
                    {
                        var filtereddata = data.FirstOrDefault(o => (string)o["TestCase"] == testcase);
                        if (filtereddata != null)
                        {
                            filtereddata["Status"] = status;
                        }
                        //else
                        //{
                        //    filtereddata = data.FirstOrDefault();
                        //    var fData = filtereddata;
                        //    var obj =  ;
                        //    data.Add(obj);
                        //}
                        var jsonResult = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data));
                        result = jsonResult.ToString();

                    }
                }
            }

            return String.IsNullOrEmpty(result) ? false : WriteJSON(filePath, result);
        }


        private static bool WriteJSON(string filePath, object fileData)
        {
            bool returnValue = false;
            try
            {
                File.WriteAllText(filePath, fileData.ToString());
                returnValue = true;
            }
            catch (Exception ex)
            { }
            return returnValue;
        }


        public static TestStep VerifyOrderTest(string OrderStatus,string PreCondition_TestCases,string Current_TestCase,TestStep testStep)
        {

            string Prereq_testCase = PreCondition_TestCases;

            if (OrderStatus.Contains("true") && ReadTestcaseStatus(Prereq_testCase).ToString().Contains("Pass"))
            {
                UpdateTestcaseStatus(Current_TestCase.ToString(), "");
                return testStep; 
            }
            else
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                string stepName = "Pre Condition Test :" + Prereq_testCase + "  Failed. Hence not able to proceed for this Test ";
                UpdateTestcaseStatus(Current_TestCase.ToString(), "Failed");
                throw new Exception(stepName);
            }
        }
        public static TestStep VerifyOrderTest(string OrderStatus, string PreCondition_TestCases1, string PreCondition_TestCases2,string PreCondition_TestCase3,string Current_TestCase, TestStep testStep)
        {
            string status1 = ReadTestcaseStatus(PreCondition_TestCases1).ToString();
            string status2 = ReadTestcaseStatus(PreCondition_TestCases2).ToString();
            string status3 = ReadTestcaseStatus(PreCondition_TestCase3).ToString();


            if (OrderStatus.Contains("true") &&(status1.Contains("Pass") && status2.ToString().Contains("Pass") && status3.Contains("Pass")))
            {
                UpdateTestcaseStatus(Current_TestCase.ToString(), "");
                return testStep;
               
            }
            else
            {
                testStep = TestStepHelper.StartTestStep(testStep);
                string stepName = "Pre Condition Criteria Failed Below are the Details TC Name :" + PreCondition_TestCases1 + " TC Status:" + status1 + ", TC Name :" + PreCondition_TestCases2 + ", TC Status:" + status2 + "TC Name : " + PreCondition_TestCase3 + ", TC Status:" + status3 + " Failed. Hence not able to proceed for this Test ";
                UpdateTestcaseStatus(Current_TestCase.ToString(), "Failed");
                throw new Exception(stepName);
            }
        }

        /// <summary>
        ///Get Product Details from Product table in Database
        /// </summary>
        /// <returns>return couponName</returns>
        public static CategoryFields GetProductDetailsFromProductTableFromDB(string ProductName, string Quantity, out string Message)
        {
            try
            {
                CategoryFields product = new CategoryFields();
                RequestCredit RequestCredit_Search_Criteria = new RequestCredit();
                product = DatabaseUtility.GetProductDetailsFromDB(ProductName);
                Message = "Product " + product.Name + " quantity is displayed as " +product.Quantity;
                return product;
            }
            catch (Exception)
            {
                throw new Exception("Failed to Search Product");
            }
        }

        public ElementLocator ErrorFieldLevel(string ErrorMessage)
        {
            ElementLocator _ErrorFieldLevel = new ElementLocator(Locator.XPath, ".//span[@class='Validator' and contains(text(),'" + ErrorMessage + "'" + ")]");
            return _ErrorFieldLevel;
        }
        public ElementLocator ErrorPageLevel(string ErrorMessage)
        {
            string singlequotestring = "//span[text()='" + ErrorMessage + "']";
            ElementLocator _ErrorPageLevel = new ElementLocator(Locator.XPath, singlequotestring);
            return _ErrorPageLevel;
        }

        public ElementLocator ErrorPageForUserNameLevel(string ErrorMessage)
        {
            string singlequotestring = "//span[contains(text(),'Unable to find') and contains(text(),'" + ErrorMessage + "')]";
            ElementLocator _ErrorPageLevel = new ElementLocator(Locator.XPath, singlequotestring);
            return _ErrorPageLevel;
        }

        public bool VerfiyDateFromDateAndCurrentDate(out string Message)
        {
            try {
                var CurrentDate = DateTime.Now;
                var StartOfMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                var startOfMonth1 = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).ToString("M/d/yyyy", new CultureInfo("en-US"));
                var EndOfMonth = StartOfMonth.AddMonths(1).AddDays(-1).ToString("M/d/yyyy", new CultureInfo("en-US"));

                var _FromDate_Actual = "";
                var _ToDate_Actual = "";
                ElementLocator _FromDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'FromDate') and contains(@class,'Enabled')]");
                ElementLocator _ToDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'ToDate') and contains(@class,'Enabled')]");
                if (Driver.IsElementPresent(_FromDate, 1) && Driver.IsElementPresent(_ToDate, 1))
                {
                    _FromDate_Actual = Driver.GetElement(_FromDate).GetAttribute("value");
                    //_FromDate_Actual=_FromDate_Actual.ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                    _ToDate_Actual = Driver.GetElement(_ToDate).GetAttribute("value");
                    //_ToDate_Actual= _ToDate_Actual.ToString("MM-dd-yyyy", new CultureInfo("en-US"));
                    if (_FromDate_Actual.Equals(startOfMonth1) && _ToDate_Actual.Equals(EndOfMonth))
                    {
                        Message = "From Date Appeared as  First  Day of current month:" + startOfMonth1 + ";To Date Appeared as Last Day of current month:" + EndOfMonth;
                        return true;
                    }
                    else
                    {
                        Message = "From Date Appeared Not Appeared as  First  Day of current month:" + startOfMonth1 + ";To Date Not Appeared as Last Day of current month:" + EndOfMonth;
                        throw new Exception(Message);
                    }
                }
                Message = "Failed To Verify Date Pickers Please refer Screenshot for More Details";
                throw new Exception(Message);
            }catch(Exception e)
            {
                throw new Exception("Failed to Verify Date pickers Due to:" + e.Message);
            }
            }


        public bool UpdateFromDateAndCurrentDate(out string Message)
        {
            try
            {
                var _From_Date = DateTime.Now.AddDays(-1).ToString("MMMMM dd, yyyy");
                var _To_Date = DateTime.Now.ToString("MMMMM dd, yyyy");
                ElementLocator _FromDate_Pickers = new ElementLocator(Locator.XPath, "//a[contains(@id,'FromDate') and contains(@class,'CalPopup')]");
                ElementLocator _ToDate_Pickers = new ElementLocator(Locator.XPath, "//a[contains(@id,'ToDate') and contains(@class,'CalPopup')]");
                 ElementLocator _FromDate_Select = new ElementLocator(Locator.XPath, "//table[contains(@id,'FromDate')]//td[contains(@title,'"+ _From_Date + "')]//a");
                ElementLocator _To_Select = new ElementLocator(Locator.XPath, "//table[contains(@id,'ToDate')]//td[contains(@title,'"+ _To_Date + "')]//a");

                Driver.GetElement(_FromDate_Pickers).ClickElement();
                Driver.GetElement(_FromDate_Select).ClickElement();
                Driver.GetElement(_ToDate_Pickers).ClickElement();
                Driver.GetElement(_To_Select).ClickElement();


                var CurrentDate = DateTime.Now.AddDays(-1).ToString("M/d/yyyy", new CultureInfo("en-US"));
                var EndOfMonth = DateTime.Now.ToString("M/d/yyyy", new CultureInfo("en-US"));


                var _FromDate_Actual = "";
                var _ToDate_Actual = "";
                ElementLocator _FromDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'FromDate') and contains(@class,'Enabled')]");
                ElementLocator _ToDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'ToDate') and contains(@class,'Enabled')]");
                if (Driver.IsElementPresent(_FromDate, 1) && Driver.IsElementPresent(_ToDate, 1))
                {
                    _FromDate_Actual = Driver.GetElement(_FromDate).GetAttribute("value");
                    //_FromDate_Actual=_FromDate_Actual.ToString("MM/dd/yyyy", new CultureInfo("en-US"));
                    _ToDate_Actual = Driver.GetElement(_ToDate).GetAttribute("value");
                    //_ToDate_Actual= _ToDate_Actual.ToString("MM-dd-yyyy", new CultureInfo("en-US"));
                    if (_FromDate_Actual.Equals(CurrentDate) && _ToDate_Actual.Equals(EndOfMonth))
                    {
                        Message = "From Date Appeared as  Selected Date:" + _FromDate_Actual + ";To Date Appeared as To Date:" + _ToDate_Actual;
                        return true;
                    }
                    else
                    {
                        Message = "From Date Appeared is not As Selected:" + _FromDate_Actual + ";To Date is Appeared as Selected:" + _ToDate_Actual;
                        throw new Exception(Message);
                    }
                }
                Message = "Failed To Verify Date Pickers Please refer Screenshot for More Details";
                throw new Exception(Message);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Verify Date pickers Due to:" + e.Message);
            }
        }

        public bool ValidateErrorMessage(ElementLocator Elm, string Errormessage, out string Message)
        {
            string AppearedErrorMesage = "";
            if (Driver.IsElementPresent(Elm, 2))
            {
                AppearedErrorMesage = Driver.GetElement(Elm).GetTextContent();
            }
            else
            {
                throw new Exception("Expected Error Message is not appeared:" + Errormessage);

            }
            if (AppearedErrorMesage.Contains(Errormessage))
            {
                Message = "Error Message appeared as expected;  Error Message Details are:" + Errormessage;
                return true;
            }
            Message = "Error Message appeared Different from expected Message; Expected Error Message Details are:" + Errormessage
                                                                                  + "Appeared Error Message Details are:" + AppearedErrorMesage;
            throw new Exception(Message);
        }
        public bool VerifyDBCount(int Value, string Query, out string Message)
        {
            int DBOutput = Convert.ToInt32(DatabaseUtility.GetCountFromDBBasedOnQuery(Query));
            if (DBOutput == Value)
            {
                Message = "Verifying Count in DB Based on Below Query:;" + Query + ";DB Ouput:" + DBOutput + " is Matching With Expected Input:" + Value;
                return true;
            }
            Message = "Verifying Count in DB Based on Below Query:;" + Query + ";DB Ouput:" + DBOutput + " is MisMatching With Expected Input:" + Value;
            throw new Exception(Message);
        }

        ElementLocator Button_AddNotes = new ElementLocator(Locator.XPath, "//a[text()='Add Note']");
        public ElementLocator Title_CSNote = new ElementLocator(Locator.XPath, "//h2[contains(text(),'Customer Service Notes')]");
        ElementLocator TextArea_Note = new ElementLocator(Locator.XPath, "//span[text()='Note']//parent::td//following-sibling::td//textarea");
        ElementLocator Button_CSSave = new ElementLocator(Locator.XPath, "//div[@id='CSNotesContainer']//a[text()='Save' and @class='button']");
        ElementLocator Button_CSCancel = new ElementLocator(Locator.XPath, "//div[@id='CSNotesContainer']//a[text()='Cancel' and @class='button']");

        public ElementLocator CSNOTES(string notes)
        {
            ElementLocator _elm = new ElementLocator(Locator.XPath, "//span[contains(text(),'"+ notes + "') and contains(@id,'CSNotes')]");
            return _elm;
        }

        public bool AddCSNotes(string Notes,out string Message )
        {
            if(Driver.IsElementPresent(Title_CSNote,1))
            {
                Driver.GetElement(Button_AddNotes).ClickElement();
                Driver.GetElement(TextArea_Note).SendText(Notes);
                Driver.GetElement(Button_CSSave).ClickElement();
                if(VerifyElementandScrollToElement(CSNOTES(Notes)))
                {
                    Message = "CS Notes Added Successfully:" + Notes;
                    return true;
                }
                throw new Exception("Failed to Add New CS Notes");
            }
            throw new Exception("No Cs Notes Appeared");

        }
        public TestStep CreateMember_UsingSoap(out Member member,out string LoyaltyID, List<TestStep> listOfTestSteps)
        {
            string stepName = "Create Member Using Soap WebServices";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                Member member1;
                member1 = CreateMemberThroughCDIS();
                member = member1;
                LoyaltyID = GetLoyaltyNumber(member);
                testStep.SetOutput("Member Created Successfully; Member Loyalty ID"+LoyaltyID +"; First Name:"+member.FirstName);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput("Unable to Create Member using Soap Request");
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception("Unable to Create Member using Soap Request Due to:"+e.Message);
            }
        }

        public TestStep VerfiyDateFromDateAndCurrentDate(List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify From Date as Start Day of the Month  and To Date as End of the Modnth On the Calendar Section";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                VerfiyDateFromDateAndCurrentDate(out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }
        public TestStep UpdateFromDateAndCurrentDate(List<TestStep> listOfTestSteps)
        {
            string stepName = "Verify From Date and To Date Can be Updated";
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                UpdateFromDateAndCurrentDate(out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }
        public TestStep AddCSNotes(string Page,string Notes, List<TestStep> listOfTestSteps)
        {
            string stepName = "Adding CS Notes Under Page:"+ Page;
            testStep = TestStepHelper.StartTestStep(testStep);
            try
            {
                AddCSNotes(Notes,out string Message);
                testStep.SetOutput(Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, true, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                return testStep;
            }
            catch (Exception e)
            {
                testStep.SetOutput(e.Message);
                testStep = TestStepHelper.EndTestStep(testCase, testStep, stepName, false, DriverContext.SendScreenshotImageContent("WEB"));
                listOfTestSteps.Add(testStep);
                throw new Exception(e.Message);
            }
        }

    }
}

