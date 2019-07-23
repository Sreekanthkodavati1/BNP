using Bnp.Core.WebPages.Models;
using BnPBaseFramework.Web;
using BnPBaseFramework.Web.Extensions;
using BnPBaseFramework.Web.Types;
using BnPBaseFramework.Web.WebElements;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Windows.Forms;

namespace Bnp.Core.WebPages.Navigator.UsersPage.Models
{
    public class Navigator_RuleTriggers : ProjectBasePage
    {

        public Navigator_RuleTriggers(DriverContext driverContext) : base(driverContext) { }

        public enum TypeofInputs
        {
            NoRule,
            MatchingRuleExisted,
            MisMatchingRuleExisted,
            UnableToEditDeleteRuleandCreateNew
        }
        #region Home Page Locators
        private readonly ElementLocator Button_CreateNewRule = new ElementLocator(Locator.XPath, CustomizedXpathWithIDandText("_lnkAddNew", "Create New Rule"));
        private readonly ElementLocator Button_RuleSave = new ElementLocator(Locator.XPath, CustomizedXpathWithIDandText("_lnkSave", "Save"));

        private readonly ElementLocator TextBox_RuleName = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Rule Name"));
        private readonly ElementLocator Select_RuleOwner = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Rule Owner"));
        private readonly ElementLocator Select_RuleType = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Rule Type"));
        private readonly ElementLocator Select_Invocation = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Invocation"));
        private readonly ElementLocator TextBox_StartDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'_StartDate') and contains(@class,'Enabled')]");
        private readonly ElementLocator TextBox_EndDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'_EndDate') and contains(@class,'Enabled')]");
        private readonly ElementLocator TextBox_Sequence = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Sequence"));
        private readonly ElementLocator TextBox_Expression = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Expression"));
        private readonly ElementLocator CheckBox_Targeted = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Targeted"));
        private readonly ElementLocator Select_Promotion = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Promotion"));
        private readonly ElementLocator CheckBox_AlwaysRun = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Always Run"));
        private readonly ElementLocator CheckBox_ContinueOnError = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Continue On Error"));
        private readonly ElementLocator CheckBox_LogExecution = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Log Execution"));
        private readonly ElementLocator CheckBox_Queueruleexecution = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Queue rule execution"));
        private readonly ElementLocator Header_CreateRule = new ElementLocator(Locator.XPath, "//h2[contains(text(),'Create New Rule')] ");
        private readonly ElementLocator Header_EditRule = new ElementLocator(Locator.XPath, "//h2[contains(text(),'Edit')] ");
        private readonly ElementLocator Button_ConfigSave = new ElementLocator(Locator.XPath, "//a[contains(@id,'RuleConfigSave') and text()='Save']");
        private readonly ElementLocator Button_ConfigCancel = new ElementLocator(Locator.XPath, "//a[contains(@id,'RuleConfigCancel') and text()='Cancel']");

        private readonly ElementLocator Message_Error = new ElementLocator(Locator.XPath, "//*[contains(@id,'pnlErrorPanel')]");


        #endregion

        #region IssueVirtualCardIfNotExist Configure ElementLocators
        private readonly ElementLocator Check_Skipcreationifmemberhasanexisting_virtualcard = new ElementLocator(Locator.XPath, "//span[contains(text(),'Skip creation if member has an existing virtual card')]//..//..//input");
        private readonly ElementLocator Select_LoyaltyId_GenerationSource = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Loyalty Id Generation Source"));
        private readonly ElementLocator TextBox_RuleVersion = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Rule Version"));
        private readonly ElementLocator TextBox_LoyaltyId_GenerationSource = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Rule Description"));

        #endregion


        #region IssueReward and Issue Reward from Catalog Configure ElementLocators
        private readonly ElementLocator Check_AssignLoyaltyWareCertificate = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Assign LoyaltyWare Certificate"));
        private readonly ElementLocator Select_FulfillmentMethod = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Fulfillment Method"));
        private readonly ElementLocator Select_PointsConsumedWhenIssued = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Points Consumed When Issued"));
        private readonly ElementLocator TextBox_ExpirationDate = new ElementLocator(Locator.XPath, "//input[contains(@id,'ExpiryDateExpression')and @autocomplete='off']");
        private readonly ElementLocator Select_RewardTypePoints = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Reward Type"));
        private readonly ElementLocator Select_ConsumedWhenIssues = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Consumed When Issued"));
        private readonly ElementLocator Check_IssuetheMemberRewardChoice = new ElementLocator(Locator.XPath, "//input[contains(@id,'propIssueMemberRewardChoice')]");
        private readonly ElementLocator Select_RewardName = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Reward Name"));

        #endregion

        #region Award Loyalty Currency Configure ElementLocators
        private readonly ElementLocator TextBox_AccrualExpression = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Accrual Expression"));
        private readonly ElementLocator Select_TierEvaluationRule = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Tier Evaluation Rule"));
        private readonly ElementLocator Check_AllowZeroPointAward    = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Allow Zero Point Award"));

        #endregion

        #region IssueMessage Configure ElementLocators
        private readonly ElementLocator Check_AllowMultipleMessages = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Allow Multiple Messages"));
        #endregion

        #region Evaluate Tier ElementLocators
        private readonly ElementLocator Select_VirtualCardLocationLogic = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Virtual Card Location Logic"));
        private readonly ElementLocator Check_TierStandard = new ElementLocator(Locator.XPath, "//label[text()='Standard' and contains(@for,'propTiers')]//preceding-sibling::input");
        private readonly ElementLocator Check_TierSilver= new ElementLocator(Locator.XPath, "//label[text()='Silver' and contains(@for,'propTiers')]//preceding-sibling::input");
        private readonly ElementLocator Check_TierGold = new ElementLocator(Locator.XPath, "//label[text()='Gold' and contains(@for,'propTiers')]//preceding-sibling::input");
        private readonly ElementLocator Check_TierPlatinum = new ElementLocator(Locator.XPath, "//label[text()='Platinum' and contains(@for,'propTiers')]//preceding-sibling::input");

        private readonly ElementLocator Expression_Sigma = new ElementLocator(Locator.XPath, "//div[@class='ExpressionBoxButton Sigma']");
        private readonly ElementLocator Expression_Wizard = new ElementLocator(Locator.XPath, "//span[text()='Expression Wizard']");
        private readonly ElementLocator Link_Points = new ElementLocator(Locator.XPath, "//a[text()='Points']");
        private readonly ElementLocator Earned_Points = new ElementLocator(Locator.XPath, "//a[text()='Earned Points*']");
        private readonly ElementLocator Input_StartDate = new ElementLocator(Locator.XPath, "//div[text()='From which date?']//following-sibling::input");
        private readonly ElementLocator Input_EndDate = new ElementLocator(Locator.XPath, "//div[text()='To which date?']//following-sibling::input");
        private readonly ElementLocator Input_Expiredpoints = new ElementLocator(Locator.XPath, "//div[text()='Include expired points?']//following-sibling::input");
        private readonly ElementLocator Button_SaveAndClose = new ElementLocator(Locator.XPath, "//a[text()='Save and Close']");




        #endregion

        #region IssueCoupon Configure ElementLocators
        private readonly ElementLocator Select_CouponName = new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Coupon Name"));
        #endregion

        #region Send Triggered Email ElementLocator
        private readonly ElementLocator TextBox_The_Emailoftherecipient = new ElementLocator(Locator.XPath, "//span[text()='The email of the recipient']//parent::td//following-sibling::td//input[contains(@id,'txtExpression')]");
        private readonly ElementLocator Select_TriggeredEmail= new ElementLocator(Locator.XPath, CustomizedXpath_ForSelect_WithLabel("Triggered Email"));
        private readonly ElementLocator TextBox_Rule_Version   = new ElementLocator(Locator.XPath, CustomizedXpath_ForInput_WithLabel("Rule Version"));

        public void ConfigCancel()
        {
            Click_OnButton(Button_ConfigCancel);
        }
        #endregion
        public static string CustomizedXpathWithIDandText(string id, string text)
        {
            string _Custompath = "//*[contains(@id,'" + id + "') and text()='" + text + "']";
            return _Custompath;
        }

        public static string CustomizedXpath_ForInput_WithLabel(string text)
        {
            string _Custompath = "//span[text()='" + text + "']//parent::td//following-sibling::td//input";
            return _Custompath;
        }

        public static string CustomizedXpath_ForSelect_WithLabel(string text)
        {
            string _Custompath = "//span[text()='" + text + "']//parent::td//following-sibling::td//Select";
            return _Custompath;
        }

        public ElementLocator Menu(string MenuName)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//ul[@class='section_menu']//span[text()='" + MenuName + "']");
            return _Menu;

        }

        //span[contains(text(),'IssueVirtualCardIfNotExist') and contains(@id,'RuleName')]//parent::td//following-sibling::td//a[text()='Edit']

        public ElementLocator ActionsofRules(string RuleName, string Action)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[(text()='" + RuleName + "') and contains(@id,'RuleName')]//parent::td//following-sibling::td//a[text()='" + Action + "']");
            return _Menu;
        }

        public ElementLocator RuleWithRuleType(string RuleName, string RuleType)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + RuleName + "') and contains(@id,'RuleName')]//parent::td//following-sibling::td//span[contains(text(),'" + RuleType + "') and contains(@id,'RuleType')]");
            return _Menu;
        }

        public ElementLocator RulesWithAllVariables_WithoutConfigure(Rules Rule)
        {
            if(Rule.Rulestatus_ToInactive)
            {
                ElementLocator _Menu1 = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + Rule.RuleName + "') and contains(@id,'RuleName')]//..//..//td//span[contains(text(),'" + Rule.RuleOwner + "')]//..//..//td//span[contains(text(),'" + Rule.RuleType + "')]//..//..//td//span[contains(text(),'" + Rule.Invocation + "')]//..//..//td//span[text()='" + Rule.Sequence + "']//..//..//td//span//input[contains(@id,'Active')and not(@checked='checked')]");
                return _Menu1;
            }
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + Rule.RuleName + "') and contains(@id,'RuleName')]//..//..//td//span[contains(text(),'" + Rule.RuleOwner + "')]//..//..//td//span[contains(text(),'" + Rule.RuleType + "')]//..//..//td//span[contains(text(),'" + Rule.Invocation + "')]//..//..//td//span[text()='" + Rule.Sequence + "']//..//..//td//span//input[contains(@id,'Active')and @checked='checked']");
            return _Menu;
        }

        public ElementLocator RulesWithAllVariables_WithConfigure(Rules Rule)
        {
            ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[contains(text(),'" + Rule.RuleName + "') and contains(@id,'RuleName')]//..//..//td//span[contains(text(),'" + Rule.RuleOwner + "')]//..//..//td//span[contains(text(),'" + Rule.RuleType + "')]//..//..//td//span[contains(text(),'" + Rule.Invocation + "')]//..//..//td//span[text()='" + Rule.Sequence + "']//..//..//td//span//input[contains(@id,'IsConfigured')and @checked='checked']");
            return _Menu;
        }

        public TypeofInputs VerifyBasicRuleMatchingifNotEdit_Rule(Rules Rule)
        {
            if (VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Edit")))
            {
                if (!Driver.IsElementPresent(RuleWithRuleType(Rule.RuleName, Rule.RuleType), 1))
                {
                    return TypeofInputs.UnableToEditDeleteRuleandCreateNew;
                }
                if (Driver.IsElementPresent(RulesWithAllVariables_WithoutConfigure(Rule), 1))
                {
                    return TypeofInputs.MatchingRuleExisted;
                }
                return TypeofInputs.MisMatchingRuleExisted;
            }
            return TypeofInputs.NoRule;
        }

        public void SelectTextInCaseTextIsAvailable(ElementLocator Elem, string PromotionValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(PromotionValue))
                {
                    SelectElement_AndSelectByText(Elem, PromotionValue);
                }
            }
            catch (Exception e) { throw new Exception("Failed to Select Promotion:" + PromotionValue); }
        }

        public void CreateRule(Rules Rule, out string Message)
        {
            TypeofInputs RuleStatus = VerifyBasicRuleMatchingifNotEdit_Rule(Rule);

            switch (RuleStatus)
            {
                case TypeofInputs.NoRule:
                    Driver.GetElement(Button_CreateNewRule).ClickElement();
                    EnterRule(Rule, out string RuleMessage);
                    if (VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Edit")))
                    {
                        Message = "New Rule Created Successfully and Rule Details are mentioned below ;" + RuleMessage; break;
                    }
                    else
                    {
                        throw new Exception("Failed to Enter Values referscreen shot for more details");
                    }
                case TypeofInputs.MisMatchingRuleExisted:

                    EditRuleDetails(Rule, out RuleMessage);
                    if (VerifyElementandScrollToElement(RulesWithAllVariables_WithoutConfigure(Rule)))
                    {

                        Message = "Exsited Rule Updated Successfully and Rule Details are mentioned below ;" + RuleMessage; break;
                    }
                    else
                    {
                        throw new Exception("Failed to Edit the Rule,Delete the Rule Manually and Run the test case");
                    }
                case TypeofInputs.UnableToEditDeleteRuleandCreateNew:
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Delete")).ClickElement();
                    Driver.SwitchTo().Alert().Accept();
                    Driver.GetElement(Button_CreateNewRule).ClickElement();

                    EnterRule(Rule, out RuleMessage);
                    if (VerifyElementandScrollToElement(RulesWithAllVariables_WithoutConfigure(Rule)))
                    {
                        Message = "Rule Delete Due to Mismatching Rule Type and New Rule Created Successfully and Rule Details are mentioned below ;" + RuleMessage; break;
                    }
                    else
                    {
                        throw new Exception("Failed to Create Rule referscreen shot for more details");
                    }
                case TypeofInputs.MatchingRuleExisted:

                    RuleMessage = "Rule Name:" + Rule.RuleName
                            + ";Rule Owner :" + Rule.RuleOwner
                            + ";Rule Type :" + Rule.RuleType
                            + ";Rule Invocation :" + Rule.Invocation
                            + ";Rule Sequence:" + Rule.Sequence
                            + ";Rule Promotion if any:" + Rule.Promotion;
                    Message = " Rule Existed Already and  Rule Details are appeared as  mentioned below ;" + RuleMessage; break;
                default:
                    throw new Exception("Failed to Edit or Create Rule , Please refer Screenshot for moredetais");
            }

        }


        public bool Configure_IssueVirtualCardIfNotExist(Rules_Configurations_CreateVirtual_Card RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (IsTextSelected(Select_LoyaltyId_GenerationSource, RuleConfig.LoyaltyId_GenerationSource) && IsChecked(Check_Skipcreationifmemberhasanexisting_virtualcard))
                {
                    Message = "Configuration Set up Already Available as mentioned below for Rule:" + Rule.RuleName + "Configuration Details ;Skip creation if member has an existingvirtualcard:" + RuleConfig.Skipcreation_Onexistingvirtualcard.ToString()
                                                                                               + ";LoyaltyId GenerationSource:" + RuleConfig.LoyaltyId_GenerationSource;
                }
                else
                {
                    CheckBoxElmandCheck(Check_Skipcreationifmemberhasanexisting_virtualcard, RuleConfig.Skipcreation_Onexistingvirtualcard);
                    SelectElement_AndSelectByText(Select_LoyaltyId_GenerationSource, RuleConfig.LoyaltyId_GenerationSource);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + "Configuration Details ;Skip creation if member has an existingvirtualcard:" + RuleConfig.Skipcreation_Onexistingvirtualcard.ToString()
                                                                                                   + ";LoyaltyId GenerationSource:" + RuleConfig.LoyaltyId_GenerationSource;
                }
                Driver.ScrollIntoMiddle(Check_Skipcreationifmemberhasanexisting_virtualcard);
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
        }
        public bool Configure_IssueRewardfrom_Catalog(Rules_IssueRewardfromcatalog RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (!IsTextSelected(Select_FulfillmentMethod, RuleConfig.FulfillmentMethod) && !IsTextSelected(Select_PointsConsumedWhenIssued, RuleConfig.PointsConsumedWhenIssued) && !IsTextSelected(Select_FulfillmentMethod, RuleConfig.FulfillmentMethod) && !IsChecked(Check_AssignLoyaltyWareCertificate))
                {
                    SelectElement_AndSelectByText(Select_FulfillmentMethod, RuleConfig.FulfillmentMethod);
                    CheckBoxElmandCheck(Check_AssignLoyaltyWareCertificate, RuleConfig.AssignLoyaltyWareCertificate);
                    SelectElement_AndSelectByText(Select_PointsConsumedWhenIssued, RuleConfig.PointsConsumedWhenIssued);
                    Driver.GetElement(TextBox_ExpirationDate).SendText(RuleConfig.ExpirationDate);

                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + "Configuration Details in below screenshot";
                }
                else
                {
                    Message = "Configuration Set up Already Available for Rule:" + Rule.RuleName + "Configuration Details in below screenshot";

                }
                Driver.ScrollIntoMiddle(Check_AssignLoyaltyWareCertificate);

                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
        }

        public bool Configure_IssueReward(Rules_IssueReward RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                if (IsTextSelected(Select_FulfillmentMethod, RuleConfig.FulfillmentMethod) && IsTextSelected(Select_RewardTypePoints, RuleConfig.RewardTypePoints) && IsTextSelected(Select_PointsConsumedWhenIssued, RuleConfig.PointsConsumedWhenIssued) && IsChecked(Check_AssignLoyaltyWareCertificate) && IsTextSelected(Select_RewardName, RuleConfig.RewardName) && IsChecked(Check_AssignLoyaltyWareCertificate))
                {
                    Message = "Configuration Set up Already Available for Rule:"+  Rule.RuleName + "Configuration Details as mentioned below;Fulfillment Method" + RuleConfig.FulfillmentMethod
                        + ";Reward Type:	" + RuleConfig.RewardTypePoints
                        + ";Points Consumed When Issued:" + RuleConfig.RewardTypePoints
                        + ";Reward Name if Any:" + RuleConfig.RewardName
                        + ";Issue the Member's Reward Choice:" + RuleConfig.IssuetheMembersRewardChoice.ToString()
                        + ";Assign LoyaltyWare Certificate:" + RuleConfig.AssignLoyaltyWareCertificate;
                }
                else
                {
                    SelectElement_AndSelectByText(Select_FulfillmentMethod, RuleConfig.FulfillmentMethod);
                    CheckBoxElmandCheck(Check_AssignLoyaltyWareCertificate, RuleConfig.AssignLoyaltyWareCertificate);
                    SelectElement_AndSelectByText(Select_RewardTypePoints, RuleConfig.RewardTypePoints);
                    SelectElement_AndSelectByText(Select_PointsConsumedWhenIssued, RuleConfig.PointsConsumedWhenIssued);
                    SelectTextInCaseTextIsAvailable(Select_RewardName, RuleConfig.RewardName);
                    CheckBoxElmandCheck(Check_IssuetheMemberRewardChoice, RuleConfig.IssuetheMembersRewardChoice);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + "Configuration Details as mentioned below;Fulfillment Method"+ RuleConfig.FulfillmentMethod
                        + ";Reward Type:	" + RuleConfig.RewardTypePoints
                        + ";Points Consumed When Issued:"+ RuleConfig.RewardTypePoints
                        + ";Reward Name if Any:"+ RuleConfig.RewardName
                        + ";Issue the Member's Reward Choice:" +RuleConfig.IssuetheMembersRewardChoice.ToString()
                        + ";Assign LoyaltyWare Certificate:"+ RuleConfig.AssignLoyaltyWareCertificate;
                }
                Driver.ScrollIntoMiddle(Check_AssignLoyaltyWareCertificate);
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details" + e);
            }
        }


        public bool Configure_AwardLoyaltyCurrency(Rules_AwardLoyaltyCurrency RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (Driver.GetElement(TextBox_AccrualExpression).GetTextContent().Equals(RuleConfig.AccrualExpression) && IsTextSelected(Select_TierEvaluationRule, RuleConfig.TierEvaluationRule)&&IsChecked(Check_AllowZeroPointAward))
                {
                    Message = "Configuration Set up is Already Available for Rule:" + Rule.RuleName + ";Accural Expression:" + RuleConfig.AccrualExpression + " ;TierEvaluationRule if any" + RuleConfig.TierEvaluationRule;

                }
                {
                    Driver.GetElement(TextBox_AccrualExpression).Clear();
                    Driver.GetElement(TextBox_AccrualExpression).ClickElement();
                    Clipboard.SetText(RuleConfig.AccrualExpression);
                    Actions actions = new Actions(Driver);
                    IWebElement webElement = Driver.FindElement(By.XPath(CustomizedXpath_ForInput_WithLabel("Accrual Expression")));
                    actions.MoveToElement(webElement).SendKeys(OpenQA.Selenium.Keys.Control + "v").Build().Perform();
                    SelectTextInCaseTextIsAvailable(Select_TierEvaluationRule, RuleConfig.TierEvaluationRule);
                    CheckBoxElmandCheck(Check_AllowZeroPointAward, RuleConfig.AllowZeroPointAward);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Message = "Configuration  Set up  Completed Successfully for Rule:" + Rule.RuleName + "; Accural Expression:" + RuleConfig.AccrualExpression + " ;TierEvaluationRule if any" + RuleConfig.TierEvaluationRule;
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                }
                Driver.GetElement(TextBox_AccrualExpression).ScrollToElement();

                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + " due to " + e + "; Refer screenshot for more details");
            }
        }

        public bool Configure_IssueMessage(Rules_IssueMessage RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (IsChecked(Check_AllowMultipleMessages))
                {
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + ";Allow Multiple Messages:" + RuleConfig.AllowMultipleMessages;
                }
                {
                    CheckBoxElmandCheck(Check_AllowMultipleMessages, RuleConfig.AllowMultipleMessages);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Message = "Configuration is Already Available for Rule:" + Rule.RuleName + "; Allow Multiple Messages:" + RuleConfig.AllowMultipleMessages;
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                }
                Driver.GetElement(Check_AllowMultipleMessages).ScrollToElement();
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + " due to " + e + "; Refer screenshot for more details");
            }
        }

        public bool Configure_IssueCoupon(Rules_IssueCoupon RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (IsChecked(Check_AssignLoyaltyWareCertificate)&& IsTextSelected(Select_CouponName,RuleConfig.CouponName))
                {
                    Message = "Configuration is Already Available for Rule:" + Rule.RuleName + "; Assign LoyaltyWare Certificate:" + RuleConfig.AssignLoyaltyWareCertificate + ";Coupon if any:" + RuleConfig.CouponName;

                }
                {
                    CheckBoxElmandCheck(Check_AssignLoyaltyWareCertificate, RuleConfig.AssignLoyaltyWareCertificate);
                    SelectTextInCaseTextIsAvailable(Select_CouponName, RuleConfig.CouponName);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + ";Assign LoyaltyWare Certificate:" + RuleConfig.AssignLoyaltyWareCertificate + ";Coupon if any:" + RuleConfig.CouponName;                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                }
                Driver.GetElement(Check_AssignLoyaltyWareCertificate).ScrollToElement();
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + " due to " + e + "; Refer screenshot for more details");
            }
        }

        public bool Configure_TriggerEmail(Rules_CFContactUsEmailRule RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (IsTextSelected(Select_TriggeredEmail,RuleConfig.Triggered_Email) && Driver.FindElement(By.XPath(TextBox_The_Emailoftherecipient.Value)).Text.ToString().Contains(RuleConfig.TheEmailoftherecipient)&& Driver.GetElement(TextBox_Rule_Version).GetTextContent().Contains(RuleConfig.Rule_Version))
                {
                    Message = "Configuration Set up Already Available for Rule:" + Rule.RuleName + ";The email of the recipient:" + RuleConfig.TheEmailoftherecipient + ";Triggered Email	:" + RuleConfig.Triggered_Email+";Rule Version:"+RuleConfig.Rule_Version;
                }
                {
                    Driver.GetElement(TextBox_The_Emailoftherecipient).SendText(RuleConfig.TheEmailoftherecipient);
                    SelectElement_AndSelectByText(Select_TriggeredEmail, RuleConfig.Triggered_Email);
                    Driver.GetElement(TextBox_Rule_Version).SendText(RuleConfig.Rule_Version);
                    Click_OnButton(Button_ConfigSave);
                    VerifyErrorMessage();
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + ";The email of the recipient:" + RuleConfig.TheEmailoftherecipient + ";Triggered Email	:" + RuleConfig.Triggered_Email + ";Rule Version:" + RuleConfig.Rule_Version;
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                }
                Driver.GetElement(TextBox_Rule_Version).ScrollToElement();
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + " due to " + e + "; Refer screenshot for more details");
            }
        }

        public bool Configure_EvaluteTier(Rules_EvaluateTier RuleConfig, Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Configure"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();

                if (IsChecked(Check_TierStandard) && IsChecked(Check_TierPlatinum) && IsChecked(Check_TierSilver) && IsChecked(Check_TierGold) && IsTextSelected(Select_VirtualCardLocationLogic, RuleConfig.VirtualCardLocationLogic))
                {
                    Message = "Configuration Set up Completed Successfully for Rule:" + Rule.RuleName + ";Virtual Card Location Logic	:" + RuleConfig.VirtualCardLocationLogic + ";Tiers: Standard,Silver,Gold and Platinum";
                }
                {
                    CheckBoxElmandCheck(Check_TierStandard, RuleConfig.Tiers_Standard);
                    CheckBoxElmandCheck(Check_TierSilver, RuleConfig.Tiers_Silver);
                    CheckBoxElmandCheck(Check_TierGold, RuleConfig.Tiers_Gold);
                    CheckBoxElmandCheck(Check_TierPlatinum, RuleConfig.Tiers_Platinum);

                    SelectTextInCaseTextIsAvailable(Select_VirtualCardLocationLogic, RuleConfig.VirtualCardLocationLogic);
                    Driver.GetElement(Button_ConfigSave).ClickElement();
                    VerifyErrorMessage();
                    Message = "Configuration is Already Available for Rule:" + Rule.RuleName + ";Virtual Card Location Logic	:" + RuleConfig.VirtualCardLocationLogic + ";Tiers: Standard,Silver,Gold and Platinum";
                    Driver.GetElement(ActionsofRules(Rule.RuleName, "Configure")).ClickElement();
                }
                Driver.GetElement(Select_VirtualCardLocationLogic).ScrollToElement();
                if (VerifyElementandScrollToElement(RulesWithAllVariables_WithConfigure(Rule)))
                {
                    return true;
                }
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + "; Refer screenshot for more details");
            }
            catch (Exception e)
            {
                throw new Exception("Failed to Add Configurations for Rule:" + Rule.RuleName + " due to " + e + "; Refer screenshot for more details");
            }
        }

        public bool VerifyErrorMessage()
        {
            if (Driver.IsElementPresent(Message_Error, 1))
            {
                Driver.ScrollIntoMiddle(Message_Error);
                throw new Exception("UnExpected Error Appeared Hence Failing this Test");

            }
            return true;
        }

        public void EditRuleDetails(Rules Rule, out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Edit"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Edit")).ClickElement();

                Message = "";
                if (Driver.IsElementPresent(Header_EditRule, 1))
                {
                    Driver.ScrollIntoMiddle(Header_EditRule);
                    SelectElement_AndSelectByText(Select_RuleOwner, Rule.RuleOwner);
                    SelectElement_AndSelectByText(Select_Invocation, Rule.Invocation);
                    Driver.GetElement(TextBox_StartDate).SendText(Rule.StartDate);
                    Driver.GetElement(TextBox_EndDate).SendText(Rule.EndDate);
                    Driver.GetElement(TextBox_Sequence).SendText(Rule.Sequence);
                    Driver.GetElement(TextBox_Expression).SendText(Rule.Expression);
                    CheckBoxElmandCheck(CheckBox_Targeted, Rule.Targeted);
                    SelectTextInCaseTextIsAvailable(Select_Promotion, Rule.Promotion);
                    Driver.GetElement(Button_RuleSave).ClickElement();
                    VerifyErrorMessage();
                    Driver.Navigate().Refresh();
                    if (VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Edit")))
                    {
                        Message = " Rule Name:" + Rule.RuleName
                            + ";Rule Owner :" + Rule.RuleOwner
                            + ";Rule Type :" + Rule.RuleType
                            + ";Rule Invocation :" + Rule.Invocation
                            + ";Rule StartDate:" + Rule.StartDate
                            + ";Rule EndDate:" + Rule.EndDate
                            + ";Rule Sequence:" + Rule.Sequence
                            + ";Rule Expression:" + Rule.Expression
                            + ";Rule Targeted Status:" + Rule.Targeted.ToString()
                            + ";Rule Promotion if any:" + Rule.Promotion;
                    }
                }
            }
            catch (Exception e) { throw new Exception("Failed to Enter Values referscreen shot for more details" + e); }
        }

        public bool Delete_Rule(Rules Rule,out string Message)
        {
            try
            {
                VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Delete"));
                Driver.GetElement(ActionsofRules(Rule.RuleName, "Delete")).ClickElement();
                Driver.SwitchTo().Alert().Accept();
                if (!Driver.IsElementPresent(RuleWithRuleType(Rule.RuleName, Rule.RuleType), 1))
                {
                    Message = "Rule Deleted Successful; Rule Name:" + Rule.RuleName;
                    return true;
                }
                throw new Exception("Failed to Delete Rule ;Rule Name:" + Rule.RuleName);
            }catch(Exception e) { throw new Exception("Failed to Delete Rule ;Rule Name:" + Rule.RuleName +"Due to"+e.Message); }
        }
        public void EnterRule(Rules Rule, out string Message)
        {
            try
            {
                Message = "";
                if (Driver.IsElementPresent(Header_CreateRule, 1))
                {
                    Driver.GetElement(TextBox_RuleName).SendText(Rule.RuleName);
                    SelectElement_AndSelectByText(Select_RuleOwner, Rule.RuleOwner);
                    SelectElement_AndSelectByText(Select_RuleType, Rule.RuleType);
                    SelectElement_AndSelectByText(Select_Invocation, Rule.Invocation);
                    Driver.GetElement(TextBox_StartDate).SendText(Rule.StartDate);
                    Driver.GetElement(TextBox_EndDate).SendText(Rule.EndDate);
                    Driver.GetElement(TextBox_Sequence).SendText(Rule.Sequence);
                    Driver.GetElement(TextBox_Expression).SendText(Rule.Expression);
                    CheckBoxElmandCheck(CheckBox_Targeted, Rule.Targeted);
                    SelectTextInCaseTextIsAvailable(Select_Promotion, Rule.Promotion);
                    Driver.GetElement(Button_RuleSave).ClickElement();
                    if (VerifyElementandScrollToElement(RuleWithRuleType(Rule.RuleName,Rule.RuleType)))
                    {
                        Message = " Rule Name:" + Rule.RuleName
                            + ";Rule Owner :" + Rule.RuleOwner
                            + ";Rule Type :" + Rule.RuleType
                            + ";Rule Invocation :" + Rule.Invocation
                            + ";Rule StartDate:" + Rule.StartDate
                            + ";Rule EndDate:" + Rule.EndDate
                            + ";Rule Sequence:" + Rule.Sequence
                            + ";Rule Expression:" + Rule.Expression
                            + ";Rule Targeted Status:" + Rule.Targeted.ToString()
                            + ";Rule Promotion if any:" + Rule.Promotion.ToString();
                    }
                }
            }
            catch (Exception e) { throw new Exception("Failed to Enter Values referscreen shot for more details"); }
        }

        public void CreateBscriptExpression(Rules Rule, out string outMsg)
        {
            Rule.StartDate = Rule.StartDate.Replace('-', '/');
            Rule.EndDate = Rule.EndDate.Replace('-', '/');
            outMsg = "";
            if (VerifyElementandScrollToElement(ActionsofRules(Rule.RuleName, "Edit")))
            {
                ElementLocator _Menu = new ElementLocator(Locator.XPath, "//span[(text()='" + Rule.RuleName + "') and contains(@id,'RuleName')]//parent::td//following-sibling::td//a[text()='Edit']");
                Driver.GetElement(_Menu).ClickElement();
                Driver.GetElement(Expression_Sigma).ClickElement();
                if (Driver.IsElementPresent(Expression_Wizard, .5))
                {
                    Driver.GetElement(Link_Points).ClickElement();
                    Driver.GetElement(Earned_Points).ClickElement();
                    Driver.GetElement(Input_StartDate).SendText(Rule.StartDate);
                    Driver.GetElement(Input_EndDate).SendText(Rule.EndDate);
                    Driver.GetElement(Input_Expiredpoints).SendText("True");
                    Driver.ScrollIntoMiddle(Button_SaveAndClose);
                    Driver.GetElement(Button_SaveAndClose).ClickElement();
                    if (!Driver.IsElementPresent(Button_SaveAndClose, .5))
                    {
                        Driver.GetElement(Button_RuleSave).ClickElement();
                        outMsg = "Bscript Expression For issue points created successfully";
                    }


                }
            }
        }
    }
}
