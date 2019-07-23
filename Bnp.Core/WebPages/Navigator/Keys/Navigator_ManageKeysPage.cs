using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using System;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.Keys
{  /// <summary>
   /// This class handles Login Navigator as Key Admin user > Navigate to Mange Keys Page
   /// </summary
    public class Navigator_ManageKeysPage : ProjectBasePage
    {
        /// <summary>
        /// Initializes the driverContext
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_ManageKeysPage(DriverContext driverContext)
        : base(driverContext)
        { }


        #region ElementLoactors
        private readonly ElementLocator orgnizationName_expand = new ElementLocator(Locator.XPath, ".//span[contains(text(),'" + Orgnization_value + "')]//preceding-sibling::span[@class='rtPlus']");
        private readonly ElementLocator organizationNode = new ElementLocator(Locator.XPath, ".//span[contains(text(),'" + Orgnization_value + "')]");
        private readonly ElementLocator env_Name = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + Orgnization_value + "')]//parent::div[contains(@class,'rtSelected')]//following-sibling::ul//span[contains(text(),'" + Env_value + "')]");
        private readonly ElementLocator keystorePassword = new ElementLocator(Locator.XPath, ".//input[contains(@id,'_tbNewKeystorePass')]");
        private readonly ElementLocator keystoreConfirmPassword = new ElementLocator(Locator.XPath, ".//input[contains(@id,'_tbConfirmKeystorePass')]");
        private readonly ElementLocator keySize = new ElementLocator(Locator.XPath, ".//select[contains(@id,'_ddlKeySize')]");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.XPath, "//*[contains(@id,'_btnSaveKeystorePass')]");
        private readonly ElementLocator changeButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'_btnChangeKeystorePass')]");
        private readonly ElementLocator exportButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'pnlKeyConfiguration_btnExportKeyfile')]");
        private readonly ElementLocator exportSymmetricKeyButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'pnlKeyConfiguration_cmdSymmetricExportKey')]");
        private readonly ElementLocator alertbox = new ElementLocator(Locator.XPath, "//*[contains(@text,'Are you sure you want to change the keystore password?')]");
        private readonly ElementLocator privateKey = new ElementLocator(Locator.XPath, "//textarea[contains(@id,'pnkTotalKeyManagement_pnlKeyConfiguration_tbKeystore')]");
        private readonly ElementLocator keyGeneratedMessage = new ElementLocator(Locator.LinkText, "New key has been generated.");
        private readonly ElementLocator verifyKeysArenotEmpty = new ElementLocator(Locator.XPath, "//textArea[contains(text(),'BEGIN ENCRYPTED PRIVATE KEY')]");
        private readonly ElementLocator verifyKeystorePassword = new ElementLocator(Locator.XPath, ".//input[contains(@id,'_tbNewKeystorePass')]//parent::span//parent::td//span[text()='Value is required.']");
        private readonly ElementLocator verifyKeystoreConfirmPassword = new ElementLocator(Locator.XPath, ".//input[contains(@id,'_tbConfirmKeystorePass')]//parent::span//parent::td//span[text()='Value is required.']");
        #endregion

        /// <summary>
        /// Enter Key Store Password 
        /// </summary>
        /// <param name="Keystore_Password">Message Name</param>
        /// <param name="Key_Size">Message Name</param>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true and Output if Keys already existed, Enter Key values if it is brand new or if the key value is empty
        /// </returns>
        public bool EnterKeyStorePassword(string Keystore_Password, string Key_Size, out string Output)
        {
            bool gen_Keys = false;
            if (Driver.IsElementPresent(changeButton, 3))
            {
                if (Driver.IsElementPresent(verifyKeysArenotEmpty, 3))
                {
                    gen_Keys = true;
                    Output = "Keys are already Avaialble and user can  export the same  Keys ";
                    return true;
                }
                else
                {
                    Driver.GetElement(changeButton).ClickElement();
                    Driver.GetElement(keystorePassword).SendText(Keystore_Password);
                    Driver.GetElement(keystoreConfirmPassword).SendText(Keystore_Password);
                    Driver.GetElement(saveButton).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    if (Driver.IsElementPresent(changeButton, 3))
                    {
                        gen_Keys = true;
                        Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                               ";Saved Successfully and Keys available  for Export";
                        return true;
                    }
                    else
                    {
                        Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                             ";Saved Successfully and Keys failed to Export";

                        throw new Exception("Key Details  are not Saved and failed to generate Keys");
                    }
                }
            }
            if (gen_Keys == false)
            {
                Driver.GetElement(keystorePassword).SendText(Keystore_Password);
                Driver.GetElement(keystoreConfirmPassword).SendText(Keystore_Password);
                Driver.GetElement(saveButton).ClickElement();
                if (Driver.IsElementPresent(changeButton, 5))
                {
                    Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                           ";Saved Successfully and Keys available  for Export";
                    return true;
                }
                else
                {
                    Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                         ";Saved Successfully and Keys failed to Export";

                    throw new Exception("Key Details  are not Saved and failed to generate Keys");

                }
            }
            Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size + ";Saved Successfully and Keys failed  to Export";
            throw new Exception("Key Details  are not Saved and failed to generate Keys");
        }

        /// <summary>
        /// Export Private Key By click on Export private key button
        /// </summary>
        /// <param name="keystore">Message Name</param>
        /// <param name="Message">Message Name</param>
        /// <returns>
        /// returns true and Message on Keystore.dat File Generation
        /// </returns>
        public void ExportPrivateKey(string keystore, out string Message)
        {
            Driver.GetElement(exportButton).ClickElement();
            Thread.Sleep(1000);
            VerifyExistedorDownloadedFile(keystore, "Keystore.dat File Generated Successfully , Keys Downloaded path:" + keystore, out Message);
        }

        /// <summary>
        /// Export Private Key By click on Export private key button
        /// </summary>
        /// <param name="symmetricKeystore">Message Name</param>
        /// <param name="Message">Message Name</param>
        /// <returns>
        /// returns true and Message on SymmetricKeystore.dat File Generation
        /// </returns>
        public void ExportSymmetrickey(string symmetricKeystore, out string Message)
        {
            Driver.GetElement(exportSymmetricKeyButton).ClickElement();
            Thread.Sleep(1000);
            VerifyExistedorDownloadedFile(symmetricKeystore, "SymmetricKeystore.dat File Generated Successfully , Keys Downloaded path:" + symmetricKeystore, out Message);
        }

        /// <summary>
        /// Verify Key generated or Not
        /// </summary>
        /// <returns> true if key is already generated otherwise return false</returns>
        public bool VerifyKeyGeneratedOrNot()
        {
            if(Driver.IsElementPresent(privateKey,.5) && Driver.IsElementPresent(verifyKeysArenotEmpty,.5) && Driver.IsElementPresent(exportSymmetricKeyButton,.5))
            {
                Driver.GetElement(changeButton).ClickElement();
                Driver.GetElement(saveButton).ClickElement();
                Driver.SwitchTo().Alert().Accept();
                Driver.ScrollIntoMiddle(keystorePassword);
                if(Driver.IsElementPresent(verifyKeystorePassword,.5) && Driver.IsElementPresent(verifyKeystoreConfirmPassword,.5))
                {
                    return true;
                }
            }
            else
            {
                if (Driver.IsElementPresent(keystorePassword,.5) && Driver.IsElementPresent(keystoreConfirmPassword,.5))
                {
                    Driver.GetElement(changeButton).ClickElement();
                    return false;
                }
            }
            return false;
        }

        public bool ChangeKey(string Keystore_Password, string Key_Size,out string  Output)
        {
            Output = "";
            if (Driver.IsElementPresent(privateKey, .5) && Driver.IsElementPresent(verifyKeysArenotEmpty, .5) && Driver.IsElementPresent(exportSymmetricKeyButton, .5))
            {
                Driver.GetElement(changeButton).ClickElement();
                if (Driver.IsElementPresent(keystorePassword, .5) && Driver.IsElementPresent(keystoreConfirmPassword, .5))
                {
                    Driver.GetElement(keystorePassword).SendText(Keystore_Password);
                    Driver.GetElement(keystoreConfirmPassword).SendText(Keystore_Password);
                    Driver.GetElement(saveButton).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    if (Driver.IsElementPresent(changeButton, 3))
                    {
                        Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                               ";Saved Successfully and Keys available  for Export";
                        return true;
                    }
                    else
                    {
                        Output = "Keystore_Password: " + Keystore_Password + ";KeyStore_Size:" + Key_Size +
                                                                             ";Saved Successfully and Keys failed to Export";

                        throw new Exception("Key Details  are not Saved and failed to generate Keys");
                    }
                }
            }
            else
            {
                Output = "There is no keys avilable to change ";
                throw new Exception("Key Details  are not Saved and failed to generate Keys");
            }
            throw new Exception("Key Details  are not Saved and failed to generate Keys");
        }

    }
}
