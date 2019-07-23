using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Helpers;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Bnp.Core.WebPages.Navigator.UsersPage
{
    /// <summary>
    /// This class handles Navigator > Users > Migration
    /// </summary>
    public class Navigator_MigrationPage : ProjectBasePage
    {

        public Navigator_MigrationPage(DriverContext driverContext)
        : base(driverContext)
        {
        }

        #region Users page locators
        private readonly ElementLocator Button_CreateNewMigrationSet = new ElementLocator(Locator.XPath, "//a[text()='Create New Migration Set'][@class='button addNew']");
        private readonly ElementLocator TextBox_Name = new ElementLocator(Locator.XPath, "//input[contains(@id,'Name')]");
        private readonly ElementLocator Option_Manual = new ElementLocator(Locator.XPath, "//label[text()='Manual']//preceding-sibling::input");
        private readonly ElementLocator Button_Save = new ElementLocator(Locator.XPath, "//a[text()='Save'][contains(@id,'lnkSave')]");
        private readonly ElementLocator Button_Save_OnMigratonSet = new ElementLocator(Locator.XPath, "//a[contains(text(),'Save')]");
        private readonly ElementLocator Button_Refresh = new ElementLocator(Locator.XPath, "//a[text()='Refresh']");
        private readonly ElementLocator Button_SelectNone = new ElementLocator(Locator.XPath, "//a[text()='Select None']");
        private readonly ElementLocator Select_SourceEnvironment = new ElementLocator(Locator.XPath, "//span[contains(text(),'Source Environment:')] //..//..//td//following-sibling::td//select");
        private readonly ElementLocator Option_ChangedSince = new ElementLocator(Locator.XPath, "//label[contains(text(),'Changed Since')]//preceding-sibling::input");
        private readonly ElementLocator Text_ChangedSince = new ElementLocator(Locator.XPath, "//input[contains(@id,'dateInput')][@type='text']");
        private readonly ElementLocator Button_GenerateItems = new ElementLocator(Locator.XPath, "//a[contains(text(),'Generate Items')]");
        private readonly ElementLocator ErrorMessage = new ElementLocator(Locator.XPath, "//strong[contains(text(),'Unable to schedule job:')]");
        private readonly ElementLocator Select_ContentAttributeDef = new ElementLocator(Locator.XPath, "//span[text()='ContentAttributeDef']");
        private readonly ElementLocator Button_ProductImage = new ElementLocator(Locator.XPath, "//span[text()='ProductImage']");
        private readonly ElementLocator Select_ProductImage = new ElementLocator(Locator.XPath, "//span[text()='ProductImage']/preceding-sibling::input[@class='rtChk']");
        private readonly ElementLocator Select_Product = new ElementLocator(Locator.XPath, "//span[text()='Product']/preceding-sibling::input[@class='rtChk']");
        private readonly ElementLocator ExpandPathOfNode_Product = new ElementLocator(Locator.XPath, "//div[@class='rtBot']//span[@class='rtPlus']");
        private readonly ElementLocator ExpandPathOfNode_LocationGroup = new ElementLocator(Locator.XPath, "  //span[@class='rtIn'][contains(text(),'LocationGroups')]");
        private readonly ElementLocator Option_Scheduled = new ElementLocator(Locator.XPath, "//label[contains(text(),'Scheduled')]//preceding-sibling::input[1]");
        private readonly ElementLocator Scheduled_Time = new ElementLocator(Locator.XPath, "//span[contains(@id,'__dtScheduleDate_dateInput_wrapper')]//input[1]");
        private readonly ElementLocator Legend_EditItemsText = new ElementLocator(Locator.XPath, "//legend[contains(text(),'Set Generation Parameters')]");
        private readonly ElementLocator Lable_EditText= new ElementLocator(Locator.XPath, "//h2[contains(text(),'Edit Migration Set')]");
        private readonly ElementLocator Node_BusinessRule = new ElementLocator(Locator.XPath, "//span[text()='BusinessRule']/preceding-sibling::input[@class='rtChk']");

        #endregion

        public ElementLocator MigrationSetName(string MigrationSetName)
        {
            ElementLocator _MigrationSetName = new ElementLocator(Locator.XPath, "//span[contains(@id,'DataMigrationSetsName')][contains(text(),'" + MigrationSetName + "')]");
            return _MigrationSetName;
        }

        public ElementLocator ActionOnMigrationSetName(string MigrationSetName, string Action)
        {
            ElementLocator _MigrationSetName = new ElementLocator(Locator.XPath, "//span[contains(@id,'DataMigrationSetsName')][contains(text(),'" + MigrationSetName + "')]//../../td//a[text()='" + Action + "']");
            return _MigrationSetName;
        }

        public ElementLocator ValidateMigrationSetName(string MigrationSetName, string Action, string status)
        {
            ElementLocator _MigrationSetName = new ElementLocator(Locator.XPath, "//span[contains(@id,'DataMigrationSetsName')][contains(text(),'" + MigrationSetName + "')]//../../td//a[text()='" + Action + "']");
            return _MigrationSetName;
        }
        public bool InitiateMigrationSet(string MigrationSet_Name, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Initiate")))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Initiate")).ClickElement();
                    if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Approve")))
                    {
                        Message = "Migration Set:" + MigrationSet_Name + " Initiation Action is completed; Approve Option Appeared as Expected";
                        return true;
                    }
                }
                throw new Exception("Failed to Initiate Migration Set; Set Details are:" + MigrationSet_Name);
            }
            catch (Exception e) { throw new Exception("Failed to Initiate Migration Set; Set Details are:" + MigrationSet_Name); }
        }

        public bool ApproveMigrationSet(string MigrationSet_Name, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Approve")))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Approve")).ClickElement();
                    if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Run Now")))
                    {
                        Message = "Migration Set:" + MigrationSet_Name + " Approve Action is completed; Run Now Option Appeared as Expected";
                        return true;
                    }
                }
                throw new Exception("Failed to Approve Migration Set; Set Details are:" + MigrationSet_Name);
            }
            catch (Exception e) { throw new Exception("Failed to Approve Migration Set; Set Details are:" + MigrationSet_Name); }
        }

        public bool RunNowMigrationSet(string MigrationSet_Name, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Run Now")))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Run Now")).ClickElement();
                    if (Driver.IsElementPresent(ErrorMessage, 1))
                    {
                        throw new Exception("Failed to Run  Migration Set Due to:Unable to schedule job Error and Scheduler is not pointed to Current Environment; Set Details are:" + MigrationSet_Name);
                    }
                    Driver.GetElement(Button_Refresh).ClickElement();
                    if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Rollback")))
                    {
                        Message = "Migration Set:" + MigrationSet_Name + " Run Now Action is completed; Rollback Option Appeared as Expected";
                        return true;
                    }
                }
                throw new Exception("Failed to Run Migration Set; Set Details are:" + MigrationSet_Name);
            }
            catch (Exception e) { throw new Exception("Failed to Run Migration Set; Set Details are:" + MigrationSet_Name); }
        }

        public bool DeleteIfMigrationSetExists(string MigrationSet_Name, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(MigrationSetName(MigrationSet_Name)))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Delete")).ClickElement();
                    string alertText = Driver.SwitchTo().Alert().Text;
                    if (alertText.Contains("Are you sure you want to "))
                    {
                        Driver.SwitchTo().Alert().Accept();
                    }
                    if (!VerifyElementandScrollToElement(MigrationSetName(MigrationSet_Name)))
                    {
                        Message = "Migration Set Delete Successfully and Set Details are:" + MigrationSet_Name;
                        return true;
                    }
                }

                Message = "No Migration Set Avilable,Creating new and Set Details are:" + MigrationSet_Name;
                return true;
            }
            catch (Exception e) { throw new Exception("Failed to Delete Migration Set; Set Details are:" + MigrationSet_Name); }
        }

        public bool MigrationSetCreation(string MigrationSet, out string Message, bool IsScheduled = false)
        {
            Driver.GetElement(Button_CreateNewMigrationSet).ClickElement();
            Driver.GetElement(TextBox_Name).SendText(MigrationSet);
            if (IsScheduled)
            {
                Driver.GetElement(Option_Scheduled).ClickElement();
                var date = DateHelper.GeneratePastTimeStampBasedonMin(1);
                Driver.GetElement(Scheduled_Time).SendText(date);
            }
            else
            {
                Driver.GetElement(Option_Manual).ClickElement();
            }
            Driver.GetElement(Button_Save).ClickElement();
            if (VerifyElementandScrollToElement(MigrationSetName(MigrationSet)))
            {
                Message = "Migration Set Created Successfully ; Migration Set Name:" + MigrationSet + " ; Scheduled  Type: Manual";
                return true;
            }
            throw new Exception("Failed to Create Migration Set:  ; Migration Set Name:" + MigrationSet + ";Refer Screenshot for moredetails");
        }

        public void EditItems(string MigrationSet, string Environment, string date)
        {
            try
            {
                Driver.GetElement(ActionOnMigrationSetName(MigrationSet, "Edit Items")).ClickElement();
                Driver.GetElement(Button_SelectNone).ClickElement();
                SelectElement_AndSelectByText(Select_SourceEnvironment, Environment);
                Driver.GetElement(Option_ChangedSince).ClickElement();
                Driver.GetElement(Text_ChangedSince).SendText(date);
            }
            catch (Exception e) { throw new Exception("Failed to Edit Items Refer Screenshot for more infromation"); }
        }

        public bool SelectItemsForCouponDef(bool CategoryStatus, string CategoryName, string Coupon, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Coupons"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("CouponDef"), false);
                Driver.GetElement(ExpandPathOfNode("CouponDef")).ClickElement();
                if (!CategoryStatus)
                {
                    Driver.GetElement(ExpandPathOfNode("Category")).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree(CategoryName));
                }
                CheckBoxElmandCheck(Migration_Obj_Tree(Coupon));

                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();

                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;Coupon:" + Coupon + ";Category:" + CategoryName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool SelectItemsForWebsiteModuleMigration(string ModuleName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Websites"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("Website"), false);
                Driver.GetElement(ExpandPathOfNode("Website")).ClickElement();
                Driver.GetElement(ExpandPathOfNode("Module")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(ModuleName));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully for Websites:";
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool SelectItemsForMessage(string MigrationSet,out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Messages"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("MessageDef"), false);
                Driver.GetElement(ExpandPathOfNode("MessageDef")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree(MigrationSet), true);
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected; " + MigrationSet + " MEssage ";
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public ElementLocator SelectTypeToGenerate(string ModuleName)
        {
            var _SelectTypeToGenerate = new ElementLocator(Locator.XPath, "//label[text()='" + ModuleName + "']//preceding-sibling::input");
            if (Driver.IsElementPresent(_SelectTypeToGenerate, 1))
            {
                return _SelectTypeToGenerate;
            }
            throw new Exception("Failed to Select Type under Types to Generate:" + ModuleName);
        }

        public ElementLocator Migration_Obj_Tree(string MainDef)
        {
            var ElementLocator_Checkbox = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + MainDef + "')]//preceding-sibling::input");
            if (Driver.IsElementPresent(ElementLocator_Checkbox, 1))
            {
                return ElementLocator_Checkbox;
            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + MainDef);

        }

        public ElementLocator ExpandPathOfNode(string MainDef)
        {
            var ElementLocator_ExpandedAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='rtTop']//span[@class='rtPlus']");
            var ElementLocator_CollapseAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='rtTop']//span[@class='rtMinus']");

            if (Driver.IsElementPresent(ElementLocator_ExpandedAll, 1))
            {
                return ElementLocator_ExpandedAll;
            }
            else if (Driver.IsElementPresent(ElementLocator_CollapseAll, 1))
            {
                return ElementLocator_ExpandedAll;

            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + ElementLocator_ExpandedAll);
        }


        public void EditItems_All(string MigrationSet, string Environment, string date)
        {
            try
            {
                Driver.GetElement(ActionOnMigrationSetName(MigrationSet, "Edit Items")).ClickElement();
                if (Driver.IsElementPresent(Select_SourceEnvironment, 2))
                {
                    Driver.ScrollIntoMiddle(Select_SourceEnvironment);
                    SelectElement_AndSelectByText(Select_SourceEnvironment, Environment);
                }
                Driver.GetElement(Button_SelectNone).ClickElement();
            }
            catch (Exception e) { throw new Exception("Failed to Edit Items Refer Screenshot for more infromation"); }
        }

        /// <summary>
        /// Verify Checkbox Is There Or Not
        /// </summary>
        /// <param name="MigrationSet"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if CheckBox is Present otherwise return false</returns>
        public bool ClickOnEditItemsAndVerifyCheckbox(string MigrationSet, string lableName, out string Output)
        {
            Driver.GetElement(ActionOnMigrationSetName(MigrationSet, "Edit Items")).ClickElement();
            Driver.GetElement(Button_SelectNone).ClickElement();
            var _SelectTypeToGenerate = new ElementLocator(Locator.XPath, "//label[text()='" + lableName + "']//preceding-sibling::input");
            if (Driver.IsElementPresent(_SelectTypeToGenerate, .5))
            {
                Output = lableName + " CheckBox Verified Successfully";
                return true;
            }
            throw new Exception("Failed To Verify CheckBox");
        }

        public ElementLocator Migration_Obj_Tree_Value(string MainDef)
        {
            var ElementLocator_Checkbox = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + MainDef + " (Initiated)" + "')]//preceding-sibling::input");
            if (Driver.IsElementPresent(ElementLocator_Checkbox, 1))
            {
                return ElementLocator_Checkbox;
            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + MainDef);

        }

        /// <summary>
        /// Select Items for Promotion while Migrating
        /// </summary>
        /// <param name="Promotion"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForPromotionDef(string Promotion, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Promotions"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("Promotion"), false);
                Driver.GetElement(ExpandPathOfNode("Promotion")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(Promotion));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;Promotion:" + Promotion;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool SelectItemsForPromotionDef_WithRule(string Rule, string Promotion, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Promotions"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("Promotion"), false);
                Driver.GetElement(ExpandPathOfNode("Promotion")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree(Promotion));

                Driver.GetElement(ExpandPathOfNode("BusinessRule")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree(Rule));


                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();

                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;Promotion:" + Promotion + ";Rule:" + Rule;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool RollbackMigrationSet(string MigrationSet_Name, out string Message)
        {
            try
            {
                if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Rollback")))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Rollback")).ClickElement();
                    if (Driver.IsElementPresent(ErrorMessage, 1))
                    {
                        throw new Exception("Failed to Run  Migration Set Due to:Unable to schedule job Error and Scheduler is not pointed to Current Environment; Set Details are:" + MigrationSet_Name);
                    }
                    Driver.GetElement(Button_Refresh).ClickElement();
                    if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSet_Name, "Restart")))
                    {
                        Message = "Migration Set:" + MigrationSet_Name + " Rollback Action is completed; Restart Option Appeared as Expected";
                        return true;
                    }
                }
                throw new Exception("Failed to Rollback Migration Set; Set Details are:" + MigrationSet_Name);
            }
            catch (Exception e) { throw new Exception("Failed to Rollback Migration Set; Set Details are:" + MigrationSet_Name); }

        }

        /// <summary>
        /// Select Items for Product with AttributeName while Migrating
        /// </summary>
        /// <param name="AttributeName"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool SelectItemsForProductAttribute(string AttributeName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Products"));
                Click_OnButton(Button_GenerateItems);
                CheckBoxElmandCheck(Migration_Obj_Tree("Product"), false);
                Driver.GetElement(ExpandPathOfNode("Product")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree("ContentAttributeDef"), false);
                Driver.GetElement(Select_ContentAttributeDef).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree(AttributeName));
                Click_OnButton(Button_Save_OnMigratonSet);
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;AttributeName:" + AttributeName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items", e); }
        }


        public bool SelectItemsForAWSEMail(bool CategoryStatus, string CategoryName, string mailName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Emails"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("Email"), false);
                Driver.GetElement(ExpandPathOfNode("Email")).ClickElement();
                if (!CategoryStatus)
                {
                    Driver.GetElement(ExpandPathOfNode("Emails")).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree(CategoryName));
                }
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(mailName));

                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();

                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected; mail: " + mailName + ";Category:" + CategoryName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool SelectItemsForTier(string tierName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Tiers"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("TierDef"), false);
                Driver.GetElement(ExpandPathOfNode("TierDef")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(tierName));

                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();

                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected; tier : " + tierName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        /// <summary>
        /// Select Items for Product while Migrating
        /// </summary>
        /// <param name="ProductName"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool SelectItemsForProductImage(string ProductName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Category"));
                CheckBoxElmandCheck(SelectTypeToGenerate("ProductImage"));
                Click_OnButton(Button_GenerateItems);
                CheckBoxElmandCheck(Migration_Obj_Tree("Category"), false);
                Driver.GetElement(ExpandPathOfNode("Category")).ClickElement();
                CheckBoxElmandCheck(Select_ProductImage, false);
                Driver.GetElement(Button_ProductImage).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree("Product"), false);
                Driver.GetElement(ExpandPathOfNode("Product")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree(ProductName));
                Click_OnButton(Button_Save_OnMigratonSet);
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected ProductName: " + ProductName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items", e); }
        }

        /// <summary>
        /// Verify the error message for newly create Migration Set in Migration Page
        /// </summary>
        /// <param name="MigrationSetName"></param>
        /// <param name="Status"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool VerifyErrorStatusInMigrationPage(string MigrationSetName, string Status, out string Output)
        {
            try
            {
                Driver.GetElement(Button_Refresh).ClickElement();
                var ElementLocator_FinishedWithErrors = new ElementLocator(Locator.XPath, "//span[contains(@id,'DataMigrationSetsName')][contains(text(),'" + MigrationSetName + "')]//parent::td//parent::tr//span[contains(text(),'" + Status + "')]");
                if (VerifyElementandScrollToElement(ElementLocator_FinishedWithErrors))
                {
                    Output = "Status for Newly created " + MigrationSetName + " is displayed as " + "FinishedWithErrors";
                    return true;
                }
                throw new Exception("Failed to get the status for newly created " + MigrationSetName);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get the status for newly created" + MigrationSetName, e);
            }
        }

        /// <summary>
        /// Verify The product with name productname no longer exists in the destination database error message
        /// </summary>
        /// <param name="MigrationSetName"></param>
        /// <param name="Status"></param>
        /// <param name="Message"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        public bool VerifyProductNoLongerExistsExceptionInMigrationViewItemsPage(string MigrationSetName, string Status, out string Message, string productName)
        {
            try
            {
                if (VerifyElementandScrollToElement(ActionOnMigrationSetName(MigrationSetName, "View Items")))
                {
                    Driver.GetElement(ActionOnMigrationSetName(MigrationSetName, "View Items")).ClickElement();
                    Message = "Migration Set: " + MigrationSetName + " View All Action is completed";
                    Driver.GetElement(Button_ProductImage).ClickElement();
                    ElementLocator ElementLocator_ErrorMessage = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + productName + "')]");
                    if (Driver.IsElementPresent(ElementLocator_ErrorMessage, 30))
                    {
                        Driver.GetElement(ElementLocator_ErrorMessage).ClickElement();
                        Message = "Verify The product with name " + productName + " no longer exists in the destination database error message";
                        return true;
                    }
                    throw new Exception("Failed to verify the error message for productImage " + productName);
                }
                throw new Exception("Failed to View Migration Set; Set Details are:" + MigrationSetName);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to View Migration Set; Set Details are:" + MigrationSetName, e);
            }
        }

        /// <summary>
        /// Select Items for Product while Migrating
        /// </summary>
        /// <param name="ProductName"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool SelectItemsForProductCategoryAndProduct(string ProductName, string ProductCategory, out string Output)
        {
            try
            {
                {
                    CheckBoxElmandCheck(SelectTypeToGenerate("Category"));
                    CheckBoxElmandCheck(SelectTypeToGenerate("Products"));
                    Click_OnButton(Button_GenerateItems);
                    CheckBoxElmandCheck(Migration_Obj_Tree("Category"), false);
                    Driver.GetElement(ExpandPathOfNode("Category")).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree("Product"), false);
                    Driver.GetElement(Select_Product).ClickElement();
                    Driver.GetElement(ExpandPathOfNode_Product).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree("ContentAttributeDef"), false);
                    Driver.GetElement(Select_ContentAttributeDef).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree(ProductCategory));
                    CheckBoxElmandCheck(Migration_Obj_Tree(ProductName));
                    Click_OnButton(Button_Save_OnMigratonSet);
                    if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                    {
                        Output = "Generate Items Completed Successfully and Below mentioned Selected ProductName: " + ProductName;
                        return true;
                    }
                    throw new Exception("Failed to Generate Items");
                }
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items", e); }
        }

        /// <summary>
        /// Select Items for Reward with Location Group
        /// </summary>
        /// <param name="RewardName"></param>
        /// <param name="LocationGroupName"></param>
        /// <param name="Output"></param>
        /// <returns></returns>

        public bool SelectItemsForRewardWithLocationGroup(string RewardName, string LocationGroupName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Rewards"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("RewardDef"), false);
                Driver.GetElement(ExpandPathOfNode("RewardDef")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree("LocationGroups"), false);
                Driver.GetElement(ExpandPathOfNode_LocationGroup).ClickElement();
                if (Driver.IsElementPresent(ExpandPathOfNode_LocationGroup, 30))
                {
                    CheckBoxElmandCheck(Migration_Obj_Tree_Value(LocationGroupName));
                    CheckBoxElmandCheck(Migration_Obj_Tree_Value(RewardName));
                    Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                }
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected Reward : " + RewardName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Generate Items", e);
            }
        }

        /// <summary>
        /// Select Items for Reward with Push Notification
        /// </summary>
        /// <param name="RewardName"></param>
        /// <param name="PushNotification"></param>
        /// <param name="Output"></param>
        /// <returns></returns>

        public bool SelectItemsForRewardWithPushNotifaction(string RewardName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Rewards"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("RewardDef"), false);
                Driver.GetElement(ExpandPathOfNode("RewardDef")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(RewardName));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected Reward : " + RewardName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Generate Items", e);
            }
        }

        public bool SelectItemsForWebsiteModule(string moduleName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Websites"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("Website"), false);
                Driver.GetElement(ExpandPathOfNode("Website")).ClickElement();
                Driver.GetElement(ExpandPathOfNode("Module")).ClickElement();
                CheckBoxElmandCheck(Migration_WebsiteObj_Tree_Value(moduleName));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();

                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected; module : " + moduleName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public bool VerifyMigrationAfterDeletion(string MigrationSet_Name, out string outMsg)
        {
            if (!VerifyElementandScrollToElement(MigrationSetName(MigrationSet_Name)))
            {
                outMsg = "Migration Deleted successfully";
                return true;
            }
            throw new Exception("Failed To Verify Migration");
        }

        public bool CloneMigrationSet(string MigrationSet_Name, out string outMsg)
        {
            Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Clone")).ClickElement();
            outMsg = "MigrationSet cloned successfully";
            return true;
        }

        public bool ClickOnEdiItemsActionAndVerifyEditsItemsPanel(string MigrationSet_Name, out string outMsg)
        {
            Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Edit Items")).ClickElement();
            if (Driver.IsElementPresent(Legend_EditItemsText,.5))
            {
                Driver.ScrollIntoMiddle(Legend_EditItemsText);
                outMsg = "Edititems Panel Verified Successfully";
                return true;
            }
            throw new Exception("Failed To verify The Edititems Panel");
        }

        public bool ClickOnEditActionAndVerifyEditPanel(string MigrationSet_Name, out string outMsg)
        {
            Driver.GetElement(ActionOnMigrationSetName(MigrationSet_Name, "Edit")).ClickElement();
            if (Driver.IsElementPresent(Lable_EditText, .5))
            {
                Driver.ScrollIntoMiddle(Lable_EditText);
                outMsg = "Edit Panel Verified Successfully";
                return true;
            }
            throw new Exception("Failed To verify The Edit Panel");
        }


        /// <summary>
        /// Select Items for Promotion while Migrating
        /// </summary>
        /// <param name="Promotion"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForPushNotoficationDef(string Notification, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("Notifications"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("NotificationDef"), false);
                Driver.GetElement(ExpandPathOfNode("NotificationDef")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(Notification));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;Notification:" + Notification;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        /// <summary>
        /// Select items for TextBlock
        /// </summary>
        /// <param name="TextBlockName"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool SelectItemsForTextBlock(string TextBlockName, out string Output)
        {
            try
            {
                {
                    CheckBoxElmandCheck(SelectTypeToGenerate("TextBlocks"));
                    Click_OnButton(Button_GenerateItems);
                    CheckBoxElmandCheck(Migration_Obj_Tree("TextBlock"), false);
                    Driver.GetElement(ExpandPathOfNode("TextBlock")).ClickElement();
                    CheckBoxElmandCheck(Migration_Obj_Tree(TextBlockName));
                    Click_OnButton(Button_Save_OnMigratonSet);
                    if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                    {
                        Output = "Generate Items Completed Successfully and Below mentioned Selected TextBlockName: " + TextBlockName;
                        return true;
                    }
                    throw new Exception("Failed to Generate Items");
                }
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items", e); }
        }

        /// <summary>
        /// select items for BusinessRules
        /// </summary>
        /// <param name="BusinessRule"></param>
        /// <param name="Output"></param>
        /// <returns></returns>
        public bool SelectItemsForRuleEvent(string BusinessRule, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("BusinessRules"));
                Driver.GetElement(Button_GenerateItems).Click();
                Driver.ScrollIntoMiddle(Node_BusinessRule);
                Driver.GetElement(Node_BusinessRule).ClickElement();
                Driver.GetElement(ExpandPathOfNode("BusinessRule")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree("UserDefinedEvent"), false);
                Driver.GetElement(ExpandPathOfNode("UserDefinedEvent")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(BusinessRule));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;Promotion:" + BusinessRule;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items",e); }
        }

        public ElementLocator ExpandPathOfSubNodeAttributeSet(string MainDef, string Classname)
        {
            var ElementLocator_ExpandedAll = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + MainDef + "')]//parent::label//parent::div//parent::li//span[text()='AttributeSet']//parent::label//parent::div[contains(@class,'" + Classname + "')]//span[contains(@class,'rtPlus')]");
            var ElementLocator_CollapseAll = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + MainDef + "')]//parent::label//parent::div//parent::li//span[text()='AttributeSet']//parent::label//parent::div[contains(@class,'" + Classname + "')]//span[contains(@class,'rtPlus')]");

            if (Driver.IsElementPresent(ElementLocator_ExpandedAll, 1))
            {
                return ElementLocator_ExpandedAll;
            }
            else if (Driver.IsElementPresent(ElementLocator_CollapseAll, 1))
            {
                return ElementLocator_ExpandedAll;

            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + ElementLocator_ExpandedAll);
        }

        public ElementLocator ExpandPathOfSubNode(string MainDef)
        {
            var ElementLocator_ExpandedAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='rtTop']//span[@class='rtPlus']");
            var ElementLocator_CollapseAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='rtTop']//span[@class='rtPlus']");

            if (Driver.IsElementPresent(ElementLocator_ExpandedAll, 1))
            {
                return ElementLocator_ExpandedAll;
            }
            else if (Driver.IsElementPresent(ElementLocator_CollapseAll, 1))
            {
                return ElementLocator_ExpandedAll;

            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + ElementLocator_ExpandedAll);
        }

        public ElementLocator ExpandPathOfChildSubNode(string MainDef)
        {
            var ElementLocator_ExpandedAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[contains(@class,'rtMid')]//span[contains(@class,'rtPlus')]");
            var ElementLocator_CollapseAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[contains(@class,'rtMid')]//span[contains(@class,'rtPlus')]");

            if (Driver.IsElementPresent(ElementLocator_ExpandedAll, 1))
            {
                return ElementLocator_ExpandedAll;
            }
            else if (Driver.IsElementPresent(ElementLocator_CollapseAll, 1))
            {
                return ElementLocator_ExpandedAll;

            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + ElementLocator_ExpandedAll);
        }

        /// <summary>
        /// Select Items for AttributeSet while Migrating
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForAttributeSet(string Name, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("AttributeSets"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("AttributeSet"), false);
                Driver.GetElement(ExpandPathOfNode("AttributeSet")).ClickElement();
                Driver.GetElement(ExpandPathOfChildSubNode(Name)).ClickElement();
                Driver.GetElement(ExpandPathOfSubNodeAttributeSet(Name, "rtBot")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(Name));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;AttributrSet:" + Name;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        /// <summary>
        /// Select Items for AttributeSet while Migrating
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForReferenceData(string Name, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("AttributeSets"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("AttributeSet"), false);
                Driver.GetElement(ExpandPathOfNode("AttributeSet")).ClickElement();
                Driver.GetElement(ExpandPathOfSubNode(Name, "rtBot")).ClickElement();
                Driver.GetElement(ExpandPathOfSubNodeAttributeSet(Name, "rtBot")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(Name));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;ReferenceData:" + Name;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        /// <summary>
        /// Select Items for AttributeSet while Migrating
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForRefCountry(string ChilldAttributrName, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("AttributeSets"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("AttributeSet"), false);
                Driver.GetElement(ExpandPathOfNode("AttributeSet")).ClickElement();
                Driver.GetElement(ExpandPathOfChildSubNode(ChilldAttributrName)).ClickElement();
                Driver.GetElement(ExpandPathOfSubNodeAttributeSet(ChilldAttributrName, "rtTop")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(ChilldAttributrName));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;RefCountry:" + ChilldAttributrName;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        /// <summary>
        /// Select Items for GooglePay while Migrating
        /// </summary>
        /// <param name="gPay"></param>
        /// <param name="Output"></param>
        /// <returns>returns true if items generated successfully otherwise return false</returns>
        public bool SelectItemsForGooglePayDef(string gPay, out string Output)
        {
            try
            {
                CheckBoxElmandCheck(SelectTypeToGenerate("AndroidPayLoyaltyCards"));
                Driver.GetElement(Button_GenerateItems).Click();
                CheckBoxElmandCheck(Migration_Obj_Tree("AndroidPayLoyaltyCard"), false);
                Driver.GetElement(ExpandPathOfNode("AndroidPayLoyaltyCard")).ClickElement();
                CheckBoxElmandCheck(Migration_Obj_Tree_Value(gPay));
                Driver.GetElement(Button_Save_OnMigratonSet).ClickElement();
                if (!Driver.IsElementPresent(Button_Save_OnMigratonSet, 1))
                {
                    Output = "Generate Items Completed Successfully and Below mentioned Selected;GooglePay:" + gPay;
                    return true;
                }
                throw new Exception("Failed to Generate Items");
            }
            catch (Exception e) { throw new Exception("Failed to Generate Items"); }
        }

        public ElementLocator Migration_WebsiteObj_Tree_Value(string MainDef)
        {
            var ElementLocator_Checkbox = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + MainDef + "')]//preceding-sibling::input");
            if (Driver.IsElementPresent(ElementLocator_Checkbox, 1))
            {
                return ElementLocator_Checkbox;
            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + MainDef);

        }

        public ElementLocator ExpandPathOfSubNode(string MainDef, string className)
        {
            var ElementLocator_ExpandedAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='" + className + "']//span[@class='rtPlus']");
            var ElementLocator_CollapseAll = new ElementLocator(Locator.XPath, "//span[text()='" + MainDef + "']//parent::label//parent::div[@class='" + className + "']//span[@class='rtPlus']");

            if (Driver.IsElementPresent(ElementLocator_ExpandedAll, 1))
            {
                return ElementLocator_ExpandedAll;
            }
            else if (Driver.IsElementPresent(ElementLocator_CollapseAll, 1))
            {
                return ElementLocator_ExpandedAll;

            }
            throw new Exception("Failed to Find Expected Object on Migration Tree:" + ElementLocator_ExpandedAll);
        }

    }
}