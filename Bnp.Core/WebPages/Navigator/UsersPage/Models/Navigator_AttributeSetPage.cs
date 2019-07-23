using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using System;
using System.Threading;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Models
{
    class Navigator_AttributeSetPage : ProjectBasePage
    {
        public Navigator_AttributeSetPage(DriverContext driverContext) : base(driverContext) { }    

        #region Attribute Set Page Locators
        private readonly ElementLocator Models_OnMainMenu = new ElementLocator(Locator.Id, "lblDataModeling");
        private readonly ElementLocator AttributeSetTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]");
        private readonly ElementLocator Status = new ElementLocator(Locator.XPath, "//span[contains(text(),'Status')]");
        private readonly ElementLocator DataModel_Assembly = new ElementLocator(Locator.XPath, "//span[contains(text(),'Data Model Assembly:')and contains(@id,'_pnlDataModeling_Label')]");
        private readonly ElementLocator CreateNewAttributeSet_Option = new ElementLocator(Locator.XPath, "//span[text()='New Attribute Set']");
        private readonly ElementLocator GenerateTable_Option = new ElementLocator(Locator.XPath, "//span[text()='Generate Table']");
        private readonly ElementLocator AttributeSetname = new ElementLocator(Locator.XPath, "//input[contains(@id,'tbAttribSetName') and contains(@class,'riEnabled')]");
        private readonly ElementLocator AttributeSetButton = new ElementLocator(Locator.XPath, "//a[contains(@id,'btnAttributeSetSave')]");
        private readonly ElementLocator CreateAttributeButton = new ElementLocator(Locator.XPath, "//a[contains(text(),'Create New Attribute')]");
        private readonly ElementLocator AttributeName = new ElementLocator(Locator.XPath, "//input[contains(@name,'Name')]");
        private readonly ElementLocator AttributeDisplayText = new ElementLocator(Locator.XPath, "//input[contains(@name,'DisplayText')]");
        private readonly ElementLocator AttributeType = new ElementLocator(Locator.XPath, "//select[contains(@name,'Type')]");
        private readonly ElementLocator AttributeAlias = new ElementLocator(Locator.XPath, "//input[contains(@name,'Alias')]");
        private readonly ElementLocator AttributeMinLength = new ElementLocator(Locator.XPath, "//input[contains(@name,'MinLength')]");
        private readonly ElementLocator AttributeMaxLength = new ElementLocator(Locator.XPath, "//input[contains(@name,'MaxLength')]");
        private readonly ElementLocator AttributeVisibleonGridCheckBox = new ElementLocator(Locator.XPath, "//input[contains(@name,'VisibleInGrid')]");
        private readonly ElementLocator AttributeSave = new ElementLocator(Locator.XPath, "//a[contains(@id,'_lnkSave')]");
        private readonly ElementLocator Txnheader_RulesTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Rules')]");
        private readonly ElementLocator Txnheader_AttributesTab = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attributes')]");
        private readonly ElementLocator Txnheader_CreateNewRule = new ElementLocator(Locator.XPath, "//a[contains(@id,'_pnlDataModeling_ctl11_ctl00_ctl05_ctl16_lnkAddNew_grdAttributeSetRules')]");
        private readonly ElementLocator PromotionValue = new ElementLocator(Locator.XPath, "//select[@id='ContentPlaceHolder1_pnlDataModeling_ctl11_ctl00_ctl05_ctl16_ctl04_Promotion']");
        #endregion
        public enum TxnHeaderTabs
        {
            Attributes,
            Rules
        }

        public bool ClickOnTree(string Tree, bool RightClick = false)
        {

            ElementLocator AttributeSet_MainTree = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div");
            ElementLocator AttributeSet_Tree = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']");
            ElementLocator AttributeSet_Tree_NotExpanded = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//preceding::span[1][@class='rtPlus']");
            ElementLocator AttributeSet_Tree_Expanded = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//preceding::span[1][@class='rtMinus']");

            if (Driver.IsElementPresent(AttributeSet_MainTree, 1) && Driver.IsElementPresent(AttributeSet_Tree_NotExpanded, 1))
            {
                Driver.GetElement(AttributeSet_Tree).ScrollToElement();
                Driver.GetElement(AttributeSet_Tree).ClickElement();
                Driver.GetElement(AttributeSet_Tree_NotExpanded).ClickElement();

                return true;
            }
            if (Driver.IsElementPresent(AttributeSet_MainTree, 1) && Driver.IsElementPresent(AttributeSet_Tree, 1))

            {
                Driver.GetElement(AttributeSet_Tree).ScrollToElement();
                Driver.GetElement(AttributeSet_Tree).ClickElement();
                if (RightClick)
                {
                    Driver.Actions().ContextClick(Driver.GetElement(AttributeSet_Tree)).Build().Perform();

                }
                return true;
            }
            else
            {
                return false;

            }
        }

        public bool ClickOnNode(string Tree, string Node, bool RightClick = false)
        {
            if (ClickOnTree(Tree))
            {
                ElementLocator Attribute_Node = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//parent::div//following-sibling::ul//span[text()='" + Node + "']");
                ElementLocator AttributeSet_Node_NotExpanded = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//parent::div//following-sibling::ul//span[text()='" + Node + "']//preceding::span[1][@class='rtPlus']");
                ElementLocator AttributeSet_Node_Expanded = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//parent::div//following-sibling::ul//span[text()='" + Node + "']//preceding::span[1][@class='rtMinus']");
                if (Driver.IsElementPresent(Attribute_Node, .1) && Driver.IsElementPresent(AttributeSet_Node_NotExpanded, .1))
                {
                    Driver.GetElement(Attribute_Node).ScrollToElement();
                    Driver.GetElement(Attribute_Node).ClickElement();
                    Driver.GetElement(AttributeSet_Node_NotExpanded).ClickElement();
                    return true;
                }
                else if (Driver.IsElementPresent(Attribute_Node, .1))
                {
                    Driver.GetElement(Attribute_Node).ScrollToElement();
                    Driver.GetElement(Attribute_Node).ClickElement();
                    if (RightClick)
                    {
                        Driver.ScrollIntoMiddle(Attribute_Node);

                        Driver.GetElement(Attribute_Node).ScrollToElement();
                        //Driver.GetElement(Attribute_Node).ClickElement();

                        Driver.Actions().ContextClick(Driver.GetElement(Attribute_Node)).Build().Perform();

                    }
                    // Driver.Actions().ContextClick(Driver.GetElement(Attribute_Node)).Build().Perform();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }

        public bool ClickOnChild(string Tree, string Node, string Child)
        {
            if (ClickOnNode(Tree, Node))
            {
                ElementLocator Attribute_Child = new ElementLocator(Locator.XPath, "//span[contains(text(),'Attribute Sets')]//parent::div//following-sibling::ul//span[text()='" + Tree + "']//parent::div//following-sibling::ul//span[text()='" + Node + "']//parent::div//following-sibling::ul//span[text()='" + Child + "']");
                if (Driver.IsElementPresent(Attribute_Child, 1))
                {
                    Driver.GetElement(Attribute_Child).ScrollToElement();
                    Driver.GetElement(Attribute_Child).ClickElement();
                    Driver.Actions().ContextClick(Driver.GetElement(Attribute_Child)).Build().Perform();
                    return true;
                }
                else { return false; }
            }
            else { return false; }
        }

        public bool IsAttributeSetExisted(string Tree, string Node)
        {
            return (ClickOnNode(Tree, Node));
        }

        public void CreateAttributeSet(string Tree, string Node)
        {
            try
            {
                if (ClickOnNode(Tree, Node))
                {

                }
                else
                {
                    ClickOnTree(Tree, true);
                    Driver.GetElement(CreateNewAttributeSet_Option).ScrollToElement();
                    Driver.GetElement(CreateNewAttributeSet_Option).ClickElement();
                    Driver.GetElement(AttributeSetname).SendText(Node);
                    Driver.GetElement(AttributeSetButton).ClickElement();
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Create Attribute Sets Refer screenshot for more info");
            }
        }

        public void GenerateTable(string Tree, string Node)
        {
            ClickOnNode(Tree, Node, true);
            Driver.GetElement(GenerateTable_Option).ScrollToElement();
            Driver.GetElement(GenerateTable_Option).JavaScriptClick();
            Thread.Sleep(100);
            ElementLocator SuccessMsg = new ElementLocator(Locator.XPath, "//strong[contains(text(),'Successfully created table for " + Node + "')]");
            if (Driver.IsElementPresent(SuccessMsg, 10)) { }
            else
            {
                throw new Exception("Failed to Generate Table for Attribute:" + Tree);
            }
        }

        public string CreateAttributes(string Tree, string Node, string Attributename, string AttributeDisplayText, string AttributeAlias, string AttributeType, string AttributeMinLength, string AttributeMaxLength)
        {
            ElementLocator AttributeElement = new ElementLocator(Locator.XPath, "//span[text()='"+ Attributename + "' and (contains(@id,'AttributesName'))]");
            try
            {
                if (Driver.IsElementPresent(AttributeElement, .1))
                {
                    return "Attribute Element is Already Available";
                }
                else
                {
                    ClickOnNode(Tree, Node);
                    Driver.GetElement(CreateAttributeButton).ScrollToElement();

                    Driver.GetElement(CreateAttributeButton).ClickElement();
                    Driver.GetElement(AttributeName).SendText(Attributename);
                    Driver.GetElement(this.AttributeDisplayText).SendText(AttributeDisplayText);
                    Driver.GetElement(this.AttributeAlias).SendText(AttributeAlias);
                    Select Attributetype = new Select(Driver.GetElement(this.AttributeType));
                    Attributetype.SelectByText(AttributeType);
                    Checkbox AttributeVisibleonGrid_CheckBox = new Checkbox(Driver.GetElement(AttributeVisibleonGridCheckBox));
                    AttributeVisibleonGrid_CheckBox.TickCheckbox();
                    Driver.GetElement(this.AttributeAlias).SendText(AttributeAlias);
                    Driver.GetElement(this.AttributeMinLength).SendText(AttributeMinLength);
                    Driver.GetElement(this.AttributeMaxLength).SendText(AttributeMaxLength);
                    Driver.GetElement(AttributeSave).ScrollToElement();
                    Driver.GetElement(AttributeSave).ClickElement();
                    return "Attribute Element is Created Successfully";
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed to Create Attributes"+Tree+" and Attribute sets:"+Node+"Refer screenshot for more info");
            }
        }

        /// <summary>
        /// NavigateTo AttributeSet > TxnHeaderTabs 
        /// </summary>
        /// <param name="txnHeaderTabs">TabName or PageName</param>
        /// <returns>
        /// Return True if able to Navigate to Respective Page otherwise return false
        /// </returns>
        public bool NavigateToAttributeSet_TxnHeaderTabs(TxnHeaderTabs txnHeaderTabs)
        {
            try
            {
                switch (txnHeaderTabs)
                {
                    case TxnHeaderTabs.Attributes:
                        Driver.GetElement(Txnheader_AttributesTab).ClickElement();
                        break;
                    case TxnHeaderTabs.Rules:
                        Driver.GetElement(Txnheader_RulesTab).ClickElement();
                        break;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifying The Promotion is listed or not on Rules Page 
        /// </summary>
        /// <param name="proCode">The PromotionCode To Verify.</param>
        /// <returns>
        /// Return True if Promotion Exist otherwise return false
        /// </returns>
        public bool ValidateProMotionIsListedInCreateRulePage(string proCode)
        {
            try
            {
                Driver.GetElement(Txnheader_CreateNewRule).ClickElement();     
                Select select = new Select(Driver.GetElement(PromotionValue));
                SelectElement_AndSelectByText(PromotionValue, proCode);
                Driver.GetElement(PromotionValue).ScrollToElement();
                return select.IsSelectOptionAvailable(proCode);
            }catch(Exception e)
            {
                throw new Exception("Promotion is Not Available,Promotion Details:"+ proCode);
            }
        }
    }
}
