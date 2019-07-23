using System;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using OpenQA.Selenium;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Models
{
    class Navigator_GenerateAssembly : ProjectBasePage
    {
        public Navigator_GenerateAssembly(DriverContext driverContext) : base(driverContext)
        {

        }

        #region ElementLoactors
        private readonly ElementLocator Link_model = new ElementLocator(Locator.XPath, "//span[text()='model']");
        private readonly ElementLocator Link_GenerateAssembly = new ElementLocator(Locator.XPath, "//a[text()='Generate Assembly']");
        private readonly ElementLocator Link_GenerateClientAssembly = new ElementLocator(Locator.XPath, "//a[text()='Client Assembly']");
        private readonly ElementLocator Button_Generate = new ElementLocator(Locator.XPath, "//a[text()='Generate']");
        private readonly ElementLocator Label_COnfig = new ElementLocator(Locator.XPath, "//span[text()='API ConfigFile:']");
        private readonly ElementLocator Radio_Java = new ElementLocator(Locator.XPath, "//input[@value='rbJava']");
        private readonly ElementLocator Radio_XSD = new ElementLocator(Locator.XPath, "//input[@value='rbXSD']");
        private readonly string Textbox_Org1 = "//input[@value='" + Orgnization_value + "' and contains(@id,'Org1')]";
        private readonly string Textbox_Env1 = "//input[@value='" + Env_value + "' and contains(@id,'Env1')]";
        #endregion

        /// <summary>
        /// Click on Generate button and generate the assembly
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful generation of assembly
        /// </returns>
        public bool GenerateAssembly(out string Output)
        {
            Click_OnButton(Link_GenerateAssembly);
            if ((Driver.IsElementPresent(By.XPath(Textbox_Org1))) & Driver.IsElementPresent(By.XPath(Textbox_Env1)))
            {
                Driver.GetElement(Button_Generate).ClickElement();
                if (Driver.IsElementPresent(Button_Generate, 1))
                {
                    Output = "Expected Orgnization and Environment details are matching ; Clicking on Generate Button is Successful";
                    return true;
                }
                else
                {
                    throw new Exception("FIle did not download");
                }
            }
            else
            {
                throw new Exception("Expected Orgnization and Environment details are not matching; Exepted Organization:" + Orgnization_value
                        + " Exepted Env: " + Env_value);
            }
        }

        /// <summary>
        /// Provide the config file path and Click on Generate button to generate the .dot net assembly
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful generation of .dotnet assembly
        /// </returns>
        public bool GenerateDotNetClientAssembly(out string Output)
        {
            Click_OnButton(Link_GenerateClientAssembly);
            if (VerifyGenerate())
            {
                Output = ".Net Assembly was generated";
                return true;
            }
            else
            {
              throw new Exception(".Net assembly was not generated");
            }
        }


        /// <summary>
        /// Provide the config file path and Click on Generate button to generate the java assembly
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful generation of java assembly
        /// </returns>
        public bool GenerateJavaClientAssembly(out string Output)
        {
            Click_OnButton(Link_GenerateClientAssembly);
            Click_OnButton(Radio_Java);
            if (VerifyGenerate())
            {
                Output = ".Java Assembly was generated";
                return true;
            }
            else
            {
                throw new Exception(".Java assembly was not generated");
            }

        }

        /// <summary>
        /// Provide the config file path and Click on Generate button to generate the XSD assembly
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful generation of XSD assembly
        /// </returns>
        public bool GenerateXSDClientAssembly(out string Output)
        {
            Click_OnButton(Link_GenerateClientAssembly);
            Click_OnButton(Radio_XSD);
            if (VerifyGenerate())
            {
                Output = ".XSD Assembly was generated";
                return true;
            }
            else
            {
                throw new Exception(".XSD assembly was not generated");
            }
         }


        /// <summary>
        /// Verifies if the assembly was generated successfully
        /// </summary>
        /// <param name="Output">Message Name</param>
        /// <returns>
        /// returns true On Successful generation of  assembly
        /// </returns>
        public bool VerifyGenerate()
        {
            try
            {
                if (Driver.IsElementPresent(Label_COnfig, 10))
                {
                    Driver.FindElement(By.XPath("//input[@class='ruFileInput']")).SendKeys(GenerateClientAssembly_value);
                    Thread.Sleep(2000);
                    Driver.GetElement(Button_Generate, BaseConfiguration.MediumTimeout).ClickElement();
                    if (Driver.IsElementPresent(Button_Generate, 5))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }catch(Exception e) { throw new Exception("Failed due t0" + e); }
        }
    }
}



