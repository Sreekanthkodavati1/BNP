using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System.Text;
using System.IO;
using System;
using Bnp.Core.WebPages.Models;

namespace Bnp.Core.WebPages.Navigator.FrameworkConfig
{
    public class Navigator_Orgnizations_FrameworkConfigurationPage : ProjectBasePage
    {
        public Navigator_Orgnizations_FrameworkConfigurationPage(DriverContext driverContext)
       : base(driverContext)
        { }

        StringBuilder stringWriter = new StringBuilder();

        #region Framework Configuration Element locators               
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//*[contains(@id,'ContentPlaceHolder1_pnlOrganizations_ConfigPanel_btnSaveOrg')]");
        private readonly ElementLocator Button_Export = new ElementLocator(Locator.XPath, "//*[@id='ContentPlaceHolder1_pnlOrganizations_ConfigPanel_btnExportConfig']");
        private readonly ElementLocator editRowOption = new ElementLocator(Locator.XPath, "//*[@id='ContentPlaceHolder1_pnlOrganizations_ConfigPanel_ctl04_ctl00_ctl02_lnkEdit_grdEnvironments_0']");
        private readonly ElementLocator TextBox_ConfigName = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlOrganizations_ConfigPanel_') and contains(@id,'_Name')]");
        private readonly ElementLocator TextBox_ConfigValue = new ElementLocator(Locator.XPath, "//textarea[contains(@id,'_pnlOrganizations_ConfigPanel_') and contains(@id,'_Value')]");
        private readonly ElementLocator Button_NewConfig_Save = new ElementLocator(Locator.XPath, "//a[contains(@id,'ContentPlaceHolder1_pnlOrganizations_ConfigPanel_ctl04_ctl01_lnkSave')]");
        private readonly ElementLocator Button_Edit = new ElementLocator(Locator.XPath, "//span[contains(text(),'LoyaltyCurrencyAsPayment DefaultCurrency')]//parent::td//parent::tr//td[3]//a[text()='Edit']");
        private readonly ElementLocator TextBox_Name = new ElementLocator(Locator.XPath, "//input[contains(@id,'_Name')]");
        private readonly ElementLocator TextBox_Value = new ElementLocator(Locator.XPath, "//textarea[contains(@id,'_Value')]");
        private readonly ElementLocator Button_SaveProperty = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkSave')]");
        #endregion

        public void ExportFrameworkCfgFile(string cfgFile)
        {
            Driver.WaitUntilElementIsNoLongerFound(Button_Save, 10);
            if (!File.Exists(cfgFile))
            { CreateFramewokConfgFile(); }
            else { CopyFile(cfgFile, ConfigUploadPath + @"\Framework.cfg"); }

            VerifyExistedorDownloadedFile(cfgFile, "Framework.cfg File Generated Successfully , Cfg File Downloaded path:" + cfgFile, out string Output);
        }

        public void CreateFramewokConfgFile()
        {
            string configFileData = File.ReadAllText(Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + "\\WebPages\\Navigator\\FrameworkConfig\\Framework.cfg");
            configFileData = configFileData.Replace("\"Report\"", "\"" + Navigator_LoginPage.Orgnization_value + "\"");
            configFileData = configFileData.Replace("Name=\"DmcUrl\" Value=\"", "Name=\"DmcUrl\" Value=\"" + FrameworkConfigData.DmcUrl);
            configFileData = configFileData.Replace("Name=\"DmcUsername\" Value=\"", "Name=\"DmcUsername\" Value=\"" + FrameworkConfigData.DmcUsername);
            configFileData = configFileData.Replace("Name=\"DmcPassword\" Value=\"", "Name=\"DmcPassword\" Value=\"" + FrameworkConfigData.DmcPassword);
            configFileData = configFileData.Replace("Name=\"LWEmailProvider\" Value=\"", "Name=\"LWEmailProvider\" Value=\"" + FrameworkConfigData.LWEmailProvider);
            configFileData = configFileData.Replace("Name=\"LoyaltyCurrencyAsPayment DefaultCurrency\" Value=\"", "Name=\"LoyaltyCurrencyAsPayment DefaultCurrency\" Value=\"" + FrameworkConfigData.LoyaltyCurrencyAsPayment);
            string[] allLines = configFileData.Split('\n');
            foreach (var line in allLines)
            {
                if (line.Trim().StartsWith("<ConfigurationEntry"))
                {
                    string[] lines = line.Split('\"');
                    var configName = lines[1];
                    var value = lines[3];
                    SetAConfigValue(configName, value);
                }
            }
            File.WriteAllLines(ConfigDownloadPath + @"\Framework.cfg", allLines);
        }

        public bool SetAConfigValue(string ConfigKey, string ValueToSetItAs)
        {
            try
            {
                try
                {
                    if (Driver.IsElementPresent(By.XPath("//span[text()='" + ConfigKey + "']")))
                    {
                        string CurrentValue = Driver.FindElement(By.XPath("//span[text()='" + ConfigKey + "']//parent::td//following-sibling::td//span")).Text;
                        if (CurrentValue == ValueToSetItAs)
                        {
                            return true;
                        }
                        else
                        {
                            try
                            {
                                Driver.FindElement(By.XPath("//span[text()='" + ConfigKey + "']//parent::td//parent::tr//following::td//a")).ClickElement();
                                if (ValueToSetItAs.Length >= 0)
                                {
                                    Driver.GetElement(TextBox_ConfigValue).SendText(ValueToSetItAs);
                                }
                                Click_OnButton(Button_NewConfig_Save);
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfgFile"></param>
        /// <param name="rate"></param>
        /// <returns></returns>

        public string EditDefaultCurrencyValueInFrameworkConfiguration(string cfgFile, string DefaultCurrencyValue, out string Status)
        {
            Status = "The Default Currency Value is updated Successfully as "+ DefaultCurrencyValue;
            try
            {
                if (!File.Exists(cfgFile))
                {
                    Click_OnButton(Button_Edit);
                    string ConfigName = Driver.GetElement(TextBox_Name).GetAttribute("value");
                    if (ConfigName.Contains("LoyaltyCurrencyAsPayment DefaultCurrency"))
                    {
                        Driver.GetElement(TextBox_Value).SendText(DefaultCurrencyValue);
                        Click_OnButton(Button_SaveProperty);
                        return Status;
                    }
                    else
                    {
                        Status = "Failed to update default Currency value in Framework Configuration file";
                        return Status;
                    }
                }
                else
                {
                    Status = "Failed to Verify the existence of Configration file";
                    return Status;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to update default Currency value in Framework Configuration file", e);
            }
        }
    }
}