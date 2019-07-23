using Bnp.Core.WebPages.Models;
using Bnp.Core.WebPages.Navigator.UsersPage.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

/// <summary>
/// This class handles Navigator > Users > Promotion > Promotions > Promotion Page elements
/// </summary>
namespace Bnp.Core.WebPages.Navigator.Promotion
{
    public class Navigator_PromotionsPage : ProjectBasePage
    {
        #region Promotion Page Locators
        private readonly ElementLocator Button_CreateNewPromotion = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkAddNew_grdPromotions')]");
        private readonly ElementLocator Promotion_OnMainMenu = new ElementLocator(Locator.XPath, "//span[@id='lblPromotion']");
        private readonly ElementLocator Textbox_Code = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_Code')]");
        private readonly ElementLocator Textbox_Name = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_Name')]");
        private readonly ElementLocator Textbox_Description = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_PromotionDescription')]");
        private readonly ElementLocator Textbox_StartDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_StartDate_dateInput')]");
        private readonly ElementLocator Textbox_EndDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_EndDate_dateInput')]");
        private readonly ElementLocator Radiobutton_EnrollmentSupportType = new ElementLocator(Locator.XPath, "//table[contains(@id,'_pnlEdit_EnrollmentSupportType')]//tr//td[2]//input");
        private readonly ElementLocator Checkbox_Targeted = new ElementLocator(Locator.XPath, "//input[contains(@id,'_pnlEdit_Targeted')]");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[contains(@id,'_pnlEdit_lnkSave')]");
        private readonly ElementLocator Label_ExtendedAttributes = new ElementLocator(Locator.XPath, "//legend[contains(text(),'Extended Attributes')]");
        private readonly ElementLocator Button_Cancel = new ElementLocator(Locator.XPath, "//a[contains(@id, 'lnkCancel')]");
        private readonly ElementLocator Select_Property = new ElementLocator(Locator.XPath, "//select[contains(@id,'_drpSrchProp_Promotion')]");
        private readonly ElementLocator Textbox_SearchValue = new ElementLocator(Locator.XPath, ".//input[contains(@id,'_txtSearchValue_Promotion')]");
        private readonly ElementLocator Button_Search = new ElementLocator(Locator.XPath, "//a[contains(@id,'_btnSearch_Promotion')]");
        private readonly ElementLocator Button_Certs = new ElementLocator(Locator.XPath, "//a[contains(text(),'Certs')]");
        private readonly ElementLocator TextBox_CertNumberFormat = new ElementLocator(Locator.XPath, "//span[contains(text(),'Cert Number Format:')]//following::input[1]");
        private readonly ElementLocator TextBox_Quantity = new ElementLocator(Locator.XPath, "//input[contains(@id,'txtQuantity')][@type='text']");
        private readonly ElementLocator TextBox_GenerateCoupon_Expiry_Date = new ElementLocator(Locator.XPath, "//input[contains(@id,'dtEndDate_dateInput')]");
        private readonly ElementLocator Label_CurrentlyAvailableCount = new ElementLocator(Locator.XPath, "//span[contains(text(),'Currently Available:')]//following::span");
        private readonly ElementLocator Button_Generate = new ElementLocator(Locator.XPath, "//a[@class='button'][contains(text(),'Generate')]");
        private readonly ElementLocator TextBox_Start_Date = new ElementLocator(Locator.XPath, "//input[contains(@id,'StartDate_dateInput')]");
        private readonly ElementLocator Label_ErrorMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'Configuration directory not found at LWConfig=')]");
        private readonly ElementLocator Promotion_Name = new ElementLocator(Locator.XPath, "//table[contains(@id,'_pnlMain')]//tbody//tr//following::tr//td[2]");
        private readonly ElementLocator Promotion_Code = new ElementLocator(Locator.XPath, "//table[contains(@id,'_pnlMain')]//tbody//tr//following::tr//td[1]");
        private readonly ElementLocator Promotion_Description = new ElementLocator(Locator.XPath, "//table[contains(@id,'_pnlMain')]//tbody//tr//following::tr//td[3]");
        private readonly ElementLocator Promotion_Enrollmenttype = new ElementLocator(Locator.XPath, "//table[contains(@id,'_pnlMain')]//tbody//tr//following::tr//td[7]");
        private readonly ElementLocator Header_RulesFor = new ElementLocator(Locator.XPath, ".//h2[contains(text(),'Rules for')]");


        #endregion
        public enum PromotionTypes
        {
            Targeted = 0,
            NonTargeted = 1
        }
        public enum EnrollmentTypes
        {
            None =0,
            Supported = 1,
            Required = 2
        }

        /// <summary>
        /// Initializes the driverContext.
        /// </summary>
        /// <param name="driverContext"></param>
        public Navigator_PromotionsPage(DriverContext driverContext) : base(driverContext)
        {
        }


        public ElementLocator ActionsofPromotion(string Promotion, string Action)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[(text()='" + Promotion + "') and contains(@id,'PromotionsName')]//parent::td//following-sibling::td//a[text()='" + Action + "']");
            return _Menu;
        }

        public ElementLocator CustomizeRuleUnderPromotion(Rules Rule)
        {
            if(Rule.Rulestatus_ToInactive)
            {
                ElementLocator _Rule = new ElementLocator(Locator.XPath, ".//span[text()='"+Rule.RuleName+"' and contains(@id,'RulesName')]//..//..//td//input[@disabled='disabled' and contains(@id,'RulesActive')]//..//..//..//span[text()='"+Rule.RuleType+"' and contains(@id,'RuleType')]//..//..//span[contains(text(),'"+Rule.RuleOwner+"')and contains(@id,'RulesOwner')]//..//..//span[text()='"+Rule.Sequence+"' and contains(@id,'RulesSequence')]");
                return _Rule;
            }
            else
            {
                ElementLocator _Rule = new ElementLocator(Locator.XPath, ".//span[text()='" + Rule.RuleName + "' and contains(@id,'RulesName')]//..//..//td//input[@checked='checked' and contains(@id,'RulesActive')]//..//..//..//span[text()='" + Rule.RuleType + "' and contains(@id,'RuleType')]//..//..//span[contains(text(),'" + Rule.RuleOwner + "')and contains(@id,'RulesOwner')]//..//..//span[text()='"+Rule.Sequence+"' and contains(@id,'RulesSequence')]");
                return _Rule;
            }
        }
        public bool VerifyRuleUnderPromotion(Promotions promotion, Rules Rule,out string Message)
        {
            //var Promotion = new Application_Nav_Util_Page(DriverContext);
            //Promotion.OpenApplication(NavigatorEnums.ApplicationName.promotion);

            if (!Driver.IsElementPresent(Button_CreateNewPromotion, 2))
            {
                throw new Exception("Failed to Open Promotion:" + promotion.Name);
            }
            if (VerifyPromotionDetailsInGrid(promotion, "Name",out string msg))
            {
                Driver.GetElement(ActionsofPromotion(promotion.Name, "Rules")).ClickElement();
                if(Driver.IsElementPresent(Header_RulesFor,1)&&Driver.IsElementPresent(CustomizeRuleUnderPromotion(Rule),1))
                {
                    Driver.ScrollIntoMiddle(Header_RulesFor);
                    Message = " Rule Name:" + Rule.RuleName
                            + ";Rule Owner :" + Rule.RuleOwner
                            + ";Rule Type :" + Rule.RuleType
                            + ";Rule Invocation :" + Rule.Invocation
                            + ";Rule Status as InActive:" + Rule.Rulestatus_ToInactive.ToString()
                            + ";Rule Sequence:" + Rule.Sequence +";Appeared Under Rules section "; return true;
                }
                else { throw new Exception("Failed to Open Rules for Promotion:" + promotion.Name); }
            }
            throw new Exception("Failed to Open Promotion:" + promotion.Name);
        }


        public string Create_Promotions(string promotiontype, Promotions promotion, string enrollmenttype)
        {

            if (!VerifyPromotionsExists(promotion.Code, promotion.Name))
            {
                var Promotion = new Application_Nav_Util_Page(DriverContext);
                Promotion.OpenApplication(NavigatorEnums.ApplicationName.promotion);
                Click_OnButton(Button_CreateNewPromotion);
                Driver.GetElement(Textbox_Code).SendText(promotion.Code);
                Driver.GetElement(Textbox_Name).SendText(promotion.Name);
                Driver.GetElement(Textbox_Description).SendText(promotion.Description);
                RadioButton_OfPromotionEnrollment(enrollmenttype);
                Driver.GetElement(Textbox_StartDate).SendText(promotion.StartDate);
                Driver.GetElement(Textbox_EndDate).SendText(promotion.EndDate);
                if (PromotionTypes.Targeted.ToString() == promotiontype)
                {
                    CheckBoxElmandCheck(Checkbox_Targeted);
                }
                Click_OnButton(Button_Save);
                if (VerifyCheckboxCheckedorNot(promotiontype, promotion.Name))
                {
                    return ";Promotion created Successfully with below Details;Promotion Code :" + promotion.Code+";Promotion :" + promotion.Name + " ;Promotion Type: " + promotiontype + ";Enrollment Type:" + enrollmenttype;
                }
                else
                {
                    return ";Failed To Create Promotion: " + promotion.Name + " ;Promotion Type: " + promotiontype + ";Enrollment Type:" + enrollmenttype;
                }
            }
            else
            {
                return promotion.Name + " :  is already exist";
            }
        }

        /// <summary>
        /// Create Promotion with ExtendedAttributes
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns> true if ExtendedAttributes present otherwise false </returns>
        public bool CreatePromotionWithExtendedAttributes(Promotions promotion, out string msg)
        {
            try
            {
                if (!VerifyPromotionsExists(promotion.Code, promotion.Name))
                {
                    Click_OnButton(Button_CreateNewPromotion);
                    Driver.GetElement(Textbox_Code).SendText(promotion.Code);
                    Driver.GetElement(Textbox_Name).SendText(promotion.Name);
                    if (VerifyAttributeExistsInExtendedAttributes(promotion.AttributeName))
                    {
                        EnterText_ExtendedAttribute(promotion.AttributeName, promotion.ValueToSetInAttribute);
                        Click_OnButton(Button_Save);
                        if (VerifyPromotionsExists(promotion.Code, promotion.Name))
                        {
                            msg = "Promotion : " + promotion.Name + " created successfully with extended attribute";
                            return true;
                        }
                        else
                        {
                            throw new Exception("Failed to create Promotion " + promotion.Name);
                        }
                    }
                    else
                    {
                        throw new Exception("Extended attribute field is not present, refer the screenshot for more details.");
                    }
                }
                else
                {
                    msg = "Promotion already exists";
                    return true;
                }
            }
            catch
            {
                throw new Exception("Failed to create new promotion with extended attribute, refer screenshot for more info");
            }
        }
        /// <summary>
        /// Verify Targeted Checkbox is Checked Or NOt.
        /// </summary>
        /// <param name="promotiontype,PromotionName"> get the type of promotion and name of the promotion </param>        
        /// <returns>
        /// Returns true if it is checked otherwise false.
        /// </returns>
        public bool VerifyCheckboxCheckedorNot(string promotiontype, string promotionName)
        {
            SearchPromotion(promotionName, "Name");
            if (PromotionTypes.Targeted.ToString() == promotiontype)
            {
                IWebElement webelement = Driver.FindElement(By.XPath("//td//span[text()='" + promotionName + "']//parent::td//following-sibling::td//input[@checked='checked']"));
                var result = webelement.Selected == true ? true : false;
                return result;
            }
            else
            {
                if (PromotionTypes.NonTargeted.ToString() == promotiontype)
                {
                    IWebElement webelement = Driver.FindElement(By.XPath("//td//span[text()='" + promotionName + "']//parent::td//following-sibling::td//input[not(@checked='checked')]"));
                    var result = webelement.Selected == false ? true : false;
                    return result;
                }
            }
            return false;
        }

        /// <summary>
        /// Verifying The Newly Created Promotion is listed or Not On Rules Page 
        /// </summary>
        /// <param name="proCode">The PromotionCode To Verify.</param>
        /// <returns>
        /// Return True if Promotion Exist otherwise return false
        /// </returns>
        public bool VerifyPromotionisCreatedOrNot(string proCode)
        {
            bool result = false;
            var navigator_ModelHomePage = new Navigator_ModelHomePage(DriverContext);
            var navigator_AttributeSetPage = new Navigator_AttributeSetPage(DriverContext);
            navigator_ModelHomePage.NavigateToModels_Page(out string Message1);
            navigator_ModelHomePage.NavigatetoToAttributeSet_Page(out string Message2);
            navigator_AttributeSetPage.ClickOnNode("Virtual Card", "TxnHeader");
            navigator_AttributeSetPage.NavigateToAttributeSet_TxnHeaderTabs(Navigator_AttributeSetPage.TxnHeaderTabs.Rules);
            result = navigator_AttributeSetPage.ValidateProMotionIsListedInCreateRulePage(proCode);
            return result;
        }

        /// <summary>
        /// Verifies whether attributeName exists in ExtendedAttributes list on Promotions page    
        /// </summary>
        /// <param name="attributeName">The Attribute Name to search</param>        
        /// <returns>
        /// true if exists, else false
        /// </returns>
        public bool VerifyAttributeExistsInExtendedAttributes(string attributeName)
        {
            try
            {
                var attributeSetElement = Label_ExtendedAttributes.Value + "";
                if (Driver.IsElementPresent(Label_ExtendedAttributes, .1))
                {
                    Driver.GetElement(Label_ExtendedAttributes).ScrollToElement();
                    if (Driver.IsElementPresent(By.XPath("//tr/td/span[text()='" + attributeName + "']")))
                    {
                        return true;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Enter text in Extended Attribute field for the matching attributeName
        /// </summary>
        /// <param name="attributeName">Attribute name field</param>
        /// <param name="attributValue">Attribute value to set</param>
        private void EnterText_ExtendedAttribute(string attributeName, string attributValue)
        {
            ElementLocator TextBox_AttributeSetInput = new ElementLocator(Locator.XPath, "//span[text()='" + attributeName + "']//following::input[1]");
            Driver.GetElement(TextBox_AttributeSetInput).SendText(attributValue);
        }

        /// <summary>
        /// Verify the error message when more than 2000 characters provided in the content attribute field
        /// </summary>
        /// <param name="promotion"></param>
        /// <returns> true if ExtendedAttributes present otherwise false </returns>
        public bool VerifyTheErrorMessage_ForContentAttributeFieldExceedingTheMaxCharLimitInPromotionDefination(Promotions promotion, out string msg)
        {
            try
            {
                Click_OnButton(Button_CreateNewPromotion);
                Driver.GetElement(Textbox_Code).SendText(promotion.Code);
                Driver.GetElement(Textbox_Name).SendText(promotion.Name);
                if (VerifyAttributeExistsInExtendedAttributes(promotion.AttributeName))
                {
                    EnterText_ExtendedAttribute(promotion.AttributeName, promotion.ValueToSetInAttribute);
                    Click_OnButton(Button_Save);
                    ElementLocator errorMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'Attribute " + promotion.AttributeName + " cannot have a value > 2000 characters.')]");
                    if (Driver.IsElementPresent(errorMessage, .5))
                    {
                        msg = "Expected error message : \"Attribute " + promotion.AttributeName + " cannot have a value > 2000 characters.\" " + "displayed.";
                        return true;
                    }
                    else
                    {
                        throw new Exception("Expected error message didn't get displayed, refer the screenshot for more details.");
                    }
                }
                else
                {
                    throw new Exception("Expected attribute field is not displayed, refer the screenshot for more details.");
                }
            }
            catch
            {
                throw new Exception("Failed to validate the error message, refer the screenshot for more details.");
            }
        }

        /// <summary>
        /// Verifies attributes displayed under Extended attributes block on Promotions page
        /// </summary>
        /// <param name="attributeName"></param>
        /// <returns>
        /// Returns true if attributeName exists, else false
        /// </returns>
        public bool VerifyPromotionsPage_ForExtendedAttributes(string attributeName)
        {
            var status = false;
            Click_OnButton(Button_CreateNewPromotion);
            if (VerifyAttributeExistsInExtendedAttributes(attributeName))
            {
                status = true;
            }
            return status;
        }

        /// <summary>
        /// Verify if Promotions already exists
        /// </summary>
        /// <param name="PromotionsCode,PromotionsName ">Name and code of the Promotions</param>
        /// <returns>
        /// returns true if promotion exists, else false
        /// </returns>
        public bool VerifyPromotionsExists(string PromotionsCode, string PromotionsName)
        {
            try
            {
                if (Driver.IsElementPresent(By.XPath("//td[contains(@colspan,'14')]")))
                {
                    List<IWebElement> pagesTd = new List<IWebElement>(Driver.FindElements(By.XPath("//td[contains(@colspan,'14')]//table//tbody//tr//td")));
                    var pageCount = pagesTd.Count;
                    for (var i = 1; i <= pageCount; i++)
                    {
                        if (Driver.IsElementPresent(By.XPath("//a[contains(text(),'" + i + "')]")))
                        {
                            Driver.FindElement(By.XPath("//a[contains(text(),'" + i + "')]")).ClickElement();
                        }
                        if (Driver.IsElementPresent(By.XPath("//td//span[text()='" + PromotionsCode + "']")) || Driver.IsElementPresent(By.XPath("//td//span[text()='" + PromotionsName + "']")))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (Driver.IsElementPresent(By.XPath("//td//span[text()='" + PromotionsCode + "']")) || Driver.IsElementPresent(By.XPath("//td//span[text()='" + PromotionsName + "']")))
                    {
                        return true;
                    }
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Search Promotion
        /// </summary>
        /// <param name="promotionName">Name of the promotion</param>
        /// <param name="searchCriteria">Search Criteria </param>
        public void SearchPromotion(string promotionName, string searchCriteria)
        {
            //Select select = new Select(Driver.GetElement(Select_Property));
            //select.SelectByText(searchCriteria);
            SelectElement_AndSelectByText(Select_Property, searchCriteria);
            //Driver.GetElement(Textbox_SearchValue).Clear();
            Driver.GetElement(Textbox_SearchValue).SendText("   ");
            Driver.GetElement(Textbox_SearchValue).SendText(promotionName);
            Click_OnButton(Button_Search);
        }

        public void SearchPromotion(string promotionName, string searchCriteria,string test)
        {
            //Select select = new Select(Driver.GetElement(Select_Property));
            //select.SelectByText(searchCriteria);
            SelectElement_AndSelectByText(Select_Property, searchCriteria);
            //Driver.GetElement(Textbox_SearchValue).Clear();
            Driver.GetElement(Textbox_SearchValue).SendText(promotionName);
            Driver.GetElement(Textbox_SearchValue).SendText(promotionName);
            Click_OnButton(Button_Search);
        }



        public ElementLocator RadioButton_OfPromotionEnrollment(string radio_button)
        {
            ElementLocator Edit = new ElementLocator(Locator.XPath, ("//label[text()='"+ radio_button + "']//preceding-sibling::input"));
            Driver.GetElement(Edit).ClickElement();
            return Edit;
        }
        /// <summary>
        /// To Generate Promotion Certificate 
        /// </summary>
        /// <param name="CertNumberFormat"></param>
        /// <param name="Quantity"></param>
        /// <param name="startDate"></param>  
        /// <param name="expiryDate"></param> 
        /// <returns>
        /// returns string message of success if certificate Generated successfully
        /// </return
        public string GeneratePromotionCertificatesAndVerify(string CertNumberFormat, int Quantity, string startDate, string expiryDate)
        {
            string message = "Failed to Generate Promotion Certificate";
            try
            {
                Click_OnButton(Button_Certs);
                Driver.GetElement(TextBox_CertNumberFormat).SendText(CertNumberFormat);
                Driver.GetElement(TextBox_Quantity).SendText(Quantity.ToString());
                Driver.GetElement(TextBox_Start_Date).SendText(startDate);
                Driver.GetElement(TextBox_GenerateCoupon_Expiry_Date).SendText(expiryDate);
                int CurrentlyAvailableCountBefore = Convert.ToInt16(Driver.GetElement(Label_CurrentlyAvailableCount).GetTextContent());
                int TotalExpectedCertificates = Convert.ToInt16(CurrentlyAvailableCountBefore) + Convert.ToInt16(Quantity);

                //Driver.GetElement(Button_Generate).JavaScriptClick();
                Click_OnButton(Button_Generate);
                if (!Driver.IsElementPresent(Label_ErrorMessage, 1))
                {
                    Click_OnButton(Button_Certs);
                    Driver.GetElement(Label_CurrentlyAvailableCount).ScrollToElement();
                    int CurrentlyAvailableCertificateCount = Convert.ToInt16(Driver.GetElement(Label_CurrentlyAvailableCount).GetTextContent());
                    if (TotalExpectedCertificates == CurrentlyAvailableCertificateCount)
                    {
                        message = "Promotion Certificate Generated Successfully and Total Expected CERTS:" + TotalExpectedCertificates;
                        return message;
                    }
                    else
                    {
                        throw new Exception("Failed to Generate Promotion Certificate as expected: Total Available CERTS:" + TotalExpectedCertificates+"But Total Avilable Certs are:" +CurrentlyAvailableCertificateCount);
                    }
                }
                throw new Exception("Failed to Generate Promotion Certificate");
            }
            catch (Exception)
            {
                throw new Exception("Failed to Generate Promotion Certificate");
            }
        }

        /// <summary>
        /// To Verify Promotion Details in Database  
        /// </summary>
        /// <param name="Name"></param>  
        /// <param name="Description"></param> 
        /// <returns>
        /// returns true if we are able to get promotion details otherwise return false
        /// </return
        public bool VerifyPromotionDetailsInDatabese(string Name, string Description, string EnrollmentType, string Targeted)
        {
            Promotions promotion = new Promotions();
            int Etype = 0;
            int Ptype = 1;
            string inputType = EnrollmentType;
            switch(EnrollmentType)
            {
                case "None":
                    Etype = (int)EnrollmentTypes.None; break;
                case "Supported":
                    Etype = (int)EnrollmentTypes.Supported; break;
                case "Required":
                    Etype = (int)EnrollmentTypes.Required; break;
                default:
                    return false;
            }
            switch (Targeted)
            {
                case "Targeted":
                    Ptype = 1;break;
                case "NonTargeted":
                    Ptype = 0; break;
                default:
                    return false;
            }

            promotion = GetPromotionDetailsFromlw_promotionTableFromDB(Name, Etype, Ptype, out string message);
            if (Name.Equals(promotion.Name) && Description.Equals(promotion.Description))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// To Verify Promotion Details in Grid  
        /// </summary>
        /// <param name="promotion"></param>  
        /// <param name="searchCriteria"></param> 
        /// <returns>
        /// returns true if we are able to get promotion details otherwise return false
        /// </return
        public bool VerifyPromotionDetailsInGrid(Promotions promotion, string searchCriteria, out string outMsg)
        {
            SearchPromotion(promotion.Name, searchCriteria);
            var promotionName = Driver.GetElement(Promotion_Name).Text;
            var promotionCode = Driver.GetElement(Promotion_Code).Text;
            var promotionDescription = Driver.GetElement(Promotion_Description).Text;
            var promotionEnrollmenttype = Driver.GetElement(Promotion_Enrollmenttype).Text;
            if (promotionName.Equals(promotion.Name) && promotionCode.Equals(promotion.Code) && promotionDescription.Equals(promotion.Description) && promotionEnrollmenttype.Equals(promotion.Enrollmenttype))
            {
                outMsg = promotion.Name + " Promotion verified successfully";
                return true;
            }
            else
            {
                throw new Exception("Failed to verify Promotion" + promotion.Name);
            }
        }

        /// <summary>
        /// To Verify Promotion Details in Grid after Rollback the Migrated Promotion 
        /// </summary>
        /// <param name="promotion"></param>  
        /// <param name="searchCriteria"></param> 
        /// <returns>
        /// returns true if we are able to get promotion details otherwise return false
        /// </return
        public bool VerifyPromotionAfterRollback(string promotionName, string searchCriteria, out string outMsg)
        {
            SearchPromotion(promotionName, searchCriteria);
            if (!Driver.IsElementPresent(Promotion_Name, .5))
            {
                outMsg = "Promotion verified successully After RollBack";
                return true;
            }
            throw new Exception("Failed To Verify Promotion After Rollback");
        }
    }
}
